using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class RoundSpawning : MonoBehaviour
{
    #region Variables
    // Private variables only changeable through script
    public List<GameObject> spawnerList = new List<GameObject>();
    public List<GameObject> activeSpawners = new List<GameObject>();
    [SerializeField] private Button startRoundButton;
    [SerializeField] private TextMeshProUGUI startRoundButtonText;

    public event EventHandler OnRoundStart;
    public event EventHandler OnRoundEnd;
    public event Action<int> OnEnemyStopSpawning;

    // Public variables
    public bool bInRound = false;
    public int roundBudget = 10;
    public int round = 1;

    private int amountToActivate = 2;
    private int amountCompleted = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Set References
        startRoundButton.interactable = true;
        startRoundButtonText.text = "Start round " + round.ToString();
        StartCoroutine(ActivateSpawners());
    }

    private IEnumerator ActivateSpawners()
    {
        if (round == 1) yield return new WaitForSeconds(0.1f);

        foreach (GameObject spawner in spawnerList)
        {
            spawner.GetComponent<EnemySpawner>().bIsActive = false;
        }

        int active = 0;
        while (active < amountToActivate)
        {
            EnemySpawner spawner = spawnerList[Random.Range(0, spawnerList.Count)].GetComponent<EnemySpawner>();
            if (!spawner.bIsActive)
            {
                spawner.ActivateSpawner();
                active++;
            }
        }
        yield return null;
    }

    public void StartRound()
    {
        bInRound = true;
        startRoundButton.interactable = false;
        OnRoundStart?.Invoke(this, EventArgs.Empty);
    }

    private void RoundComplete()
    {
        if (!GameHandler.Instance.bIsGameOver)
        {
            OnRoundEnd?.Invoke(this, EventArgs.Empty);
            bInRound = false;
            amountCompleted = 0;
            round++;
            roundBudget = (int)(roundBudget * 1.1);

            switch (round)
            {
                case 5: amountToActivate = 4; break;
                case 10: amountToActivate = 6; break;
                case 15: OnEnemyStopSpawning?.Invoke(GameAssets.Instance.corporal.GetComponent<Enemy>().EnemyStrength); break;
                case 25: OnEnemyStopSpawning?.Invoke(GameAssets.Instance.sergeant.GetComponent<Enemy>().EnemyStrength); amountToActivate = 8; break;
                case 40: OnEnemyStopSpawning?.Invoke(GameAssets.Instance.lieutenant.GetComponent<Enemy>().EnemyStrength); break;
                default: Debug.Log("This is the default switch"); break;
            }

            StartCoroutine(ActivateSpawners());

            startRoundButton.interactable = true;
            startRoundButtonText.text = "Start round " + round.ToString();
        }
    }

    public void CheckRoundCompletion()
    {
        amountCompleted++;

        if (amountCompleted >= amountToActivate)
        {
            RoundComplete();
        }
    }

    public void ActivateRoundCheat()
    {
        if (!GameHandler.Instance.bIsGameOver)
        {
            bInRound = false;
            amountCompleted = 0;
            round++;
            roundBudget = (int)(roundBudget * 1.1);

            switch (round)
            {
                case 5: amountToActivate = 4; break;
                case 10: amountToActivate = 6; break;
                case 15: OnEnemyStopSpawning?.Invoke(GameAssets.Instance.corporal.GetComponent<Enemy>().EnemyStrength); break;
                case 25: OnEnemyStopSpawning?.Invoke(GameAssets.Instance.sergeant.GetComponent<Enemy>().EnemyStrength); amountToActivate = 8; break;
                case 40: OnEnemyStopSpawning?.Invoke(GameAssets.Instance.lieutenant.GetComponent<Enemy>().EnemyStrength); break;
                default: ; break;
            }

            StartCoroutine(ActivateSpawners());

            startRoundButton.interactable = true;
            startRoundButtonText.text = "Start round " + round.ToString();
        }
    }
}
