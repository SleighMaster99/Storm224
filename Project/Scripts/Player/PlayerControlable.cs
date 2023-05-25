using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlable : Controlable
{
    [Header("Components")]
    [SerializeField]
    private PlayerUI playerUI;                  // Player UI Script
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Health health;
    [SerializeField]
    private AudioListener audioListener;

    // 현재 장착 장비
    [Header("Weapon")]
    public EquipManager equipManager;
    public Equip currentEquip;

    //public Text showTitleText;

    [Header("Speed Property")]
    public float moveSpeed;                     // Player 현재 움직임 속도
    [SerializeField]
    private float walkSpeed;                    // 걸을 때 속도
    [SerializeField]
    private float runSpeed;                     // 달릴 때 속도
    [SerializeField]
    private bool isRun = false;                 // 달리기 여부
    [SerializeField]
    private bool isLShiftDown = false;          // LShift 누름 여부
    public bool isMove = false;                 // 움직임 여부
    public float footSoundTerm;                 // 발소리 텀
    [SerializeField]
    private float walkTerm;
    [SerializeField]
    private float runTerm;

    [Header("Mouse Sensitive")]
    public float rotateXSpeed;                  // 마우스 가로 감도
    public float rotateYSpeed;                  // 마우스 수직 감도

    private Vector3 smoothRotation;
    private Vector3 currentVelocity;

    // 마우스 움직인 값
    private float rotateX;                      // 마우스 가로 움직임
    private float rotateY;                      // 마우스 세로 움직임

    private bool isClick = false;                        // 마우스 Down Up

    [Header("Stand/Sit/Down")]
    [SerializeField]
    private CapsuleCollider capsuleCollider;
    [SerializeField]
    private float decreaseMoveSpeed;            // 움직임 속도 감소량
    [SerializeField]
    private float standHeight;                  // 서 있을 때 높이
    [SerializeField]
    private float sitHeight;                    // 앉아 있을 때 높이
    [SerializeField]
    private float standsitCenter;               // 앉거나 서 있을 때 중심
    [SerializeField]
    private float downCenter;                   // 엎드려 있을 때 중심

    [Header("Jump")]
    [SerializeField]
    private Rigidbody playerRigidbody;          // rigidbody
    [SerializeField]
    private float jumpPower;                    // 점프 AddForce 수치
    public bool isJump;                         // 점프 중인지 True : 점프중, False : 땅에 있음
    private bool isSpaceBar;


    // Player 상태 Stand/Sit/Down
    private enum PlayerBodyStatus
    {
        Stand,
        Sit,
        Down
    }
    [SerializeField]
    private PlayerBodyStatus bodyState = PlayerBodyStatus.Stand;


    private void Start()
    {
        moveSpeed = walkSpeed;

        rotateX = 0;
        rotateY = this.transform.eulerAngles.y;

        GameManager.optionUI.SensitiveInitalize();
    }

    // 플레이어 보직 선택 후 무기 초기화
    public void PlayerInitialize()
    {
        currentEquip = equipManager.InitializeCurrentEquip();
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void LateUpdate()
    {
        // 앉기, 서기 기능
        SitDownStandUp();

        if (health.HP <= 0)
        {
            gameManager.GameOver();
            StartCoroutine(PlayerDie());
        }
    }

    private IEnumerator PlayerDie()
    {
        yield return new WaitForSeconds(1.0f);

        audioListener.enabled = false;
    }

    // Player 움직임 - WASD
    public override void Move(Vector2 input)
    {
        Vector3 lookForward = new Vector3(mainCamera.forward.x, 0, mainCamera.forward.z).normalized;
        Vector3 lookRight = new Vector3(mainCamera.right.x, 0, mainCamera.right.z).normalized;
        Vector3 moveDir = lookForward * input.y + lookRight * input.x;
        moveDir = moveDir * moveSpeed * Time.deltaTime;

        if (input.magnitude != 0)
        {
            isMove = true;
            transform.position += moveDir;
        }
        else
        {
            isMove = false;
            isRun = false;

            currentEquip.animator.SetBool("IsRun", isRun);
        }
    }

    // 마우스 움직임
    public override void Rotate(Vector2 input)
    {
        if(mainCamera != null)
        {
            float inputX = input.x * rotateXSpeed * 100 * Time.deltaTime;
            float inputY = input.y * rotateYSpeed * 100 * Time.deltaTime;
            rotateX += -inputY;
            rotateY += inputX;

            // 마우스 각도 제한
            if(bodyState.Equals(PlayerBodyStatus.Down))
                rotateX = Mathf.Clamp(rotateX, -30.0f, 10.0f);
            else
                rotateX = Mathf.Clamp(rotateX, -70.0f, 50.0f);

            smoothRotation = Vector3.SmoothDamp(smoothRotation, new Vector3(rotateX, rotateY), ref currentVelocity, 0.1f);

            this.transform.eulerAngles = new Vector3(0, rotateY, 0);
            mainCamera.transform.eulerAngles = smoothRotation;
        }
    }

    // 상호작용 - F
    public override void Interact()
    {
        RaycastHit hit;

        Debug.DrawRay(mainCamera.position, mainCamera.forward * 2.0f, Color.red, 1.0f);
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 2.0f))
        {
            if (hit.transform.CompareTag("AmmoBox"))
                InteractAmmoBox(hit);
            else if (hit.transform.CompareTag("Vehicle"))
                InteractVehicle(hit);

            switch (hit.transform.tag)
            {
                case "AmmoBox":
                    InteractAmmoBox(hit);
                    break;
                case "Vehicle":
                    InteractVehicle(hit);
                    break;
                case "BridgeBomber":
                    InteractBomber(hit);
                    break;
                case "BridgeBomb":
                    InteractInstallBomb(hit);
                    break;
                case "Artillery":
                    InteractArtillery(hit);
                    break;
                default:
                    break;
            }
        }
    }

    // 상호작용 - 탄약통
    private void InteractAmmoBox(RaycastHit hit)
    {
        if (currentEquip.equipType == EquipType.Gun)
        {
            if (currentEquip.ammo != currentEquip.maxAmmo)
            {
                currentEquip.ammo = hit.transform.GetComponent<AmmoBox>().AddAmmo(currentEquip.maxAmmo);
                playerUI.UpdateEquipUI(currentEquip.ammo, currentEquip.reloadedAmmo);
            }
            else
                playerUI.ShowEquipMessage("더 이상 탄약을 챙길 수 없습니다.", 1.0f);
        }
        else
            playerUI.ShowEquipMessage("탄약이 필요한 장비가 아닙니다.", 1.0f);
    }

    // 상호작용 - 이동수단
    private void InteractVehicle(RaycastHit hit)
    {
        VehicleControlable vehicle = hit.transform.GetComponent<VehicleControlable>();
        Health vehicleHealth = hit.transform.GetComponent<Health>();

        currentEquip.gameObject.SetActive(false);

        Ride(vehicle);
        playerUI.UpdateVehicleUI(true, vehicle, vehicleHealth);
    }

    // 이동수단 탑승 기능
    private void Ride(VehicleControlable vehicle)
    {
        var controller = FindObjectOfType<Control>();
        var playerBody = GetComponent<Rigidbody>();
        var playerCollider = GetComponent<CapsuleCollider>();
        playerBody.isKinematic = true;
        playerCollider.isTrigger = true;

        Transform closeSeat = vehicle.characterSeat[0];
        float seatDistance = Vector3.Distance(this.transform.position, closeSeat.position);

        foreach (Transform seat in vehicle.characterSeat)
        {
            float newSeatDistance = Vector3.Distance(this.transform.position, seat.position);

            if (seatDistance > newSeatDistance)
                closeSeat = seat;
        }

        this.transform.position = closeSeat.position;
        vehicle.currentSeat = closeSeat;

        vehicle.drivePlayer = this;
        controller.ChangeControlTarget(vehicle);
        vehicle.cameraSocket = this.cameraSocket;
        vehicle.mainCamera = this.mainCamera;
        transform.SetParent(vehicle.currentSeat);

        if (vehicle.currentSeat == vehicle.characterSeat[0])
            vehicle.Keyboard1();
    }

    // 상호작용 - 폭파
    private void InteractBomber(RaycastHit hit)
    {
        BridgeBomb bomber = GameObject.FindWithTag("BridgeBomber").GetComponent<BridgeBomb>();

        bomber.ActiveBomber();
    }

    // 상호작용 - 폭탄설치
    private void InteractInstallBomb(RaycastHit hit)
    {
        BridgeBomb bomb = GameObject.FindWithTag("BridgeBomber").GetComponent<BridgeBomb>();

        bomb.InstallBomb(hit.transform.position);
    }


    // 상호작용 - 야포
    private void InteractArtillery(RaycastHit hit)
    {
        var artillery = hit.transform.GetComponent<ArtilleryControlable>();
        var artilleryHealth = hit.transform.GetComponent<Health>();

        currentEquip.gameObject.SetActive(false);

        UseArtillery(artillery);
        playerUI.UpdateArtilleryUI(true, artillery, artilleryHealth);
    }

    private void UseArtillery(ArtilleryControlable artillery)
    {
        var controller = FindObjectOfType<Control>();
        var playerBody = GetComponent<Rigidbody>();
        var playerCollider = GetComponent<CapsuleCollider>();

        playerBody.isKinematic = true;
        playerCollider.isTrigger = true;

        this.transform.position = artillery.characterSeat.position;
        this.transform.rotation = artillery.characterSeat.rotation;
        mainCamera.rotation = artillery.characterSeat.rotation;

        artillery.drivePlayer = this;
        controller.ChangeControlTarget(artillery);
        artillery.cameraSocket = this.cameraSocket;
        artillery.mainCamera = this.mainCamera;
        transform.SetParent(artillery.characterSeat);
    }

    // 좌클릭 - 발사
    public override void LMB()
    {
        isClick = !isClick;

        if (isRun)
            return;

        RaycastHit hit;
        bool isClose;
        
        Debug.DrawRay(mainCamera.position, mainCamera.forward * 1.9f, Color.red, 2.0f);
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 1.9f))
            isClose = true;
        else
            isClose = false;

        currentEquip.Fire(isClose, hit);
    }

    // 우클릭 -조준
    public override void RMB()
    {
        if(isRun)
            return;

        currentEquip.Aiming();
    }

    // 달리기 - LShift
    public override void LShift()
    {
        isLShiftDown = !isLShiftDown;

        if (!isMove || !bodyState.Equals(PlayerBodyStatus.Stand))
        {
            if (bodyState.Equals(PlayerBodyStatus.Sit))             // 앉아 있을 때 서기
                KeyboardC();
            if (bodyState.Equals(PlayerBodyStatus.Down))            // 엎드려 있을 때 서기
                KeyboardZ();

            return;
        }

        if (isMove && isLShiftDown)
        {
            isRun = true;

            moveSpeed = runSpeed;
            footSoundTerm = runTerm;
        }
        else
        {
            isRun = false;

            moveSpeed = walkSpeed;
            footSoundTerm = walkTerm;
        }

        currentEquip.animator.SetBool("IsRun", isRun);
    }

    // 앉기 - C
    public override void KeyboardC()
    {
        if (bodyState.Equals(PlayerBodyStatus.Stand))           // Stand -> Sit
        {
            bodyState = PlayerBodyStatus.Sit;
            moveSpeed -= decreaseMoveSpeed;
        }
        else if (bodyState.Equals(PlayerBodyStatus.Down))       // Down -> Sit
        {
            bodyState = PlayerBodyStatus.Sit;
            capsuleCollider.center = new Vector3(0, standsitCenter, 0);
            this.transform.position += new Vector3(0, 0.2f, 0);
        }
        else                                                    // Sit -> Stand
        {
            bodyState = PlayerBodyStatus.Stand;
            moveSpeed += decreaseMoveSpeed;
        }
    }

    // 엎드리기 - Z
    public override void KeyboardZ()
    {
        if (bodyState.Equals(PlayerBodyStatus.Stand))           // Stand -> Down
        {
            bodyState = PlayerBodyStatus.Down;
            moveSpeed -= decreaseMoveSpeed;
            capsuleCollider.center = new Vector3(0, downCenter, 0);
        }
        else if (bodyState.Equals(PlayerBodyStatus.Sit))        // Sit -> Down
        {
            bodyState = PlayerBodyStatus.Down;
            capsuleCollider.center = new Vector3(0, downCenter, 0);
        }
        else                                            // Down -> Stand
        {
            bodyState = PlayerBodyStatus.Stand;
            moveSpeed += decreaseMoveSpeed;
            capsuleCollider.center = new Vector3(0, standsitCenter, 0);
            this.transform.position += new Vector3(0, 0.2f, 0);
        }
    }

    // 재장전 - R
    public override void KeyboardR()
    {
        if(isRun)
            return;

        currentEquip.Reload();
    }

    // 커서 On/off - Tab
    public override void KeyboardESC()
    {
        gameManager.PauseGame(!GameManager.isPause);
    }

    // 점프 - SpaceBar
    public override void SpaceBar()
    {
        isSpaceBar = !isSpaceBar;

        if (isSpaceBar)
        {
            if (!isJump)
                playerRigidbody.AddForce(this.transform.up * jumpPower);

            isJump = true;
        }
    }

    // 장비 선택 - 1
    public override void Keyboard1()
    {
        if (isClick)
            return;

        if (currentEquip.isAiming)
            currentEquip.Aiming();

        currentEquip = equipManager.CurrentEquipChanger(1);
    }

    // 장비 선택 - 2
    public override void Keyboard2()
    {
        if (isClick)
            return;

        if (currentEquip.isAiming)
            currentEquip.Aiming();

        currentEquip = equipManager.CurrentEquipChanger(2);
    }

    // 장비 선택 - 3
    public override void Keyboard3()
    {
        if (isClick)
            return;

        if (currentEquip.isAiming)
            currentEquip.Aiming();

        currentEquip = equipManager.CurrentEquipChanger(3);
    }

    // 장비 선택 - 4
    public override void Keyboard4()
    {
        if (isClick)
            return;

        if (currentEquip.isAiming)
            currentEquip.Aiming();

        currentEquip = equipManager.CurrentEquipChanger(4);
    }

    // 장비 선택 - 5
    public override void Keyboard5()
    {
        if (isClick)
            return;

        if (currentEquip.isAiming)
            currentEquip.Aiming();

        currentEquip = equipManager.CurrentEquipChanger(5);
    }

    // 앉기, 서기 기능
    private void SitDownStandUp()
    {
        if (bodyState.Equals(PlayerBodyStatus.Sit) && capsuleCollider.height > sitHeight)
            capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, sitHeight, Time.deltaTime * 6.0f);
        else if (bodyState.Equals(PlayerBodyStatus.Stand) && capsuleCollider.height < standHeight)
            capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, standHeight, Time.deltaTime * 6.0f);
        else if (bodyState.Equals(PlayerBodyStatus.Down) && capsuleCollider.height > sitHeight)
            capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, sitHeight, Time.deltaTime * 6.0f);
    }
}
