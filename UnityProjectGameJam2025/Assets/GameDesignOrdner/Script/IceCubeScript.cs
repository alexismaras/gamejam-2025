using System.Collections;
using UnityEngine;

public class IceCubeScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("_selfDestroy");
    }

    private IEnumerator _selfDestroy()
    {
        yield return new WaitForSeconds(2);
        SpawnManagerScript.Instance.SpawnObject();
        Destroy(gameObject);
    }
}
