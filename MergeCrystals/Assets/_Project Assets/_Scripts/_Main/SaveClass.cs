using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveClass
{
    // Normal Slots
    public List<float> SavedSlotPosXList = new List<float>();
    public List<float> SavedSlotPosYList = new List<float>();
    public List<float> SavedSlotPosZList = new List<float>();

    // Purchasable Slots
    public List<float> SavedPurchasableSlotPosXList = new List<float>();
    public List<float> SavedPurchasableSlotPosYList = new List<float>();
    public List<float> SavedPurchasableSlotPosZList = new List<float>();
    public List<int> SavedPurchasableSlotCostsList = new List<int>();

    public int[] SavedCrystalsLevelsList;
    public string[] SavedCrystalsTypesList;

    public int currentLevel;
    public float currentXp;
    public int currentCoin;

} // class
