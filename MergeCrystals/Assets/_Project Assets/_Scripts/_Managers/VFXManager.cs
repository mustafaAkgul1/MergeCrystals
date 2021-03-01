using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager _instance;

    public Transform mergePhaseObj;

    public GameObject fireOrbMergeVFX;
    public GameObject waterOrbMergeVFX;
    public GameObject airOrbMergeVFX;
    public GameObject earthOrbMergeVFX;
    public GameObject voidOrbMergeVFX;
    public GameObject poisonOrbMergeVFX;
    public GameObject lightningOrbMergeVFX;
    public GameObject lightningAttackHitVFX;
    public GameObject explosionAfterLightningVFX;
    public GameObject boosterTriggerVFX;
    public GameObject orbDestroyerTriggerVFX;
    public GameObject orbCreationTriggerVFX;
    public GameObject boosterActiveTextVFX;
    public GameObject boosterEndedTextVFX;
    public GameObject boosterActiveOrbCreationVFX;
    public GameObject positiveCrystalTriggerVFX;
    public GameObject enemyButtonReadyVFX;

    [Header("Projectile Hit VFXS")]
    public bool useOrbTypeHitVfxs = true;
    public GameObject projectileCreationTriggerVFX;
    public GameObject fireProjectileHitVFX;
    public GameObject waterProjectileHitVFX;
    public GameObject airProjectileHitVFX;
    public GameObject earthProjectileHitVFX;
    public GameObject voidProjectileHitVFX;
    public GameObject poisonProjectileHitVFX;
    public GameObject lightningProjectileHitVFX;

    [Header("Production VFXS")]
    public GameObject fireFirstProductionVFX;
    public GameObject waterFirstProductionVFX;
    public GameObject airFirstProductionVFX;
    public GameObject earthFirstProductionVFX;
    public GameObject voidFirstProductionVFX;
    public GameObject poisonFirstProductionVFX;
    public GameObject lightningFirstProductionVFX;
    public GameObject fireFullProductionVFX;
    public GameObject waterFullProductionVFX;
    public GameObject airFullProductionVFX;
    public GameObject earthFullProductionVFX;
    public GameObject voidFullProductionVFX;
    public GameObject poisonFullProductionVFX;
    public GameObject lightningFullProductionVFX;


    void Awake()
    {
        _instance = this;

    } // Awake()

    public void SpawnFireOrbMergeVFX(Transform _transform)
    {
        Instantiate(fireOrbMergeVFX, _transform.position, Quaternion.identity, mergePhaseObj);

    } // SpawnFireOrbMergeVFX()

    public void SpawnWaterOrbMergeVFX(Transform _transform)
    {
        Instantiate(waterOrbMergeVFX, _transform.position, Quaternion.identity, mergePhaseObj);

    } // SpawnWaterOrbMergeVFX()

    public void SpawnAirOrbMergeVFX(Transform _transform)
    {
        Instantiate(airOrbMergeVFX, _transform.position, Quaternion.identity, mergePhaseObj);

    } // SpawnAirOrbMergeVFX()

    public void SpawnEarthOrbMergeVFX(Transform _transform)
    {
        Instantiate(earthOrbMergeVFX, _transform.position, Quaternion.identity, mergePhaseObj);

    } // SpawnEarthOrbMergeVFX()

    public void SpawnVoidOrbMergeVFX(Transform _transform)
    {
        Instantiate(voidOrbMergeVFX, _transform.position, Quaternion.identity, mergePhaseObj);

    } // SpawnVoidOrbMergeVFX()

    public void SpawnPoisonOrbMergeVFX(Transform _transform)
    {
        Instantiate(poisonOrbMergeVFX, _transform.position, Quaternion.identity, mergePhaseObj);

    } // SpawnPoisonOrbMergeVFX()

    public void SpawnLightningOrbMergeVFX(Transform _transform)
    {
        Instantiate(lightningOrbMergeVFX, _transform.position, Quaternion.identity, mergePhaseObj);

    } // SpawnLightningOrbMergeVFX()

    public void SpawnPositiveCrystalTriggerVFX(Transform _transform)
    {
        Instantiate(positiveCrystalTriggerVFX, _transform.position, Quaternion.identity, mergePhaseObj);

    } // SpawnPositiveCrystalTriggerVFX()

    public void SpawnOrbHolderCrystalHitVFX(Transform _transform)
    {
        Instantiate(orbCreationTriggerVFX, _transform.position, Quaternion.Euler(0f, 0f, 90f));

    } // SpawnOrbHolderCrystalHitVFX()

    public void SpawnProjectileCreationVFX(Transform _transform, Quaternion _rotation, OrbProjectile.OrbType _orbType)
    {
        if (useOrbTypeHitVfxs)
        {
            switch (_orbType)
            {
                case OrbProjectile.OrbType.Fire:
                    Instantiate(fireProjectileHitVFX, _transform.position, _rotation);
                    break;
                case OrbProjectile.OrbType.Water:
                    Instantiate(waterProjectileHitVFX, _transform.position, _rotation);
                    break;
                case OrbProjectile.OrbType.Air:
                    Instantiate(airProjectileHitVFX, _transform.position, _rotation);
                    break;
                case OrbProjectile.OrbType.Earth:
                    Instantiate(earthProjectileHitVFX, _transform.position, _rotation);
                    break;
                case OrbProjectile.OrbType.Void:
                    Instantiate(voidProjectileHitVFX, _transform.position, _rotation);
                    break;
                case OrbProjectile.OrbType.Poison:
                    Instantiate(poisonProjectileHitVFX, _transform.position, _rotation);
                    break;
                case OrbProjectile.OrbType.Lightning:
                    Instantiate(lightningProjectileHitVFX, _transform.position, _rotation);
                    break;
                default:
                    break;
            }
        }
        else
        {
            Instantiate(projectileCreationTriggerVFX, _transform.position, _rotation);
        }

    } // SpawnProjectileCreationVFX()

    public GameObject SpawnCrystalFirstProductionVFX(Transform _transform, OrbClass.OrbType _orbType)
    {
        GameObject createdVFXObj = null;
        Vector3 tmpZ = new Vector3(0f, 0f, -2f);

        switch (_orbType)
        {
            case OrbClass.OrbType.Fire:
                createdVFXObj = Instantiate(fireFirstProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Water:
                createdVFXObj = Instantiate(waterFirstProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Air:
                createdVFXObj = Instantiate(airFirstProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Earth:
                createdVFXObj = Instantiate(earthFirstProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Void:
                createdVFXObj = Instantiate(voidFirstProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Poison:
                createdVFXObj = Instantiate(poisonFirstProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Lightning:
                createdVFXObj = Instantiate(lightningFirstProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            default:
                break;
        }

        return createdVFXObj;

    } // SpawnCrystalFirstProductionVFX()

    public GameObject SpawnCrystalFullProductionVFX(Transform _transform, OrbClass.OrbType _orbType)
    {
        GameObject createdVFXObj = null;
        Vector3 tmpZ = new Vector3(0f, 0f, -2f);

        switch (_orbType)
        {
            case OrbClass.OrbType.Fire:
                createdVFXObj = Instantiate(fireFullProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Water:
                createdVFXObj = Instantiate(waterFullProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Air:
                createdVFXObj = Instantiate(airFullProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Earth:
                createdVFXObj = Instantiate(earthFullProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Void:
                createdVFXObj = Instantiate(voidFullProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Poison:
                createdVFXObj = Instantiate(poisonFullProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            case OrbClass.OrbType.Lightning:
                createdVFXObj = Instantiate(lightningFullProductionVFX, _transform.position + tmpZ, Quaternion.identity, _transform);
                break;
            default:
                break;
        }

        return createdVFXObj;

    } // SpawnCrystalFullProductionVFX()

    public GameObject SpawnEnemyButtonReadyVFX(Transform _transform)
    {
        Vector3 tmpPos = new Vector3(0f, 0f, 768.4f);

        GameObject tmpVFX = Instantiate(enemyButtonReadyVFX, _transform.position, Quaternion.identity, _transform);
        tmpVFX.transform.localPosition = new Vector3(1.65f, 0f, 0f);
        tmpVFX.transform.localScale = new Vector3(2f, 2f, 2f);
        return tmpVFX;

    } // SpawnEnemyButtonReadyVFX()

} // class
