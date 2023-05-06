using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class EquipM1Garand : Equip
{
    [Header("Gun Components")]
    [SerializeField]
    private AudioSource audioSource_Clip;       // 클립 소리 재생용 오디오 소스

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleEffect;            // 화염 효과

    [Header("Spawn Points")]
    [SerializeField]
    private Transform bulletSpawnPoint;         // 총알 스폰 포인트
    [SerializeField]
    private Transform cartridgeSpawnPoint;      // 탄피 스폰 포인트

    [Header("Sounds")]
    [SerializeField]
    private AudioClip fireSound;                // 발사 소리
    [SerializeField]
    private AudioClip reloadSound;              // 장전 소리
    [SerializeField]
    private AudioClip clipSound;                // 클립 소리
    [SerializeField]
    private AudioClip noAmmoSound;              // 총알 없을 때 트리거 소리

    [Header("Spawn Prefabs")]
    [SerializeField]
    private GameObject bulletPrefab;            // 총알 Prefab
    [SerializeField]
    private GameObject cartridgeCasePrefab;     // 탄피 Prefab

    [Header("Gun Property")]
    [SerializeField]
    private float bulletDamage;                 // 데미지
    [SerializeField]
    private float fireRate;                     // 연사속도
    [SerializeField]
    private float fireForce;                    // 총알 발사 힘
    [SerializeField]
    private float reloadTime;                   // 재장전 시간 - 애니메이션 재생 후 탄 처리
    [SerializeField]
    private Vector3 bulletHoleScale;            // 탄흔 Scale

    [Header("Cartridge Case Property")]
    [SerializeField]
    private float cartridgeCaseforce;           // 탄피 발사 힘
    [SerializeField]
    private float cartridgeCaseWaitTime;        // 탄피 분출 대기 시간
    [SerializeField]
    private float cartridgeCaseTime;            // 탄피 풀에 반납 되는 시간

    [Header("PlayerControlabe")]
    [SerializeField]
    private float rotateXAmount;                // 조준 수평 감도 감소량
    [SerializeField]
    private float rotateYAmount;                // 조준 수직 감도 감소량

    // 오브젝트 풀
    private PoolManager poolManager;            // Pool Manager

    private void Awake()
    {
        poolManager = GameObject.Find("Pool Manager").GetComponent<PoolManager>();
    }

    private void OnEnable()
    {
        StartCoroutine("Equipping");
        playerUI.InitializeEquipUI(equipName, ammo, reloadedAmmo);

        muzzleEffect.SetActive(false);
    }

    // 무기 장착 중...
    private IEnumerator Equipping()
    {
        isShoot = true;
        isClick = false;
        yield return new WaitForSeconds(1.0f);
        isShoot = CheckReloadedBullet();
    }

    // 발사
    public override void Fire(bool isClose, RaycastHit hit)
    {
        isClick = !isClick;

        // 총알이 없으면 발사시 트리거 소리 재생
        if(CheckReloadedBullet() && isClick)
            audioSource_Clip.PlayOneShot(noAmmoSound);
        
        if (!isShoot && isClick)
        {
            isShoot = true;

            audioSource.PlayOneShot(fireSound);

            if (isAiming)
                animator.Play("AimingFire", -1, 0);
            else
                animator.Play("IdleFire", -1, 0);

            reloadedAmmo--;
            playerUI.UpdateEquipUI(ammo, reloadedAmmo);

            // 총알 풀에서 가져오기
            var bullet = poolManager.bulletPool.Get();
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.FireBullet(poolManager, bulletHoleScale, bulletSpawnPoint.forward, fireForce, bulletDamage);

            // 화염 효과
            StartCoroutine("MuzzleFlash");

            // 탄피 배출
            StartCoroutine("RemoveCartridgeCase");
            
            // 약실에 총알이 없을 때
            if(reloadedAmmo == 0)
            {
                animator.SetTrigger("NoAmmo");
                audioSource_Clip.PlayOneShot(clipSound);
            }
        }
    }

    // 화염 효과
    private IEnumerator MuzzleFlash()
    {
        if (!muzzleEffect.activeSelf)
            muzzleEffect.SetActive(true);
        else
        {
            muzzleEffect.SetActive(false);
            muzzleEffect.SetActive(true);
        }

        yield return new WaitForSeconds(fireRate);

        isShoot = CheckReloadedBullet();
    }

    // 탄피 배출
    private IEnumerator RemoveCartridgeCase()
    {
        yield return new WaitForSeconds(cartridgeCaseWaitTime);

        var cartridgeCase = poolManager.cartridgeCasePool.Get();
        cartridgeCase.transform.position = cartridgeSpawnPoint.position;
        cartridgeCase.transform.eulerAngles = new Vector3(cartridgeSpawnPoint.eulerAngles.x - 90.0f,
                                                          cartridgeSpawnPoint.eulerAngles.y,
                                                          cartridgeSpawnPoint.eulerAngles.z);
        cartridgeCase.RemoveCartridgeCase(cartridgeSpawnPoint.right, cartridgeCaseforce, cartridgeCaseTime);
    }

    // 조준
    public override void Aiming()
    {
        isAiming = !isAiming;
        animator.SetBool("IsAiming", isAiming);
        
        if (isAiming)
        {
            playerControlable.rotateXSpeed -= rotateXAmount;
            playerControlable.rotateYSpeed -= rotateYAmount;
        }
        else
        {
            playerControlable.rotateXSpeed += rotateXAmount;
            playerControlable.rotateYSpeed += rotateYAmount;
        }
    }

    // 재장전
    public override void Reload()
    {
        if (ammo <= 0)
            return;

        // 재장전 기능 처리
        StartCoroutine("Reloading");
    }

    // 재장전중...
    private IEnumerator Reloading()
    {
        isShoot = true;

        // 약실에 총알이 있을 때
        if (reloadedAmmo > 0)
        {
            animator.SetTrigger("NoAmmo");
            audioSource_Clip.PlayOneShot(clipSound);
        }

        animator.SetTrigger("ReloadTrigger");

        yield return new WaitForSeconds(reloadTime);

        ammo--;
        reloadedAmmo = maxReloadedAmmo;
        playerUI.UpdateEquipUI(ammo, reloadedAmmo);

        isShoot = CheckReloadedBullet();
    }

    // 장전된 총알이 있는지 확인
    // True  : 총알 없음
    // False : 총알 있음
    private bool CheckReloadedBullet()
    {
        if (reloadedAmmo <= 0)
            return true;
        else
            return false;
    }
}
