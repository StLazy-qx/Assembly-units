using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private readonly string _horizontalCameraMove = "Horizontal";
    private readonly string _verticalCameraMove = "Vertical";

    private int _keyCheckResourcesOnMap = 0;
    private float _deltaMove = 0.01f;

    public event Action MapCoinChecking;
    public event Action<float> HorizontalamCameraMoving;
    public event Action<float> VerticalCameraMoving;

    private void Update()
    {
        float moveX = Input.GetAxis(_horizontalCameraMove);
        float moveZ = Input.GetAxis(_verticalCameraMove);

        if (Input.GetMouseButtonDown(_keyCheckResourcesOnMap))
        {
            MapCoinChecking?.Invoke();
        }

        if (Mathf.Abs(moveX) > _deltaMove || Mathf.Abs(moveZ) > _deltaMove)
        {
            HorizontalamCameraMoving?.Invoke(moveX);
            VerticalCameraMoving?.Invoke(moveZ);
        }
    }
}
