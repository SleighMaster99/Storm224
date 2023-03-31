using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Property")]
    [SerializeField]
    private float maxHP;            // 최대 체력
    [SerializeField]
    private float HP;               // 체력

    private void Start()
    {
        HealthInitialize();
    }

    // 초기화
    private void HealthInitialize() => HP = maxHP;

    // 데미지 주기
    public void Damage(float damageAmount) => HP -= damageAmount;

    // 회복하기
    public void Heal(float healAmount) => HP += healAmount;
}
