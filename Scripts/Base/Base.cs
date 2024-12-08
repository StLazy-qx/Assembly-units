using System;
using UnityEngine;

public class Base : MonoBehaviour
{
    private readonly string _layerName = "Base";
    private readonly int _animationSearchCoin = Animator.StringToHash("ClickOnBase");

    [SerializeField] private KnightPool _knightPool;
    [SerializeField] private CoinPool _coinPool;
    [SerializeField] private Animator _animator;

    private int _keyCheckResources = 0;

    public event Action<int> ResourcesSearching;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_keyCheckResources))
            PerformResourceSearch();
    }

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

    private void PerformResourceSearch()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(_layerName))
            {
                Animator targetAnimator = hitInfo.collider.GetComponent<Animator>();

                if (targetAnimator != null)
                {
                    targetAnimator.SetTrigger(_animationSearchCoin);
                    ResourcesSearching?.Invoke(_coinPool.CountActivatedObjects());

                    CollectResources();
                }
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
}
