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
    /// 0 : Hand
    /// 1 : M1 Garand
    /// 2 : Arisaka 38
    /// 3 : m3a1
    /// 4 : MK2
    /// 5 : M18
    /// 6 : Morphine
    /// 7 : Bandage Roll
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
    public void SelectPosition(int num)
    {
        if(num == 1)
        {   // 보병
            // Hand
            hasArmsEquip[0] = armsEquip[0];
            hasArmsEquipAnimators[0] = armsEquipAnimators[0];

            // Arisaka 38
            hasArmsEquip[1] = armsEquip[2];
            hasArmsEquipAnimators[1] = armsEquipAnimators[2];

            // MK2
            hasArmsEquip[2] = armsEquip[4];
            hasArmsEquipAnimators[2] = armsEquipAnimators[4];

            // Morphine
            hasArmsEquip[3] = armsEquip[6];
            hasArmsEquipAnimators[3] = armsEquipAnimators[6];

            // 탄약통
            hasArmsEquip[4] = armsEquip[8];
            hasArmsEquipAnimators[4] = armsEquipAnimators[8];
        }
        else if (num == 2)
        {   // 정비병
            // Hand
            hasArmsEquip[0] = armsEquip[0];
            hasArmsEquipAnimators[0] = armsEquipAnimators[0];

            // M1 Garand
            hasArmsEquip[1] = armsEquip[1];
            hasArmsEquipAnimators[1] = armsEquipAnimators[1];

            // M18
            hasArmsEquip[2] = armsEquip[5];
            hasArmsEquipAnimators[2] = armsEquipAnimators[5];

            // Morphine
            hasArmsEquip[3] = armsEquip[6];
            hasArmsEquipAnimators[3] = armsEquipAnimators[6];

            // 스패너
            hasArmsEquip[4] = armsEquip[9];
            hasArmsEquipAnimators[4] = armsEquipAnimators[9];
        }
        else
        {   // 의무병
            // Hand
            hasArmsEquip[0] = armsEquip[0];
            hasArmsEquipAnimators[0] = armsEquipAnimators[0];

            // m3a1
            hasArmsEquip[1] = armsEquip[3];
            hasArmsEquipAnimators[1] = armsEquipAnimators[3];

            // M18
            hasArmsEquip[2] = armsEquip[5];
            hasArmsEquipAnimators[2] = armsEquipAnimators[5];

            // Bandage Roll
            hasArmsEquip[3] = armsEquip[7];
            hasArmsEquipAnimators[3] = armsEquipAnimators[7];

            // 구급상자
            hasArmsEquip[4] = armsEquip[10];
            hasArmsEquipAnimators[4] = armsEquipAnimators[10];
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
