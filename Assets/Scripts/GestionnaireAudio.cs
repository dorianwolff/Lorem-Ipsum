using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GestionnaireAudio : MonoBehaviour
{
    [SerializeField] 
    private AudioMixer audioMixer;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AjusteVolumeMusique(float volume)
    {
        audioMixer.SetFloat("VolumeMusique", volume);
    }

    /*public void AjusteVolumeEffets(float volume)
    {
        audioMixer.SetFloat("VolumeEffets", volume);
    }*/

}
