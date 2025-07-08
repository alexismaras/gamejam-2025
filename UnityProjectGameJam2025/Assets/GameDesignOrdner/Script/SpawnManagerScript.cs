using UnityEngine;
using System.Collections;

public class SpawnManagerScript : MonoBehaviour
{

    public static SpawnManagerScript Instance;

    [SerializeField] private GameObject[] _spawnPoint;

    [SerializeField] private GameObject _objectToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        SpawnObject();
    }


    public void SpawnObject()
    {
        Debug.Log("RUF SPAWN OBJECT!");
        int RandomIndex = Random.Range(0, _spawnPoint.Length);
        GameObject instance = Instantiate(_objectToSpawn, _spawnPoint[RandomIndex].transform.position, Quaternion.identity);
    }
}
