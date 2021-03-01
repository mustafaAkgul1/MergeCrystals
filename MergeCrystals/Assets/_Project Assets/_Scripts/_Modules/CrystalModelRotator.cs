using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalModelRotator : MonoBehaviour
{
    public float rotateSpeed = 50f;
    Quaternion defaultRotation;

    public enum OrbType
    {
        Fire,
        Water,
        Air,
        Earth,
        Void,
        Poison,
        Lightning
    }
    public OrbType orbType;

    void Start()
    {
        defaultRotation = transform.rotation;

    } // Start()

    void Update()
    {
        switch (orbType)
        {
            case OrbType.Fire:
                transform.Rotate(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime));
                break;
            case OrbType.Water:
                transform.Rotate(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime));
                break;
            case OrbType.Air:
                transform.Rotate(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime));
                break;
            case OrbType.Earth:
                transform.Rotate(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime));
                break;
            case OrbType.Void:
                transform.Rotate(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime));
                break;
            case OrbType.Poison:
                transform.Rotate(new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));
                break;
            case OrbType.Lightning:
                transform.Rotate(new Vector3(0f, 0f, rotateSpeed * Time.deltaTime));
                break;
            default:
                break;
        }

    } // Update()

} // class
