using UnityEngine;

public class Knight : ObjectablePoll
{
    [SerializeField] private Transform _holdPoint;

    private readonly string _layerName = "Knight";
    private Coin _currentCoin;
    private Wallet _wallet;
    private Vector3 _initialPosition;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        IsBusy = false;
        _currentCoin = null;
        gameObject.layer = LayerMask.NameToLayer(_layerName);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(_layerName), 
            LayerMask.NameToLayer(_layerName));
    }

    public void Initialize(Wallet wallet, Vector3 position)
    {
        _wallet = wallet;
        _initialPosition = position;
    }

    public void ToBusy()
    {
        IsBusy = true;
    }

    public bool HasCoin() => _currentCoin != null;

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