using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource soundSource;

    [SerializeField] private AudioClip getPearl;
    [SerializeField] private AudioClip levelUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            soundSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayGetPearl()
    {
        soundSource.PlayOneShot(getPearl);
    }
    public void PlayLevelUp()
    {
        soundSource.PlayOneShot(levelUp);
    }
}
