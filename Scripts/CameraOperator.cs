using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraOperator : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _minX = -30f;
    [SerializeField] private float _minZ = -17f;
    [SerializeField] private float _maxX = 30f;
    [SerializeField] private float _maxZ = 30f;

    private Vector3 _movementInput;
    private float _deltaMove = 0.01f;
    private readonly string _horizontalCameraMove = "Horizontal";
    private readonly string _verticalCameraMove = "Vertical";

    void Update()
    {
        CheckGetMove();
    }

    private void CheckGetMove()
    {
        float moveX = Input.GetAxis(_horizontalCameraMove);
        float moveZ = Input.GetAxis(_verticalCameraMove);

        if (Mathf.Abs(moveX) > _deltaMove || Mathf.Abs(moveZ) > _deltaMove)
        {
            _movementInput = new Vector3(-moveX, 0f, -moveZ);

            MoveCamera(_movementInput);
        }
    }

    private void MoveCamera(Vector3 movement)
    {
        Vector3 newPosition = transform.position + movement 
            * _moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, _minZ, _maxZ);
        transform.position = newPosition;
    }
}
