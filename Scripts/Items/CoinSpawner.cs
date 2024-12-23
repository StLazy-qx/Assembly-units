using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinSpawner : Spawner<Coin>
{
    [SerializeField] private float _cooldown;
    [SerializeField] private Base _base;
    [SerializeField] private float _distanceAroundBase = 8f;

    private float _heightSpawned = 0.8f;
    private WaitForSeconds _delay;
    private HashSet<Coin> _activeCoins = new HashSet<Coin>();

    protected override void OnAwake()
    {
        _delay = new WaitForSeconds(_cooldown);
    }

    private void Start()
    {
        StartCoroutine(PerformProduction());
    }

    protected override void OutputObjects()
    {
        Vector3 spawnPosition = DetermineSpawnCoordinate();

        if (IsValidPoint(spawnPosition))
        {
            Vector3 newPostion = new Vector3(spawnPosition.x, 
                _heightSpawned, spawnPosition.z);
            PoolObjects.GetObject(newPostion);
        }
    }

    private IEnumerator PerformProduction()
    {
        while (PoolObjects.IsAllObjectsActive() == false)
        {
            OutputObjects();

            yield return _delay;
        }
    }

    private bool IsValidPoint(Vector3 point)
    {
        return (Vector3.Distance(_base.transform.position, point) 
            >= _distanceAroundBase);
    }

    private void ReleaseOldCoins()
    {
        int maxActiveCoins = 3;

        if (_activeCoins.Count > maxActiveCoins)
        {
            var coinsToRemove = _activeCoins.Take
                (_activeCoins.Count - maxActiveCoins).ToList();

            foreach (var coin in coinsToRemove)
            {
                _activeCoins.Remove(coin);
            }
        }
    }

    public Coin GetNextCoin()
    {
        List<Coin> tempList = PoolObjects.GetListActiceObjects();

        foreach (Coin coin in tempList)
        {
            if (_activeCoins.Contains(coin) == false)
            {
                _activeCoins.Add(coin);
                
                return coin;
            }
        }

        ReleaseOldCoins();

        return null;
    }

    public int CountDeliveredCoins()
    {
        return PoolObjects.CountActivatedObjects();
    }
}
