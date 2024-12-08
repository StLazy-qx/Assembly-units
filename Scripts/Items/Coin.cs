using UnityEngine;

public class Coin : ObjectablePoll
{
    [SerializeField] private float _speedRotation;

    private void Update()
    {
        Rotate();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Knight knight))
        {
            knight.PickUpCoin(this);
        }

        Deactivate();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward * _speedRotation * Time.deltaTime);
    }
}
