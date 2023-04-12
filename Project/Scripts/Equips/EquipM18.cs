using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipM18 : Equip
{
    [Header("Spawn Prefabs")]
    [SerializeField]
    private GameObject grenadePrefab;           // 스폰할 Prefab

    [Header("Spawn Points")]
    [SerializeField]
    private GameObject grenadeSpawnPoint;       // 투척류 스폰 위치

    [Header("Grenade Property")]
    [SerializeField]
    private int hasAmmo;                        // 가지고 있는 개수
    [SerializeField]
    private float throwForce;                   // 던지는 힘
    [SerializeField]
    private float spawnWaitTime;                // 클릭 후 Prefab 스폰까지 기다리는 시간
    [SerializeField]
    private float explosionWaitTime;            // 코킹 후 폭발까지 시간
    [SerializeField]
    private float destroyTime;                  // 폭발 후 제거 시간

    [Header("Sounds")]
    [SerializeField]
    private AudioClip removeSaftyPinSound;      // 안전핀 제거 소리
    [SerializeField]
    private AudioClip removeSaftyHandleSound;   // 안전손잡이 제거 소리

    private M18 grenade;                        // 스폰된 Grenade Script

    // 무기 장착
    private void OnEnable()
    {
        StartCoroutine("Equipping");
    }

    // 무기 장착 중...
    private IEnumerator Equipping()
    {
        isShoot = true;
        yield return new WaitForSeconds(1.0f);
        isShoot = CheckHasAmmo();
    }

    // ThrowReady && Trow
    public override void Fire()
    {
        isClick = !isClick;

        if (isShoot)
            return;

        if (isClick)
        {
            // 안전핀 뽑는 소리
            if (removeSaftyPinSound != null)
                audioSource.PlayOneShot(removeSaftyPinSound);

            StartCoroutine("SpawnGrenade");
            animator.SetTrigger("Ready");
        }
        else
        {
            StartCoroutine("Throwing");
            animator.SetTrigger("Throw");
        }
    }

    // Grenade 스폰
    private IEnumerator SpawnGrenade()
    {
        yield return new WaitForSeconds(spawnWaitTime);

        grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.transform.position, grenadeSpawnPoint.transform.rotation).GetComponent<M18>();
        grenade.InitializedGrenade(grenadeSpawnPoint.transform, explosionWaitTime, destroyTime);
    }

    // Grenade 던지는 중 클릭 차단
    private IEnumerator Throwing()
    {
        isShoot = true;

        yield return new WaitForSeconds(spawnWaitTime);

        isShoot = CheckHasAmmo();
    }

    public override void Aiming()
    {
        return;
    }

    // 안전 손잡이 분리
    public override void Reload()
    {
        if (grenade.isCocking)
            return;

        if (removeSaftyHandleSound != null)
            audioSource.PlayOneShot(removeSaftyHandleSound);

        grenade.Cocking();
    }

    // Grenade 날리기
    public void Throw()
    {
        Vector3 throwPosition = grenadeSpawnPoint.transform.position;
        Vector3 throwDirection = Camera.main.transform.forward;

        hasAmmo--;

        if (!grenade.isCocking)
            grenade.Cocking();

        grenade.transform.SetParent(null);
        grenade.Throw(throwPosition, throwDirection, throwForce);
    }

    // 투척 장비가 있는지 확인
    private bool CheckHasAmmo()
    {
        if (hasAmmo <= 0)
            return true;
        else
            return false;
    }
}
