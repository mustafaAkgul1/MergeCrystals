using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStorage : MonoBehaviour
{
    public static ProjectileStorage _instance;

    [HideInInspector] public List<GameObject> generalProjectileList;
    [HideInInspector] public List<GameObject> fireProjectiles;
    [HideInInspector] public List<GameObject> waterProjectiles;
    [HideInInspector] public List<GameObject> airProjectiles;
    [HideInInspector] public List<GameObject> earthProjectiles;
    [HideInInspector] public List<GameObject> voidProjectiles;
    [HideInInspector] public List<GameObject> poisonProjectiles;
    [HideInInspector] public List<GameObject> lightningProjectiles;

    void Awake()
    {
        _instance = this;

    } // Awake()

    public void AddProjectileToStorage(OrbClass.OrbType _projectileType, GameObject _projectileObj)
    {
        switch (_projectileType)
        {
            case OrbClass.OrbType.Fire:
                fireProjectiles.Add(_projectileObj);
                int tmp = 0;
                if (PlayerPrefs.HasKey("FireOrbStorageCount"))
                {
                    tmp = PlayerPrefs.GetInt("FireOrbStorageCount");
                    tmp++;
                    PlayerPrefs.SetInt("FireOrbStorageCount", tmp);
                }
                else
                {
                    PlayerPrefs.SetInt("FireOrbStorageCount", fireProjectiles.Count);
                }
                //Debug.Log("Fire count : " + PlayerPrefs.GetInt("FireOrbStorageCount"));
                generalProjectileList.Add(_projectileObj);
                UIManager._instance.SetFireOrbAmountText(fireProjectiles.Count);
                break;

            case OrbClass.OrbType.Water:
                waterProjectiles.Add(_projectileObj);
                int tmp2 = 0;
                if (PlayerPrefs.HasKey("WaterOrbStorageCount"))
                {
                    tmp2 = PlayerPrefs.GetInt("WaterOrbStorageCount");
                    tmp2++;
                    PlayerPrefs.SetInt("WaterOrbStorageCount", tmp2);
                }
                else
                {
                    PlayerPrefs.SetInt("WaterOrbStorageCount", waterProjectiles.Count);
                }
                //Debug.Log("Water count : " + waterProjectiles.Count);
                generalProjectileList.Add(_projectileObj);
                UIManager._instance.SetWaterOrbAmountText(waterProjectiles.Count);
                break;

            case OrbClass.OrbType.Air:
                airProjectiles.Add(_projectileObj);
                int tmp3 = 0;
                if (PlayerPrefs.HasKey("AirOrbStorageCount"))
                {
                    tmp3 = PlayerPrefs.GetInt("AirOrbStorageCount");
                    tmp3++;
                    PlayerPrefs.SetInt("AirOrbStorageCount", tmp3);
                }
                else
                {
                    PlayerPrefs.SetInt("AirOrbStorageCount", airProjectiles.Count);
                }
                //Debug.Log("Air count : " + airProjectiles.Count);
                generalProjectileList.Add(_projectileObj);
                UIManager._instance.SetAirOrbAmountText(airProjectiles.Count);
                break;

            case OrbClass.OrbType.Earth:
                earthProjectiles.Add(_projectileObj);
                int tmp4 = 0;
                if (PlayerPrefs.HasKey("EarthOrbStorageCount"))
                {
                    tmp4 = PlayerPrefs.GetInt("EarthOrbStorageCount");
                    tmp4++;
                    PlayerPrefs.SetInt("EarthOrbStorageCount", tmp4);
                }
                else
                {
                    PlayerPrefs.SetInt("EarthOrbStorageCount", earthProjectiles.Count);
                }
                //Debug.Log("Earth count : " + earthProjectiles.Count);
                generalProjectileList.Add(_projectileObj);
                UIManager._instance.SetEarthOrbAmountText(earthProjectiles.Count);
                break;

            case OrbClass.OrbType.Void:
                voidProjectiles.Add(_projectileObj);
                int tmp5 = 0;
                if (PlayerPrefs.HasKey("VoidOrbStorageCount"))
                {
                    tmp5 = PlayerPrefs.GetInt("VoidOrbStorageCount");
                    tmp5++;
                    PlayerPrefs.SetInt("VoidOrbStorageCount", tmp5);
                }
                else
                {
                    PlayerPrefs.SetInt("VoidOrbStorageCount", voidProjectiles.Count);
                }
                //Debug.Log("Void count : " + voidProjectiles.Count);
                generalProjectileList.Add(_projectileObj);
                UIManager._instance.SetVoidOrbAmountText(voidProjectiles.Count);
                break;

            case OrbClass.OrbType.Poison:
                poisonProjectiles.Add(_projectileObj);
                int tmp6 = 0;
                if (PlayerPrefs.HasKey("PoisonOrbStorageCount"))
                {
                    tmp6 = PlayerPrefs.GetInt("PoisonOrbStorageCount");
                    tmp6++;
                    PlayerPrefs.SetInt("PoisonOrbStorageCount", tmp6);
                }
                else
                {
                    PlayerPrefs.SetInt("PoisonOrbStorageCount", poisonProjectiles.Count);
                }
                //Debug.Log("Poison count : " + poisonProjectiles.Count);
                generalProjectileList.Add(_projectileObj);
                UIManager._instance.SetPoisonOrbAmountText(poisonProjectiles.Count);
                break;

            case OrbClass.OrbType.Lightning:
                lightningProjectiles.Add(_projectileObj);
                int tmp7 = 0;
                if (PlayerPrefs.HasKey("LightningOrbStorageCount"))
                {
                    tmp7 = PlayerPrefs.GetInt("LightningOrbStorageCount");
                    tmp7++;
                    PlayerPrefs.SetInt("LightningOrbStorageCount", tmp7);
                }
                else
                {
                    PlayerPrefs.SetInt("LightningOrbStorageCount", lightningProjectiles.Count);
                }
                //Debug.Log("Lightning count : " + lightningProjectiles.Count);
                generalProjectileList.Add(_projectileObj);
                UIManager._instance.SetLightningOrbAmountText(lightningProjectiles.Count);
                break;

            default:
                break;
        }

    } // AddProjectileToStorage()

    public float GetTotalDamageInStorage()
    {
        float totalDamage = 0;

        if (generalProjectileList.Count > 0)
        {
            for (int i = 0; i < generalProjectileList.Count; i++)
            {
                totalDamage += generalProjectileList[i].GetComponent<OrbProjectile>().damageAmount;
            }
        }

        return totalDamage;

    } // GetTotalDamageInStorage()

    public void ClearAllStorage()
    {
        generalProjectileList.Clear();
        fireProjectiles.Clear();
        waterProjectiles.Clear();
        airProjectiles.Clear();
        earthProjectiles.Clear();
        voidProjectiles.Clear();
        poisonProjectiles.Clear();
        lightningProjectiles.Clear();

        UIManager._instance.ClearAllOrbAmounts();

    } // ClearAllStorage()

    void OnMouseDown()
    {
        UIManager._instance.OpenStorageScreen();

    } // OnMouseDown()

    public void AddProjectileToStorageFromSaveData(OrbClass.OrbType _projectileType, GameObject _projectileObj)
    {
        switch (_projectileType)
        {
            case OrbClass.OrbType.Fire:
                fireProjectiles.Add(_projectileObj);
                generalProjectileList.Add(_projectileObj);
                break;

            case OrbClass.OrbType.Water:
                waterProjectiles.Add(_projectileObj);
                generalProjectileList.Add(_projectileObj);
                break;

            case OrbClass.OrbType.Air:
                airProjectiles.Add(_projectileObj);
                generalProjectileList.Add(_projectileObj);
                break;

            case OrbClass.OrbType.Earth:
                earthProjectiles.Add(_projectileObj);
                generalProjectileList.Add(_projectileObj);
                break;

            case OrbClass.OrbType.Void:
                voidProjectiles.Add(_projectileObj);
                generalProjectileList.Add(_projectileObj);
                break;

            case OrbClass.OrbType.Poison:
                poisonProjectiles.Add(_projectileObj);
                generalProjectileList.Add(_projectileObj);
                break;

            case OrbClass.OrbType.Lightning:
                lightningProjectiles.Add(_projectileObj);
                generalProjectileList.Add(_projectileObj);
                break;

            default:
                break;
        }

    } // AddProjectileToStorageFromSaveData()

} // class
