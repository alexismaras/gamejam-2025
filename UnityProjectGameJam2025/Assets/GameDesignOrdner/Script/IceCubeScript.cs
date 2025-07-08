using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeScript : MonoBehaviour
{

    //[SerializeField] private Transform[] _littleBlockSpawn;
    [SerializeField] private GameObject _littleBlock;

    [SerializeField] private List<Transform> _littleBlockSpawn = new List<Transform>();

    private List<Transform> _littleBlockSpawnLaufzeit = new List<Transform>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("_selfDestroy");

        _littleBlockSpawnLaufzeit = _littleBlockSpawn;

        _spawnLittleCubes();
    }

    private IEnumerator _selfDestroy()
    {
        yield return new WaitForSeconds(5);
        SpawnManagerScript.Instance.SpawnObject();
        Destroy(gameObject);
    }

    private void _spawnLittleCubes()
    {
        while (_littleBlockSpawnLaufzeit.Count > 3)
        {
            int randomIndex = Random.Range(0, _littleBlockSpawnLaufzeit.Count);

            // Übernehme den Spawnpoint von der Liste
            Transform CurrentSpawnPoint = _littleBlockSpawnLaufzeit[randomIndex];

            // Kleinen Cube auf pos und rotation 0 instanziieren und Random Zahl würfeln
            GameObject instance = Instantiate(_littleBlock, CurrentSpawnPoint.transform.position, Quaternion.identity);

            // Binde den kleinen Cube an den SpawnPoint
            instance.transform.parent = CurrentSpawnPoint;

            // Entferne ihn aus der Laufzeit Liste, damit er nicht am gleichen Punkt wieder Spawnen kann
            _littleBlockSpawnLaufzeit.RemoveAt(randomIndex);
        }
    }
}
