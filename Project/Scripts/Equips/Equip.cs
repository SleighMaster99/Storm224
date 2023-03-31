using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum EquipType
{
    Gun,
    ThrowingWeapon,
    UsesItem,
    UniqueAbility
}

public enum FireType
{
    Bolt,
    SemiAutometic,
    Autometic
}

public abstract class Equip : MonoBehaviour
{
    [Header("Equip Property")]
    public string equipName;                // 장비이름
    public EquipType equipType;             // 장비 타입
    public bool isAiming;                   // 조준 상태
    public bool isShoot;                    // 공격 가능 여부
    public bool isClick;                    // 마우스 좌클릭 여부

    [Header("Gun Property")]
    public FireType fireType;                    // 총기 종류

    [Header("Equip Components")]
    public Animator animator;               // 애니메이터
    public AudioSource audioSource;         // 오디오 소스

    public abstract void Fire();
    public abstract void Aiming();
    public abstract void Reload();
}
