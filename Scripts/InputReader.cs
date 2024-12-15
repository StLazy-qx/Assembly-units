using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private readonly string _horizontalCameraMove = "Horizontal";
    private readonly string _verticalCameraMove = "Vertical";
    private int _keyClick = 0;
    private float _deltaMove = 0.01f;

    public event Action ClickKeyPressing;
    public event Action<float> HorizontalKeyPressing;
    public event Action<float> VerticalKeyPressing;

    private void Update()
    {
        float moveX = Input.GetAxis(_horizontalCameraMove);
        float moveZ = Input.GetAxis(_verticalCameraMove);

        if (Input.GetMouseButtonDown(_keyClick))
        {
            ClickKeyPressing?.Invoke();
        }

        if (Mathf.Abs(moveX) > _deltaMove || Mathf.Abs(moveZ) > _deltaMove)
        {
            HorizontalKeyPressing?.Invoke(moveX);
            VerticalKeyPressing?.Invoke(moveZ);
        }
    }
}
