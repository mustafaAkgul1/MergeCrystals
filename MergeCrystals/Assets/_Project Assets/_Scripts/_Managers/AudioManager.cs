using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    public AudioMixerGroup sfxGroup;

    public int crystalMergeSFXindex;
    public int crystalDropSFXindex;
    public int enemyHitSFXindex;
    public int slotPurchasedindex;
    public int crystalPurchasedindex;
    public AudioSource[] sfxList;

    void Awake()
    {
        _instance = this;

    } // Awake()

    public void SetSFXVolume(bool _check)
    {
        if (_check)
        {
            for (int i = 0; i < sfxList.Length; i++)
            {
                sfxList[i].mute = false;
            }
        }
        else
        {
            for (int i = 0; i < sfxList.Length; i++)
            {
                sfxList[i].mute = true;
            }
        }

    } // SetSFXVolume()

    public void PlaySFX(int _index)
    {
        sfxList[_index].Play();

    } // PlaySFX()

} // class
