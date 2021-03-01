using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCrystal : MonoBehaviour
{
    public Animator anim;
    public int purchaseCost;

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

    private void OnMouseDown()
    {
        if (GameManager._instance.coinAmount >= purchaseCost)
        {
            if (GameManager._instance.AttachOrbToSlot(orbType))
            {
                AudioManager._instance.PlaySFX(AudioManager._instance.crystalPurchasedindex);
                GameManager._instance.DecreaseCoinAmount(purchaseCost);
                //Debug.Log("curr Coin : " + GameManager._instance.coinAmount);
                anim.SetTrigger("Merged");
            }
            else
            {
                //Debug.Log("Gridde boş alan yok !");
            }
        }
        else
        {
            //Debug.Log("yeterli para yok");
        }

    } // OnMouseDown()

} // class
