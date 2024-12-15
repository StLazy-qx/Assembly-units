using UnityEngine;

public class Coin : ObjectablePoll
{
    [SerializeField] private float _speedRotation;

    private bool _isRotating;
    private bool _isHolded;
    private Transform _followTarget;
    private Quaternion _beginRotation = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _layRotation = Quaternion.Euler(0f, 0f, 0f);

    public bool IsHolded => _isHolded;

    private void Start()
    {
        SetBeginState();
    }

    private void Update()
    {
        if (_isRotating)
            Rotate();

        if (_followTarget != null)
        {
            LayDown();

            transform.position = _followTarget.position;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Knight knight))
        {
            if (knight.HasCoin())
                return;

            knight.PickUpCoin(this);
            SetFollowTarget(knight.HoldPoint);

            _isHolded = true;
        }
    }

    private void SetBeginState()
    {
        transform.rotation = _beginRotation;
        _followTarget = null;
        _isHolded = false;
        _isRotating = true;
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward * _speedRotation * Time.deltaTime);
    }

    private void LayDown()
    {
        _isRotating = false;
        transform.rotation = _layRotation;
    }

    private void SetFollowTarget(Transform target)
    {
        _followTarget = target;
    }

    public void StopFollowing()
    {
        SetBeginState();
        Deactivate();
    }
}
