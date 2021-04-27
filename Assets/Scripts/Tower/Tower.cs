using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    // Tower stat variables
    private float health = 50f;
    private float fireRate = 100f;
    private float projectileSpeed = 25f;
    private float physicalDamage = 1f;
    private float fireDamage = 0f;
    private float physicalResistance = 0f;
    private float fireResistance = 0f;

    // Get only variables to access tower stats
    public float Health { get => health; }
    public float FireRate { get => fireRate; }
    public float ProjectileSpeed { get => projectileSpeed; }
    public float PhysicalDamage { get => physicalDamage; }
    public float FireDamage { get => fireDamage; }
    public float PhysicalResistance { get => physicalResistance; }
    public float FireResistance { get => fireResistance; }

    // Health variable used to restore tower health at the end of the round
    private float maxHealth;

    // Module variables
    public string[] modules;

    // Reference variables
    private Transform projectileSpawn;
    private GameObject projectile;
    public Placement placement;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 startPosition;
    private bool moved = false;
    private RoundSpawning roundSpawning;
    private UIBehaviourManager manager;
    private Animator animator;
    private AudioSource audioSource;
    private InventoryMenu inventoryMenu;
    private bool bGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        inventoryMenu = GameObject.Find("GameHandler/UI/UICanvas/TowerInfoPanel").GetComponent<InventoryMenu>();
        roundSpawning = GameHandler.Instance.GetComponent<RoundSpawning>();
        roundSpawning.OnRoundStart += OnRoundStart;
        roundSpawning.OnRoundEnd += OnRoundEnd;

        GameHandler.ApplyGameOver += GameOver;

        canvas = GameObject.Find("GameHandler/Background").GetComponent<Canvas>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        manager = GameObject.Find("GameHandler/UI/UICanvas").GetComponent<UIBehaviourManager>();

        projectileSpawn = transform.Find("ProjectileSpawnLocation");
        this.projectile = GameAssets.Instance.projectile;

        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();

        if (roundSpawning.bInRound)
        {
            Invoke("FireProjectile", 1.0f);
        }
    }

    private void GameOver()
    {
        CancelInvoke("FireProjectile");
        bGameOver = true;
    }

    public IEnumerator ApplyModule(GameObject module, int moduleSlot)
    {
        // Check if a module is already applied in this slot
        if (!String.IsNullOrEmpty(modules[moduleSlot]))
        {
            // If a module is already applied, remove its buff
            GameObject moduleToRemove = GameAssets.Instance.GetModuleAsset(modules[moduleSlot]);
            switch (moduleToRemove.GetComponent<DragDrop>().moduleType)
            {
                case ModuleType.HEALTH:
                    this.health -= moduleToRemove.GetComponent<Health>().GetModifier();
                    this.maxHealth -= moduleToRemove.GetComponent<Health>().GetModifier();
                    if (this.health <= 0) this.health = 1;
                    break;
                case ModuleType.PHYSICALDAMAGE: this.physicalDamage -= moduleToRemove.GetComponent<PhysicalDamage>().GetModifier(); break;
                case ModuleType.FIREDAMAGE: this.fireDamage -= moduleToRemove.GetComponent<FireDamage>().GetModifier(); break;
                case ModuleType.FIRERATE: this.fireRate -= moduleToRemove.GetComponent<FireRate>().GetModifier(); break;
                case ModuleType.PHYSICALRESISTANCE: this.physicalResistance -= moduleToRemove.GetComponent<PhysicalResistance>().GetModifier(); break;
                case ModuleType.FIRERESISTANCE: this.fireResistance -= moduleToRemove.GetComponent<FireResistance>().GetModifier(); break;
            }
        }
        
        // Add the buff from the applied module to the tower
        switch (module.GetComponent<DragDrop>().moduleType)
        {
            case ModuleType.HEALTH: this.health += module.GetComponent<Health>().GetModifier(); this.maxHealth += module.GetComponent<Health>().GetModifier(); break;
            case ModuleType.PHYSICALDAMAGE: this.physicalDamage += module.GetComponent<PhysicalDamage>().GetModifier(); break;
            case ModuleType.FIREDAMAGE: this.fireDamage += module.GetComponent<FireDamage>().GetModifier(); break;
            case ModuleType.FIRERATE: this.fireRate += module.GetComponent<FireRate>().GetModifier(); break;
            case ModuleType.PHYSICALRESISTANCE: this.physicalResistance += module.GetComponent<PhysicalResistance>().GetModifier(); break;
            case ModuleType.FIRERESISTANCE: this.fireResistance += module.GetComponent<FireResistance>().GetModifier(); break;
        }

        modules[moduleSlot] = module.name;

        yield return null;
    }

    private void OnRoundStart(object sender, EventArgs e)
    {
        Invoke("FireProjectile", 1.0f);
    }

    private void OnRoundEnd(object sender, EventArgs e)
    {
        CancelInvoke("FireProjectile");

        int repairAmount = (int)(0.15 * maxHealth);

        if (health + repairAmount > maxHealth) health = maxHealth;
        else health += repairAmount;

        if (inventoryMenu.bIsVisible && inventoryMenu.tower) inventoryMenu.ShowMenuWithTowerStats(inventoryMenu.tower);
    }

    private void FireProjectile()
    {
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(GameAssets.Instance.towerFire);
        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawn.transform.position, Quaternion.identity, GameObject.Find("GameHandler/Background/PlayArea/Projectiles").transform);
        spawnedProjectile.GetComponent<Projectile>().SetProperties(projectileSpeed, physicalDamage, fireDamage, gameObject.transform.localScale);
        Invoke("FireProjectile", 200.0f / fireRate);
    }

    public void TakeDamage(float incomingPhysicalDamage, float incomingFireDamage)
    {
        float totalIncomingDamage = (incomingPhysicalDamage - physicalResistance) + (incomingFireDamage - fireResistance);
        if (totalIncomingDamage <= 0)
        {
            health--;
        }
        else
        {
            health -= totalIncomingDamage;
        }

        if (inventoryMenu.bIsVisible) inventoryMenu.ShowMenuWithTowerStats(this);
        if (health <= 0)
        {
            if (inventoryMenu.bIsVisible) inventoryMenu.HideMenu();
            placement.bIsPlacementOccupied = false;
            placement.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            GameHandler.Instance.DecreaseTowerCost();
            audioSource.PlayOneShot(GameAssets.Instance.towerDeath);
            roundSpawning.OnRoundStart -= OnRoundStart;
            roundSpawning.OnRoundEnd -= OnRoundEnd;
            GameHandler.ApplyGameOver -= GameOver;
            Destroy(gameObject);
        }
        else audioSource.PlayOneShot(GameAssets.Instance.towerHurt);
    }

    #region Moving Position
    public void ChangePlacement(Placement placement)
    {
        if (this.placement != null)
        {
            this.placement.bIsPlacementOccupied = false;
            this.placement = placement;
            moved = true;
        }
        else
        {
            this.placement = placement;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!bGameOver)
        {
            if (!GameHandler.Instance.bIsBuyingTower)
            {
                manager.NewTowerSelected(this);
                placement.GetComponent<Image>().color = new Color(0f, 255f, 0f, 0.3f);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!bGameOver)
        {
            if (!GameHandler.Instance.TryMoveTower(false))
            {
                eventData.pointerDrag = null;
                return;
            }
            GetComponent<Collider2D>().enabled = false;
            eventData.eligibleForClick = false;
            GameHandler.Instance.inventoryMenu.HideMenu();
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
            CancelInvoke("FireProjectile");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!bGameOver)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!bGameOver)
        {
            if (!moved)
            {
                rectTransform.anchoredPosition = startPosition;
            }
            else
            {
                startPosition = rectTransform.anchoredPosition;
            }
            GetComponent<Collider2D>().enabled = true;
            moved = false;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            if (roundSpawning.bInRound)
            {
                Invoke("FireProjectile", 1.0f);
            }
        }
    }
    #endregion
}
