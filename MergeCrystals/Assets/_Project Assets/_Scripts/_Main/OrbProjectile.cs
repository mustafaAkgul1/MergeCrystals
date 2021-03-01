using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class OrbProjectile : MonoBehaviour
{
    public float damageAmount;
    public float projectileSpeed = 9f;
    public float rotationSpeed = 12f;
    [HideInInspector] public PathCreator pathCreator;
    [HideInInspector] public GameObject levelEnemy;
    public Animator anim;

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

    public enum ProjectileState
    {
        Intro,
        GoingToStorage,
        GoingToEnemy
    }
    public ProjectileState projectileState;

    float distanceTraveled = 0f;
    Vector3 pathLastPoint;

    void Update()
    {
        switch (projectileState)
        {
            case ProjectileState.Intro:
                break;
            case ProjectileState.GoingToStorage:
                GoingToStorage();
                break;
            case ProjectileState.GoingToEnemy:
                FollowPath();
                break;
            default:
                break;

        } // switch (projectileState)

    } // Update()

    void GoingToStorage()
    {
        if (GameManager._instance.storageObj != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager._instance.storageObj.transform.position, (projectileSpeed / 2f) * Time.deltaTime);
            transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
        }

    } // GoingToStorage()

    void FollowPath()
    {
        pathLastPoint = pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1);

        if (transform.position == pathLastPoint)
        {
            Destroy(gameObject);
        }
        else
        {
            distanceTraveled += projectileSpeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTraveled, EndOfPathInstruction.Stop);
            Quaternion angle = pathCreator.path.GetRotationAtDistance(distanceTraveled);
            angle.x = 0;
            angle.y = 0;
            transform.rotation = angle;
        }

    } // FollowPath()

    public void IntroAnimationEndedEvent()
    {
        projectileState = ProjectileState.GoingToStorage;

    } // IntroAnimationEndedEvent()

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().GetHit(damageAmount);
            ESGameManager._instance.enemyPhaseActiveOrbs.Remove(this.gameObject);
            ESUIManager._instance.SpawnFloatingText("-" + damageAmount.ToString(), Color.red, transform);
            ESVFXManager._instance.SpawnProjectileCreationVFX(transform, transform.rotation, orbType);
            Destroy(gameObject);
        }

        if (collision.CompareTag("ProjectileStorage"))
        {
            VFXManager._instance.SpawnProjectileCreationVFX(transform, transform.rotation, orbType);
            Destroy(gameObject);
        }

    } // OnTriggerEnter2D()

} // class
