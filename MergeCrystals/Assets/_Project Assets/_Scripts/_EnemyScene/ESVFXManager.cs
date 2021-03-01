using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESVFXManager : MonoBehaviour
{
    public static ESVFXManager _instance;
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
    public GameObject enemyDestroyVFX;
    public GameObject confettiLeftVFX;
    public GameObject confettiRightVFX;

    void Awake()
    {
        _instance = this;
    }

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

    public void SpawnEnemyDestoryVFX(Transform _transform)
    {
        Instantiate(enemyDestroyVFX, _transform.position, Quaternion.identity);

    } // SpawnEnemyDestoryVFX()

    public void SpawnEnemyDestroyConfettiVFX()
    {
        Instantiate(confettiLeftVFX, new Vector3(-3f, -4f, 0f), Quaternion.Euler(new Vector3(-110f, -90f, 90f)));
        Instantiate(confettiRightVFX, new Vector3(3f, -4f, 0f), Quaternion.Euler(new Vector3(-70f, -90f, 90f)));

    } // SpawnEnemyDestroyConfettiVFX()

} // class
