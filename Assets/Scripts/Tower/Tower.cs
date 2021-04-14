using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    // Tower stat variables
    private float health = 50f;
    private float fireRate = 1.5f;
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

    // Module variables
    public string[] modules;

    // Reference variables
    private Transform projectileSpawn;
    private GameObject projectile;
    private Placement placement;
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
            InvokeRepeating("FireProjectile", 1.0f, fireRate);
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
                case ModuleType.HEALTH: this.health -= moduleToRemove.GetComponent<Health>().GetModifier(); break;
                case ModuleType.PHYSICALDAMAGE: this.physicalDamage -= moduleToRemove.GetComponent<PhysicalDamage>().GetModifier(); break;
                case ModuleType.FIREDAMAGE: this.fireDamage -= moduleToRemove.GetComponent<FireDamage>().GetModifier(); break;
                case ModuleType.FIRERATE: this.fireRate += moduleToRemove.GetComponent<FireRate>().GetModifier(); break;
                case ModuleType.PHYSICALRESISTANCE: this.physicalResistance -= moduleToRemove.GetComponent<PhysicalResistance>().GetModifier(); break;
                case ModuleType.FIRERESISTANCE: this.fireResistance -= moduleToRemove.GetComponent<FireResistance>().GetModifier(); break;
            }
        }
        
        // Add the buff from the applied module to the tower
        switch (module.GetComponent<DragDrop>().moduleType)
        {
            case ModuleType.HEALTH: this.health += module.GetComponent<Health>().GetModifier(); break;
            case ModuleType.PHYSICALDAMAGE: this.physicalDamage += module.GetComponent<PhysicalDamage>().GetModifier(); break;
            case ModuleType.FIREDAMAGE: this.fireDamage += module.GetComponent<FireDamage>().GetModifier(); break;
            case ModuleType.FIRERATE: this.fireRate -= module.GetComponent<FireRate>().GetModifier(); break;
            case ModuleType.PHYSICALRESISTANCE: this.physicalResistance += module.GetComponent<PhysicalResistance>().GetModifier(); break;
            case ModuleType.FIRERESISTANCE: this.fireResistance += module.GetComponent<FireResistance>().GetModifier(); break;
        }

        modules[moduleSlot] = module.name;

        yield return null;
    }

    private void OnRoundStart(object sender, EventArgs e)
    {
        InvokeRepeating("FireProjectile", 1.0f, fireRate);
    }

    private void OnRoundEnd(object sender, EventArgs e)
    {
        CancelInvoke("FireProjectile");
    }

    private void FireProjectile()
    {
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(GameAssets.Instance.towerFire);
        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawn.transform.position, Quaternion.identity, GameObject.Find("GameHandler/Background/PlayArea").transform);
        spawnedProjectile.GetComponent<Projectile>().SetProperties(projectileSpeed, physicalDamage, fireDamage, gameObject.transform.localScale);
    }

    public void TakeDamage(float incomingPhysicalDamage, float incomingFireDamage)
    {
        health -= ((incomingPhysicalDamage - (incomingPhysicalDamage * physicalResistance)) + (incomingFireDamage - (incomingFireDamage * fireResistance)));
        if (inventoryMenu.bIsVisible) inventoryMenu.ShowMenuWithTowerStats(this);
        if (health <= 0)
        {
            audioSource.PlayOneShot(GameAssets.Instance.towerDeath);
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
                InvokeRepeating("FireProjectile", 1.0f, fireRate);
            }
        }
    }
    #endregion
}
