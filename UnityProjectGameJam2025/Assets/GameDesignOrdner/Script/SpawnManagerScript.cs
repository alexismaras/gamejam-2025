using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManagerScript : MonoBehaviour
{

    public static SpawnManagerScript Instance;

    [SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();
    private GameObject _placeHolder;

    [SerializeField] private GameObject _objectToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        SpawnObject();
    }


    public void SpawnObject()
    {
        int RandomIndex = Random.Range(0, _spawnPoints.Count);

        if (_placeHolder != _spawnPoints[RandomIndex])
        {
            _placeHolder = _spawnPoints[RandomIndex];
            _spawnPoints.Add(_placeHolder);
            Debug.Log("Neuer Cube gespawned!");
            GameObject instance = Instantiate(_objectToSpawn, _spawnPoints[RandomIndex].transform.position, Quaternion.identity);
            _spawnPoints.RemoveAt(RandomIndex);
        }
        else if (_placeHolder == _spawnPoints[RandomIndex])
        {
            SpawnObject();
            return;
        }
        else if (_placeHolder == null)
        {
            _placeHolder = _spawnPoints[RandomIndex];
            Debug.Log("Neuer Cube gespawned!");
            GameObject instance = Instantiate(_objectToSpawn, _spawnPoints[RandomIndex].transform.position, Quaternion.identity);
            _spawnPoints.RemoveAt(RandomIndex);
        }
    }
}
