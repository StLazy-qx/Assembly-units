using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class ResourceGetter : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Knight knight))
        {
            Coin coin = knight.LayOutResource();

            if(coin != null)
                _wallet.AddCoin();
        }
    }
}
