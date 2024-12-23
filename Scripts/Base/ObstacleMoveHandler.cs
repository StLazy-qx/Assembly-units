using System.Collections;
using UnityEngine;

public class ObstacleMoveHandler : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _moveSpeed;
    private float _rotationSpeed = 100f;
    private float _angleRotation = 60f;
    private float _distanceToTarget = 1f;
    private float _maxAvoidanceDistance = 3f;
    private float _detectObstacleDistance = 1f;

    private Quaternion GetAvoidanceRotation(RaycastHit hit)
    {
        Vector3 obstacleNormal = hit.normal;
        float angle = Vector3.SignedAngle(transform.forward,
            obstacleNormal, Vector3.up);

        return Quaternion.Euler(0, transform.eulerAngles.y +
            (angle >= 0 ? _angleRotation : -_angleRotation), 0);
    }

    private IEnumerator Rotate(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) >
            _distanceToTarget)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                targetRotation, _rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator MoveForwardForDistance()
    {
        float traveledDistance = 0f;

        while (traveledDistance < _maxAvoidanceDistance)
        {
            _rigidbody.MovePosition(transform.position +
            transform.forward * _moveSpeed * Time.deltaTime);

            traveledDistance += _moveSpeed * Time.deltaTime;

            yield return null;
        }
    }

    public void Init(Rigidbody rigidbody, float moveSpeed)
    {
        _rigidbody = rigidbody;
        _moveSpeed = moveSpeed;
    }

    public bool IsObstacleOnWay(Transform checkPoint, out RaycastHit hit)
    {
        return Physics.Raycast(checkPoint.position, transform.forward,
            out hit, _detectObstacleDistance) &&
               hit.collider.gameObject.TryGetComponent<Building>(out _);
    }

    public IEnumerator GoAroundObstacle(RaycastHit hit)
    {
        Quaternion avoidanceRotation = GetAvoidanceRotation(hit);

        yield return Rotate(avoidanceRotation);
        yield return MoveForwardForDistance();
    }
}
