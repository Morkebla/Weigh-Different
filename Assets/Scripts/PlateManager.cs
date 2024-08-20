using System.Collections.Generic;
using UnityEngine;

public class PlateManager : MonoBehaviour
{
    public GameObject btn;
    public AudioClip levelCompleteClip;
    private AudioSource audioSource;
    private List<TriggerPlate> plates = new List<TriggerPlate>();
    private bool levelComplete = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void RegisterPlate(TriggerPlate plate)
    {
        if (!plates.Contains(plate))
        {
            plates.Add(plate);
        }
    }

    public void CheckPlates()
    {
        if (levelComplete) return;

        foreach (TriggerPlate plate in plates)
        {
            if (!plate.IsCorrectMass())
            {
                return;
            }
        }

        levelComplete = true;
        btn.SetActive(true);
        PlayLevelCompleteSound();
    }

    private void PlayLevelCompleteSound()
    {
        if (levelCompleteClip != null)
        {
            audioSource.PlayOneShot(levelCompleteClip);
        }
    }
}
