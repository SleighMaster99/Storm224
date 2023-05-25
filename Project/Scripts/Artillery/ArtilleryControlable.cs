using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryControlable : Controlable
{
    [Header("Components")]
    [SerializeField]
    private PlayerUI playerUI;
    [SerializeField]
    private GameManager gameManager;
    public PlayerControlable drivePlayer;
    private Control control;
    private PoolManager poolManager;            // Pool Manager

    [Header("Artillery")]
    [SerializeField]
    private GameObject fireEffect;
    [SerializeField]
    private GameObject cannonBody;
    [SerializeField]
    private Transform ammoSpawnPoint;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private float fireForce;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float damageRange;
    [SerializeField]
    private float explosionForce;
    [SerializeField]
    private float cameraShakeTime;
    [SerializeField]
    private float cameraShakeForce;
    public int ammo;
    public string artilleryName;
    private bool isFire;
    private bool isClick;

    [Header("Seat")]
    public Transform characterSeat; // 좌석
    public Transform ridePosition; // 하차 위치

    [Header("Audio Component")]
    public AudioSource[] audioSource; // 차량 오디오 소스

    private void Start()
    {
        control = FindObjectOfType<Control>();
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        poolManager = GameObject.Find("Pool Manager").GetComponent<PoolManager>();
    }

    public override void Interact()
    {
        GetOut();
        playerUI.UpdateArtilleryUI(false, null, null);
    }

    // 야포 나가기
    private void GetOut()
    {
        var controller = control;
        var playerBody = drivePlayer.GetComponent<Rigidbody>();
        var playerCollider = drivePlayer.GetComponent<CapsuleCollider>();

        drivePlayer.transform.SetParent(null);
        drivePlayer.cameraSocket = this.cameraSocket;
        drivePlayer.mainCamera = this.mainCamera;

        drivePlayer.transform.rotation = ridePosition.rotation;
        drivePlayer.mainCamera.rotation = ridePosition.rotation;
        drivePlayer.transform.position = ridePosition.position;

        controller.ChangeControlTarget(drivePlayer);

        playerBody.isKinematic = false;
        playerCollider.isTrigger = false;

        drivePlayer.GetComponent<PlayerControlable>().currentEquip.gameObject.SetActive(true);

        drivePlayer = null;
    }

    // 일시정지
    public override void KeyboardESC()
    {
        gameManager.PauseGame(!GameManager.isPause);
    }

    // 포신 움직임
    public override void Move(Vector2 input)
    {
        float rotateY = this.transform.eulerAngles.y + input.x * rotationSpeed * Time.deltaTime;
        float rotateX = cannonBody.transform.eulerAngles.x + -input.y * rotationSpeed * Time.deltaTime;

        if(rotateX > 0 && rotateX < 90.0f)
            rotateX = Mathf.Clamp(rotateX, 0, 35.0f);
        else if(rotateX > 270.0f && rotateX < 360.0f)
            rotateX = Mathf.Clamp(rotateX, 315.0f, 360.0f);

        this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, rotateY, this.transform.eulerAngles.z);
        cannonBody.transform.rotation = Quaternion.Euler(rotateX, cannonBody.transform.eulerAngles.y, cannonBody.transform.eulerAngles.z);

        if (input.x != 0 || input.y != 0)
        {
            if (!audioSource[0].isPlaying)
                audioSource[0].Play();
        }
        else
            audioSource[0].Stop();
    }

    // 야포 사격
    public override void LMB()
    {
        isClick = !isClick;

        if(ammo > 0 && isClick && !isFire)
        {
            isFire = true;

            StartCoroutine("ArtilleryFire");
        }
    }

    // 야포 포탄 발사
    private IEnumerator ArtilleryFire()
    {
        RaycastHit hit;

        ammo--;
        audioSource[1].Play();
        fireEffect.SetActive(true);

        StartCoroutine(CameraShake(cameraShakeTime, cameraShakeForce));

        playerUI.UpdateArtilleryAmmoUI(ammo);

        var artilleryAmmo = poolManager.artilleryAmmoPool.Get();

        Debug.DrawRay(ammoSpawnPoint.position, ammoSpawnPoint.forward * 0.3f, Color.red, 2.0f);
        Debug.DrawRay(ammoSpawnPoint.position, -ammoSpawnPoint.forward * 0.3f, Color.red, 2.0f);

        if (Physics.Raycast(ammoSpawnPoint.position, ammoSpawnPoint.forward, out hit, 0.3f) || Physics.Raycast(ammoSpawnPoint.position, -ammoSpawnPoint.forward, out hit, 0.3f))
            artilleryAmmo.transform.position = hit.point;
        else
        {
            artilleryAmmo.transform.position = ammoSpawnPoint.position;
            artilleryAmmo.FireBullet(ammoSpawnPoint.forward, fireForce, damage, damageRange, explosionForce);
        }

        //artilleryAmmo.transform.position = ammoSpawnPoint.position;
        //artilleryAmmo.FireBullet(ammoSpawnPoint.forward, fireForce, damage, damageRange, explosionForce);

        StartCoroutine("ArtilleryReload");

        yield return new WaitForSeconds(reloadTime);

        fireEffect.SetActive(false);
        isFire = false;
    }

    // 카메라 쉐이크
    private IEnumerator CameraShake(float shakeTime, float shakeForce)
    {
        float timer = 0;
        Vector3 cameraOriginPosition = mainCamera.position;

        while(timer <= shakeTime)
        {
            mainCamera.position = Random.insideUnitSphere * shakeForce + cameraOriginPosition;

            timer += Time.deltaTime;
            yield return null;
        }

        mainCamera.position = cameraOriginPosition;
    }

    // 야포 재장전 소리 재생
    private IEnumerator ArtilleryReload()
    {
        yield return new WaitForSeconds(2.0f);

        audioSource[2].Play();
    }

    public override void Rotate(Vector2 input) { }
    public override void Keyboard1() { }
    public override void Keyboard2() { }
    public override void Keyboard3() { }
    public override void Keyboard4() { }
    public override void Keyboard5() { }
    public override void KeyboardC() { }
    public override void KeyboardR() { }
    public override void KeyboardZ() { }
    public override void LShift() { }
    public override void RMB() { }
    public override void SpaceBar() { }
}
