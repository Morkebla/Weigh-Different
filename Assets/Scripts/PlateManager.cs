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
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing.");
        }
        if (btn == null)
        {
            Debug.LogError("Button (btn) is not assigned.");
        }
    }

    public void RegisterPlate(TriggerPlate plate)
    {
        if (!plates.Contains(plate))
        {
            plates.Add(plate);
            Debug.Log("Plate registered: " + plate.name);
        }
    }

    public void CheckPlates()
    {
        if (levelComplete) return;

        bool allPlatesCorrect = true;

        foreach (TriggerPlate plate in plates)
        {
            if (!plate.IsCorrectMass())
            {
                allPlatesCorrect = false;
                break;
            }
        }

        if (allPlatesCorrect)
        {
            levelComplete = true;
            if (btn != null)
            {
                btn.SetActive(true);
                Debug.Log("Level Complete! Button activated.");
            }
            PlayLevelCompleteSound();
        }
    }

    private void PlayLevelCompleteSound()
    {
        if (levelCompleteClip != null)
        {
            audioSource.PlayOneShot(levelCompleteClip);
            Debug.Log("Level complete sound played.");
        }
        else
        {
            Debug.LogWarning("Level complete sound clip is missing.");
        }
    }
}
