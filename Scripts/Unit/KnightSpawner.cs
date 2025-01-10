using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnightSpawner : Spawner<Knight>
{
    [SerializeField] private Wallet _wallet;

    private float _distanceBetweenPoint = 0.3f;

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

            knight.Initialize(_wallet);
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

    public void SendUnitToResource(Knight knight, Coin target)
    {
        if (knight.TryGetComponent(out KnightMover knightMover))
        {
            knightMover.GoToTarget(target);
        }
    }

    public bool TryGetFreeUnit(out Knight knight)
    {
        knight = PoolObjects.GetListActiveObjects().
            FirstOrDefault(knight => knight.IsBusy == false);

        return knight != null;
    }
}
