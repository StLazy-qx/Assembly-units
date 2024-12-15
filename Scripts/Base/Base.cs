using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private KnightPool _knightPool;
    [SerializeField] private CoinPool _coinPool;
    [SerializeField] private Animator _animator;

    private readonly int _animationSearchCoin = Animator.StringToHash("ClickOnBase");

    public event Action<int> ResourcesSearching;

    private void CollectResources()
    {
        if (_coinPool.CountActivatedObjects() <= 0)
            return;

        if (_knightPool.IsFreeKnight() == false)
            return;

        Transform coinPosition = _coinPool.GetPositionCoin();

        if (coinPosition != null)
        {
            Vector3 groundPosition = GetGroundPosition(coinPosition.position);

            if (groundPosition != Vector3.zero)
            {
                _knightPool.OrderToMove(groundPosition);
            }
        }
    }

    private Vector3 GetGroundPosition(Vector3 position)
    {
        Ray ray = new Ray(position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            return hit.point;

        return Vector3.zero;
    }

    public void PerformResourceSearch()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(_animationSearchCoin);
            ResourcesSearching?.Invoke(_coinPool.CountActivatedObjects());

            CollectResources();
        }
    }
}
