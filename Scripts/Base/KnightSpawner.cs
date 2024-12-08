using System.Collections.Generic;
using UnityEngine;

public class KnightSpawner : Spawner<KnightPool, Knight>
{
    private List<Vector3> _spawnPoints = new List<Vector3>();

    private void Awake()
    {
        DistanceBetweenPoint = 0.25f;

        if (SpawnPlace.TryGetComponent(out BoxCollider boxCollider))
        {
            Vector3 size = boxCollider.size;
            Vector3 center = boxCollider.center;
            Vector3 spawnPosition = SpawnPlace.position;
            MinAreaX = spawnPosition.x + center.x - size.x * Half;
            MaxAreaX = spawnPosition.x + center.x + size.x * Half;
            MinAreaZ = spawnPosition.z + center.z - size.z * Half;
            MaxAreaZ = spawnPosition.z + center.z + size.z * Half;
        }
        
        Pool.Initialize();
    }

    private void Start()
    {
        OutputObjects();
    }

    protected override void OutputObjects()
    {
        GenerateSpawnPoints();

        foreach (Vector3 position in _spawnPoints)
        {
            GetObject(position);
        }
    }

    private void GenerateSpawnPoints()
    {
        _spawnPoints.Clear();

        for (int i = 0; i < Pool.Count; i++)
        {
            Vector3 newPoint = DetermineSpawnCoordinate();

            if (IsPointValid(newPoint))
                _spawnPoints.Add(newPoint);
            else
                i--;
        }
    }

    private bool IsPointValid(Vector3 point)
    {
        foreach (Vector3 existingPoint in _spawnPoints)
        {
            if (Vector3.Distance(existingPoint, point) < DistanceBetweenPoint)
                return false;
        }

        return true;
    }

    private Vector3 DetermineSpawnCoordinate()
    {
        return new Vector3(Random.Range(MinAreaX, MaxAreaX),
            SpawnPlace.position.y, Random.Range(MinAreaZ, MaxAreaZ));
    }
}
