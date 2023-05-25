using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    [SerializeField]
    private EquipManager equipManager;

    public void SelectArmyButton1()
    {
        equipManager.SelectPosition(1);
    }

    public void SelectArmyButton2()
    {
        equipManager.SelectPosition(2);
    }

    public void SelectArmyButton3()
    {
        equipManager.SelectPosition(3);
    }
}
