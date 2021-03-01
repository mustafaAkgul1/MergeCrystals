using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OfflineTimeCalculator : MonoBehaviour
{
    [HideInInspector] public int offlineSeconds;

    void Awake()
    {
        if (PlayerPrefs.HasKey("LastLoginTime"))
        {
            DateTime lastLoginTime = DateTime.Parse(PlayerPrefs.GetString("LastLoginTime"));

            TimeSpan ts = DateTime.Now - lastLoginTime;
            offlineSeconds = (int)ts.TotalSeconds;
        }
        else
        {
            //Debug.Log("İlk giriş");
        }

    } // Awake()

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastLoginTime", DateTime.Now.ToString());
        //Debug.Log(PlayerPrefs.GetString("LastLoginTime"));

    } // OnApplicationQuit()

    public void ChangeSceneToEnemyTrigger()
    {
        PlayerPrefs.SetString("LastLoginTime", DateTime.Now.ToString());
        //Debug.Log(PlayerPrefs.GetString("LastLoginTime"));

    } // ChangeSceneToEnemyTrigger()

} // class
