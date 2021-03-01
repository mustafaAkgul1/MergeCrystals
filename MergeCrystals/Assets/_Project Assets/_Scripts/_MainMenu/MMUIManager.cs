using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MMUIManager : MonoBehaviour
{
    public static MMUIManager _instance;

    void Awake()
    {
        _instance = this;

    } // Awake()

} // class
