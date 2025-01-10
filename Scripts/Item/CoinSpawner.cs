using System.Collections;
using UnityEngine;

public class CoinSpawner : Spawner<Coin>
{
    [SerializeField] private ResourceHandler _resourceHandler;
    [SerializeField] private float _cooldown;
    [SerializeField] private Base _base;
    [SerializeField] private float _distanceAroundBase = 8f;

    private float _heightSpawned = 0.8f;
    private WaitForSeconds _delay;

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

            Coin newCoin = PoolObjects.GetObject(newPostion);
            _resourceHandler.EnqueueItem(newCoin);
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
}
