using UnityEngine;
using System;
using System.Collections;
public class CollectibleSpawner : MonoBehaviour
{
    private GameObject _prefabCollectible;
    private GameObject _currentCollectible;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnCollectible();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnCollectible()
    {
        GameObject _instanceCollectible = Instantiate(_prefabCollectible);
        _instanceCollectible.transform.parent = gameObject.transform;
        _instanceCollectible.transform.position = new Vector3(0, 0, 0);
        _currentCollectible = _instanceCollectible;

        StartCoroutine(WaitForCollected());


    }

    IEnumerator WaitForCollected()
    {
        while (_currentCollectible != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(10);

        SpawnCollectible();

    }
}
