using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public bool isPositiveEventActive = true;
    public float slotCostHorMultiplier = 1f;
    public float slotCostVerMultiplier = 3f;
    bool canVibrate = true;
    [HideInInspector] public int coinAmount;
    public int initializeCoinAmount = 125;

    public GameObject mergePhaseObj;
    public GameObject positiveCrystalEncounterObj;
    [HideInInspector] public GameObject createdPosiCrystalEncounterObj;
    public float positiveCrystalSpawnMinTime = 10f;
    public float positiveCrystalSpawnMaxTime = 20f;
    public float positiveCrystalSpawnMinX = -2.25f;
    public float positiveCrystalSpawnMaxX = 2.25f;
    public float positiveCrystalSpawnY = 5.5f;

    public List<GameObject> slots;
    public List<GameObject> purchasableSlots;
    public GameObject[] fireCrystals;
    public GameObject[] waterCrystals;
    public GameObject[] airCrystals;
    public GameObject[] earthCrystals;
    public GameObject[] voidCrystals;
    public GameObject[] poisonCrystals;
    public GameObject[] lightningCrystals;
    public GameObject[] projectileList;

    [HideInInspector] public OrbClass heldOrbEndOfMergePhase;
    [HideInInspector] public GameObject heldCrystalHarvester;

    //updated game design variables
    public GameObject storageObj;
    public int HarvestedProjectileShownGap = 5;
    float totalDamageInStorage = 0f;
    float activeEnemyHp = 100f;
    public GameObject purchasableSlotPrefab;
    public GameObject slotPrefab;
    [HideInInspector] public Dictionary<Vector2, int> slotCostList;
    [HideInInspector] public List<Vector3> savedSlotPositionList;
    [HideInInspector] public List<OrbClass> savedSlotActiveOrbsList;
    [HideInInspector] public List<Vector3> savedPurchasableSlotPositionList;
    [HideInInspector] public List<int> savedPurchasableSlotCostsList;
    public string saveFileName = "Savefile.mbda";
    public OfflineTimeCalculator offlineControllerScript;
    public float firstEnemyHp;

    [HideInInspector] public float currentXpAmount;
    public float maxXpAmountToNextLevel = 100f;
    public float levelXpMultiplier = 1.5f;
    [HideInInspector] public int currentLevel = 1;
    [HideInInspector] public GameObject enemyReadyButtonVFXObj;

    public int fireCrystalPurchaseCost = 5;
    public int waterCrystalPurchaseCost = 10;
    public int airCrystalPurchaseCost = 15;
    public int earthCrystalPurchaseCost = 20;
    public int voidCrystalPurchaseCost = 25;
    public int poisonCrystalPurchaseCost = 30;
    public int lightningCrystalPurchaseCost = 25;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        PlayerPrefs.SetString("CurrentScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        //FetchDataFromFirebaseObject();

        heldOrbEndOfMergePhase = null;

        ControlVibrationSetting();

        if (isPositiveEventActive)
        {
            StartCoroutine(CreatePositiveFallingCrystal());
        }

        slotCostList = new Dictionary<Vector2, int>();
        savedSlotPositionList = new List<Vector3>();
        savedSlotActiveOrbsList = new List<OrbClass>();
        savedPurchasableSlotPositionList = new List<Vector3>();
        savedPurchasableSlotCostsList = new List<int>();

        FillxPositiveAxisSlotCostList();
        FillxNegativeAxisSlotCostList();
        FillExcludeSlotCostList();

        if (!CheckSaveFileExists())
        {
            AddCurrentSlotsToSaveData();
        }
        else
        {
            LoadGameData();
            AddCurrentSlotsToSaveData();
        }

        CheckCollectedCoinAndXp();
        UIManager._instance.SetCoinAmountTextTest(coinAmount);

        GetStorageDataFromPrefs();
        GetStorageDataFromEnemyScene();

        Debug.Log("Offline Seconds : " + offlineControllerScript.offlineSeconds);

    } // Start()

    void Update()
    {
        totalDamageInStorage = ProjectileStorage._instance.GetTotalDamageInStorage();

        if (PlayerPrefs.HasKey("ActiveEnemyHp"))
        {
            activeEnemyHp = PlayerPrefs.GetFloat("ActiveEnemyHp");           
        }

        if (totalDamageInStorage >= activeEnemyHp)
        {
            if (enemyReadyButtonVFXObj == null)
            {
                enemyReadyButtonVFXObj = VFXManager._instance.SpawnEnemyButtonReadyVFX(ProjectileStorage._instance.gameObject.transform);
            }
        }
        else
        {
            if (enemyReadyButtonVFXObj != null)
            {
                Destroy(enemyReadyButtonVFXObj);
                enemyReadyButtonVFXObj = null;
            }
        }

        //cheat
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseXpAmount(15f);
            IncreaseCoinAmount(50000);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            IncreaseXpAmount(1000f);
        }

    } // Update()

    void FetchDataFromFirebaseObject()
    {
        initializeCoinAmount = FirebaseDataHolder._instance.FirsStartUpCoinAmount;
        maxXpAmountToNextLevel = FirebaseDataHolder._instance.FirstMaxXpAmountToLevelUp;
        levelXpMultiplier = FirebaseDataHolder._instance.MaxXpAmountMultiplerValue;

        fireCrystalPurchaseCost = FirebaseDataHolder._instance.FireCrystalPurchaseCost;
        waterCrystalPurchaseCost = FirebaseDataHolder._instance.WaterCrystalPurchaseCost;
        airCrystalPurchaseCost = FirebaseDataHolder._instance.AirCrystalPurchaseCost;
        earthCrystalPurchaseCost = FirebaseDataHolder._instance.EarthCrystalPurchaseCost;
        voidCrystalPurchaseCost = FirebaseDataHolder._instance.VoidCrystalPurchaseCost;
        poisonCrystalPurchaseCost = FirebaseDataHolder._instance.PoisonCrystalPurchaseCost;
        lightningCrystalPurchaseCost = FirebaseDataHolder._instance.LightningCrystalPurchaseCost;

    } // FetchDataFromFirebaseObject()

    void AddCurrentSlotsToSaveData()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!savedSlotPositionList.Contains(slots[i].transform.position))
            {
                savedSlotPositionList.Add(slots[i].transform.position);
                savedSlotActiveOrbsList.Add(slots[i].GetComponent<Slot>().activeOrb);
            }
        }

        for (int i = 0; i < purchasableSlots.Count; i++)
        {
            if (!savedPurchasableSlotPositionList.Contains(purchasableSlots[i].transform.position))
            {
                savedPurchasableSlotPositionList.Add(purchasableSlots[i].transform.position);
                savedPurchasableSlotCostsList.Add(purchasableSlots[i].GetComponent<PurchasableSlot>().purchaseCost);
            }
        }

        SaveGameData();

    } // AddCurrentSlotsToSaveData()

    void ClearAllSlotGrid()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Destroy(slots[i]);
        }
        slots.Clear();

        for (int i = 0; i < purchasableSlots.Count; i++)
        {
            Destroy(purchasableSlots[i]);
        }
        purchasableSlots.Clear();

    } // ClearAllSlotGrid()

    void FillxPositiveAxisSlotCostList()
    {
        float multiplier = 0f;
        float innerMultiplier = 1f;

        for (int x = 2; x < 17; x++)
        {
            multiplier += slotCostHorMultiplier;

            for (float y = 0.5f; y < 3.5f; y += 1f)
            {
                if (y <= 0.5f)
                {
                    innerMultiplier = multiplier;
                }
                // + 
                Vector2 tmp = new Vector2(x, y);
                slotCostList.Add(tmp, Mathf.RoundToInt(100 * innerMultiplier));

                // -
                Vector2 tmp2 = new Vector2(x, -y);
                slotCostList.Add(tmp2, Mathf.RoundToInt(100 * innerMultiplier));

                innerMultiplier *= slotCostVerMultiplier;

                if (y >= 2.5f)
                {
                    innerMultiplier = multiplier;
                }

            } // for y

        } // for x

    } // FillxPositiveAxisSlotCostList()

    void FillxNegativeAxisSlotCostList()
    {
        float multiplier = 0f;
        float innerMultiplier = 1f;

        for (int x = 2; x < 17; x++)
        {
            multiplier += slotCostHorMultiplier;

            for (float y = 0.5f; y < 3.5f; y += 1f)
            {
                if (y <= 0.5f)
                {
                    innerMultiplier = multiplier;
                }

                // + 
                Vector2 tmp = new Vector2(-x, y);
                slotCostList.Add(tmp, Mathf.RoundToInt(100 * innerMultiplier));

                // -
                Vector2 tmp2 = new Vector2(-x, -y);
                slotCostList.Add(tmp2, Mathf.RoundToInt(100 * innerMultiplier));

                innerMultiplier *= slotCostVerMultiplier;

                if (y >= 2.5f)
                {
                    innerMultiplier = multiplier;
                }

            } // for y

        } // for x

    } // FillxNegativeAxisSlotCostList()

    void FillExcludeSlotCostList()
    {
        float multiplier = 0f;
        float innerMultiplier = 1f;

        for (int x = 0; x < 2; x++)
        {
            multiplier += slotCostHorMultiplier;

            for (float y = 1.5f; y < 3.5f; y += 1f)
            {
                if (y <= 1.5f)
                {
                    innerMultiplier = multiplier;
                }

                if (x == 0)
                {
                    Vector2 tmp1 = new Vector2(x, y);
                    slotCostList.Add(tmp1, Mathf.RoundToInt(100 * innerMultiplier));
                    Vector2 tmp2 = new Vector2(-x, -y);
                    slotCostList.Add(tmp2, Mathf.RoundToInt(100 * innerMultiplier));
                }
                else
                {
                    // + 
                    Vector2 tmp = new Vector2(x, y);
                    slotCostList.Add(tmp, Mathf.RoundToInt(100 * innerMultiplier));
                    Vector2 tmp2 = new Vector2(-x, y);
                    slotCostList.Add(tmp2, Mathf.RoundToInt(100 * innerMultiplier));

                    // -
                    Vector2 tmp3 = new Vector2(x, -y);
                    slotCostList.Add(tmp3, Mathf.RoundToInt(100 * innerMultiplier));
                    Vector2 tmp4 = new Vector2(-x, -y);
                    slotCostList.Add(tmp4, Mathf.RoundToInt(100 * innerMultiplier));
                }

                innerMultiplier *= slotCostVerMultiplier;

                if (y >= 2.5f)
                {
                    innerMultiplier = multiplier;
                }

            } // for y

        } // for x

    } // FillExcludeSlotCostList()

    public bool CheckEmptySlot()
    {
        bool emptySlotCheck = false;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetComponent<Slot>().slotType == Slot.SlotType.Empty)
            {
                emptySlotCheck = true;
                return emptySlotCheck;
            }
        }

        return emptySlotCheck;

    } // CheckEmptySlot()

    // mağazada ki crystal üzerine tıklandığında slot kontrolü ve satın alım gerçekleşiyor
    public bool AttachOrbToSlot(ShopCrystal.OrbType _orbType)
    {
        bool emptySlotCheck = false;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetComponent<Slot>().slotType == Slot.SlotType.Empty)
            {
                switch (_orbType)
                {
                    case ShopCrystal.OrbType.Fire:

                        Vector3 tmpPos1 = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, 0f);

                        GameObject purchasedCrystal = Instantiate(fireCrystals[0], tmpPos1, Quaternion.identity, mergePhaseObj.transform);
                        OrbClass purchasedCrystalClass = purchasedCrystal.GetComponent<OrbClass>();
                        purchasedCrystalClass.anim.SetTrigger("Merged");
                        slots[i].GetComponent<Slot>().activeOrb = purchasedCrystalClass;
                        slots[i].GetComponent<Slot>().slotType = Slot.SlotType.Active;

                        break;

                    case ShopCrystal.OrbType.Water:

                        Vector3 tmpPos2 = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, 0f);

                        GameObject purchasedCrystal2 = Instantiate(waterCrystals[0], tmpPos2, Quaternion.identity, mergePhaseObj.transform);
                        OrbClass purchasedCrystalClass2 = purchasedCrystal2.GetComponent<OrbClass>();
                        purchasedCrystalClass2.anim.SetTrigger("Merged");
                        slots[i].GetComponent<Slot>().activeOrb = purchasedCrystalClass2;
                        slots[i].GetComponent<Slot>().slotType = Slot.SlotType.Active;

                        break;

                    case ShopCrystal.OrbType.Air:

                        Vector3 tmpPos3 = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, 0f);

                        GameObject purchasedCrystal3 = Instantiate(airCrystals[0], tmpPos3, Quaternion.identity, mergePhaseObj.transform);
                        OrbClass purchasedCrystalClass3 = purchasedCrystal3.GetComponent<OrbClass>();
                        purchasedCrystalClass3.anim.SetTrigger("Merged");
                        slots[i].GetComponent<Slot>().activeOrb = purchasedCrystalClass3;
                        slots[i].GetComponent<Slot>().slotType = Slot.SlotType.Active;

                        break;

                    case ShopCrystal.OrbType.Earth:

                        Vector3 tmpPos4 = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, 0f);

                        GameObject purchasedCrystal4 = Instantiate(earthCrystals[0], tmpPos4, Quaternion.identity, mergePhaseObj.transform);
                        OrbClass purchasedCrystalClass4 = purchasedCrystal4.GetComponent<OrbClass>();
                        purchasedCrystalClass4.anim.SetTrigger("Merged");
                        slots[i].GetComponent<Slot>().activeOrb = purchasedCrystalClass4;
                        slots[i].GetComponent<Slot>().slotType = Slot.SlotType.Active;

                        break;

                    case ShopCrystal.OrbType.Void:

                        Vector3 tmpPos5 = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, 0f);

                        GameObject purchasedCrystal5 = Instantiate(voidCrystals[0], tmpPos5, Quaternion.identity, mergePhaseObj.transform);
                        OrbClass purchasedCrystalClass5 = purchasedCrystal5.GetComponent<OrbClass>();
                        purchasedCrystalClass5.anim.SetTrigger("Merged");
                        slots[i].GetComponent<Slot>().activeOrb = purchasedCrystalClass5;
                        slots[i].GetComponent<Slot>().slotType = Slot.SlotType.Active;

                        break;

                    case ShopCrystal.OrbType.Poison:

                        Vector3 tmpPos6 = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, 0f);

                        GameObject purchasedCrystal6 = Instantiate(poisonCrystals[0], tmpPos6, Quaternion.identity, mergePhaseObj.transform);
                        OrbClass purchasedCrystalClass6 = purchasedCrystal6.GetComponent<OrbClass>();
                        purchasedCrystalClass6.anim.SetTrigger("Merged");
                        slots[i].GetComponent<Slot>().activeOrb = purchasedCrystalClass6;
                        slots[i].GetComponent<Slot>().slotType = Slot.SlotType.Active;

                        break;

                    case ShopCrystal.OrbType.Lightning:

                        Vector3 tmpPos7 = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, 0f);

                        GameObject purchasedCrystal7 = Instantiate(lightningCrystals[0], tmpPos7, Quaternion.identity, mergePhaseObj.transform);
                        OrbClass purchasedCrystalClass7 = purchasedCrystal7.GetComponent<OrbClass>();
                        purchasedCrystalClass7.anim.SetTrigger("Merged");
                        slots[i].GetComponent<Slot>().activeOrb = purchasedCrystalClass7;
                        slots[i].GetComponent<Slot>().slotType = Slot.SlotType.Active;

                        break;
                    default:
                        break;

                } // switch orbType

                GameManager._instance.AddSlotDataToSave(slots[i].gameObject.transform.position, slots[i].GetComponent<Slot>().activeOrb); //Test Save

                //AudioManager._instance.PlaySFX(AudioManager._instance.crystalDropSFXindex);
                emptySlotCheck = true;
                return emptySlotCheck;

            } // empty slot check

        } // slots for

        return emptySlotCheck;

    } // AttachOrbToSlot()

    public void ChangeOrbLevelAfterMerge(int _orbLevel, OrbClass.OrbType _orbType, Transform _transform)
    {
        switch (_orbType)
        {
            case OrbClass.OrbType.Fire:

                Vector3 tmpPos1 = new Vector3(_transform.position.x, _transform.position.y, 0f);
                GameObject tmpObject1 = Instantiate(fireCrystals[_orbLevel - 1], tmpPos1, Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                OrbClass _activeOrb1 = tmpObject1.GetComponent<OrbClass>();
                _activeOrb1.previousPosition = _activeOrb1.transform.position;
                _transform.gameObject.GetComponent<Slot>().activeOrb = _activeOrb1;
                _transform.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                _activeOrb1.anim.SetTrigger("Merged");

                break;

            case OrbClass.OrbType.Water:

                Vector3 tmpPos2 = new Vector3(_transform.position.x, _transform.position.y, 0f);
                GameObject tmpObject2 = Instantiate(waterCrystals[_orbLevel - 1], tmpPos2, Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                OrbClass _activeOrb2 = tmpObject2.GetComponent<OrbClass>();
                _activeOrb2.previousPosition = _activeOrb2.transform.position;
                _transform.gameObject.GetComponent<Slot>().activeOrb = _activeOrb2;
                _transform.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                _activeOrb2.anim.SetTrigger("Merged");

                break;

            case OrbClass.OrbType.Air:

                Vector3 tmpPos3 = new Vector3(_transform.position.x, _transform.position.y, 0f);
                GameObject tmpObject3 = Instantiate(airCrystals[_orbLevel - 1], tmpPos3, Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                OrbClass _activeOrb3 = tmpObject3.GetComponent<OrbClass>();
                _activeOrb3.previousPosition = _activeOrb3.transform.position;
                _transform.gameObject.GetComponent<Slot>().activeOrb = _activeOrb3;
                _transform.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                _activeOrb3.anim.SetTrigger("Merged");

                break;

            case OrbClass.OrbType.Earth:

                Vector3 tmpPos4 = new Vector3(_transform.position.x, _transform.position.y, 0f);
                GameObject tmpObject4 = Instantiate(earthCrystals[_orbLevel - 1], tmpPos4, Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                OrbClass _activeOrb4 = tmpObject4.GetComponent<OrbClass>();
                _activeOrb4.previousPosition = _activeOrb4.transform.position;
                _transform.gameObject.GetComponent<Slot>().activeOrb = _activeOrb4;
                _transform.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                _activeOrb4.anim.SetTrigger("Merged");

                break;

            case OrbClass.OrbType.Void:

                Vector3 tmpPos5 = new Vector3(_transform.position.x, _transform.position.y, 0f);
                GameObject tmpObject5 = Instantiate(voidCrystals[_orbLevel - 1], tmpPos5, Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                OrbClass _activeOrb5 = tmpObject5.GetComponent<OrbClass>();
                _activeOrb5.previousPosition = _activeOrb5.transform.position;
                _transform.gameObject.GetComponent<Slot>().activeOrb = _activeOrb5;
                _transform.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                _activeOrb5.anim.SetTrigger("Merged");

                break;

            case OrbClass.OrbType.Poison:

                Vector3 tmpPos6 = new Vector3(_transform.position.x, _transform.position.y, 0f);
                GameObject tmpObject6 = Instantiate(poisonCrystals[_orbLevel - 1], tmpPos6, Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                OrbClass _activeOrb6 = tmpObject6.GetComponent<OrbClass>();
                _activeOrb6.previousPosition = _activeOrb6.transform.position;
                _transform.gameObject.GetComponent<Slot>().activeOrb = _activeOrb6;
                _transform.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;           
                _activeOrb6.anim.SetTrigger("Merged");

                break;

            case OrbClass.OrbType.Lightning:

                Vector3 tmpPos7 = new Vector3(_transform.position.x, _transform.position.y, 0f);
                GameObject tmpObject7 = Instantiate(lightningCrystals[_orbLevel - 1], tmpPos7, Quaternion.identity, GameManager._instance.mergePhaseObj.transform);
                OrbClass _activeOrb7 = tmpObject7.GetComponent<OrbClass>();
                _activeOrb7.previousPosition = _activeOrb7.transform.position;
                _transform.gameObject.GetComponent<Slot>().activeOrb = _activeOrb7;
                _transform.gameObject.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                _activeOrb7.anim.SetTrigger("Merged");

                break;

            default:
                break;

        } // switch orbType

        Vector3 tmpPoss = new Vector3(_transform.position.x, _transform.position.y, 0f);
        _transform.gameObject.GetComponent<Slot>().activeOrb.gameObject.transform.position = tmpPoss;
        AddSlotDataToSave(_transform.position, _transform.gameObject.GetComponent<Slot>().activeOrb); //Test Save

    } // ChangeOrbLevelAfterMerge()

    IEnumerator CreatePositiveFallingCrystal()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (createdPosiCrystalEncounterObj == null)
            {
                float tmpRandom = Random.Range(positiveCrystalSpawnMinTime, positiveCrystalSpawnMaxTime);
                yield return new WaitForSeconds(tmpRandom);

                GameObject createdPosObj = Instantiate(positiveCrystalEncounterObj, new Vector3(Random.Range(positiveCrystalSpawnMinX, positiveCrystalSpawnMaxX), positiveCrystalSpawnY, 0f), Quaternion.identity, mergePhaseObj.transform);
                createdPosiCrystalEncounterObj = createdPosObj;

            }
        }

    } // CreatePositiveFallingCrystal()

    void ControlVibrationSetting()
    {
        if (PlayerPrefs.HasKey("CanVibrate"))
        {
            string tmpString = PlayerPrefs.GetString("CanVibrate");

            if (tmpString == "True")
            {
                canVibrate = true;
            }
            else
            {
                canVibrate = false;
            }
        }
        else
        {
            canVibrate = true;
        }

    } // ControlVibrationSetting()

    public void SetVibrationSetting(bool _check)
    {
        if (_check)
        {
            canVibrate = true;
        }
        else
        {
            canVibrate = false;
        }

    } // SetVibrationSetting()

    public bool GetVibrationSetting()
    {
        return canVibrate;

    } // GetVibrationSetting()

    public bool PurchaseNewSlot(int purchaseCost)
    {
        if (coinAmount < purchaseCost)
        {
            return false;
        }
        else
        {
            AudioManager._instance.PlaySFX(AudioManager._instance.slotPurchasedindex);
            DecreaseCoinAmount(purchaseCost);
            return true;
        }

    } // PurchaseNewSlot()


    //    ***************************!!! SAVE SYSTEM !!!********************************

    public void AddSlotDataToSave(Vector3 _position, OrbClass _activeOrb)
    {
        if (savedSlotPositionList.Contains(_position))
        {
            for (int i = 0; i < savedSlotPositionList.Count; i++)
            {
                if (savedSlotPositionList[i] == _position)
                {
                    savedSlotActiveOrbsList[i] = _activeOrb;
                }
            }
        }
        else
        {
            savedSlotPositionList.Add(_position);
            savedSlotActiveOrbsList.Add(_activeOrb);
        }

        SaveGameData();

    } // AddSlotDataToSave()

    public void AddPurchasableSlotDataToSave(Vector3 _position, int _cost)
    {
        if (savedPurchasableSlotPositionList.Contains(_position))
        {
            //Debug.Log("zaten bu kordinatta satın alınabilir slot kaydı var, yenisi eklenmeyecek.");
        }
        else
        {
            savedPurchasableSlotPositionList.Add(_position);
            savedPurchasableSlotCostsList.Add(_cost);
        }

        SaveGameData();

    } // AddPurchasableSlotDataToSave()

    bool CheckSaveFileExists()
    {
        return File.Exists(Application.persistentDataPath + "/" + saveFileName);

    } // CheckSaveFileExists()

    public void SaveGameData()
    {
        SaveClass saveClassObject = new SaveClass();
        saveClassObject.SavedCrystalsLevelsList = new int[savedSlotPositionList.Count];
        saveClassObject.SavedCrystalsTypesList = new string[savedSlotPositionList.Count];

        for (int i = 0; i < savedSlotPositionList.Count; i++)
        {
            saveClassObject.SavedSlotPosXList.Add(savedSlotPositionList[i].x);
            saveClassObject.SavedSlotPosYList.Add(savedSlotPositionList[i].y);
            saveClassObject.SavedSlotPosZList.Add(savedSlotPositionList[i].z);

            if (savedSlotActiveOrbsList[i] != null)
            {
                saveClassObject.SavedCrystalsLevelsList[i] = savedSlotActiveOrbsList[i].orbLevel;
                saveClassObject.SavedCrystalsTypesList[i] = savedSlotActiveOrbsList[i].orbType.ToString();
            }
        }

        for (int i = 0; i < savedPurchasableSlotPositionList.Count; i++)
        {
            saveClassObject.SavedPurchasableSlotPosXList.Add(savedPurchasableSlotPositionList[i].x);
            saveClassObject.SavedPurchasableSlotPosYList.Add(savedPurchasableSlotPositionList[i].y);
            saveClassObject.SavedPurchasableSlotPosZList.Add(savedPurchasableSlotPositionList[i].z);
        }
        saveClassObject.SavedPurchasableSlotCostsList = savedPurchasableSlotCostsList;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + saveFileName);
        bf.Serialize(file, saveClassObject);
        file.Close();

    } // SaveGameData()

    public void LoadGameData()
    {
        if (CheckSaveFileExists())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveFileName, FileMode.Open);
            SaveClass loadClassObject = (SaveClass)bf.Deserialize(file);
            file.Close();

            ClearAllSlotGrid(); // temizleme testi

            for (int i = 0; i < loadClassObject.SavedSlotPosXList.Count; i++)
            {
                Vector3 tmpPos = new Vector3(loadClassObject.SavedSlotPosXList[i], loadClassObject.SavedSlotPosYList[i], loadClassObject.SavedSlotPosZList[i]);
                Vector3 tmpCrystalPos = new Vector3(tmpPos.x, tmpPos.y, 0f);
                GameObject tmpCreatedSlot = Instantiate(slotPrefab, tmpPos, Quaternion.identity, mergePhaseObj.transform);

                if (!string.IsNullOrEmpty(loadClassObject.SavedCrystalsTypesList[i]))
                {
                    switch (loadClassObject.SavedCrystalsTypesList[i])
                    {
                        case "Fire":
                            GameObject tmpCrystal1 = Instantiate(fireCrystals[loadClassObject.SavedCrystalsLevelsList[i] - 1], tmpCrystalPos, Quaternion.identity, mergePhaseObj.transform);
                            tmpCreatedSlot.GetComponent<Slot>().activeOrb = tmpCrystal1.GetComponent<OrbClass>();
                            tmpCreatedSlot.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                            tmpCrystal1.GetComponent<OrbClass>().createdFromSave = true;
                            break;

                        case "Water":
                            GameObject tmpCrystal2 = Instantiate(waterCrystals[loadClassObject.SavedCrystalsLevelsList[i] - 1], tmpCrystalPos, Quaternion.identity, mergePhaseObj.transform);
                            tmpCreatedSlot.GetComponent<Slot>().activeOrb = tmpCrystal2.GetComponent<OrbClass>();
                            tmpCreatedSlot.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                            tmpCrystal2.GetComponent<OrbClass>().createdFromSave = true;
                            break;

                        case "Air":
                            GameObject tmpCrystal3 = Instantiate(airCrystals[loadClassObject.SavedCrystalsLevelsList[i] - 1], tmpCrystalPos, Quaternion.identity, mergePhaseObj.transform);
                            tmpCreatedSlot.GetComponent<Slot>().activeOrb = tmpCrystal3.GetComponent<OrbClass>();
                            tmpCreatedSlot.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                            tmpCrystal3.GetComponent<OrbClass>().createdFromSave = true;
                            break;

                        case "Earth":
                            GameObject tmpCrystal4 = Instantiate(earthCrystals[loadClassObject.SavedCrystalsLevelsList[i] - 1], tmpCrystalPos, Quaternion.identity, mergePhaseObj.transform);
                            tmpCreatedSlot.GetComponent<Slot>().activeOrb = tmpCrystal4.GetComponent<OrbClass>();
                            tmpCreatedSlot.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                            tmpCrystal4.GetComponent<OrbClass>().createdFromSave = true;
                            break;

                        case "Void":
                            GameObject tmpCrystal5 = Instantiate(voidCrystals[loadClassObject.SavedCrystalsLevelsList[i] - 1], tmpCrystalPos, Quaternion.identity, mergePhaseObj.transform);
                            tmpCreatedSlot.GetComponent<Slot>().activeOrb = tmpCrystal5.GetComponent<OrbClass>();
                            tmpCreatedSlot.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                            tmpCrystal5.GetComponent<OrbClass>().createdFromSave = true;
                            break;

                        case "Poison":
                            GameObject tmpCrystal6 = Instantiate(poisonCrystals[loadClassObject.SavedCrystalsLevelsList[i] - 1], tmpCrystalPos, Quaternion.identity, mergePhaseObj.transform);
                            tmpCreatedSlot.GetComponent<Slot>().activeOrb = tmpCrystal6.GetComponent<OrbClass>();
                            tmpCreatedSlot.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                            tmpCrystal6.GetComponent<OrbClass>().createdFromSave = true;
                            break;

                        case "Lightning":
                            GameObject tmpCrystal7 = Instantiate(lightningCrystals[loadClassObject.SavedCrystalsLevelsList[i] - 1], tmpCrystalPos, Quaternion.identity, mergePhaseObj.transform);
                            tmpCreatedSlot.GetComponent<Slot>().activeOrb = tmpCrystal7.GetComponent<OrbClass>();
                            tmpCreatedSlot.GetComponent<Slot>().slotType = Slot.SlotType.Active;
                            tmpCrystal7.GetComponent<OrbClass>().createdFromSave = true;
                            break;

                        default:
                            break;

                    } // switch typeList

                } // activeOrb != null check

                slots.Add(tmpCreatedSlot);

                // ilk oluşturmada slotun altında, satın alınabilir slot varsa siliyorum
                Collider2D purchasableSlotCheckColl = Physics2D.OverlapBox(tmpPos, new Vector2(0.1f, 0.1f), 0f, 1 << LayerMask.NameToLayer("PurchasableSlot"));
                if (purchasableSlotCheckColl != null)
                {
                    purchasableSlots.Remove(purchasableSlotCheckColl.gameObject);
                    Destroy(purchasableSlotCheckColl.gameObject);
                }
            }

            for (int i = 0; i < loadClassObject.SavedPurchasableSlotPosXList.Count; i++)
            {
                Vector3 tmpPos1 = new Vector3(loadClassObject.SavedPurchasableSlotPosXList[i], loadClassObject.SavedPurchasableSlotPosYList[i], loadClassObject.SavedPurchasableSlotPosZList[i]);
                Collider2D slotCheckColl = Physics2D.OverlapBox(tmpPos1, new Vector2(0.1f, 0.1f), 0f, 1 << LayerMask.NameToLayer("Slot"));

                if (slotCheckColl == null)
                {
                    GameObject tmpCreatedPurchasableSlot = Instantiate(purchasableSlotPrefab, tmpPos1, Quaternion.identity, mergePhaseObj.transform);
                    tmpCreatedPurchasableSlot.GetComponent<PurchasableSlot>().purchaseCost = loadClassObject.SavedPurchasableSlotCostsList[i];
                    purchasableSlots.Add(tmpCreatedPurchasableSlot);
                }
            }
        }
        else
        {
            Debug.Log("Kayıt dosyası yok, normal açılış.");
        }

    } // LoadGameData()

    public void SetCoinAmount(int _amount)
    {
        coinAmount = _amount;
        UIManager._instance.SetCoinAmountTextTest(coinAmount);
        PlayerPrefs.SetInt("CurrentCoinAmount", coinAmount);

    } // SetCoinAmount()

    public void IncreaseCoinAmount(int _amount)
    {
        coinAmount += _amount;
        UIManager._instance.SetCoinAmountTextTest(coinAmount);
        PlayerPrefs.SetInt("CurrentCoinAmount", coinAmount);

    } // IncreaseCoinAmount()

    public void DecreaseCoinAmount(int _amount)
    {
        coinAmount -= _amount;
        UIManager._instance.SetCoinAmountTextTest(coinAmount);
        PlayerPrefs.SetInt("CurrentCoinAmount", coinAmount);

    } // DecreaseCoinAmount()

    public void TransferDataToEnemyScene()
    {
        PlayerPrefs.SetInt("FireOrbStorageCount", ProjectileStorage._instance.fireProjectiles.Count);
        PlayerPrefs.SetInt("WaterOrbStorageCount", ProjectileStorage._instance.waterProjectiles.Count);
        PlayerPrefs.SetInt("AirOrbStorageCount", ProjectileStorage._instance.airProjectiles.Count);
        PlayerPrefs.SetInt("EarthOrbStorageCount", ProjectileStorage._instance.earthProjectiles.Count);
        PlayerPrefs.SetInt("VoidOrbStorageCount", ProjectileStorage._instance.voidProjectiles.Count);
        PlayerPrefs.SetInt("PoisonOrbStorageCount", ProjectileStorage._instance.poisonProjectiles.Count);
        PlayerPrefs.SetInt("LightningOrbStorageCount", ProjectileStorage._instance.lightningProjectiles.Count);

        ProjectileStorage._instance.ClearAllStorage();

        offlineControllerScript.ChangeSceneToEnemyTrigger();

    } // TransferDataToEnemyScene()

    void GetStorageDataFromEnemyScene()
    {
        if (PlayerPrefs.HasKey("fireOrbProjectileListCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("fireOrbProjectileListCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Fire, projectileList[0]);
            }
        }

        if (PlayerPrefs.HasKey("waterOrbProjectileListCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("waterOrbProjectileListCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Water, projectileList[1]);
            }
        }

        if (PlayerPrefs.HasKey("airOrbProjectileListCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("airOrbProjectileListCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Air, projectileList[2]);
            }
        }

        if (PlayerPrefs.HasKey("earthOrbProjectileListCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("earthOrbProjectileListCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Earth, projectileList[3]);
            }
        }

        if (PlayerPrefs.HasKey("voidOrbProjectileListCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("voidOrbProjectileListCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Void, projectileList[4]);
            }
        }

        if (PlayerPrefs.HasKey("poisonOrbProjectileListCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("poisonOrbProjectileListCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Poison, projectileList[5]);
            }
        }

        if (PlayerPrefs.HasKey("lightningOrbProjectileListCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("lightningOrbProjectileListCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Lightning, projectileList[6]);
            }
        }


        PlayerPrefs.SetInt("fireOrbProjectileListCount", 0);
        PlayerPrefs.SetInt("waterOrbProjectileListCount", 0);
        PlayerPrefs.SetInt("airOrbProjectileListCount", 0);
        PlayerPrefs.SetInt("earthOrbProjectileListCount", 0);
        PlayerPrefs.SetInt("voidOrbProjectileListCount", 0);
        PlayerPrefs.SetInt("poisonOrbProjectileListCount", 0);
        PlayerPrefs.SetInt("lightningOrbProjectileListCount", 0);

        PlayerPrefs.SetInt("FireOrbStorageCount", ProjectileStorage._instance.fireProjectiles.Count);
        PlayerPrefs.SetInt("WaterOrbStorageCount", ProjectileStorage._instance.waterProjectiles.Count);
        PlayerPrefs.SetInt("AirOrbStorageCount", ProjectileStorage._instance.airProjectiles.Count);
        PlayerPrefs.SetInt("EarthOrbStorageCount", ProjectileStorage._instance.earthProjectiles.Count);
        PlayerPrefs.SetInt("VoidOrbStorageCount", ProjectileStorage._instance.voidProjectiles.Count);
        PlayerPrefs.SetInt("PoisonOrbStorageCount", ProjectileStorage._instance.poisonProjectiles.Count);
        PlayerPrefs.SetInt("LightningOrbStorageCount", ProjectileStorage._instance.lightningProjectiles.Count);

        UIManager._instance.SetFireOrbAmountText(ProjectileStorage._instance.fireProjectiles.Count);
        UIManager._instance.SetWaterOrbAmountText(ProjectileStorage._instance.waterProjectiles.Count);
        UIManager._instance.SetAirOrbAmountText(ProjectileStorage._instance.airProjectiles.Count);
        UIManager._instance.SetEarthOrbAmountText(ProjectileStorage._instance.earthProjectiles.Count);
        UIManager._instance.SetVoidOrbAmountText(ProjectileStorage._instance.voidProjectiles.Count);
        UIManager._instance.SetPoisonOrbAmountText(ProjectileStorage._instance.poisonProjectiles.Count);
        UIManager._instance.SetLightningOrbAmountText(ProjectileStorage._instance.lightningProjectiles.Count);

    } // GetStorageDataFromEnemyScene()

    void GetStorageDataFromPrefs()
    {
        if (PlayerPrefs.HasKey("FireOrbStorageCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("FireOrbStorageCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Fire, projectileList[0]);
            }
        }

        if (PlayerPrefs.HasKey("WaterOrbStorageCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("WaterOrbStorageCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Water, projectileList[1]);
            }
        }

        if (PlayerPrefs.HasKey("AirOrbStorageCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("AirOrbStorageCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Air, projectileList[2]);
            }
        }

        if (PlayerPrefs.HasKey("EarthOrbStorageCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("EarthOrbStorageCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Earth, projectileList[3]);
            }
        }

        if (PlayerPrefs.HasKey("VoidOrbStorageCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("VoidOrbStorageCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Void, projectileList[4]);
            }
        }

        if (PlayerPrefs.HasKey("PoisonOrbStorageCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("PoisonOrbStorageCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Poison, projectileList[5]);
            }
        }

        if (PlayerPrefs.HasKey("LightningOrbStorageCount"))
        {
            int tmp1 = PlayerPrefs.GetInt("LightningOrbStorageCount");

            for (int i = 0; i < tmp1; i++)
            {
                ProjectileStorage._instance.AddProjectileToStorageFromSaveData(OrbClass.OrbType.Lightning, projectileList[6]);
            }
        }

        UIManager._instance.SetFireOrbAmountText(ProjectileStorage._instance.fireProjectiles.Count);
        UIManager._instance.SetWaterOrbAmountText(ProjectileStorage._instance.waterProjectiles.Count);
        UIManager._instance.SetAirOrbAmountText(ProjectileStorage._instance.airProjectiles.Count);
        UIManager._instance.SetEarthOrbAmountText(ProjectileStorage._instance.earthProjectiles.Count);
        UIManager._instance.SetVoidOrbAmountText(ProjectileStorage._instance.voidProjectiles.Count);
        UIManager._instance.SetPoisonOrbAmountText(ProjectileStorage._instance.poisonProjectiles.Count);
        UIManager._instance.SetLightningOrbAmountText(ProjectileStorage._instance.lightningProjectiles.Count);

    } // GetStorageDataFromPrefs()

    void CheckCollectedCoinAndXp()
    {
        //Level
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            UIManager._instance.ControlLevelText(currentLevel);
        }
        else
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            UIManager._instance.ControlLevelText(currentLevel);
        }

        //Coin
        if (PlayerPrefs.HasKey("CurrentCoinAmount"))
        {
            SetCoinAmount(PlayerPrefs.GetInt("CurrentCoinAmount"));
        }
        else
        {
            SetCoinAmount(initializeCoinAmount);
        }

        if (PlayerPrefs.HasKey("CollectedCoinAmount"))
        {
            //Debug.Log("collected coin : " + PlayerPrefs.GetInt("CollectedCoinAmount"));
            IncreaseCoinAmount(PlayerPrefs.GetInt("CollectedCoinAmount"));
            PlayerPrefs.SetInt("CollectedCoinAmount", 0);
        }

        //Xp
        if (PlayerPrefs.HasKey("MaxXpAmountToLevelUp"))
        {
            maxXpAmountToNextLevel = PlayerPrefs.GetFloat("MaxXpAmountToLevelUp");
        }
        else
        {
            maxXpAmountToNextLevel = 100f;
            UIManager._instance.ControlXpBar(currentXpAmount, maxXpAmountToNextLevel);
            PlayerPrefs.SetFloat("MaxXpAmountToLevelUp", maxXpAmountToNextLevel);
        }

        if (PlayerPrefs.HasKey("CurrentXpAmount"))
        {
            SetXpAmount(PlayerPrefs.GetFloat("CurrentXpAmount"));
        }
        else
        {
            SetXpAmount(0f);
        }

        if (PlayerPrefs.HasKey("CollectedXpAmount"))
        {
            //Debug.Log("collected xp : " + PlayerPrefs.GetFloat("CollectedXpAmount"));
            IncreaseXpAmount(PlayerPrefs.GetFloat("CollectedXpAmount"));
            PlayerPrefs.SetFloat("CollectedXpAmount", 0);
        }

        CheckCrystalActivationAfterLevelUp();

    } // CheckCollectedCoinAndXp()

    public void IncreaseXpAmount(float _amount)
    {
        //xp artır level check

        currentXpAmount += _amount;

        if (currentXpAmount >= maxXpAmountToNextLevel)
        {
            while (currentXpAmount >= maxXpAmountToNextLevel)
            {
                currentXpAmount -= maxXpAmountToNextLevel;
                currentLevel++;

                maxXpAmountToNextLevel *= levelXpMultiplier;

                CheckCrystalActivationAfterLevelUp();
                PlayerPrefs.SetInt("CurrentLevel", currentLevel);
                PlayerPrefs.SetFloat("MaxXpAmountToLevelUp", maxXpAmountToNextLevel);
                UIManager._instance.ControlLevelText(currentLevel);
            }

            UIManager._instance.ControlXpBar(currentXpAmount, maxXpAmountToNextLevel);
            PlayerPrefs.SetFloat("CurrentXpAmount", currentXpAmount);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            UIManager._instance.ControlLevelText(currentLevel);
        }
        else
        {
            UIManager._instance.ControlXpBar(currentXpAmount, maxXpAmountToNextLevel);
            PlayerPrefs.SetFloat("CurrentXpAmount", currentXpAmount);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            UIManager._instance.ControlLevelText(currentLevel);
        }

        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        UIManager._instance.ControlLevelText(currentLevel);

    } // IncreaseXpAmount()

    void SetXpAmount(float _amount)
    {
        currentXpAmount = _amount;
        UIManager._instance.ControlXpBar(currentXpAmount, maxXpAmountToNextLevel);
        PlayerPrefs.SetFloat("CurrentXpAmount", currentXpAmount);

    } // SetXpAmount()

    void CheckCrystalActivationAfterLevelUp()
    {
        UIManager._instance.ActivateNextShopCrystal(currentLevel);

    } // CheckCrystalActivationAfterLevelUp()

} // class
