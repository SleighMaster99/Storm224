using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipHeal : Equip
{
    [Header("Heal Property")]
    [SerializeField]
    private float healWaitTime;                 // 힐 하는 시간
    [SerializeField]
    private float healAmount;                   // 힐 하는 양
    [SerializeField]
    private string noAmmoMessage;               // 회복템 없을 때 메세지
    [SerializeField]
    private float noAmmoMessageShowingTime;     // 회복템 없을 때 메세지 보여주는 시간

    [Header("Sounds")]
    [SerializeField]
    private AudioClip useSound;                 // 회복 아이템 사용하는 소리

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
        isShoot = CheckHasItem();
    }

    // Heal
    public override void Fire(bool isClose, RaycastHit hit)
    {
        isClick = !isClick;

        if (isShoot)
            return;

        if ((useSound != null) && isClick)
            audioSource.PlayOneShot(useSound);

        if (isClick)
            StartCoroutine("Heal");
        else
        {
            StopCoroutine("Heal");
            audioSource.Stop();
        }

        animator.SetBool("IsClick", isClick);
    }

    // 힐 아이템 개수 줄이고 HP 회복
    private IEnumerator Heal()
    {
        yield return new WaitForSeconds(healWaitTime);

        reloadedAmmo--;
        playerUI.UpdateEquipUI(ammo, reloadedAmmo);

        this.transform.root.GetComponent<Health>().Heal(healAmount);

        StartCoroutine("ReloadHealItem");
    }

    // Heal 아이템 사용 후 Ammo관련 UI 변경
    private IEnumerator ReloadHealItem()
    {
        yield return new WaitForSeconds(1.0f);

        while (isClick)
            yield return null;

        if (ammo > 0)
        {
            ammo--;
            reloadedAmmo = maxReloadedAmmo;
            playerUI.UpdateEquipUI(ammo, reloadedAmmo);
        }
        else
        {
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

    // 회복 장비가 있는지 확인
    private bool CheckHasItem()
    {
        if (ammo <= 0)
            return true;
        else
            return false;
    }
}
