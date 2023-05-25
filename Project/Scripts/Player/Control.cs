using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Controlable controlTarget;

    //컨트롤 타겟 변경 
    public void ChangeControlTarget(Controlable target)
    {
        controlTarget = target;
    }
}
