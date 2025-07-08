using UnityEngine;
using System.Collections;
public class DestroyableSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefabDestoryable;

    [SerializeField] private Transform[] _spawnPoints;
    private GameObject _currentDestroyable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnDestroyable();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnDestroyable()
    {
        GameObject _instanceDestroyable = Instantiate(_prefabDestoryable);
        _instanceDestroyable.transform.parent = gameObject.transform;
        _instanceDestroyable.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
        _instanceDestroyable.SetActive(true);
        _currentDestroyable = _instanceDestroyable;
    
        StartCoroutine(WaitForDestroy());


    }

    IEnumerator WaitForDestroy()
    {
        while (_currentDestroyable != null)
        {
            Debug.Log("Collectible is not null" + _currentDestroyable != null);
            yield return null;
        }

        SpawnDestroyable();

    }
}