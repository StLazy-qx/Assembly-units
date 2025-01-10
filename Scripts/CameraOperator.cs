using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraOperator : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Base _base;
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _minX = -30f;
    [SerializeField] private float _minZ = -17f;
    [SerializeField] private float _maxX = 30f;
    [SerializeField] private float _maxZ = 30f;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _playerInput.MapCoinChecking += OnBaseCLick;
        _playerInput.HorizontalamCameraMoving += OnHorizontalInput;
        _playerInput.VerticalCameraMoving += OnVerticalInput;
    }

    private void OnDisable()
    {
        _playerInput.MapCoinChecking -= OnBaseCLick;
        _playerInput.HorizontalamCameraMoving -= OnHorizontalInput;
        _playerInput.VerticalCameraMoving -= OnVerticalInput;
    }

    private void OnBaseCLick()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (((1 << hitInfo.collider.gameObject.layer) & _targetLayer) != 0)
            {
                _base.PerformResourceSearch();
            }
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

    private void OnHorizontalInput(float horizontalInput)
    {
        Vector3 movement = new Vector3(-horizontalInput, 0f, 0f);

        MoveCamera(movement);
    }

    private void OnVerticalInput(float verticalInput)
    {
        Vector3 movement = new Vector3(0f, 0f, -verticalInput);

        MoveCamera(movement);
    }
}
