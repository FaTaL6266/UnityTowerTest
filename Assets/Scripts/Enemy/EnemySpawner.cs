using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // Private variables that are changable in the editor

    // Private variables only changeable through script
    private bool bDoneSpawning;
    public bool bIsActive;
    private bool bFlashRow;
    private int round;
    private float spawnRate = 1.75f;
    private int roundBudget;
    private int lowestEnemyStrength;

    // Lists of enemies
    private List<GameObject> enemiesToSpawn = new List<GameObject>();
    private List<GameObject> enemiesSpawned = new List<GameObject>();

    // Enemy variation prefabs
    private GameObject soldier;
    private GameObject corporal;
    private GameObject sergeant;
    private GameObject lieutenant;
    private GameObject colonel;
    private GameObject general;
    private GameObject greatGeneral;
    private GameObject masterGeneral;

    // Reference variables
    private RoundSpawning roundSpawning;
    private Image flashArea;

    // Awake is called before Start()
    private void Awake()
    {
        this.soldier = GameAssets.Instance.soldier;
        this.corporal = GameAssets.Instance.corporal;
        this.sergeant = GameAssets.Instance.sergeant;
        this.lieutenant = GameAssets.Instance.lieutenant;
        this.colonel = GameAssets.Instance.colonel;
        this.general = GameAssets.Instance.general;
        this.greatGeneral = GameAssets.Instance.greatGeneral;
        this.masterGeneral = GameAssets.Instance.masterGeneral;
    }

    // Start is called before the first frame update
    void Start()
    {
        roundSpawning = GameHandler.Instance.transform.GetComponent<RoundSpawning>();
        roundSpawning.OnRoundStart += OnRoundStart;
        roundSpawning.spawnerList.Add(gameObject);

        flashArea = transform.Find("ActiveLane").GetComponent<Image>();

        lowestEnemyStrength = soldier.GetComponent<Enemy>().EnemyStrength;
    }

    private void Update()
    {
        if (bIsActive && bFlashRow)
        {
            flashArea.color = Color.Lerp(Color.clear, new Color(1, 0, 0, 0.5f), Mathf.PingPong(Time.time, 1));
        }
        else flashArea.color = Color.Lerp(flashArea.color, Color.clear, 2.5f);

    }

    public void ActivateSpawner()
    {
        bIsActive = true;
        PreloadRound();
    }

    private void OnRoundStart(object sender, System.EventArgs e)
    {
        if (bIsActive)
        {
            bDoneSpawning = false;
            bFlashRow = true;
            Invoke("CancelFlashing", 4.0f);
            Invoke("Spawn", 5.0f);
        }
    }

    private void CancelFlashing()
    {
        bFlashRow = false;
    }

    public void PreloadRound()
    {
        if (enemiesToSpawn.Count > 0)
        {
            enemiesToSpawn.Clear();
        }

        this.roundBudget = roundSpawning.roundBudget;
        this.round = roundSpawning.round;
        
        // Waterfall spawning method - stupid
        while (roundBudget >= lowestEnemyStrength)
        {
            // Soldier
            if (Random.Range(0, 101) > 15 && (roundBudget >= soldier.GetComponent<Enemy>().EnemyStrength))
            {
                //Debug.Log("Soldier: Decreasing budget by " + soldier.EnemyStrength.ToString());
                enemiesToSpawn.Add(soldier);
                roundBudget -= soldier.GetComponent<Enemy>().EnemyStrength;
            }
            else
            {
                // Corporal
                if (Random.Range(0, 101) > 40 && (roundBudget >= corporal.GetComponent<Enemy>().EnemyStrength) && round >= 2)
                {
                    //Debug.Log("Corporal: Decreasing budget by " + corporal.EnemyStrength.ToString());
                    enemiesToSpawn.Add(corporal);
                    roundBudget -= corporal.GetComponent<Enemy>().EnemyStrength;
                }
                else
                {
                    // Sergeant
                    if (Random.Range(0, 101) > 50 && (roundBudget >= sergeant.GetComponent<Enemy>().EnemyStrength) && round >= 4)
                    {
                        //Debug.Log("Sergeant: Decreasing budget by " + sergeant.GetComponent<Sergeant>().EnemyStrength.ToString());
                        enemiesToSpawn.Add(sergeant);
                        roundBudget -= sergeant.GetComponent<Enemy>().EnemyStrength;
                    }
                    else
                    {
                        // Lieutenant
                        if (Random.Range(0, 101) > 60 && (roundBudget >= lieutenant.GetComponent<Enemy>().EnemyStrength) && round >= 7)
                        {
                            //Debug.Log("Lieutenant: Decreasing budget by " + lieutenant.GetComponent<Lieutenant>().EnemyStrength.ToString());
                            enemiesToSpawn.Add(lieutenant);
                            roundBudget -= lieutenant.GetComponent<Enemy>().EnemyStrength;
                        }
                        else
                        {
                            // Colonel
                            if (Random.Range(0, 101) > 75 && (roundBudget >= colonel.GetComponent<Enemy>().EnemyStrength) && round >= 12)
                            {
                                //Debug.Log("Colonel: Decreasing budget by " + colonel.GetComponent<Colonel>().EnemyStrength.ToString());
                                enemiesToSpawn.Add(colonel);
                                roundBudget -= colonel.GetComponent<Enemy>().EnemyStrength;
                            }
                            else
                            {
                                // General
                                if (Random.Range(0, 101) > 90 && (roundBudget >= general.GetComponent<Enemy>().EnemyStrength) && round >= 14)
                                {
                                    //Debug.Log("General: Decreasing budget by " + general.GetComponent<General>().EnemyStrength.ToString());
                                    enemiesToSpawn.Add(general);
                                    roundBudget -= general.GetComponent<Enemy>().EnemyStrength;
                                }
                                else
                                {
                                    // Great General
                                    if (Random.Range(0, 101) > 95 && (roundBudget >= greatGeneral.GetComponent<Enemy>().EnemyStrength) && round >= 16)
                                    {
                                        //Debug.Log("Great_General: Decreasing budget by " + greatGeneral.GetComponent<Great_General>().EnemyStrength.ToString());
                                        enemiesToSpawn.Add(greatGeneral);
                                        roundBudget -= greatGeneral.GetComponent<Enemy>().EnemyStrength;
                                    }
                                    else
                                    {
                                        // Master General
                                        if (Random.Range(0, 101) > 99 && (roundBudget >= masterGeneral.GetComponent<Enemy>().EnemyStrength) && round >= 20)
                                        {
                                            //Debug.Log("Master_General: Decreasing budget by " + masterGeneral.GetComponent<Master_General>().EnemyStrength.ToString());
                                            enemiesToSpawn.Add(masterGeneral);
                                            roundBudget -= masterGeneral.GetComponent<Enemy>().EnemyStrength;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    void Spawn()
    {
        enemiesSpawned.Add(Instantiate(enemiesToSpawn[0], this.transform));
        enemiesToSpawn.RemoveAt(0);
        if (enemiesToSpawn.Count > 0)
        {
            Invoke("Spawn", spawnRate);
        }
        else
        {
            bDoneSpawning = true;
        }
    }

    public void RemoveSpawnedEnemy(GameObject enemyToRemove)
    {
        enemiesSpawned.Remove(enemyToRemove);

        if (bDoneSpawning && enemiesSpawned.Count <= 0)
        {
            roundSpawning.CheckRoundCompletion();
            bIsActive = false;
        }
    }



}
