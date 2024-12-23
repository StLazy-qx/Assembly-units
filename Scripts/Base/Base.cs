using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private KnightSpawner _spawner; 
    [SerializeField] private MapScanner _mapScanner;
    [SerializeField] private Animator _animator;

    private readonly int _animationSearchCoin = Animator.StringToHash("ClickOnBase");

    public event Action<int> ResourcesSearching;

    private void CollectResources()
    {
        Coin coinTarget = _mapScanner.GetNextCoin();

        if (coinTarget != null)
            _spawner.SendKnightToResource(coinTarget);
    }

    public void PerformResourceSearch()
    {
        if (_animator != null)
            _animator.SetTrigger(_animationSearchCoin);

        CollectResources();
        ResourcesSearching?.Invoke(_mapScanner.ShowNumberCoins());
    }
}
