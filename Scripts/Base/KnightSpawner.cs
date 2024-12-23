using System.Collections.Generic;
using UnityEngine;

public class KnightSpawner : Spawner<Knight>
{
    [SerializeField] private Wallet _wallet;

    private float _distanceBetweenPoint = 1f;

    private List<Vector3> _spawnPoints = new List<Vector3>();

    private void Start()
    {
        OutputObjects();
    }

    protected override void OutputObjects()
    {
        GenerateSpawnPoints();

        foreach (Vector3 position in _spawnPoints)
        {
            Knight knight = PoolObjects.GetObject(position);

            knight.Initialize(_wallet, position);
        }
    }

    private void GenerateSpawnPoints()
    {
        _spawnPoints.Clear();

        for (int i = 0; i < PoolObjects.Count; i++)
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
            if (Vector3.Distance(existingPoint, point) < _distanceBetweenPoint)
                return false;
        }

        return true;
    }

    public void SendKnightToResource(Coin target)
    {
        foreach (Knight knight in PoolObjects.GetListActiceObjects())
        {
            if (knight.IsBusy == false)
            {
                if (knight.TryGetComponent(out KnightMover knightMover))
                {
                    knightMover.GoToTarget(target);

                    return;
                }
            }
        }
    }
}
