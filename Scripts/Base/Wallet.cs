using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private int _coinCount;

    public event Action<int> BalanceChanged;

    private void Awake()
    {
        _coinCount = 0;
    }

    private void Start()
    {
        BalanceChanged?.Invoke(_coinCount);
    }

    public void AddCoin()
    {
        _coinCount++;

        BalanceChanged?.Invoke(_coinCount);
    }
}
