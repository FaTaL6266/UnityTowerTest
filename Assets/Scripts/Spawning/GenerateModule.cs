using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleRarity { COMMON, UNCOMMON, RARE, EXOTIC, LEGENDARY, DEFAULT };
public enum ModuleType { HEALTH, PHYSICALDAMAGE, FIREDAMAGE, FIRERATE, PHYSICALRESISTANCE, FIRERESISTANCE, DEFAULT }

public class GenerateModule : MonoBehaviour
{
    private List<GameObject> commonModules = new List<GameObject>();
    private List<GameObject> uncommonModules = new List<GameObject>();
    private List<GameObject> rareModules = new List<GameObject>();
    private List<GameObject> exoticModules = new List<GameObject>();
    private List<GameObject> legendaryModules = new List<GameObject>();

    private Transform playArea;

    private GameObject moduleToSpawn;
    private GameObject addedItem;

    void Awake()
    {
        commonModules = GameAssets.Instance.commonModules;
        uncommonModules = GameAssets.Instance.uncommonModules;
        rareModules = GameAssets.Instance.rareModules;
        exoticModules = GameAssets.Instance.exoticModules;
        legendaryModules = GameAssets.Instance.legendaryModules;

        playArea = GameObject.Find("GameHandler/Background/SpawnedModules").transform;
    }


    public void SpawnModule(ModuleRarity rarity, Vector3 position)
    {
        position = new Vector3(position.x + Random.Range(-5, 5), position.y + Random.Range(-5, 5), position.z);

        switch (rarity)
        {
            case ModuleRarity.COMMON:
                if (commonModules.Count > 0)    moduleToSpawn = commonModules[Random.Range(0, commonModules.Count)];       break;
            case ModuleRarity.UNCOMMON:
                if (uncommonModules.Count > 0)  moduleToSpawn = uncommonModules[Random.Range(0, uncommonModules.Count)];   break;
            case ModuleRarity.RARE:
                if (rareModules.Count > 0)      moduleToSpawn = rareModules[Random.Range(0, rareModules.Count)];           break;
            case ModuleRarity.EXOTIC:
                if (exoticModules.Count > 0)    moduleToSpawn = exoticModules[Random.Range(0, exoticModules.Count)];       break;
            case ModuleRarity.LEGENDARY:
                if (legendaryModules.Count > 0) moduleToSpawn = legendaryModules[Random.Range(0, legendaryModules.Count)]; break;
        }

        addedItem = Instantiate(moduleToSpawn, position, Quaternion.identity, playArea);
        addedItem.name = moduleToSpawn.name;
        addedItem.tag = "SpawnedOnEnemyDeath";


    }
}
