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
        if (GameManager.isPause)
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
        if (GameManager.isPause)
            return;

        controlTarget.LMB();
    }

    // 마우스 우클릭
    void OnRMB()
    {
        if (GameManager.isPause)
            return;

        controlTarget.RMB();
    }

    // LShift
    void OnLShift()
    {
        if (GameManager.isPause)
            return;

        controlTarget.LShift();
    }

    // C
    void OnKeyboardC()
    {
        if (GameManager.isPause)
            return;

        controlTarget.KeyboardC();
    }

    // Z
    void OnKeyboardZ()
    {
        if (GameManager.isPause)
            return;

        controlTarget.KeyboardZ();
    }

    // R
    void OnKeyboardR()
    {
        if (GameManager.isPause)
            return;

        controlTarget.KeyboardR();
    }

    // Tab
    void OnKeyboardESC()
    {
        controlTarget.KeyboardESC();
    }

    // SpaceBar
    void OnSpaceBar()
    {
        if (GameManager.isPause)
            return;

        controlTarget.SpaceBar();
    }

    // 1
    void OnKeyboard1()
    {
        if (GameManager.isPause)
            return;

        controlTarget.Keyboard1();
    }

    // 2
    void OnKeyboard2()
    {
        if (GameManager.isPause)
            return;

        controlTarget.Keyboard2();
    }

    // 3
    void OnKeyboard3()
    {
        if (GameManager.isPause)
            return;

        controlTarget.Keyboard3();
    }

    // 4
    void OnKeyboard4()
    {
        if (GameManager.isPause)
            return;

        controlTarget.Keyboard4();
    }

    // 5
    void OnKeyboard5()
    {
        if (GameManager.isPause)
            return;

        controlTarget.Keyboard5();
    }
}
