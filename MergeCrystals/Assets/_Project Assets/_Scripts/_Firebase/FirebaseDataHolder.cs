//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class FirebaseDataHolder : MonoBehaviour
{
    public static FirebaseDataHolder _instance = null;

    [Header("Initialize Variables")]
    [HideInInspector] public int FirsStartUpCoinAmount;
    [HideInInspector] public float FirstMaxXpAmountToLevelUp;
    [HideInInspector] public float MaxXpAmountMultiplerValue;

    [Header("Crystals Purchase Costs")]
    [HideInInspector] public int FireCrystalPurchaseCost;
    [HideInInspector] public int WaterCrystalPurchaseCost;
    [HideInInspector] public int AirCrystalPurchaseCost;
    [HideInInspector] public int EarthCrystalPurchaseCost;
    [HideInInspector] public int VoidCrystalPurchaseCost;
    [HideInInspector] public int PoisonCrystalPurchaseCost;
    [HideInInspector] public int LightningCrystalPurchaseCost;

    [Header("Enemy Stats Variables")]
    [HideInInspector] public float EnemyHpFloatAmountToAdd;
    [HideInInspector] public float EnemyHpMultiplyValue;
    [HideInInspector] public float EnemyXpFloatAmountToAdd;
    [HideInInspector] public float EnemyXpMultiplyValue;
    [HideInInspector] public int EnemyCoinIntAmountToAdd;
    [HideInInspector] public float EnemyCoinMultiplyValue;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }

    } // Awake()

} // class
