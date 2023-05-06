using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipHand : Equip
{
    private Transform mainCamera;               // 메인 카메라

    private RaycastHit fistHit;                 // RayCast 정보

    [Header("Fist Property")]
    [SerializeField]
    private float fistWaitTime;                 // 클릭 후 Ray 쏘기까지 대기 시간
    [SerializeField]
    private float fistDistance;                 // Ray 쏘는 거리

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    private void OnEnable()
    {
        StartCoroutine("Equipping");
        playerUI.InitializeEquipUI(equipName, ammo, reloadedAmmo);
    }

    // 무기 장착 중...
    private IEnumerator Equipping()
    {
        isShoot = true;
        isClick = false;
        yield return new WaitForSeconds(1.0f);
        isShoot = false;
    }

    // 왼손 펀치
    public override void Fire(bool isClose, RaycastHit hit)
    {
        isClick = !isClick;

        if (isClick && !isShoot)
        {
            StartCoroutine("Fist");

            animator.SetTrigger("LeftFist");
        }
    }

    // 오른손 펀치
    public override void Aiming()
    {
        isClick = !isClick;

        if (!isShoot)
        {
            StartCoroutine("Fist");

            animator.SetTrigger("RightFist");
        }
    }

    // Ray를 쏴서 주먹에 맞았는지 안맞았는지 판별
    private IEnumerator Fist()
    {
        isShoot = !isShoot;

        yield return new WaitForSeconds(fistWaitTime);

        // Ray 쏘기
        Debug.DrawRay(mainCamera.position, mainCamera.forward * fistDistance, Color.red, 2.0f);
        if(Physics.Raycast(mainCamera.position, mainCamera.forward, out fistHit, fistDistance))
        {
            // Ray 맞았을 때 처리
            Debug.Log(fistHit.transform.name);
        }

        isShoot = !isShoot;
    }

    public override void Reload()
    {
        return;
    }
}
