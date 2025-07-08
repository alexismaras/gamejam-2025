using UnityEngine;
using System;
public class IceCubeUnit : MonoBehaviour
{
    public static event Action<int> OnAssignPointsToPlayer;
    [SerializeField] private GameObject[] _fractures;
    [SerializeField] private AudioSource _iceBreakAudio;
    [SerializeField] private AudioSource _iceCubeDropAudio;

    public int CollectedPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CheckFractures(GameObject fractureBeingDestroyed)
    {
        bool arrayContainsFractures = false;
        foreach (GameObject fracture in _fractures)
        {
            if (fracture != null && fracture != fractureBeingDestroyed)
            {
                arrayContainsFractures = true;
                break;
            }
        }

        if (!arrayContainsFractures)
        {
            Destroy(gameObject);
            
        }
    }

    public void PlayIceBreakSound()
    {
        _iceBreakAudio.Play();
    }

    void OnDestroy()
    {
        _iceCubeDropAudio.Play();
        OnAssignPointsToPlayer?.Invoke(CollectedPoints);
    }
}
