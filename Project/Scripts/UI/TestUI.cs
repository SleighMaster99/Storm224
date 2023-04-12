using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    [SerializeField]
    private EquipManager equipManager;

    public void SelectArmyButton1()
    {
        equipManager.TestInitializeEquip(1);
    }

    public void SelectArmyButton2()
    {
        equipManager.TestInitializeEquip(2);
    }

    public void SelectArmyButton3()
    {
        equipManager.TestInitializeEquip(3);
    }
}
