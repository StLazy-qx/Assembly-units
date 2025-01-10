using System.Collections.Generic;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    [SerializeField] private CoinSpawner _coinSpawner;

    private Queue<Coin> _queueItems = new Queue<Coin>();

    public int CoinCount => _queueItems.Count;

    public void EnqueueItem(Coin item)
    {
        _queueItems.Enqueue(item);
    }

    public Coin DequeueItem()
    {
        if (_queueItems.Count > 0)
        {
            return _queueItems.Dequeue();
        }

        return null;
    }
}
