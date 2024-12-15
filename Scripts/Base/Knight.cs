using UnityEngine;

public class Knight : ObjectablePoll
{
    [field: SerializeField]
    public Transform HoldPoint { get; private set; }

    private Coin _coin;
    private readonly string _layerName = "Knight";

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        IsBusy = false;
        _coin = null;
        gameObject.layer = LayerMask.NameToLayer(_layerName);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer(_layerName), 
            LayerMask.NameToLayer(_layerName));
    }

    public void ToBusy()
    {
        IsBusy = true;
    }

    public void ToFree()
    {
        IsBusy = false;
    }

    public bool HasCoin() => _coin != null;

    public void PickUpCoin(Coin coin)
    {
        if (HasCoin() == false && coin != null)
            _coin = coin;
    }

    public Coin LayOutResource()
    {
        if (HasCoin())
        {
            Coin tempCoin = _coin;

            _coin.StopFollowing();
            _coin = null;

            return tempCoin;
        }

        return null;
    }
}