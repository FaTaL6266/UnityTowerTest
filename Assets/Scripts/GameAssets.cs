using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public Sprite[] appliedModuleAssets;
    public GameObject[] moduleAssets;

    private void Awake()
    {
        instance = this;
    }

    public static GameAssets Instance
    {
        get
        {
            if (instance == null) instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return instance;
        }
    }

    public Sprite GetAppliedModuleAsset(string moduleName)
    {
        for (int i = 0; i < GameHandler.Instance.slots.Length; i++)
        {
            if (moduleName == appliedModuleAssets[i].name)
            {
                return appliedModuleAssets[i];
            }
        }
        return null;
    }

    public GameObject GetModuleAsset(string moduleName)
    {
        for (int i = 0; i < GameHandler.Instance.slots.Length; i++)
        {
            if (moduleName == moduleAssets[i].name)
            {
                return moduleAssets[i];
            }
        }
        return null;
    }

    public EnemyLoadingScreenAssets[] enemyLoadings;

    [Serializable]
    public class EnemyLoadingScreenAssets
    {
        public Sprite enemyImage;
        public String enemyDescription;
    }


    // Audio clips
    public AudioClip enemyHurt;
    public AudioClip enemyDeath;
    public AudioClip itemPickup;
    public AudioClip towerHurt;
    public AudioClip towerDeath;
    public AudioClip towerFire;

    // Public referencable objects
    public GameObject projectile;
    public GameObject tower;
    public GameObject dummyTower;

    // Enemy references
    public GameObject soldier;
    public GameObject corporal;
    public GameObject sergeant;
    public GameObject lieutenant;
    public GameObject colonel;
    public GameObject general;
    public GameObject greatGeneral;
    public GameObject masterGeneral;

    // Module lists
    public List<GameObject> commonModules;
    public List<GameObject> uncommonModules;
    public List<GameObject> rareModules;
    public List<GameObject> exoticModules;
    public List<GameObject> legendaryModules;

    // Other stuff
    public Sprite defaultSlotSprite;
    public GameObject GameOver;




}
