using System.Collections;
using UnityEngine;

public class PlayerPlatzhalterV2Script : MonoBehaviour
{
    bool _isInGrannyRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FriendlyNPC"))
        {
            _isInGrannyRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FriendlyNPC"))
        {
            _isInGrannyRange = false;
        }
    }

    IEnumerator GrannyCountdown()
    {
        yield return new WaitForSeconds(1);
        if (!_isInGrannyRange)
        {
            yield return;
        }
    }
}
