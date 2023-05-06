using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInfantryItem : Equip
{
    [Header("AmmoBox Property")]
    [SerializeField]
    private GameObject ammoBoxPrefab;           // AmmoBox Prefab
    [SerializeField]
    private Transform ammoBoxSpawnPoint;        // AmmoBox 설치할 위치
    [SerializeField]
    private float waitTime;                     // 탄약통 배치 하는 시간
    [SerializeField]
    private string noAmmoMessage;               // 탄약통 없을 때 메세지
    [SerializeField]
    private float noAmmoMessageShowingTime;     // 탄약통 없을 때 메세지 보여주는 시간

    [Header("Sounds")]
    [SerializeField]
    private AudioClip useSound;                 // AmmoBox 배치 하는 소리

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
        isShoot = CheckHasItem();
    }

    // 탄약통 설치
    public override void Fire(bool isClose, RaycastHit hit)
    {
        isClick = !isClick;

        if (isShoot)
            return;

        if ((useSound != null) && isClick)
            audioSource.PlayOneShot(useSound);

        if (isClick)
            StartCoroutine("InstallAmmoBox");
        else
        {
            StopCoroutine("InstallAmmoBox");
            audioSource.Stop();
        }

        animator.SetBool("IsUse", isClick);
    }

    // AmmoBox 아이템 개수 줄이고 설치
    private IEnumerator InstallAmmoBox()
    {
        yield return new WaitForSeconds(waitTime);

        Instantiate(ammoBoxPrefab, ammoBoxSpawnPoint.position, ammoBoxSpawnPoint.rotation);

        reloadedAmmo--;
        playerUI.UpdateEquipUI(ammo, reloadedAmmo);

        StartCoroutine("ReloadAmmoBox");
    }

    // AmmoBox 아이템 사용 후 Ammo관련 UI 변경
    private IEnumerator ReloadAmmoBox()
    {
        yield return new WaitForSeconds(1.0f);

        while (isClick)
            yield return null;      // 마우스 클릭 상태라면 대기

        if (ammo > 0)
        {   // AmmoBox가 남아있을 때
            ammo--;
            reloadedAmmo = maxReloadedAmmo;
            playerUI.UpdateEquipUI(ammo, reloadedAmmo);
        }
        else
        {   // AmmoBox가 없을 때
            playerControlable.Keyboard1();
        }
    }

    public override void Aiming()
    {
        return;
    }

    public override void Reload()
    {
        return;
    }

    // 장비가 있는지 확인
    private bool CheckHasItem()
    {
        if (ammo <= 0)
            return true;
        else
            return false;
    }
}
