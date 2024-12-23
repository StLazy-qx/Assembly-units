using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class KnightMover : MonoBehaviour
{
    [SerializeField] private Knight _knight;
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private ObstacleMoveHandler _obstacleHandler;

    private Vector3 _beginPosition;
    private Rigidbody _rigidbody;
    private float _moveSpeed = 25f;
    private float _rotationSpeed = 100f;
    private float _distanceToTarget = 0.15f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _obstacleHandler.Init(_rigidbody, _rotationSpeed, _moveSpeed);
    }

    private void Start()
    {
        _beginPosition = transform.position;
    }

    private IEnumerator MoveToTargetAndBack(Coin coin)
    {
        yield return MoveToTarget(coin.transform.position);

        _knight.PickUpCoin(coin);

        yield return MoveToTarget(_beginPosition);

        transform.position = _beginPosition;
        transform.rotation = Quaternion.identity;

        _knight.DropOffCoin();
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        Vector3 flatTarget = new Vector3(target.x, transform.position.y, target.z);

        while (transform.position.IsEnoughClose(flatTarget,
            _distanceToTarget) == false)
        {
            if (_obstacleHandler.IsObstacleOnWay(_checkPoint, out RaycastHit hit))
            {
                yield return _obstacleHandler.GoAroundObstacle(hit);
            }

            MoveTowards(flatTarget);

            yield return null;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        RotateTowards(direction);
        MoveForward();
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        while (Quaternion.Angle(transform.rotation, targetRotation) > _distanceToTarget)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveForward()
    {
        _rigidbody.MovePosition(transform.position +
            transform.forward * _moveSpeed * Time.deltaTime);
    }

    public void GoToTarget(Coin target)
    {
        if (_knight.IsBusy == false)
        {
            _knight.ToBusy();
            StartCoroutine(MoveToTargetAndBack(target));
        }
    }
}
