using System;
using UnityEngine;

public class DeadZoneScript : MonoBehaviour
{

    public static event Action OnDeadZoneEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnDeadZoneEntered?.Invoke();
        }
    }
}
