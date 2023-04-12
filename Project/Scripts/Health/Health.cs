using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Property")]
    [SerializeField]
    private float maxHP;            // 최대 체력
    [SerializeField]
    private float initHP;           // 초기화 체력
    public float HP;               // 체력

    private void Start()
    {
        HealthInitialize();
    }

    // 초기화
    private void HealthInitialize() => HP = initHP;

    // 데미지 주기
    public void Damage(float damageAmount)
    {
        HP -= damageAmount;

        if (HP < 0)
            HP = 0;
    }

    // 회복하기
    public void Heal(float healAmount)
    {
        HP += healAmount;

        if (HP > maxHP)
            HP = maxHP;
    }
}
