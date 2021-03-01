using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;

    public Toggle sfxToggle;
    public Toggle vibrationToggle;

    //new game design 
    public GameObject storageScreen;
    public TextMeshProUGUI fireOrbAmountText;
    public TextMeshProUGUI waterOrbAmountText;
    public TextMeshProUGUI airOrbAmountText;
    public TextMeshProUGUI earthOrbAmountText;
    public TextMeshProUGUI voidOrbAmountText;
    public TextMeshProUGUI poisonOrbAmountText;
    public TextMeshProUGUI lightningOrbAmountText;
    public TextMeshProUGUI coinAmountTextTest;
    public Button enemySceneButton;

    //Shop Crystals
    public BoxCollider2D shopWaterCrystalColl;
    public GameObject shopWaterCrystalModelToOpen;
    public GameObject shopWaterCrystalModelToClose;

    public BoxCollider2D shopAirCrystalColl;
    public GameObject shopAirCrystalModelToOpen;
    public GameObject shopAirCrystalModelToClose;

    public BoxCollider2D shopEarthCrystalColl;
    public GameObject shopEarthCrystalModelToOpen;
    public GameObject shopEarthCrystalModelToClose;

    public BoxCollider2D shopVoidCrystalColl;
    public GameObject shopVoidCrystalModelToOpen;
    public GameObject shopVoidCrystalModelToClose;

    public BoxCollider2D shopPoisonCrystalColl;
    public GameObject shopPoisonCrystalModelToOpen;
    public GameObject shopPoisonCrystalModelToClose;

    public BoxCollider2D shopLightningCrystalColl;
    public GameObject shopLightningCrystalModelToOpen;
    public GameObject shopLightningCrystalModelToClose;

    public TextMeshProUGUI levelText;
    public Image xpBar;
    public TextMeshProUGUI xpPercText;

    public GameObject fireCrystalCostTextObj;
    public GameObject waterCrystalCostTextObj;
    public GameObject airCrystalCostTextObj;
    public GameObject earthCrystalCostTextObj;
    public GameObject voidCrystalCostTextObj;
    public GameObject poisonCrystalCostTextObj;
    public GameObject lightningCrystalCostTextObj;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        ControlXpBar(PlayerPrefs.GetFloat("CurrentXpAmount"), PlayerPrefs.GetFloat("MaxXpAmountToLevelUp"));
        ControlLevelText(GameManager._instance.currentLevel);
        ControlToggles();

        ControlCrystalShopPriceTexts();

    } // Start()

    void ControlToggles()
    {
        if (PlayerPrefs.HasKey("CanVibrate"))
        {
            string tmpString = PlayerPrefs.GetString("CanVibrate");
            bool tmpBool = true;

            if (tmpString == "True")
            {
                tmpBool = true;
            }
            else
            {
                tmpBool = false;
            }
            vibrationToggle.isOn = tmpBool;
        }
        else
        {
            vibrationToggle.isOn = true;
        }

        if (PlayerPrefs.HasKey("SFXBool"))
        {
            string tmpString = PlayerPrefs.GetString("SFXBool");
            bool tmpBool = true;

            if (tmpString == "True")
            {
                tmpBool = true;
            }
            else
            {
                tmpBool = false;
            }
            sfxToggle.isOn = tmpBool;
        }
        else
        {
            sfxToggle.isOn = true;
        }

    } // ControlToggles()

    public void VibrationToggleClicked(bool _check)
    {
        //Debug.Log("Vibration toggle test clicked : " + _check.ToString());
        PlayerPrefs.SetString("CanVibrate", _check.ToString());
        GameManager._instance.SetVibrationSetting(_check);

    } // VibrationToggleClicked()

    public void SFXToggleClicked(bool _check)
    {
        //Debug.Log("ses trigger : " + _check.ToString());
        PlayerPrefs.SetString("SFXBool", _check.ToString());
        AudioManager._instance.SetSFXVolume(_check);

    } // SFXToggleClicked()

    public void OpenStorageScreen()
    {
        if (storageScreen.activeInHierarchy)
        {
            storageScreen.SetActive(false);
        }
        else
        {
            storageScreen.SetActive(true);
        }

    } // OpenStorageScreen()

    public void CloseStorageScreenButton()
    {
        storageScreen.SetActive(false);

    } // CloseStorageScreenButton()

    public void SetFireOrbAmountText(int amount)
    {
        fireOrbAmountText.text = "x " + amount.ToString();

    } // SetFireOrbAmountText()

    public void SetWaterOrbAmountText(int amount)
    {
        waterOrbAmountText.text = "x " + amount.ToString();

    } // SetWaterOrbAmountText()

    public void SetAirOrbAmountText(int amount)
    {
        airOrbAmountText.text = "x " + amount.ToString();

    } // SetAirOrbAmountText()

    public void SetEarthOrbAmountText(int amount)
    {
        earthOrbAmountText.text = "x " + amount.ToString();

    } // SetEarthOrbAmountText()

    public void SetVoidOrbAmountText(int amount)
    {
        voidOrbAmountText.text = "x " + amount.ToString();

    } // SetVoidOrbAmountText()

    public void SetPoisonOrbAmountText(int amount)
    {
        poisonOrbAmountText.text = "x " + amount.ToString();

    } // SetPoisonOrbAmountText()

    public void SetLightningOrbAmountText(int amount)
    {
        lightningOrbAmountText.text = "x " + amount.ToString();

    } // SetLightningOrbAmountText()

    public void ClearAllOrbAmounts()
    {
        fireOrbAmountText.text = "x 0";
        waterOrbAmountText.text = "x 0";
        airOrbAmountText.text = "x 0";
        earthOrbAmountText.text = "x 0";
        voidOrbAmountText.text = "x 0";
        poisonOrbAmountText.text = "x 0";
        lightningOrbAmountText.text = "x 0";

    } // ClearAllOrbAmounts()

    public void SetCoinAmountTextTest(int _amount)
    {
        coinAmountTextTest.text = _amount.ToString();

    } // SetCoinAmountTextTest()

    public void EnemySceneButton()
    {
        GameManager._instance.TransferDataToEnemyScene();

        PlayerPrefs.SetString("CurrentScene", "EnemyScene");
        SceneManager.LoadScene("EnemyScene");

    } // EnemySceneButton()

    public void ActivateNextShopCrystal(int _currentLevel)
    {
        if (_currentLevel >= 3 && _currentLevel < 5)
        {
            shopWaterCrystalColl.enabled = true;
            shopWaterCrystalModelToOpen.SetActive(true);
            shopWaterCrystalModelToClose.SetActive(false);
        }

        if (_currentLevel >= 5 && _currentLevel < 7)
        {
            shopWaterCrystalColl.enabled = true;
            shopWaterCrystalModelToOpen.SetActive(true);
            shopWaterCrystalModelToClose.SetActive(false);

            shopAirCrystalColl.enabled = true;
            shopAirCrystalModelToOpen.SetActive(true);
            shopAirCrystalModelToClose.SetActive(false);
        }

        if (_currentLevel >= 7 && _currentLevel < 9)
        {
            shopWaterCrystalColl.enabled = true;
            shopWaterCrystalModelToOpen.SetActive(true);
            shopWaterCrystalModelToClose.SetActive(false);

            shopAirCrystalColl.enabled = true;
            shopAirCrystalModelToOpen.SetActive(true);
            shopAirCrystalModelToClose.SetActive(false);

            shopEarthCrystalColl.enabled = true;
            shopEarthCrystalModelToOpen.SetActive(true);
            shopEarthCrystalModelToClose.SetActive(false);
        }

        if (_currentLevel >= 9 && _currentLevel < 11)
        {
            shopWaterCrystalColl.enabled = true;
            shopWaterCrystalModelToOpen.SetActive(true);
            shopWaterCrystalModelToClose.SetActive(false);

            shopAirCrystalColl.enabled = true;
            shopAirCrystalModelToOpen.SetActive(true);
            shopAirCrystalModelToClose.SetActive(false);

            shopEarthCrystalColl.enabled = true;
            shopEarthCrystalModelToOpen.SetActive(true);
            shopEarthCrystalModelToClose.SetActive(false);

            shopVoidCrystalColl.enabled = true;
            shopVoidCrystalModelToOpen.SetActive(true);
            shopVoidCrystalModelToClose.SetActive(false);
        }

        if (_currentLevel >= 11 && _currentLevel < 13)
        {
            shopWaterCrystalColl.enabled = true;
            shopWaterCrystalModelToOpen.SetActive(true);
            shopWaterCrystalModelToClose.SetActive(false);

            shopAirCrystalColl.enabled = true;
            shopAirCrystalModelToOpen.SetActive(true);
            shopAirCrystalModelToClose.SetActive(false);

            shopEarthCrystalColl.enabled = true;
            shopEarthCrystalModelToOpen.SetActive(true);
            shopEarthCrystalModelToClose.SetActive(false);

            shopVoidCrystalColl.enabled = true;
            shopVoidCrystalModelToOpen.SetActive(true);
            shopVoidCrystalModelToClose.SetActive(false);

            shopPoisonCrystalColl.enabled = true;
            shopPoisonCrystalModelToOpen.SetActive(true);
            shopPoisonCrystalModelToClose.SetActive(false);
        }

        if (_currentLevel >= 13 && _currentLevel < 15)
        {
            shopWaterCrystalColl.enabled = true;
            shopWaterCrystalModelToOpen.SetActive(true);
            shopWaterCrystalModelToClose.SetActive(false);

            shopAirCrystalColl.enabled = true;
            shopAirCrystalModelToOpen.SetActive(true);
            shopAirCrystalModelToClose.SetActive(false);

            shopEarthCrystalColl.enabled = true;
            shopEarthCrystalModelToOpen.SetActive(true);
            shopEarthCrystalModelToClose.SetActive(false);

            shopVoidCrystalColl.enabled = true;
            shopVoidCrystalModelToOpen.SetActive(true);
            shopVoidCrystalModelToClose.SetActive(false);

            shopPoisonCrystalColl.enabled = true;
            shopPoisonCrystalModelToOpen.SetActive(true);
            shopPoisonCrystalModelToClose.SetActive(false);

            shopLightningCrystalColl.enabled = true;
            shopLightningCrystalModelToOpen.SetActive(true);
            shopLightningCrystalModelToClose.SetActive(false);
        }

        if (_currentLevel >= 15)
        {
            shopWaterCrystalColl.enabled = true;
            shopWaterCrystalModelToOpen.SetActive(true);
            shopWaterCrystalModelToClose.SetActive(false);

            shopAirCrystalColl.enabled = true;
            shopAirCrystalModelToOpen.SetActive(true);
            shopAirCrystalModelToClose.SetActive(false);

            shopEarthCrystalColl.enabled = true;
            shopEarthCrystalModelToOpen.SetActive(true);
            shopEarthCrystalModelToClose.SetActive(false);

            shopVoidCrystalColl.enabled = true;
            shopVoidCrystalModelToOpen.SetActive(true);
            shopVoidCrystalModelToClose.SetActive(false);

            shopPoisonCrystalColl.enabled = true;
            shopPoisonCrystalModelToOpen.SetActive(true);
            shopPoisonCrystalModelToClose.SetActive(false);

            shopLightningCrystalColl.enabled = true;
            shopLightningCrystalModelToOpen.SetActive(true);
            shopLightningCrystalModelToClose.SetActive(false);
        }

    } // ActivateNextShopCrystal()

    public void ControlXpBar(float _currentXpAmount, float _maxXpAmount)
    {
        float fillValue = (_currentXpAmount / _maxXpAmount);
        xpBar.fillAmount = fillValue;       

    } // ControlXpBar()

    public void ControlLevelText(int _currentLevel)
    {
        levelText.text = "Level : " + _currentLevel.ToString();

    } // ControlLevelText()

    public void ControlCrystalShopPriceTexts()
    {
        fireCrystalCostTextObj.GetComponent<TextMeshPro>().text = GameManager._instance.fireCrystalPurchaseCost.ToString();
        waterCrystalCostTextObj.GetComponent<TextMeshPro>().text = GameManager._instance.waterCrystalPurchaseCost.ToString();
        airCrystalCostTextObj.GetComponent<TextMeshPro>().text = GameManager._instance.airCrystalPurchaseCost.ToString();
        earthCrystalCostTextObj.GetComponent<TextMeshPro>().text = GameManager._instance.earthCrystalPurchaseCost.ToString();
        voidCrystalCostTextObj.GetComponent<TextMeshPro>().text = GameManager._instance.voidCrystalPurchaseCost.ToString();
        poisonCrystalCostTextObj.GetComponent<TextMeshPro>().text = GameManager._instance.poisonCrystalPurchaseCost.ToString();
        lightningCrystalCostTextObj.GetComponent<TextMeshPro>().text = GameManager._instance.lightningCrystalPurchaseCost.ToString();

    } // ControlCrystalShopPriceTexts()

} // class
