using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinPool : Pool<Coin>
{
    private Queue<Transform> _positionQueue = new Queue<Transform>();
    private HashSet<Vector3> _activeTargets = new HashSet<Vector3>();

    private void CollectCoinTargets()
    {
        foreach (Coin coin in ObjectsPool)
        {
            if (coin.IsActive)
            {
                Transform targetCoin = coin.transform;

                if (!_positionQueue.Contains(targetCoin) && 
                    _activeTargets.Contains(targetCoin.position) == false)
                {
                    _positionQueue.Enqueue(targetCoin);
                }
            }
        }
    }

    private void ReleasePositions()
    {
        int elementsToRemove = _activeTargets.Count - 10;

        for (int i = 0; i < elementsToRemove; i++)
        {
            _activeTargets.Remove(_activeTargets.First());
        }
    }

    public Transform GetPositionCoin()
    {
        CollectCoinTargets();

        while (_positionQueue.Count > 0)
        {
            Transform target = _positionQueue.Dequeue();

            if (_activeTargets.Contains(target.position) == false)
            {
                _activeTargets.Add(target.position);

                return target;
            }
        }

        ReleasePositions();

        return null;
    }
}
