using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _joystickBackground;
    [SerializeField] private RectTransform _joystickHandle;
    private Vector2 _inputVector;

    public Action OnStartDragJoystick;
    public Action OnEndDragJoystick;


    public void OnBeginDrag(PointerEventData eventData)
    {
        OnStartDragJoystick?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground, eventData.position, eventData.pressEventCamera, out pos))
        {
            // 배경 내에서의 상대적 위치 계산
            pos.x = (pos.x / _joystickBackground.sizeDelta.x);
            pos.y = (pos.y / _joystickBackground.sizeDelta.y);

            _inputVector = new Vector2(pos.x * 2, pos.y * 2);
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

            // 핸들 이동
            _joystickHandle.anchoredPosition = new Vector2(_inputVector.x * (_joystickBackground.sizeDelta.x / 2), _inputVector.y * (_joystickBackground.sizeDelta.y / 2));
        }
    }

    public Vector3 GetMoveDirection()
    {
        // 쿼터뷰 시점(45도 회전)에 맞게 입력 방향 변환
        return new Vector3(_inputVector.x, 0, _inputVector.y).normalized;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 시 초기화
        _inputVector = Vector2.zero;
        _joystickHandle.anchoredPosition = Vector2.zero;

        OnEndDragJoystick?.Invoke();
    }

}