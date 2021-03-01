using System;
//using System.Collections;
//using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseRemoteConfigController : MonoBehaviour
{
    [Header("Default Initialize Variables")]
    public int FirsStartUpCoinAmount = 50;
    public float FirstMaxXpAmountToLevelUp = 100f;
    public float MaxXpAmountMultiplerValue = 1.2f;

    [Header("Default Crystals Purchase Costs")]
    public int FireCrystalPurchaseCost = 5;
    public int WaterCrystalPurchaseCost = 10;
    public int AirCrystalPurchaseCost = 15;
    public int EarthCrystalPurchaseCost = 20;
    public int VoidCrystalPurchaseCost = 25;
    public int PoisonCrystalPurchaseCost = 30;
    public int LightningCrystalPurchaseCost = 35;

    [Header("Default Enemy Stats Variables")]
    public float EnemyHpFloatAmountToAdd = 0f;
    public float EnemyHpMultiplyValue = 1f;
    public float EnemyXpFloatAmountToAdd = 0f;
    public float EnemyXpMultiplyValue = 1f;
    public int EnemyCoinIntAmountToAdd = 0;
    public float EnemyCoinMultiplyValue = 1f;
    int tmpCounter = 0; //test

    [HideInInspector] public bool isFetchComplete = false;
    [HideInInspector] public bool isFetchSuccess = false;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Dependency Status : Available");
                InitializeDefaults();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

    } // Awake()

    void InitializeDefaults()
    {
        System.Collections.Generic.Dictionary<string, object> defaults =
            new System.Collections.Generic.Dictionary<string, object>();

        defaults.Add("FirsStartUpCoinAmount", FirsStartUpCoinAmount);
        defaults.Add("FirstMaxXpAmountToLevelUp", FirstMaxXpAmountToLevelUp);
        defaults.Add("MaxXpAmountMultiplerValue", MaxXpAmountMultiplerValue);
        defaults.Add("FireCrystalPurchaseCost", FireCrystalPurchaseCost);
        defaults.Add("WaterCrystalPurchaseCost", WaterCrystalPurchaseCost);
        defaults.Add("AirCrystalPurchaseCost", AirCrystalPurchaseCost);
        defaults.Add("EarthCrystalPurchaseCost", EarthCrystalPurchaseCost);
        defaults.Add("VoidCrystalPurchaseCost", VoidCrystalPurchaseCost);
        defaults.Add("PoisonCrystalPurchaseCost", PoisonCrystalPurchaseCost);
        defaults.Add("LightningCrystalPurchaseCost", LightningCrystalPurchaseCost);
        defaults.Add("EnemyHpFloatAmountToAdd", EnemyHpFloatAmountToAdd);
        defaults.Add("EnemyHpMultiplyValue", EnemyHpMultiplyValue);
        defaults.Add("EnemyXpFloatAmountToAdd", EnemyXpFloatAmountToAdd);
        defaults.Add("EnemyXpMultiplyValue", EnemyXpMultiplyValue);
        defaults.Add("EnemyCoinIntAmountToAdd", EnemyCoinIntAmountToAdd);
        defaults.Add("EnemyCoinMultiplyValue", EnemyCoinMultiplyValue);

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults(defaults);
        Debug.Log("Remote config defaults ready!");

        FetchDataAsync();

    } // InitializeDefaults()

    public Task FetchDataAsync()
    {
        Debug.Log("Fetching Remote Config Data...");
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.

        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWith(FetchComplete);

    } // FetchDataAsync()

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Remote Config Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Remote Config Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Remote Config Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;

        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();

                Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                    info.FetchTime));

                FirebaseDataHolder._instance.FirsStartUpCoinAmount = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("FirsStartUpCoinAmount").StringValue.ToString());
                FirebaseDataHolder._instance.FirstMaxXpAmountToLevelUp = float.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("FirstMaxXpAmountToLevelUp").StringValue.ToString());
                FirebaseDataHolder._instance.MaxXpAmountMultiplerValue = float.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("MaxXpAmountMultiplerValue").StringValue.ToString());
                FirebaseDataHolder._instance.FireCrystalPurchaseCost = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("FireCrystalPurchaseCost").StringValue.ToString());
                FirebaseDataHolder._instance.WaterCrystalPurchaseCost = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("WaterCrystalPurchaseCost").StringValue.ToString());
                FirebaseDataHolder._instance.AirCrystalPurchaseCost = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("AirCrystalPurchaseCost").StringValue.ToString());
                FirebaseDataHolder._instance.EarthCrystalPurchaseCost = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("EarthCrystalPurchaseCost").StringValue.ToString());
                FirebaseDataHolder._instance.VoidCrystalPurchaseCost = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("VoidCrystalPurchaseCost").StringValue.ToString());
                FirebaseDataHolder._instance.PoisonCrystalPurchaseCost = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("PoisonCrystalPurchaseCost").StringValue.ToString());
                FirebaseDataHolder._instance.LightningCrystalPurchaseCost = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("LightningCrystalPurchaseCost").StringValue.ToString());
                FirebaseDataHolder._instance.EnemyHpFloatAmountToAdd = float.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("EnemyHpFloatAmountToAdd").StringValue.ToString());
                FirebaseDataHolder._instance.EnemyHpMultiplyValue = float.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("EnemyHpMultiplyValue").StringValue.ToString());
                FirebaseDataHolder._instance.EnemyXpFloatAmountToAdd = float.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("EnemyXpFloatAmountToAdd").StringValue.ToString());
                FirebaseDataHolder._instance.EnemyXpMultiplyValue = float.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("EnemyXpMultiplyValue").StringValue.ToString());
                FirebaseDataHolder._instance.EnemyCoinIntAmountToAdd = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("EnemyCoinIntAmountToAdd").StringValue.ToString());
                FirebaseDataHolder._instance.EnemyCoinMultiplyValue = float.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("EnemyCoinMultiplyValue").StringValue.ToString());


                Debug.Log("Fetched Coin Amount : " + Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue("FirsStartUpCoinAmount").StringValue.ToString());

                isFetchSuccess = true;
                isFetchComplete = true;

                break;

            case Firebase.RemoteConfig.LastFetchStatus.Failure:

                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }

                FirebaseDataHolder._instance.FirsStartUpCoinAmount = FirsStartUpCoinAmount;
                FirebaseDataHolder._instance.FirstMaxXpAmountToLevelUp = FirstMaxXpAmountToLevelUp;
                FirebaseDataHolder._instance.MaxXpAmountMultiplerValue = MaxXpAmountMultiplerValue;
                FirebaseDataHolder._instance.FireCrystalPurchaseCost = FireCrystalPurchaseCost;
                FirebaseDataHolder._instance.WaterCrystalPurchaseCost = WaterCrystalPurchaseCost;
                FirebaseDataHolder._instance.AirCrystalPurchaseCost = AirCrystalPurchaseCost;
                FirebaseDataHolder._instance.EarthCrystalPurchaseCost = EarthCrystalPurchaseCost;
                FirebaseDataHolder._instance.VoidCrystalPurchaseCost = VoidCrystalPurchaseCost;
                FirebaseDataHolder._instance.PoisonCrystalPurchaseCost = PoisonCrystalPurchaseCost;
                FirebaseDataHolder._instance.LightningCrystalPurchaseCost = LightningCrystalPurchaseCost;
                FirebaseDataHolder._instance.EnemyHpFloatAmountToAdd = EnemyHpFloatAmountToAdd;
                FirebaseDataHolder._instance.EnemyHpMultiplyValue = EnemyHpMultiplyValue;
                FirebaseDataHolder._instance.EnemyXpFloatAmountToAdd = EnemyXpFloatAmountToAdd;
                FirebaseDataHolder._instance.EnemyXpMultiplyValue = EnemyXpMultiplyValue;
                FirebaseDataHolder._instance.EnemyCoinIntAmountToAdd = EnemyCoinIntAmountToAdd;
                FirebaseDataHolder._instance.EnemyCoinMultiplyValue = EnemyCoinMultiplyValue;

                isFetchSuccess = false;
                isFetchComplete = true;

                break;

            case Firebase.RemoteConfig.LastFetchStatus.Pending:

                Debug.Log("Latest Fetch call still pending.");

                tmpCounter++;
                //MMUIManager._instance.SetPendingText(tmpCounter);

                break;

        } // switch fecth status

    } // FetchComplete()

} // class
