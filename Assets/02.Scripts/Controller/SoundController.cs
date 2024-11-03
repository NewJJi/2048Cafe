using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSound;
    [SerializeField] private AudioSource moneySound;
    [SerializeField] private AudioSource doorBellSound;
    [SerializeField] private AudioSource cashRegisterSound;

    private float bgmMaxValue = 0.6f;
    private float moneyMaxValue = 1.0f;
    private float bellMaxValue = 0.8f;

    private float currentSoundVolumn = 0;

    public void Init()
    {
        if (PlayerPrefs.HasKey(soundSaveVolumn))
        {
            currentSoundVolumn = PlayerPrefs.GetFloat(soundSaveVolumn);
        }
        else
        {
            SetVolumn(1);
        }
        ApplySoundVolumn();
    }

    public void SetVolumn(float volumn)
    {
        PlayerPrefs.SetFloat(soundSaveVolumn, volumn);
        ApplySoundVolumn();
    }

    public void ApplySoundVolumn()
    {
        bgmSound.volume = currentSoundVolumn * bgmMaxValue;
        moneySound.volume = currentSoundVolumn * moneyMaxValue;
        doorBellSound.volume = currentSoundVolumn * bellMaxValue;
    }

    public void PlayMoneySound()
    {
        moneySound.Play();
    }

    public void PlayDoorBellSound()
    {
        doorBellSound.Play();
    }
    public void PlayCashRegisterSound()
    {
        cashRegisterSound.Play();
    }
}
