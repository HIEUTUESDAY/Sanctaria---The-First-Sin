using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource musicAudioSource;

    public AudioClip mainMenuMusicClip;
    public AudioClip towerMusicClip;
    public AudioClip forestMusicClip;
    public AudioClip villageMusicClip;
    public AudioClip tenPiedadBossMusicClip;
    public AudioClip tenPiedadBrethingClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        musicAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        musicAudioSource.clip = mainMenuMusicClip;
        PlayMusic();
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    public void PlayMusic()
    {
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void ChangeMusicOnSceene(string sceeneName)
    {
        if (sceeneName.Equals("MainMenu"))
        {
            if (musicAudioSource.clip != null && musicAudioSource.clip != mainMenuMusicClip)
            {
                musicAudioSource.clip = mainMenuMusicClip;
                PlayMusic();
            }
            else
            {
                return;
            }
        }
        else if (sceeneName.Equals("BrotherTower") || sceeneName.Equals("BrotherTowerEntrance") || sceeneName.Equals("BrotherTowerHall"))
        {
            if (musicAudioSource.clip != null && musicAudioSource.clip != towerMusicClip)
            {
                musicAudioSource.clip = towerMusicClip;
                PlayMusic();
            }
            else
            {
                return;
            }
        }
        else if (sceeneName.Equals("Level1.1") || sceeneName.Equals("Level1.2") || sceeneName.Equals("Level1.3"))
        {
            if (musicAudioSource.clip != null && musicAudioSource.clip != forestMusicClip)
            {
                musicAudioSource.clip = forestMusicClip;
                PlayMusic();
            }
            else
            {
                return;
            }
        }
        else if (sceeneName.Equals("VillageEntrance") || sceeneName.Equals("VillageHouseBasement") || sceeneName.Equals("VillageHouseFloor") || sceeneName.Equals("VillageShop"))
        {
            if (musicAudioSource.clip != null && musicAudioSource.clip != villageMusicClip)
            {
                musicAudioSource.clip = villageMusicClip;
                PlayMusic();
            }
            else
            {
                return;
            }
        }
        else if (sceeneName.Equals("BossTenPiedad"))
        {
            if (musicAudioSource.clip != null && musicAudioSource.clip != tenPiedadBrethingClip)
            {
                musicAudioSource.clip = tenPiedadBrethingClip;
                PlayMusic();
            }
            else
            {
                return;
            }
        }
    }
}
