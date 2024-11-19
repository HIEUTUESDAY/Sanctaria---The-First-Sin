using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    public AudioSource soundFXObject2D;
    public AudioSource soundFXObject3D;

    #region Player animation SFX

    [Header("Player SFX")]
    public AudioClip[] playerWalkSoundClips;
    public AudioClip[] playerAttackSoundClip;
    public AudioClip[] playerLadderClimbSoundClips;
    public AudioClip playerHitSoundClips;
    public AudioClip playerJumpAndLandingSoundClip;
    public AudioClip playerDashSoundClip;
    public AudioClip playerSpikeDeathSoundClip;
    public AudioClip playerDeathSoundClip;
    public AudioClip playerSpawnSoundClip;
    public AudioClip playerUseSpellClip;
    public AudioClip playerActiveCheckpointClip;
    public AudioClip playerHealingClip;
    public AudioClip playerHealthRestoreSoundClip;
    public AudioClip playerWallGrabSoundClip;
    [Space(5)]

    #endregion

    #region Menu SFX

    [Header("Menu SFX")]
    public AudioClip changeSelectionSoundClip;
    public AudioClip equipSoundClip;
    public AudioClip unequipSoundClip;
    public AudioClip changeTabSoundClip;
    [Space(5)]

    #endregion

    #region Title SFX

    [Header("Title SFX")]
    public AudioClip acquireTitleSoundClip;
    public AudioClip deathTitleSoundClip;
    public AudioClip defeatBossTitleSoundClip;
    [Space(5)]

    #endregion

    #region Enemies SFX

    [Header("Wheel Broken")]
    public AudioClip[] WBWalkSound;
    public AudioClip WBAttackSound;
    public AudioClip WBDeathSound;
    [Space(5)]

    [Header("Enraged Pilgrim")]
    public AudioClip[] EPWalkSound;
    public AudioClip EPDeathSound;
    [Space(5)]

    [Header("Winged Face")]
    public AudioClip[] WFFlySound;
    public AudioClip WFDeathSound;

    #endregion

    #region Bosses SFX

    [Header("Ten Piedad")]
    public AudioClip TPAwakeSound;
    public AudioClip TPWalkSound;
    public AudioClip TPAttackFootSound;
    public AudioClip TPAttackHandSound;
    public AudioClip TPDeathSound;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Play2DSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject2D, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void Play2DRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int random = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(soundFXObject2D, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[random];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void Play3DSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject3D, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void Play3DRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int random = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(soundFXObject3D, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[random];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    #region UIs sound effects

    public void PlayDeathTitleSound()
    {
        Play2DSoundFXClip(deathTitleSoundClip, transform, 1f);
    }

    public void PlayDefeatBossTitleSound()
    {
        Play2DSoundFXClip(defeatBossTitleSoundClip, transform, 1f);
    }

    public void PlayAcquireTitleSound()
    {
        Play2DSoundFXClip(acquireTitleSoundClip, transform, 1f);
    }

    public void PlayChangeSelectionSound()
    {
        Play2DSoundFXClip(changeSelectionSoundClip, transform, 1f);
    }

    public void PlayEquipItemSound()
    {
        Play2DSoundFXClip(equipSoundClip, transform, 1f);
    }

    public void PlayUnequipItemSound()
    {
        Play2DSoundFXClip(unequipSoundClip, transform, 1f);
    }

    public void PlayChangeTabSound()
    {
        Play2DSoundFXClip(changeTabSoundClip, transform, 1f);
    }

    #endregion


}
