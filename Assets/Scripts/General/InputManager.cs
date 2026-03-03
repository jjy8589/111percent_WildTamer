using System;
using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private DynamicJoystick joystick;

    public event Action<Vector2> OnTouchBeganHandler;
    public event Action<Vector2> OnTouchMoveHandler;
    public event Action<Vector2> OnTouchEndHandler;

    private Vector2 _touchPosition;
    [SerializeField] private float _dragTime = 0.13f;
    private bool _isDragging;
    private Coroutine _dragCoroutine;

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touchPosition = Input.mousePosition;
            ShowJoystick(_touchPosition);
            OnTouchBeganHandler?.Invoke(_touchPosition);
            _dragCoroutine = StartCoroutine(DetectDrag());
        }

        if (Input.GetMouseButtonUp(0))
        {
            _touchPosition = Input.mousePosition;
            HideJoystick();
            OnTouchEndHandler?.Invoke(_touchPosition);
            if (_dragCoroutine != null) StopCoroutine(_dragCoroutine);
            _isDragging = false;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _touchPosition = touch.position;
                ShowJoystick(_touchPosition);
                OnTouchBeganHandler?.Invoke(_touchPosition);
                _dragCoroutine = StartCoroutine(DetectDrag());
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _touchPosition = touch.position;
                HideJoystick();
                OnTouchEndHandler?.Invoke(_touchPosition);
                if (_dragCoroutine != null) StopCoroutine(_dragCoroutine);
                _isDragging = false;
            }
        }
    }

    private IEnumerator DetectDrag()
    {
        yield return new WaitForSeconds(_dragTime);
        _isDragging = true;

        while (_isDragging)
        {
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                _touchPosition = Input.mousePosition;
                OnTouchMoveHandler?.Invoke(_touchPosition);
            }
            yield return null;
        }
    }

    private void ShowJoystick(Vector2 screenPos)
    {
        //joystick.SetPosition(screenPos);
        joystick.gameObject.SetActive(true);
    }

    private void HideJoystick()
    {
        joystick.gameObject.SetActive(false);
    }
}