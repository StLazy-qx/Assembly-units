using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : Spawner<CoinPool, Coin>
{
    [SerializeField] private float _cooldown;
    [SerializeField] private Transform _base;

    private float _heightSpawned = 0.8f;
    private float _distanceAroundBase = 8f;
    private WaitForSeconds _delay;

    private List<Vector3> _spawnPoints = new List<Vector3>();

    private void Awake()
    {
        _delay = new WaitForSeconds(_cooldown);
        DistanceBetweenPoint = 3f;
        Vector3 spawnScale = SpawnPlace.localScale;
        Vector3 spawnPosition = SpawnPlace.position;
        MinAreaX = spawnPosition.x - (spawnScale.x * Half);
        MaxAreaX = spawnPosition.x + (spawnScale.x * Half);
        MinAreaZ = spawnPosition.z - (spawnScale.z * Half);
        MaxAreaZ = spawnPosition.z + (spawnScale.z * Half);

        Pool.Initialize();
    }

    private void Start()
    {
        StartCoroutine(Spawned());
    }

    private IEnumerator Spawned()
    {
        while (Pool.IsAllObjectsActive() == false)
        {
            yield return _delay;

            OutputObjects();
        }
    }

    protected override void OutputObjects()
    {
        Vector3 newPoint;

        do
        {
            newPoint = DetermineSpawnCoordinate();
        }
        while (IsPointValid(newPoint) == false);

        _spawnPoints.Add(newPoint);
        GetObject(newPoint);
    }

    private bool IsPointValid(Vector3 point)
    {
        if (_base == null)
            return true;

        if (Vector3.Distance(_base.transform.position, point) < _distanceAroundBase)
            return false;

        foreach (Vector3 existingPoint in _spawnPoints)
        {
            if (Vector3.Distance(existingPoint, point) < DistanceBetweenPoint)
                return false;
        }

        return true;
    }

    private Vector3 DetermineSpawnCoordinate()
    {
        return new Vector3(
            Random.Range(MinAreaX, MaxAreaX),
            _heightSpawned,
            Random.Range(MinAreaZ, MaxAreaZ)
        );
    }
}
