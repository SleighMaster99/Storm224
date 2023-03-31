using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controlable : MonoBehaviour
{
    public abstract void Move(Vector2 input);
    public abstract void Rotate(Vector2 input);
    public abstract void Interact();
    public abstract void LMB();
    public abstract void RMB();
    public abstract void LShift();
    public abstract void KeyboardC();
    public abstract void KeyboardZ();
    public abstract void KeyboardR();
}
