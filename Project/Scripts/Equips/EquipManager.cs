using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    [Header("Persnal Equips")]
    public GameObject[] armsEquip;              // 모든 개인 장비
    public Animator[] armsEquipAnimators;       // 모든 개인 장비 애니메이터
    /// <summary>
    /// index : Equip Name
    /// 0 : M1 Garand
    /// 1 : Arisaka 38
    /// 2 : m3a1
    /// 3 : MK2
    /// 4 : M18
    /// 5 : Morphine
    /// 6 : Bandage Roll
    /// </summary>

    [Header("Current Has Equip")]
    [SerializeField]
    GameObject[] hasArmsEquip;                  // 플레이어가 소지한 장비
    Animator[] hasArmsEquipAnimators;           // 플레이어가 소지한 장비 애니메이터

    [Header("Current Equip")]
    [SerializeField]
    private GameObject currentEquip;             // 현재 장착된 장비
    [SerializeField]
    private Animator currentEquipAnimator;       // 현재 장착된 장비 애니메이터

    private void Awake()
    {
        hasArmsEquip = new GameObject[5];
        hasArmsEquipAnimators = new Animator[5];
    }

    // 개인 장비 초기화
    public Equip InitializeCurrentEquip()
    {
        return currentEquip.GetComponent<Equip>();
    }

    // 테스트씬 장비 장착
    public void TestInitializeEquip(int num)
    {
        if(num == 1)
        {   // 보병
            // Arisaka 38
            hasArmsEquip[0] = armsEquip[1];
            hasArmsEquipAnimators[0] = armsEquipAnimators[1];

            // MK2
            hasArmsEquip[1] = armsEquip[3];
            hasArmsEquipAnimators[1] = armsEquipAnimators[3];

            // M18
            hasArmsEquip[1] = armsEquip[3];
            hasArmsEquipAnimators[1] = armsEquipAnimators[3];

            // Morphine
            hasArmsEquip[2] = armsEquip[5];
            hasArmsEquipAnimators[2] = armsEquipAnimators[5];

            // Bandage Roll
            hasArmsEquip[3] = armsEquip[6];
            hasArmsEquipAnimators[3] = armsEquipAnimators[6];
        }
        else if (num == 2)
        {   // 공병
            // M1 Garand
            hasArmsEquip[0] = armsEquip[0];
            hasArmsEquipAnimators[0] = armsEquipAnimators[0];

            // MK2
            hasArmsEquip[1] = armsEquip[3];
            hasArmsEquipAnimators[1] = armsEquipAnimators[3];

            // M18
            hasArmsEquip[1] = armsEquip[3];
            hasArmsEquipAnimators[1] = armsEquipAnimators[3];

            // Morphine
            hasArmsEquip[2] = armsEquip[5];
            hasArmsEquipAnimators[2] = armsEquipAnimators[5];

            // Bandage Roll
            hasArmsEquip[3] = armsEquip[6];
            hasArmsEquipAnimators[3] = armsEquipAnimators[6];
        }
        else
        {   // 의무병
            // m3a1
            hasArmsEquip[0] = armsEquip[2];
            hasArmsEquipAnimators[0] = armsEquipAnimators[2];

            // MK2
            hasArmsEquip[1] = armsEquip[3];
            hasArmsEquipAnimators[1] = armsEquipAnimators[3];

            // M18
            hasArmsEquip[1] = armsEquip[3];
            hasArmsEquipAnimators[1] = armsEquipAnimators[3];

            // Morphine
            hasArmsEquip[2] = armsEquip[5];
            hasArmsEquipAnimators[2] = armsEquipAnimators[5];

            // Bandage Roll
            hasArmsEquip[3] = armsEquip[6];
            hasArmsEquipAnimators[3] = armsEquipAnimators[6];
        }
    }

    // 장비 변경
    public Equip CurrentEquipChanger(int num)
    {
        if(num > 0 && num < 6)
            ChangingCurrentEquip(num - 1);

        return currentEquip.GetComponent<Equip>();
    }

    // 장비 변경 처리
    private void ChangingCurrentEquip(int i)
    {
        currentEquip.SetActive(false);
        currentEquip = hasArmsEquip[i];
        currentEquip.SetActive(true);

        currentEquipAnimator = hasArmsEquipAnimators[i];
    }
}
