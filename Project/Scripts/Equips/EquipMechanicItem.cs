using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMechanicItem : Equip
{
    [Header("AmmoBox Property")]
    [SerializeField]
    private float repairTime;                       // 수리 해야 하는 시간
    [SerializeField]
    private float repairingTime;                    // 수리한 시간
    [SerializeField]
    private string noTargetMessage;                 // 수리 대상이 아닐 때 메세지
    [SerializeField]
    private float noTargetMessageShowingTime;       // 수리 대상이 아닐 때 메세지 보여주는 시간
    RaycastHit hit;                                 // 수리 대상 확인 RaycastHit

    [Header("Sounds")]
    [SerializeField]
    private AudioClip repairSound;                     // 수리 하는 소리

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
        isShoot = false;
    }

    // 차량 수리
    public override void Fire(bool isClose, RaycastHit playerControlablehit)
    {
        isClick = !isClick;
        animator.SetBool("IsClick", isClick);

        if (isClick)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2.0f))
            {
                if (hit.transform.CompareTag("Vehicle"))
                {   // 추후 차량 스크립트의 리페어 타임으로 변경
                    repairTime = 10.0f;
                    //repairTime = hit.transform.GetComponent<Vehicle>().repairTime;
                    StartCoroutine("Repair");
                    StartCoroutine("RepairingTimer");
                }
                else
                    playerUI.ShowEquipMessage(noTargetMessage, noTargetMessageShowingTime);
            }
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 2.0f, Color.green, 1.0f);
        }
        else
        {
            StopCoroutine("Repair");
            StopCoroutine("RepairingTimer");
            repairingTime = 0;
        }
    }

    private IEnumerator Repair()
    {
        yield return new WaitForSeconds(repairTime);

        hit.transform.GetComponent<Health>().Heal(1000.0f);
    }

    private IEnumerator RepairingTimer()
    {
        while (true)
        {
            repairingTime += Time.deltaTime;
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(repairSound);

            yield return null;
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
}
