using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : PoolableObject
{
    [SerializeField] private Transform _holdPoint;

    private readonly string _layerName = "Knight";

    private Coin _currentCoin;
    private Wallet _wallet;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        IsBusy = false;
        _currentCoin = null;
        gameObject.layer = LayerMask.NameToLayer(_layerName);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(_layerName),
            LayerMask.NameToLayer(_layerName));
    }

    public void Initialize(Wallet wallet)
    {
        _wallet = wallet;
    }

    public void ToBusy()
    {
        IsBusy = true;
    }

    public void PickUpCoin(Coin coin)
    {
        coin.transform.SetParent(_holdPoint);
        coin.SetHoldState(_holdPoint.position);

        _currentCoin = coin;
    }

    public void DropOffCoin()
    {
        _currentCoin.StopHolded();
        _wallet.AddCoin();

        IsBusy = false;
        _currentCoin = null;
    }
}
