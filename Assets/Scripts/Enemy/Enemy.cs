using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    // Variables for storing probabilities of spawning a drop
    [SerializeField] private float common;
    [SerializeField] private float uncommon;
    [SerializeField] private float rare;
    [SerializeField] private float exotic;
    [SerializeField] private float legendary;

    // Enemy stat variables
    [SerializeField] private float enemySpeed;
    [SerializeField] private int enemyStrength;
    [SerializeField] private int enemyValue;
    [SerializeField] private float attackRate;
    [SerializeField] private float health;
    [SerializeField] private float physicalDamage;
    [SerializeField] private float fireDamage;
    [SerializeField] private float physicalResistance;
    [SerializeField] private float fireResistance;

    // Boolean variables for checking
    private bool bAlive;
    private bool bAttacking;
    private EnemySpawner parentSpawner;

    // Reference variables
    private GenerateModule moduleGeneration;
    private Animator animator;
    private Tower towerToAttack;
    private AudioSource audioSource;

    // Getter and Setter functions
    public EnemySpawner ParentSpawner { get => parentSpawner; set => parentSpawner = value; }
    public float EnemySpeed { get => enemySpeed; set => enemySpeed = value; }
    public int EnemyStrength { get => enemyStrength; set => enemyStrength = value; }
    public int EnemyValue { get => enemyValue; set => enemyValue = value; }
    public float AttackRate { get => attackRate; set => attackRate = value; }
    public float EnemyHealth { get => health; set => health = value; }
    public float EnemyPhysicalDamage { get => physicalDamage; set => physicalDamage = value; }
    public float EnemyFireDamage { get => fireDamage; set => fireDamage = value; }
    public float EnemyPhysicalDefense { get => physicalResistance; set => physicalResistance = value; }
    public float EnemyFireDefense { get => fireResistance; set => fireResistance = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameHandler.ApplyGameOver += GameOver;
        audioSource = GetComponent<AudioSource>();
        moduleGeneration = GameHandler.Instance.transform.GetComponent<GenerateModule>();
        animator = GetComponent<Animator>();
        ParentSpawner = this.transform.parent.gameObject.GetComponent<EnemySpawner>();
        bAlive = true;
        bAttacking = false;
    }

    private void GameOver()
    {
        bAlive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bAttacking && bAlive)
        {
            transform.Translate(Vector3.left * EnemySpeed * Time.deltaTime);
        }
    }

    #region Attack
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            towerToAttack = collision.gameObject.GetComponent<Tower>();
            bAttacking = true;
            InvokeRepeating("AttackTower", 1.5f, AttackRate);
        }
        else if (collision.gameObject.CompareTag("Generator"))
        {
            StartCoroutine(ReachedGenerator());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            CancelInvoke("AttackTower");
            bAttacking = false;
        }
    }

    public void AttackTower()
    {
        animator.SetTrigger("Attack");
        towerToAttack.TakeDamage(physicalDamage, fireDamage);
    }
    #endregion

    #region Damage and Death
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

        if (EnemyHealth <= 0)
        {
            StartCoroutine(DeathCoroutine());
        }
        else
        {
            audioSource.PlayOneShot(GameAssets.Instance.enemyHurt);
        }
    }

    IEnumerator DeathCoroutine()
    {
        bAlive = false;
        GetComponent<Collider2D>().enabled = false;
        GameHandler.Instance.IncreaseMoney(EnemyValue);
        Vector3 position = this.transform.position;

        // Probability of spawning a module when Soldier is destroyed
        float itemRarity = Random.Range(0.0f, 100.0f);
        if (itemRarity >= common && itemRarity < uncommon) { moduleGeneration.SpawnModule(ModuleRarity.COMMON, position); }
        else if (itemRarity >= uncommon && itemRarity < rare) { moduleGeneration.SpawnModule(ModuleRarity.UNCOMMON, position); }
        else if (itemRarity >= rare && itemRarity < exotic) { moduleGeneration.SpawnModule(ModuleRarity.RARE, position); }
        else if (itemRarity >= exotic && itemRarity < legendary) { moduleGeneration.SpawnModule(ModuleRarity.EXOTIC, position); }
        else if (itemRarity >= legendary) { moduleGeneration.SpawnModule(ModuleRarity.LEGENDARY, position); }

        audioSource.PlayOneShot(GameAssets.Instance.enemyDeath);

        if (animator)
        {
            animator.SetBool("bDead", true);
        }
        yield return new WaitForSeconds(1);

        ParentSpawner.RemoveSpawnedEnemy(gameObject);
        Destroy(gameObject);
    }

    public IEnumerator ReachedGenerator()
    {
        bAlive = false;
        GetComponent<Collider2D>().enabled = false;

        if (animator)
        {
            //animator.SetTrigger("SelfDestruct");
            yield return new WaitForSeconds(1);
        }

        yield return null;
        GameHandler.Instance.DecreaseLives(EnemyStrength);
        ParentSpawner.RemoveSpawnedEnemy(gameObject);
        Destroy(gameObject);
    }
    #endregion
}
