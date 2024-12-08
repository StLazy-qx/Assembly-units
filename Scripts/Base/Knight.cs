using UnityEngine;

public class Knight : ObjectablePoll
{
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

    public bool HasCoin()
    {
        return _coin != null;
    }

    public void PickUpCoin(Coin coin)
    {
        if (HasCoin() == false)
        {
            _coin = coin;
        }
    }

    public Coin LayOutResource()
    {
        if (HasCoin())
        {
            Coin tempCoin = _coin;
            _coin = null;

            return tempCoin;
        }

        return null;
    }
}