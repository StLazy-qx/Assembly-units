using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private KnightSpawner _spawner;
    [SerializeField] private ResourceHandler _mapScanner;
    [SerializeField] private Animator _animator;

    private readonly int _animationSearchCoin = Animator.StringToHash("ClickOnBase");

    public event Action<int> ResourcesSearching;

    private void AssignResourceKnight()
    {
        if (_spawner.TryGetFreeUnit(out Knight freeKnight) == false)
            return;

        Coin coinTarget = _mapScanner.DequeueItem();

        if (coinTarget != null)
            _spawner.SendUnitToResource(freeKnight, coinTarget);
    }

    public void PerformResourceSearch()
    {
        _animator.SetTrigger(_animationSearchCoin);
        AssignResourceKnight();
        ResourcesSearching?.Invoke(_mapScanner.CoinCount);
    }
}
