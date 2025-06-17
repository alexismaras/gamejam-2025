using System.Collections;
using UnityEngine;

public class PlayerPlatzhalterV2Script : MonoBehaviour
{
    public float GrannySaviorTime = 2;
    GameObject _granny;

    private void OnTriggerEnter(Collider other)             // Starte ein Invoke wenn man in der Granny Range ist
    {
        if (other.gameObject.CompareTag("FriendlyNPC"))
        {
            _granny = other.gameObject;
            Invoke("GrannyCountdown", GrannySaviorTime);
        }
    }

    private void OnTriggerExit(Collider other)              // Breche den Granny Invoke ab
    {
        if (other.gameObject.CompareTag("FriendlyNPC"))
        {
            CancelInvoke("GrannyCountdown");
        }
    }

    void GrannyCountdown()                                  // Teile mit, dass Granny gerettet wurde und zerstöre die Granny dann
    {
        Debug.Log("Granny gerettet!");
        if (_granny != null)
        {
            Destroy(_granny.gameObject);
        }
    }
}
