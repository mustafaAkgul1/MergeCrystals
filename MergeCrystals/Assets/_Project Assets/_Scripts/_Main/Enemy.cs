using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public float EnemyXP;
    public int earnCoin;

    void Start()
    {
        HPControl();
        
    } // Start()

    private void HPControl()
    {
        if (!PlayerPrefs.HasKey("ActiveEnemyHp"))
        {
            PlayerPrefs.SetFloat("ActiveEnemyHp", maxHp);
        }

        if (PlayerPrefs.HasKey("ActiveEnemyHp"))
        {
            if (PlayerPrefs.GetFloat("ActiveEnemyHp") > 0)
            {
                hp = PlayerPrefs.GetFloat("ActiveEnemyHp");
                ESUIManager._instance.ControlEnemyHpBar(hp, maxHp);
            }
            else
            {
                hp = maxHp;
                PlayerPrefs.SetFloat("ActiveEnemyHp", hp);
                ESUIManager._instance.ControlEnemyHpBar(hp, maxHp);
            }
        }
        else
        {
            hp = maxHp;
            PlayerPrefs.SetFloat("ActiveEnemyHp", hp);
            ESUIManager._instance.ControlEnemyHpBar(hp, maxHp);
        }

        if (PlayerPrefs.GetFloat("ActiveEnemyHp") >= 0)
        {
            ESUIManager._instance.ControlEnemyHpBar(PlayerPrefs.GetFloat("ActiveEnemyHp"), maxHp);
        }

    } // HPControl()

    void Update()
    {
        if (hp <= 0f)
        {
            Destroy(gameObject);
        }

    } // Update()

    public void GetHit(float _damageAmount)
    {
        //AudioManager._instance.PlaySFX(AudioManager._instance.enemyHitSFXindex);

        hp -= _damageAmount;
        PlayerPrefs.SetFloat("ActiveEnemyHp", hp);

        if (PlayerPrefs.GetFloat("ActiveEnemyHp") >= 0)
        {
            ESUIManager._instance.ControlEnemyHpBar(PlayerPrefs.GetFloat("ActiveEnemyHp"), maxHp);
        }

        if (PlayerPrefs.GetFloat("ActiveEnemyHp") <= 0f)
        {
            ESVFXManager._instance.SpawnEnemyDestoryVFX(transform);
            ESVFXManager._instance.SpawnEnemyDestroyConfettiVFX();

            //if (ESGameManager._instance.GetVibrationSetting())
            //{
            //    MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.HeavyImpact);
            //}

            ESGameManager._instance.ShakeCamera();
            ESUIManager._instance.ControlEnemyHpBar(0, maxHp);
           
            int tmpIndex = PlayerPrefs.GetInt("currentEnemyIndex");
            tmpIndex++;
            PlayerPrefs.SetInt("currentEnemyIndex", tmpIndex);
            ESGameManager._instance.currentEnemyIndex = PlayerPrefs.GetInt("currentEnemyIndex");

            ESGameManager._instance.CollectedXP(EnemyXP);
            ESGameManager._instance.CollectedCoin(earnCoin);
            ESGameManager._instance.GetEnemy();
        }

    } // GetHit()

} // class
