using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 키 입력시 컨트롤 타겟의 해당 함수 실행
public class PlayerControl : Control
{
    public Vector2 inputMoveVector;
    public Vector2 inputRotateVector;

    public Transform mainCamera;


    void FixedUpdate()
    {
        if (controlTarget.isCursor)
            return;

        controlTarget.Move(inputMoveVector);
        controlTarget.Rotate(inputRotateVector);
    }


    // WASD 움직임
    void OnMove(InputValue value)
    {
        inputMoveVector = value.Get<Vector2>();
    }

    // 마우스
    void OnLook(InputValue value)
    {
        inputRotateVector = value.Get<Vector2>();
    }

    // 상호작용, F
    void OnInteract()
    {
        controlTarget.Interact();
    }

    // 마우스 좌클릭
    void OnLMB()
    {
        if (controlTarget.isCursor)
            return;

        controlTarget.LMB();
    }

    // 마우스 우클릭
    void OnRMB()
    {
        if (controlTarget.isCursor)
            return;

        controlTarget.RMB();
    }

    // LShift
    void OnLShift()
    {
        controlTarget.LShift();
    }

    // C
    void OnKeyboardC()
    {
        controlTarget.KeyboardC();
    }

    // Z
    void OnKeyboardZ()
    {
        controlTarget.KeyboardZ();
    }

    // R
    void OnKeyboardR()
    {
        controlTarget.KeyboardR();
    }

    // Tab
    void OnKeyboardTab()
    {
        controlTarget.KeyboardTab();
    }

    // SpaceBar
    void OnSpaceBar()
    {
        controlTarget.SpaceBar();
    }

    // 1
    void OnKeyboard1()
    {
        controlTarget.Keyboard1();
    }

    // 2
    void OnKeyboard2()
    {
        controlTarget.Keyboard2();
    }

    // 3
    void OnKeyboard3()
    {
        controlTarget.Keyboard3();
    }

    // 4
    void OnKeyboard4()
    {
        controlTarget.Keyboard4();
    }

}
