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
    [SerializeField] private float enemyHealth;
    [SerializeField] private float enemyPhysicalDamage;
    [SerializeField] private float enemyFireDamage;
    [SerializeField] private float enemyPhysicalDefense;
    [SerializeField] private float enemyFireDefense;

    // Boolean variables for checking
    private bool bAlive;
    private bool bAttacking;
    private EnemySpawner parentSpawner;

    // Reference variables
    private GenerateModule moduleGeneration;
    private Animator animator;
    private Tower towerToAttack;
    private Canvas canvas;

    // Getter and Setter functions
    public EnemySpawner ParentSpawner { get => parentSpawner; set => parentSpawner = value; }
    public float EnemySpeed { get => enemySpeed; set => enemySpeed = value; }
    public int EnemyStrength { get => enemyStrength; set => enemyStrength = value; }
    public int EnemyValue { get => enemyValue; set => enemyValue = value; }
    public float AttackRate { get => attackRate; set => attackRate = value; }
    public float EnemyHealth { get => enemyHealth; set => enemyHealth = value; }
    public float EnemyPhysicalDamage { get => enemyPhysicalDamage; set => enemyPhysicalDamage = value; }
    public float EnemyFireDamage { get => enemyFireDamage; set => enemyFireDamage = value; }
    public float EnemyPhysicalDefense { get => enemyPhysicalDefense; set => enemyPhysicalDefense = value; }
    public float EnemyFireDefense { get => enemyFireDefense; set => enemyFireDefense = value; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("GameHandler/Background").GetComponent<Canvas>();
        moduleGeneration = GameHandler.Instance.transform.GetComponent<GenerateModule>();
        animator = GetComponent<Animator>();
        ParentSpawner = this.transform.parent.gameObject.GetComponent<EnemySpawner>();
        bAlive = true;
        bAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bAttacking && bAlive)
        {
            transform.Translate(Vector3.left * EnemySpeed * 0.25f * Time.deltaTime);
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
        towerToAttack.TakeDamage(enemyPhysicalDamage, enemyFireDamage);
    }
    #endregion

    #region Damage and Death
    public void TakeDamage(float incomingPhysicalDamage, float incomingFireDamage)
    {
        EnemyHealth -= ((incomingPhysicalDamage - (incomingPhysicalDamage * EnemyPhysicalDefense)) + (incomingFireDamage - (incomingFireDamage * EnemyFireDefense)));

        if (EnemyHealth <= 0)
        {
            StartCoroutine(DeathCoroutine());
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

        if (animator)
        {
            animator.SetBool("bIsDead", true);
            yield return new WaitForSeconds(1);
        }
        else yield return null;

        ParentSpawner.RemoveSpawnedEnemy(gameObject);
        Destroy(gameObject);
    }

    public IEnumerator ReachedGenerator()
    {
        Debug.Log("Hitting generator");
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
