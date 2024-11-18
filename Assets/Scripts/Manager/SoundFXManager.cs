using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;
    [SerializeField] private AudioSource soundFXObject;

    #region Player animation SFX

    public AudioClip[] footstepSoundClips;
    public AudioClip[] attackSoundClip;
    public AudioClip[] ladderClimbSoundClips;
    public AudioClip hitSoundClips;
    public AudioClip jumpAndLandingSoundClip;
    public AudioClip dashSoundClip;
    public AudioClip spikeDeathSoundClip;
    public AudioClip deathSoundClip;
    public AudioClip spawnSoundClip;
    public AudioClip useSpellClip;
    public AudioClip activeCheckpointClip;
    public AudioClip healingClip;
    public AudioClip healthRestoreSoundClip;
    public AudioClip wallGrabSoundClip;

    #endregion

    #region Menu SFX

    public AudioClip changeSelectionSoundClip;
    public AudioClip equipSoundClip;
    public AudioClip unequipSoundClip;
    public AudioClip changeTabSoundClip;

    #endregion

    #region Title SFX

    public AudioClip acquireTitleSoundClip;
    public AudioClip deathTitleSoundClip;
    public AudioClip defeatBossTitleSoundClip;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        int random = Random.Range(0, audioClip.Length);
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip[random];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    #region UIs sound effects

    public void PlayDeathTitleSound()
    {
        PlaySoundFXClip(deathTitleSoundClip, transform, 1f);
    }

    public void PlayDefeatBossTitleSound()
    {
        PlaySoundFXClip(defeatBossTitleSoundClip, transform, 1f);
    }

    public void PlayAcquireTitleSound()
    {
        PlaySoundFXClip(acquireTitleSoundClip, transform, 1f);
    }

    public void PlayChangeSelectionSound()
    {
        PlaySoundFXClip(changeSelectionSoundClip, transform, 1f);
    }

    public void PlayEquipItemSound()
    {
        PlaySoundFXClip(equipSoundClip, transform, 1f);
    }

    public void PlayUnequipItemSound()
    {
        PlaySoundFXClip(unequipSoundClip, transform, 1f);
    }

    public void PlayChangeTabSound()
    {
        PlaySoundFXClip(changeTabSoundClip, transform, 1f);
    }

    #endregion
}
