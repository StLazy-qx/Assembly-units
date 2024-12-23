using UnityEngine;

public class MapScanner : MonoBehaviour
{
    [SerializeField] private CoinSpawner _coinSpawner;

    public int ShowNumberCoins()
    {
        return _coinSpawner.CountDeliveredCoins();
    }

    public Coin GetNextCoin()
    {
        return _coinSpawner.GetNextCoin();
    }
}
