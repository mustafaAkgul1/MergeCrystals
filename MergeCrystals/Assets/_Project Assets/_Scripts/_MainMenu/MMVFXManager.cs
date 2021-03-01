using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMVFXManager : MonoBehaviour
{
    public static MMVFXManager _instance;

    void Awake()
    {
        _instance = this;

    } // Awake()

} // class
