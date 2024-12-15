using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class KnightMover : MonoBehaviour
{
    [SerializeField] private Knight _knight;
    [SerializeField] private Transform _checkPoint;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 25f;
    [SerializeField] private float _rotationSpeed = 45f;
    [SerializeField] private float _angleRotation = 45f;
    [SerializeField] private float _distanceToTarget = 0.1f;
    [SerializeField] private float _detectObstacleDistance = 1.5f;
    [SerializeField] private float _maxAvoidanceDistance = 2f;

    private Vector3 _beginPosition;
    private Rigidbody _rigidbody;

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
            if (IsObstacleOnWay(out RaycastHit hit))
            {
                yield return HandleObstacle(hit);
            }
            else
            {
                MoveTowardsTarget(target);
            }

            yield return null;
        }
    }

    private IEnumerator HandleObstacle(RaycastHit hit)
    {
        Quaternion avoidanceRotation = GetAvoidanceRotation(hit);

        yield return RotateTowards(avoidanceRotation);

        yield return MoveForwardForDistance(_maxAvoidanceDistance);
    }

    private bool IsObstacleOnWay(out RaycastHit hit)
    {
        return Physics.Raycast(_checkPoint.position, transform.forward, 
            out hit, _detectObstacleDistance) &&
               hit.collider.gameObject.TryGetComponent<Building>(out _);
    }

    private Quaternion GetAvoidanceRotation(RaycastHit hit)
    {
        Vector3 obstacleNormal = hit.normal;
        float angle = Vector3.SignedAngle(transform.forward, 
            obstacleNormal, Vector3.up);

        return Quaternion.Euler(0, transform.eulerAngles.y + 
            (angle > 0 ? _angleRotation : -_angleRotation), 0);
    }

    private IEnumerator RotateTowards(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 
            _distanceToTarget)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                targetRotation, _rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator MoveForwardForDistance(float distance)
    {
        float traveledDistance = 0f;

        while (traveledDistance < distance)
        {
            MoveForward();

            traveledDistance += _moveSpeed * Time.deltaTime;

            yield return null;
        }
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        RotateTowards(direction);
        MoveForward();
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, 
            targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void MoveForward()
    {
        _rigidbody.MovePosition(transform.position + 
            transform.forward * _moveSpeed * Time.deltaTime);
    }

    public void GoToTarget(Vector3 target)
    {
        if (_knight.IsBusy == false)
            StartCoroutine(MoveToTargetAndBack(target));
    }
}
