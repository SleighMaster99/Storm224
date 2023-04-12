using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipHeal : Equip
{
    [Header("Grenade Property")]
    [SerializeField]
    private float healWaitTime;                 // 힐 하는 시간
    [SerializeField]
    private float healAmount;                   // 힐 하는 양

    [Header("Sounds")]
    [SerializeField]
    private AudioClip useSound;                 // 폭발 소리

    // 무기 장착
    private void OnEnable()
    {
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
    public override void Fire()
    {
        isClick = !isClick;

        if (isShoot)
            return;

        if ((useSound != null) && isClick)
            audioSource.PlayOneShot(useSound);

        animator.SetBool("IsClick", isClick);

        if (isClick)
            StartCoroutine("Heal");
        else
        {
            StopCoroutine("Heal");
            audioSource.Stop();
        }
    }

    private IEnumerator Heal()
    {
        yield return new WaitForSeconds(healWaitTime);

        animator.SetTrigger("Heal");

        reloadedAmmo--;
        playerUI.UpdateEquipUI(ammo, reloadedAmmo);

        isShoot = CheckHasItem();

        this.transform.root.GetComponent<Health>().Heal(healAmount);

        StartCoroutine("ReloadGrenade");
    }

    // Heal 아이템 사용 후 Ammo관련 UI 변경
    private IEnumerator ReloadGrenade()
    {
        yield return new WaitForSeconds(1.0f);

        ammo--;
        reloadedAmmo = maxReloadedAmmo;
        playerUI.UpdateEquipUI(ammo, reloadedAmmo);
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
