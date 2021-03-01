using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class ESGameManager : MonoBehaviour
{
    public static ESGameManager _instance;
    public float enemySpawnDelay = 1.5f;
    public float cameraShakeMagnitute = 2.5f;
    public float cameraShakeRoughness = 2.5f;

    //Variables For Firebase Remote Config
    public float enemyHpFloatMultiplier = 0f;
    public float enemyHpFactorMultiplier = 1f;
    float enemyHpCurrentMultiplier = 1f;
    float enemyHpTempFloatMultiplier;
    List<float> enemyHpMultipliers;

    public float enemyXpFloatMultiplier = 0f;
    public float enemyXpFactorMultiplier = 1f;
    float enemyXpCurrentMultiplier = 1f;
    float enemyXpTempFloatMultiplier;
    List<float> enemyXpMultipliers;

    public float enemyCoinFloatMultiplier = 0f;
    public float enemyCoinFactorMultiplier = 1f;
    float enemyCoinCurrentMultiplier = 1f;
    float enemyCoinTempFloatMultiplier;
    List<int> enemyCoinMultipliers;

    public GameObject fireOrbProjectile;
    public GameObject waterOrbProjectile;
    public GameObject airOrbProjectile;
    public GameObject earthOrbProjectile;
    public GameObject voidOrbProjectile;
    public GameObject poisonOrbProjectile;
    public GameObject lightningOrbProjectile;

    public GameObject orbUsingPosition;

    public List<GameObject> enemies;
    [HideInInspector] public GameObject enemyPosition;

    [HideInInspector] public List<GameObject> fireOrbProjectileList;
    [HideInInspector] public List<GameObject> waterOrbProjectileList;
    [HideInInspector] public List<GameObject> airOrbProjectileList;
    [HideInInspector] public List<GameObject> earthOrbProjectileList;
    [HideInInspector] public List<GameObject> voidOrbProjectileList;
    [HideInInspector] public List<GameObject> poisonOrbProjectileList;
    [HideInInspector] public List<GameObject> lightningOrbProjectileList;
    [HideInInspector] public int currentEnemyIndex;
    [HideInInspector] public GameObject activeEnemy;

    [HideInInspector] public List<GameObject> enemyPhaseActiveOrbs;

    public GameObject[] projectilePathsPrefabList;
    [HideInInspector] public int totalOrbCount;
    [HideInInspector] public bool anyOrbLeft = true;

    bool canThrow = false;
    bool canVibrate = true;
    List<int> randomIndexArray = new List<int>();

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
        //FetchDataFromFirebaseObject();

        SetEnemyHpMultiplierList();
        SetEnemyXpMultiplierList();
        SetEnemyCoinMultiplierList();

        SetOrbCountPrefs();
        OrbRecord();
        InitializeEnemy();

        StartCoroutine(UseOrbAgaintsEnemy());

    } // Start()

    void Update()
    {
        //SetNewEnemyToOrbs();

        if (totalOrbCount <= 0)
        {
            anyOrbLeft = false;
        }
        else
        {
            anyOrbLeft = true;
        }

    } // Update()

    void FetchDataFromFirebaseObject()
    {
        enemyHpFloatMultiplier = FirebaseDataHolder._instance.EnemyHpFloatAmountToAdd;
        enemyHpFactorMultiplier = FirebaseDataHolder._instance.EnemyHpMultiplyValue;
        enemyXpFloatMultiplier = FirebaseDataHolder._instance.EnemyXpFloatAmountToAdd;
        enemyXpFactorMultiplier = FirebaseDataHolder._instance.EnemyXpMultiplyValue;
        enemyCoinFloatMultiplier = FirebaseDataHolder._instance.EnemyCoinIntAmountToAdd;
        enemyCoinFactorMultiplier = FirebaseDataHolder._instance.EnemyCoinMultiplyValue;

    } // FetchDataFromFirebaseObject()

    void SetEnemyHpMultiplierList()
    {
        if (!PlayerPrefs.HasKey("EnemyHpTempFloatMultiplier"))
        {
            enemyHpTempFloatMultiplier = enemyHpFloatMultiplier;
            PlayerPrefs.SetFloat("EnemyHpTempFloatMultiplier", enemyHpTempFloatMultiplier);
            enemyHpMultipliers = new List<float>();

            for (int i = 0; i < enemies.Count; i++)
            {
                enemyHpCurrentMultiplier = enemyHpTempFloatMultiplier * enemyHpFactorMultiplier;
                enemyHpMultipliers.Add(Mathf.Round(enemyHpCurrentMultiplier));
                enemyHpTempFloatMultiplier = enemyHpCurrentMultiplier;
            }
        }
        else
        {
            if (PlayerPrefs.GetFloat("EnemyHpTempFloatMultiplier") != enemyHpFloatMultiplier)
            {
                enemyHpTempFloatMultiplier = enemyHpFloatMultiplier;
                PlayerPrefs.SetFloat("EnemyHpTempFloatMultiplier", enemyHpTempFloatMultiplier);
                enemyHpMultipliers = new List<float>();

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemyHpCurrentMultiplier = enemyHpTempFloatMultiplier * enemyHpFactorMultiplier;
                    enemyHpMultipliers.Add(Mathf.Round(enemyHpCurrentMultiplier));
                    enemyHpTempFloatMultiplier = enemyHpCurrentMultiplier;
                }
            }
            else
            {
                enemyHpTempFloatMultiplier = PlayerPrefs.GetFloat("EnemyHpTempFloatMultiplier");
                enemyHpMultipliers = new List<float>();

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemyHpCurrentMultiplier = enemyHpTempFloatMultiplier * enemyHpFactorMultiplier;
                    enemyHpMultipliers.Add(Mathf.Round(enemyHpCurrentMultiplier));
                    enemyHpTempFloatMultiplier = enemyHpCurrentMultiplier;
                }
            }
        }

    } // SetEnemyHpMultiplierList()

    void SetEnemyXpMultiplierList()
    {
        if (!PlayerPrefs.HasKey("EnemyXpTempFloatMultiplier"))
        {
            enemyXpTempFloatMultiplier = enemyXpFloatMultiplier;
            PlayerPrefs.SetFloat("EnemyXpTempFloatMultiplier", enemyXpTempFloatMultiplier);

            //diziyi oluştur
            enemyXpMultipliers = new List<float>();

            for (int i = 0; i < enemies.Count; i++)
            {
                enemyXpCurrentMultiplier = enemyXpTempFloatMultiplier * enemyXpFactorMultiplier;
                enemyXpMultipliers.Add(Mathf.Round(enemyXpCurrentMultiplier));
                enemyXpTempFloatMultiplier = enemyXpCurrentMultiplier;
            }
        }
        else
        {
            if (PlayerPrefs.GetFloat("EnemyXpTempFloatMultiplier") != enemyXpFloatMultiplier)
            {
                enemyXpTempFloatMultiplier = enemyXpFloatMultiplier;
                PlayerPrefs.SetFloat("EnemyXpTempFloatMultiplier", enemyXpTempFloatMultiplier);

                enemyXpMultipliers = new List<float>();

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemyXpCurrentMultiplier = enemyXpTempFloatMultiplier * enemyXpFactorMultiplier;
                    enemyXpMultipliers.Add(Mathf.Round(enemyXpCurrentMultiplier));
                    enemyXpTempFloatMultiplier = enemyXpCurrentMultiplier;
                }
            }
            else
            {
                enemyXpTempFloatMultiplier = PlayerPrefs.GetFloat("EnemyXpTempFloatMultiplier");
                enemyXpMultipliers = new List<float>();

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemyXpCurrentMultiplier = enemyXpTempFloatMultiplier * enemyXpFactorMultiplier;
                    enemyXpMultipliers.Add(Mathf.Round(enemyXpCurrentMultiplier));
                    enemyXpTempFloatMultiplier = enemyXpCurrentMultiplier;
                }
            }
        }

    } // SetEnemyXpMultiplierList()

    void SetEnemyCoinMultiplierList()
    {
        if (!PlayerPrefs.HasKey("EnemyCoinTempFloatMultiplier"))
        {
            enemyCoinTempFloatMultiplier = enemyCoinFloatMultiplier;
            PlayerPrefs.SetFloat("EnemyCoinTempFloatMultiplier", enemyCoinTempFloatMultiplier);
            enemyCoinMultipliers = new List<int>();

            for (int i = 0; i < enemies.Count; i++)
            {
                enemyCoinCurrentMultiplier = enemyCoinTempFloatMultiplier * enemyCoinFactorMultiplier;
                enemyCoinMultipliers.Add(Mathf.RoundToInt(enemyCoinCurrentMultiplier));
                enemyCoinTempFloatMultiplier = enemyCoinCurrentMultiplier;
            }
        }
        else
        {
            if (PlayerPrefs.GetFloat("EnemyCoinTempFloatMultiplier") != enemyCoinFloatMultiplier)
            {
                enemyCoinTempFloatMultiplier = enemyCoinFloatMultiplier;
                PlayerPrefs.SetFloat("EnemyCoinTempFloatMultiplier", enemyCoinTempFloatMultiplier);

                enemyCoinMultipliers = new List<int>();

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemyCoinCurrentMultiplier = enemyCoinTempFloatMultiplier * enemyCoinFactorMultiplier;
                    enemyCoinMultipliers.Add(Mathf.RoundToInt(enemyCoinCurrentMultiplier));
                    enemyCoinTempFloatMultiplier = enemyCoinCurrentMultiplier;
                }
            }
            else
            {
                enemyCoinTempFloatMultiplier = PlayerPrefs.GetFloat("EnemyCoinTempFloatMultiplier");
                enemyCoinMultipliers = new List<int>();

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemyCoinCurrentMultiplier = enemyCoinTempFloatMultiplier * enemyCoinFactorMultiplier;
                    enemyCoinMultipliers.Add(Mathf.RoundToInt(enemyCoinCurrentMultiplier));
                    enemyCoinTempFloatMultiplier = enemyCoinCurrentMultiplier;
                }
            }
        }

    } // SetEnemyCoinMultiplierList()

    private void OrbRecord()
    {
        if (PlayerPrefs.GetInt("fireOrbProjectileListCount") == 0 && PlayerPrefs.GetInt("waterOrbProjectileListCount") == 0 && PlayerPrefs.GetInt("airOrbProjectileListCount") == 0 && PlayerPrefs.GetInt("earthOrbProjectileListCount") == 0 && PlayerPrefs.GetInt("voidOrbProjectileListCount") == 0 && PlayerPrefs.GetInt("poisonOrbProjectileListCount") == 0 && PlayerPrefs.GetInt("lightningOrbProjectileListCount") == 0)
        {
            GetProjectilesFromStorage();
        }
        else
        {
            if (PlayerPrefs.GetInt("fireOrbProjectileListCount") != 0)
            {
                for (int i = 0; i < PlayerPrefs.GetInt("fireOrbProjectileListCount"); i++)
                {
                    fireOrbProjectileList.Add(fireOrbProjectile);
                }
            }

            if (PlayerPrefs.GetInt("waterOrbProjectileListCount") != 0)
            {
                for (int i = 0; i < PlayerPrefs.GetInt("waterOrbProjectileListCount"); i++)
                {
                    waterOrbProjectileList.Add(waterOrbProjectile);
                }
            }

            if (PlayerPrefs.GetInt("airOrbProjectileListCount") != 0)
            {
                for (int i = 0; i < PlayerPrefs.GetInt("airOrbProjectileListCount"); i++)
                {
                    airOrbProjectileList.Add(airOrbProjectile);
                }
            }

            if (PlayerPrefs.GetInt("earthOrbProjectileListCount") != 0)
            {
                for (int i = 0; i < PlayerPrefs.GetInt("earthOrbProjectileListCount"); i++)
                {
                    earthOrbProjectileList.Add(earthOrbProjectile);
                }
               
            }

            if (PlayerPrefs.GetInt("voidOrbProjectileListCount") != 0)
            {
                for (int i = 0; i < PlayerPrefs.GetInt("voidOrbProjectileListCount"); i++)
                {
                    voidOrbProjectileList.Add(voidOrbProjectile);
                }
            }

            if (PlayerPrefs.GetInt("poisonOrbProjectileListCount") != 0)
            {
                for (int i = 0; i < PlayerPrefs.GetInt("poisonOrbProjectileListCount"); i++)
                {
                    poisonOrbProjectileList.Add(poisonOrbProjectile);
                }
            }

            if (PlayerPrefs.GetInt("lightningOrbProjectileListCount") != 0)
            {
                for (int i = 0; i < PlayerPrefs.GetInt("lightningOrbProjectileListCount"); i++)
                {
                    lightningOrbProjectileList.Add(lightningOrbProjectile);
                }
            }

            totalOrbCount = fireOrbProjectileList.Count + waterOrbProjectileList.Count + airOrbProjectileList.Count + earthOrbProjectileList.Count + voidOrbProjectileList.Count + poisonOrbProjectileList.Count + lightningOrbProjectileList.Count;
        }

    } // OrbRecord()

    private void SetOrbCountPrefs()
    {
        PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);

        if (!PlayerPrefs.HasKey("currentEnemyIndex"))
        {
            PlayerPrefs.SetInt("currentEnemyIndex", 0);
            currentEnemyIndex = 0;
        }

        if (!PlayerPrefs.HasKey("fireOrbProjectileListCount"))
        {
            PlayerPrefs.SetInt("fireOrbProjectileListCount", 0);
        }

        if (!PlayerPrefs.HasKey("waterOrbProjectileListCount"))
        {
            PlayerPrefs.SetInt("waterOrbProjectileListCount", 0);
        }

        if (!PlayerPrefs.HasKey("airOrbProjectileListCount"))
        {
            PlayerPrefs.SetInt("airOrbProjectileListCount", 0);
        }

        if (!PlayerPrefs.HasKey("earthOrbProjectileListCount"))
        {
            PlayerPrefs.SetInt("earthOrbProjectileListCount", 0);
        }

        if (!PlayerPrefs.HasKey("voidOrbProjectileListCount"))
        {
            PlayerPrefs.SetInt("voidOrbProjectileListCount", 0);
        }

        if (!PlayerPrefs.HasKey("poisonOrbProjectileListCount"))
        {
            PlayerPrefs.SetInt("poisonOrbProjectileListCount", 0);
        }

        if (!PlayerPrefs.HasKey("lightningOrbProjectileListCount"))
        {
            PlayerPrefs.SetInt("lightningOrbProjectileListCount", 0);
        }

    }

    private void GetProjectilesFromStorage()
    {
        if (PlayerPrefs.HasKey("FireOrbStorageCount"))
        {
            int tmp = PlayerPrefs.GetInt("FireOrbStorageCount");
            for (int i = 0; i < tmp; i++)
            {
                fireOrbProjectileList.Add(fireOrbProjectile);
                PlayerPrefs.SetInt("fireOrbProjectileListCount", fireOrbProjectileList.Count);
            }
            if (PlayerPrefs.GetInt("FireOrbStorageCount")>0)
            {
                randomIndexArray.Add(0);
            }

        }
        else
        {
            //Debug.Log("There is no fireOrbStorage record");
        }

        if (PlayerPrefs.HasKey("WaterOrbStorageCount"))
        {
            int tmp = PlayerPrefs.GetInt("WaterOrbStorageCount");
            for (int i = 0; i < tmp; i++)
            {
                waterOrbProjectileList.Add(waterOrbProjectile);
                PlayerPrefs.SetInt("waterOrbProjectileListCount", waterOrbProjectileList.Count);
            }
            if (PlayerPrefs.GetInt("WaterOrbStorageCount") > 0)
            {
                randomIndexArray.Add(1);
            }

        }
        else
        {
            //Debug.Log("There is no waterOrbStorage record");
        }

        if (PlayerPrefs.HasKey("AirOrbStorageCount"))
        {
            int tmp = PlayerPrefs.GetInt("AirOrbStorageCount");
            for (int i = 0; i < tmp; i++)
            {
                airOrbProjectileList.Add(airOrbProjectile);
                PlayerPrefs.SetInt("airOrbProjectileListCount", airOrbProjectileList.Count);
            }
            if (PlayerPrefs.GetInt("AirOrbStorageCount") > 0)
            {
                randomIndexArray.Add(2);
            }
        }
        else
        {
            //Debug.Log("There is no airOrbStorage record");
        }

        if (PlayerPrefs.HasKey("EarthOrbStorageCount"))
        {
            int tmp = PlayerPrefs.GetInt("EarthOrbStorageCount");
            for (int i = 0; i < tmp; i++)
            {
                earthOrbProjectileList.Add(earthOrbProjectile);
                PlayerPrefs.SetInt("earthOrbProjectileListCount", earthOrbProjectileList.Count);
            }
            if (PlayerPrefs.GetInt("EarthOrbStorageCount") > 0)
            {
                randomIndexArray.Add(3);
            }
        }
        else
        {
            //Debug.Log("There is no earthOrbStorage record");
        }

        if (PlayerPrefs.HasKey("VoidOrbStorageCount"))
        {
            int tmp = PlayerPrefs.GetInt("VoidOrbStorageCount");
            for (int i = 0; i < tmp; i++)
            {
                voidOrbProjectileList.Add(voidOrbProjectile);
                PlayerPrefs.SetInt("voidOrbProjectileListCount", voidOrbProjectileList.Count);
            }
            if (PlayerPrefs.GetInt("VoidOrbStorageCount") > 0)
            {
                randomIndexArray.Add(4);
            }
        }
        else
        {
            //Debug.Log("There is no voidOrbStorage record");
        }

        if (PlayerPrefs.HasKey("PoisonOrbStorageCount"))
        {
            int tmp = PlayerPrefs.GetInt("PoisonOrbStorageCount");
            for (int i = 0; i < tmp; i++)
            {
                poisonOrbProjectileList.Add(poisonOrbProjectile);
                PlayerPrefs.SetInt("poisonOrbProjectileListCount", poisonOrbProjectileList.Count);
            }
            if (PlayerPrefs.GetInt("PoisonOrbStorageCount") > 0)
            {
                randomIndexArray.Add(5);
            }
        }
        else
        {
            //Debug.Log("There is no poisonOrbStorage record");
        }

        if (PlayerPrefs.HasKey("LightningOrbStorageCount"))
        {
            int tmp = PlayerPrefs.GetInt("LightningOrbStorageCount");
            for (int i = 0; i < tmp; i++)
            {
                lightningOrbProjectileList.Add(lightningOrbProjectile);
                PlayerPrefs.SetInt("lightningOrbProjectileListCount", lightningOrbProjectileList.Count);
            }
            if (PlayerPrefs.GetInt("LightningOrbStorageCount") > 0)
            {
                randomIndexArray.Add(6);
            }
        }
        else
        {
            //Debug.Log("There is no lightningOrbStorageCount record");
        }

        PlayerPrefs.SetInt("FireOrbStorageCount", 0);
        PlayerPrefs.SetInt("WaterOrbStorageCount", 0);
        PlayerPrefs.SetInt("AirOrbStorageCount", 0);
        PlayerPrefs.SetInt("EarthOrbStorageCount", 0);
        PlayerPrefs.SetInt("VoidOrbStorageCount", 0);
        PlayerPrefs.SetInt("PoisonOrbStorageCount", 0);
        PlayerPrefs.SetInt("LightningOrbStorageCount", 0);

        //Debug.Log("Fire Count : " + fireOrbProjectileList.Count);
        //Debug.Log("Water Count : " + waterOrbProjectileList.Count);
        //Debug.Log("Air Count : " + airOrbProjectileList.Count);
        //Debug.Log("Earth Count : " + earthOrbProjectileList.Count);
        //Debug.Log("Void Count : " + voidOrbProjectileList.Count);
        //Debug.Log("Poison Count : " + poisonOrbProjectileList.Count);
        //Debug.Log("Lightning Count : " + lightningOrbProjectileList.Count);

        totalOrbCount = fireOrbProjectileList.Count + waterOrbProjectileList.Count + airOrbProjectileList.Count + earthOrbProjectileList.Count + voidOrbProjectileList.Count + poisonOrbProjectileList.Count + lightningOrbProjectileList.Count;
    }

    void InitializeEnemy()
    {
        Vector3 enemyPositiont = enemyPosition.transform.position;
        enemyPositiont.z = -1f;

        if (PlayerPrefs.HasKey("currentEnemyIndex"))
        {
            activeEnemy = Instantiate(enemies[PlayerPrefs.GetInt("currentEnemyIndex")], enemyPositiont, Quaternion.identity);
            //Debug.Log("dizideki can değeri : " + PlayerPrefs.GetInt("currentEnemyIndex"));
            activeEnemy.GetComponent<Enemy>().hp += enemyHpMultipliers[PlayerPrefs.GetInt("currentEnemyIndex")];
            activeEnemy.GetComponent<Enemy>().maxHp += enemyHpMultipliers[PlayerPrefs.GetInt("currentEnemyIndex")];
            activeEnemy.GetComponent<Enemy>().EnemyXP += enemyXpMultipliers[PlayerPrefs.GetInt("currentEnemyIndex")];
            activeEnemy.GetComponent<Enemy>().earnCoin += enemyCoinMultipliers[PlayerPrefs.GetInt("currentEnemyIndex")];
        }       

    } // InitializeEnemy()

    public void GetEnemy()
    {
        StartCoroutine(SpawnNewEnemy());     

    } // GetEnemy()

    IEnumerator SpawnNewEnemy()
    {
        yield return new WaitForSeconds(enemySpawnDelay);

        Vector3 enemyPositiont = enemyPosition.transform.position;
        enemyPositiont.z = -1f;

        if (PlayerPrefs.HasKey("currentEnemyIndex"))
        {
            activeEnemy = Instantiate(enemies[PlayerPrefs.GetInt("currentEnemyIndex")], enemyPositiont, Quaternion.identity);
            activeEnemy.GetComponent<Enemy>().hp += enemyHpMultipliers[PlayerPrefs.GetInt("currentEnemyIndex")];
            activeEnemy.GetComponent<Enemy>().maxHp += enemyHpMultipliers[PlayerPrefs.GetInt("currentEnemyIndex")];
            activeEnemy.GetComponent<Enemy>().EnemyXP += enemyXpMultipliers[PlayerPrefs.GetInt("currentEnemyIndex")];
            activeEnemy.GetComponent<Enemy>().earnCoin += enemyCoinMultipliers[PlayerPrefs.GetInt("currentEnemyIndex")];
        }   

    } // SpawnNewEnemy()

    private void OnMouseDown()
    {
        canThrow = true;
    }

    private void OnMouseUp()
    {
        canThrow = false;
    }

    IEnumerator UseOrbAgaintsEnemy()
    {
        while (true)
        {
            int randListOrb = Random.Range(0, randomIndexArray.Count);
            yield return new WaitForSeconds(0.05f);

            if (canThrow && activeEnemy != null)
            {
                Vector2 tmpSpawnPos = orbUsingPosition.transform.position;

                if (fireOrbProjectileList.Count != 0 && randListOrb == randomIndexArray.IndexOf(0))
                {
                    GameObject tmpUsedOrbObj = Instantiate(fireOrbProjectileList[0], tmpSpawnPos, Quaternion.identity);
                    int tmpRndPathIndex = Random.Range(0, projectilePathsPrefabList.Length);
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().projectileState = OrbProjectile.ProjectileState.GoingToEnemy;
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().pathCreator = projectilePathsPrefabList[tmpRndPathIndex].GetComponent<PathCreator>();
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().levelEnemy = activeEnemy;
                    enemyPhaseActiveOrbs.Add(tmpUsedOrbObj);
                    fireOrbProjectileList.RemoveAt(0);
                    PlayerPrefs.SetInt("fireOrbProjectileListCount", fireOrbProjectileList.Count);
                    if (fireOrbProjectileList.Count == 0)
                    {
                        randomIndexArray.RemoveAt(randomIndexArray.IndexOf(0));
                    }
                    //Debug.Log("FireOrb: " + fireOrbProjectileList.Count);
                    totalOrbCount--;
                    int tmp1 = PlayerPrefs.GetInt("FireOrbStorageCount");
                    tmp1--;
                    PlayerPrefs.SetInt("FireOrbStorageCount", tmp1);
                    yield return new WaitForSeconds(0.1f);
                }

                if (waterOrbProjectileList.Count != 0 && randListOrb == randomIndexArray.IndexOf(1))
                {
                    GameObject tmpUsedOrbObj = Instantiate(waterOrbProjectileList[0], tmpSpawnPos, Quaternion.identity);
                    int tmpRndPathIndex = Random.Range(0, projectilePathsPrefabList.Length);
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().projectileState = OrbProjectile.ProjectileState.GoingToEnemy;
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().pathCreator = projectilePathsPrefabList[tmpRndPathIndex].GetComponent<PathCreator>();
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().levelEnemy = activeEnemy;
                    enemyPhaseActiveOrbs.Add(tmpUsedOrbObj);
                    waterOrbProjectileList.RemoveAt(0);
                    PlayerPrefs.SetInt("waterOrbProjectileListCount", waterOrbProjectileList.Count);
                    if (waterOrbProjectileList.Count == 0)
                    {
                        randomIndexArray.RemoveAt(randomIndexArray.IndexOf(1));
                    }
                    //Debug.Log("WaterOrb: " + waterOrbProjectileList.Count);
                    totalOrbCount--;
                    int tmp1 = PlayerPrefs.GetInt("WaterOrbStorageCount");
                    tmp1--;
                    PlayerPrefs.SetInt("WaterOrbStorageCount", tmp1);
                    yield return new WaitForSeconds(0.1f);

                }

                if (airOrbProjectileList.Count != 0 && randListOrb == randomIndexArray.IndexOf(2))
                {
                    GameObject tmpUsedOrbObj = Instantiate(airOrbProjectileList[0], tmpSpawnPos, Quaternion.identity);
                    int tmpRndPathIndex = Random.Range(0, projectilePathsPrefabList.Length);
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().projectileState = OrbProjectile.ProjectileState.GoingToEnemy;
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().pathCreator = projectilePathsPrefabList[tmpRndPathIndex].GetComponent<PathCreator>();
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().levelEnemy = activeEnemy;
                    enemyPhaseActiveOrbs.Add(tmpUsedOrbObj);
                    airOrbProjectileList.RemoveAt(0);
                    PlayerPrefs.SetInt("airOrbProjectileListCount", airOrbProjectileList.Count);
                    if (airOrbProjectileList.Count == 0)
                    {
                        randomIndexArray.RemoveAt(randomIndexArray.IndexOf(2));
                    }
                    //Debug.Log("AirOrb: " + airOrbProjectileList.Count);
                    totalOrbCount--;
                    int tmp1 = PlayerPrefs.GetInt("AirOrbStorageCount");
                    tmp1--;
                    PlayerPrefs.SetInt("AirOrbStorageCount", tmp1);
                    yield return new WaitForSeconds(0.1f);

                }

                if (earthOrbProjectileList.Count != 0 && randListOrb == randomIndexArray.IndexOf(3))
                {
                    GameObject tmpUsedOrbObj = Instantiate(earthOrbProjectileList[0], tmpSpawnPos, Quaternion.identity);
                    int tmpRndPathIndex = Random.Range(0, projectilePathsPrefabList.Length);
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().projectileState = OrbProjectile.ProjectileState.GoingToEnemy;
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().pathCreator = projectilePathsPrefabList[tmpRndPathIndex].GetComponent<PathCreator>();
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().levelEnemy = activeEnemy;
                    enemyPhaseActiveOrbs.Add(tmpUsedOrbObj);
                    earthOrbProjectileList.RemoveAt(0);
                    PlayerPrefs.SetInt("earthOrbProjectileListCount", earthOrbProjectileList.Count);
                    if (earthOrbProjectileList.Count == 0)
                    {
                        randomIndexArray.RemoveAt(randomIndexArray.IndexOf(3));
                    }
                    //Debug.Log("EarthOrb: " + earthOrbProjectileList.Count);
                    totalOrbCount--;
                    int tmp1 = PlayerPrefs.GetInt("EarthOrbStorageCount");
                    tmp1--;
                    PlayerPrefs.SetInt("EarthOrbStorageCount", tmp1);
                    yield return new WaitForSeconds(0.1f);

                }

                if (voidOrbProjectileList.Count != 0 && randListOrb == randomIndexArray.IndexOf(4))
                {
                    GameObject tmpUsedOrbObj = Instantiate(voidOrbProjectileList[0], tmpSpawnPos, Quaternion.identity);
                    int tmpRndPathIndex = Random.Range(0, projectilePathsPrefabList.Length);
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().projectileState = OrbProjectile.ProjectileState.GoingToEnemy;
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().pathCreator = projectilePathsPrefabList[tmpRndPathIndex].GetComponent<PathCreator>();
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().levelEnemy = activeEnemy;
                    enemyPhaseActiveOrbs.Add(tmpUsedOrbObj);
                    voidOrbProjectileList.RemoveAt(0);
                    PlayerPrefs.SetInt("voidOrbProjectileListCount", voidOrbProjectileList.Count);
                    if (voidOrbProjectileList.Count == 0)
                    {
                        randomIndexArray.RemoveAt(randomIndexArray.IndexOf(4));
                    }
                    //Debug.Log("VoidOrb: " + voidOrbProjectileList.Count);
                    totalOrbCount--;
                    int tmp1 = PlayerPrefs.GetInt("VoidOrbStorageCount");
                    tmp1--;
                    PlayerPrefs.SetInt("VoidOrbStorageCount", tmp1);
                    yield return new WaitForSeconds(0.1f);

                }

                if (poisonOrbProjectileList.Count != 0 && randListOrb == randomIndexArray.IndexOf(5))
                {
                    GameObject tmpUsedOrbObj = Instantiate(poisonOrbProjectileList[0], tmpSpawnPos, Quaternion.identity);
                    int tmpRndPathIndex = Random.Range(0, projectilePathsPrefabList.Length);
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().projectileState = OrbProjectile.ProjectileState.GoingToEnemy;
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().pathCreator = projectilePathsPrefabList[tmpRndPathIndex].GetComponent<PathCreator>();
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().levelEnemy = activeEnemy;
                    enemyPhaseActiveOrbs.Add(tmpUsedOrbObj);
                    poisonOrbProjectileList.RemoveAt(0);
                    PlayerPrefs.SetInt("poisonOrbProjectileListCount", poisonOrbProjectileList.Count);
                    if (poisonOrbProjectileList.Count == 0)
                    {
                        randomIndexArray.RemoveAt(randomIndexArray.IndexOf(5));
                    }
                    //Debug.Log("PoisonOrb: " + poisonOrbProjectileList.Count);
                    totalOrbCount--;
                    int tmp1 = PlayerPrefs.GetInt("PoisonOrbStorageCount");
                    tmp1--;
                    PlayerPrefs.SetInt("PoisonOrbStorageCount", tmp1);
                    yield return new WaitForSeconds(0.1f);

                }

                if (lightningOrbProjectileList.Count != 0 && randListOrb == randomIndexArray.IndexOf(6))
                {
                    GameObject tmpUsedOrbObj = Instantiate(lightningOrbProjectileList[0], tmpSpawnPos, Quaternion.identity);
                    int tmpRndPathIndex = Random.Range(0, projectilePathsPrefabList.Length);
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().projectileState = OrbProjectile.ProjectileState.GoingToEnemy;
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().pathCreator = projectilePathsPrefabList[tmpRndPathIndex].GetComponent<PathCreator>();
                    tmpUsedOrbObj.GetComponent<OrbProjectile>().levelEnemy = activeEnemy;
                    enemyPhaseActiveOrbs.Add(tmpUsedOrbObj);
                    lightningOrbProjectileList.RemoveAt(0);
                    PlayerPrefs.SetInt("lightningOrbProjectileListCount", lightningOrbProjectileList.Count);
                    if (lightningOrbProjectileList.Count == 0)
                    {
                        randomIndexArray.RemoveAt(randomIndexArray.IndexOf(6));
                    }
                    //Debug.Log("LightningOrb: " + lightningOrbProjectileList.Count);
                    totalOrbCount--;
                    int tmp1 = PlayerPrefs.GetInt("LightningOrbStorageCount");
                    tmp1--;
                    PlayerPrefs.SetInt("LightningOrbStorageCount", tmp1);
                    yield return new WaitForSeconds(0.1f);

                }
            }
        }

    } // UseOrbAgaintsEnemy()

    public bool GetVibrationSetting()
    {
        return canVibrate;
    }

    public void CollectedXP(float _enemyXP)
    {
        if (!PlayerPrefs.HasKey("CollectedXpAmount"))
        {
            PlayerPrefs.SetFloat("CollectedXpAmount", _enemyXP);
        }
        else
        {
            float tmp = PlayerPrefs.GetFloat("CollectedXpAmount");
            tmp += _enemyXP;
            PlayerPrefs.SetFloat("CollectedXpAmount", tmp);
        }

    } // CollectedXP()

    public void CollectedCoin(int _collectedCoin)
    {
        if (!PlayerPrefs.HasKey("CollectedCoinAmount"))
        {
            PlayerPrefs.SetInt("CollectedCoinAmount", _collectedCoin);
        }
        else
        {
            int tmp = PlayerPrefs.GetInt("CollectedCoinAmount");
            tmp += _collectedCoin;
            PlayerPrefs.SetInt("CollectedCoinAmount", tmp);
        }
    }

    public void ShakeCamera()
    {
        CameraShaker.Instance.ShakeOnce(cameraShakeMagnitute, cameraShakeRoughness, .1f, 1f);

    } // ShakeCamera()

} // class
