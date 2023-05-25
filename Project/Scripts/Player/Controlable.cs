using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controlable : MonoBehaviour
{
    public Transform cameraSocket;              // 카메라소켓
    public Transform mainCamera;                // 메인 카메라

    public abstract void Move(Vector2 input);
    public abstract void Rotate(Vector2 input);
    public abstract void Interact();
    public abstract void LMB();
    public abstract void RMB();
    public abstract void LShift();
    public abstract void KeyboardC();
    public abstract void KeyboardZ();
    public abstract void KeyboardR();
    public abstract void KeyboardESC();
    public abstract void SpaceBar();
    public abstract void Keyboard1();
    public abstract void Keyboard2();
    public abstract void Keyboard3();
    public abstract void Keyboard4();
    public abstract void Keyboard5();
}
