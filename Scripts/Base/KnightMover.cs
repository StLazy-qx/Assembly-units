using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class KnightMover : MonoBehaviour
{
    [SerializeField] private Knight _knight;
    [SerializeField] private Transform _checkPoint;

    private Vector3 _beginPosition;
    private Rigidbody _rigidbody;
    private float _distanceToTarget = 0.1f;
    private float _rotationSpeed = 50f;
    private float _angleRotation = 45f;
    private float _moveSpeed = 30f;
    private float _forwardDistance;
    private float _maxPassDistance = 2f;
    private bool _obstacleDetected;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _beginPosition = transform.position;
    }

    private IEnumerator MoveToTargetAndBack(Vector3 target)
    {
        _knight.ToBusy();

        yield return MoveTowards(target);

        if (_knight.HasCoin())
            yield return MoveTowards(_beginPosition);

        _knight.ToFree();
    }

    private IEnumerator MoveTowards(Vector3 target)
    {
        while (transform.position.IsEnoughClose(target, _distanceToTarget) == false)
        {
            if (IsObstacleOnWay())
            {
                _obstacleDetected = true;

                yield return AvoidObstacle();

                _obstacleDetected = false;

                continue;
            }

            if (_obstacleDetected == false)
            {
                Vector3 targetDirection = (target - transform.position).normalized;

                RotateTowards(targetDirection);
                MoveForward();
            }

            yield return null;
        }
    }

    private IEnumerator AvoidObstacle()
    {
        if (Physics.Raycast(_checkPoint.position, transform.forward, out RaycastHit hit, 1.5f))
        {
            if (IsObstacleOnWay())
            {
                Quaternion avoidanceRotation = Quaternion.Euler(0, 
                    transform.eulerAngles.y + TurnAwayObstacle(hit), 0);

                while (Quaternion.Angle(transform.rotation, avoidanceRotation) > _distanceToTarget)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                        avoidanceRotation, _rotationSpeed * Time.deltaTime);

                    yield return null;
                }

                if (IsObstacleOnWay() == false)
                {
                    _forwardDistance = 0f;

                    while (_forwardDistance < _maxPassDistance)
                    {
                        MoveForward();

                        _forwardDistance += _moveSpeed * Time.deltaTime;

                        yield return null;
                    }
                }
            }
        }
    }

    private bool IsObstacleOnWay()
    {
        if (Physics.Raycast(_checkPoint.position, transform.forward, 
            out RaycastHit hit, 1.5f))
        {
            if (hit.collider.gameObject.TryGetComponent<Base>(out _))
                return true;
        }

        return false;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation
            (direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,
            targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private float TurnAwayObstacle(RaycastHit hit)
    {
        Vector3 obstacleNormal = hit.normal;
        float angle = Vector3.SignedAngle(transform.forward, obstacleNormal, Vector3.up);

        return angle > 0 ? _angleRotation : (-1 * _angleRotation);
    }

    private void MoveForward()
    {
        _rigidbody.MovePosition(transform.position 
            + transform.forward * _moveSpeed * Time.deltaTime);
    }

    public void GoToTarget(Vector3 target)
    {
        if (_knight.IsBusy == false)
        {
            StartCoroutine(MoveToTargetAndBack(target));
        }
    }
}
