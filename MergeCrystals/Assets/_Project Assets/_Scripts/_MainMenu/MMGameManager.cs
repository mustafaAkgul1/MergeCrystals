using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMGameManager : MonoBehaviour
{
    public static MMGameManager _instance;
    public FirebaseRemoteConfigController fbRCController;

    void Awake()
    {
        _instance = this;

    } // Awake()
    
    void Update()
    {
        if (fbRCController.isFetchSuccess)
        {
            Debug.Log("Fetch Başarılı");

            if (!PlayerPrefs.HasKey("CurrentScene"))
            {
                SceneManager.LoadScene("MergingScene");
            }
            else
            {
                if (PlayerPrefs.GetString("CurrentScene") == "EnemyScene")
                {
                    SceneManager.LoadScene("EnemyScene");
                }
                else
                {
                    SceneManager.LoadScene("MergingScene");
                }
            }
        }

        if (fbRCController.isFetchComplete && !fbRCController.isFetchSuccess)
        {
            Debug.Log("Fetch başarısız");
            //MMUIManager._instance.failText.enabled = true;

            if (!PlayerPrefs.HasKey("CurrentScene"))
            {
                SceneManager.LoadScene("MergingScene");
            }
            else
            {
                if (PlayerPrefs.GetString("CurrentScene") == "EnemyScene")
                {
                    SceneManager.LoadScene("EnemyScene");
                }
                else
                {
                    SceneManager.LoadScene("MergingScene");
                }
            }
        }

    } // Update()

} // class
