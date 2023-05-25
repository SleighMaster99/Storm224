using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipGrenade : Equip
{
    [Header("Spawn Prefabs")]
    [SerializeField]
    private GameObject grenadePrefab;           // 스폰할 Prefab

    [Header("Spawn Points")]
    [SerializeField]
    private GameObject grenadeSpawnPoint;       // 투척류 스폰 위치

    [Header("Grenade Property")]
    [SerializeField]
    private float throwForce;                   // 던지는 힘
    [SerializeField]
    private float spawnWaitTime;                // 클릭 후 Prefab 스폰까지 기다리는 시간
    [SerializeField]
    private float grenadeEffectForce;           // 투척 폭발 힘
    [SerializeField]
    private float grenadeEffectUpperForce;      // 투척 폭발 위쪽 힘
    [SerializeField]
    private float grenadeEffectRadius;          // 투척류 범위(추후 필요없으면 삭제)
    [SerializeField]
    private float explosionWaitTime;            // 코킹 후 폭발까지 시간
    [SerializeField]
    private float destroyTime;                  // 폭발 후 제거 시간
    [SerializeField]
    private string noAmmoMessage;               // 연막탄 없을 때 메세지
    [SerializeField]
    private float noAmmoMessageShowingTime;     // 연막탄 없을 때 메세지 보여주는 시간

    [Header("Sounds")]
    [SerializeField]
    private AudioClip removeSaftyPinSound;      // 안전핀 제거 소리
    [SerializeField]
    private AudioClip removeSaftyHandleSound;   // 안전손잡이 제거 소리

    private Grenade grenade;                    // 스폰된 Grenade Script

    private void Awake()
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
    }

    // 무기 장착
    private void OnEnable()
    {
        if (reloadedAmmo <= 0)
        {
            playerControlable.Keyboard1();
            playerUI.ShowEquipMessage(noAmmoMessage, noAmmoMessageShowingTime);
            return;
        }

        StartCoroutine("Equipping");
        playerUI.InitializeEquipUI(equipName, ammo, reloadedAmmo);
    }

    // 무기 장착 중...
    private IEnumerator Equipping()
    {
        isShoot = true;
        yield return new WaitForSeconds(1.0f);
        isShoot = CheckHasAmmo();
    }
    
    // 무기 변경시
    private void OnDisable()
    {
        if (grenade != null)
        {
            Destroy(grenade.gameObject);
        }
    }
    

    // ThrowReady && Trow
    public override void Fire(bool isClose, RaycastHit hit)
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

        grenade = Instantiate(grenadePrefab, grenadeSpawnPoint.transform.position, grenadeSpawnPoint.transform.rotation).GetComponent<Grenade>();
        grenade.InitializedGrenade(grenadeSpawnPoint.transform, grenadeEffectForce, grenadeEffectUpperForce, grenadeEffectRadius, explosionWaitTime, destroyTime);
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
        if ((grenade == null) || grenade.isCocking)
            return;

        if (removeSaftyHandleSound != null)
            audioSource.PlayOneShot(removeSaftyHandleSound);

        grenade.Cocking();
    }

    // Grenade 날리기
    public void Throw()
    {
        Vector3 throwPosition = grenadeSpawnPoint.transform.position;       // 던지는 포지션
        Vector3 throwDirection = Camera.main.transform.forward;             // 던지는 방향

        reloadedAmmo--;
        playerUI.UpdateEquipUI(ammo, reloadedAmmo);

        if (!grenade.isCocking)
            grenade.Cocking();

        grenade.transform.SetParent(null);
        grenade.Throw(throwPosition, throwDirection, throwForce);

        StartCoroutine("ReloadGrenade");
    }

    // Grenade 투척 후 Ammo관련 UI 변경
    private IEnumerator ReloadGrenade()
    {
        yield return new WaitForSeconds(1.0f);

        grenade = null;

        if (ammo > 0)
        {
            ammo--;
            reloadedAmmo = maxReloadedAmmo;
            playerUI.UpdateEquipUI(ammo, reloadedAmmo);
        }
        else
        {
            playerControlable.Keyboard1();
            isShoot = false;
        }
    }

    // 투척 장비가 있는지 확인
    private bool CheckHasAmmo()
    {
        if (ammo <= 0)
            return true;
        else
            return false;
    }
}
