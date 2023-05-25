using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
    [Header("Components")]
    [SerializeField]
    private PlayerUI playerUI;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Health health;
    [SerializeField]
    private GameObject centerOfMass;

    [Header("Mouse Sensitive")]
    public float rotateXSpeed;                  // 마우스 가로 감도
    public float rotateYSpeed;                  // 마우스 수직 감도

    private Vector3 smoothRotation;
    private Vector3 currentVelocity;

    // 마우스 움직인 값
    private float rotateX;                      // 마우스 가로 움직임
    private float rotateY;                      // 마우스 세로 움직임

    [Header("Vehicle Force")]
    [SerializeField]
    private float accelationForce;
    [SerializeField]
    private float wheelForce;
    [SerializeField]
    private float brakeForce;

    [Header("Seat")]
    public Transform[] characterSeat; // 좌석
    public Transform[] ridePosition; // 하차 위치

    Rigidbody CarRigidbody;
    Collider vehicleBoxCollider;

    [Header("Audio Component")]
    public AudioSource[] audioSource; // 차량 오디오 소스

    [Header("Wheel Component")]
    [SerializeField]
    private WheelCollider[] wheelColliders;
    [SerializeField]
    private GameObject[] wheelMeshes;

    public PlayerControlable drivePlayer;
    public Transform currentSeat;

    private Control control;

    private bool isSpaceBar;

    [Header("Fuel")]
    [SerializeField]
    private float fuelDecreaseAmount;
    [SerializeField]
    private float maxVehicleVelocity;
    public float fuel;
    public float vehicleVelocity;
    public string vehicleName;

    public float vehicleRepairTime;

    [SerializeField]
    private GameObject fireEffect;

    private void Start()
    {
        control = FindObjectOfType<Control>();
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        CarRigidbody = this.transform.GetComponent<Rigidbody>();
        health = this.GetComponent<Health>();
        if(vehicleName.Equals("지프"))
            CarRigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void Update()
    {
        vehicleVelocity = CarRigidbody.velocity.magnitude;
        if (health.HP <= 0)
            fireEffect.SetActive(true);
    }

    // 운전석
    public override void Keyboard1()
    {
        MoveDriverSeat(characterSeat[0]);
        StartCoroutine("VehicleEngineOnSound");
    }

    // 보조석
    public override void Keyboard2()
    {
        MoveDriverSeat(characterSeat[1]);
        StartCoroutine("VehicleEngineOffSound");
    }

    // 좌석변경처리
    void MoveDriverSeat(Transform seatPosition)
    {
        drivePlayer.transform.position = seatPosition.position;
        drivePlayer.transform.rotation = seatPosition.rotation;

        currentSeat = seatPosition;
        drivePlayer.transform.SetParent(currentSeat);
    }

    private IEnumerator VehicleEngineOnSound()
    {
        audioSource[0].Play();

        yield return new WaitForSeconds(0.5f);

        audioSource[1].Play();
    }

    private IEnumerator VehicleEngineOffSound()
    {
        yield return null;

        if(audioSource[0].isPlaying)
            audioSource[0].Stop();

        if (audioSource[1].isPlaying)
            audioSource[1].Stop();
    }

    // 차량 탑승 상호작용
    public override void Interact()
    {
        GetOut();
        playerUI.UpdateVehicleUI(false, null, null);
    }

    // 차량 하차 
    private void GetOut()
    {
        var controller = control;
        var playerBody = drivePlayer.GetComponent<Rigidbody>();
        var playerCollider = drivePlayer.GetComponent<CapsuleCollider>();

        StopCoroutine("LookForward");

        drivePlayer.transform.SetParent(null);
        drivePlayer.cameraSocket = this.cameraSocket;
        drivePlayer.mainCamera = this.mainCamera;
        controller.ChangeControlTarget(drivePlayer);

        if (characterSeat[0] == currentSeat)
            drivePlayer.transform.position = ridePosition[0].position;
        else
            drivePlayer.transform.position = ridePosition[1].position;

        playerBody.isKinematic = false;
        playerCollider.isTrigger = false;

        drivePlayer.GetComponent<PlayerControlable>().currentEquip.gameObject.SetActive(true);

        drivePlayer = null;

        // 차량에서 내릴 때 사운드 종료
        foreach (AudioSource audios in audioSource)
            audios.Stop();
    }

    public override void SpaceBar()
    {
        isSpaceBar = !isSpaceBar;

        if (isSpaceBar)
        {
            StartCoroutine("Brake");

            for (int i = 0; i < wheelMeshes.Length; i++)
                wheelColliders[i].motorTorque = 0;

            audioSource[3].Play();
        }
        else
        {
            StopCoroutine("Brake");

            for (int i = 0; i < wheelMeshes.Length; i++)
                wheelColliders[i].brakeTorque = 0;
        }
    }

    private IEnumerator Brake()
    {
        while(true)
        {
            for (int i = 0; i < wheelMeshes.Length; i++)
            {
                wheelColliders[i].brakeTorque += brakeForce * 99999.0f * Time.deltaTime;
            }

            yield return null;
        }
    }

    //방향 전환
    public override void Rotate(Vector2 input)
    {
        if(input.x == 0 || input.y == 0)
        {
            StartCoroutine("LookForward");
        }
        else
        {
            if (mainCamera != null)
            {
                StopCoroutine("LookForward");

                float inputX = input.x * rotateXSpeed * 100 * Time.deltaTime;
                float inputY = input.y * rotateYSpeed * 100 * Time.deltaTime;
                rotateX += -inputY;
                rotateY += inputX;

                // 마우스 각도 제한
                rotateX = Mathf.Clamp(rotateX, -70.0f, 50.0f);

                smoothRotation = Vector3.SmoothDamp(smoothRotation, new Vector3(rotateX, rotateY), ref currentVelocity, 0.1f);

                mainCamera.transform.eulerAngles = new Vector3(0, rotateY, 0);
                mainCamera.transform.eulerAngles = smoothRotation;
            }
        }
    }

    private IEnumerator LookForward()
    {
        yield return new WaitForSeconds(1.0f);

        while (mainCamera.transform.rotation != currentSeat.rotation)
        {
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, currentSeat.rotation, 0.05f);
            yield return null;
        }
    }

    //Vehicle 움직이기
    public override void Move(Vector2 input)
    {
        if (health.HP <= 0)
        {
            //  사운드 종료
            foreach (AudioSource audios in audioSource)
                audios.Stop();

            return;
        }

        if(characterSeat[0] == currentSeat)
        {
            wheelColliders[0].steerAngle = input.x * wheelForce;
            wheelColliders[1].steerAngle = input.x * wheelForce;
        }

        if (characterSeat[0] == currentSeat && fuel > 0)
        {
            if (vehicleVelocity <= maxVehicleVelocity)
            {   // 최대속력이 아닐 때
                for (int i = 0; i < wheelMeshes.Length; i++)
                {
                    Quaternion quat;
                    Vector3 position;
                    wheelColliders[i].GetWorldPose(out position, out quat);
                    wheelMeshes[i].transform.SetPositionAndRotation(position, quat);
                }

                if(input.y != 0)
                {   // W, S 입력시
                    for (int i = 0; i < wheelMeshes.Length; i++)
                        wheelColliders[i].brakeTorque = 0;

                    // 가속
                    for (int i = 0; i < wheelMeshes.Length; i++)
                        wheelColliders[i].motorTorque = input.y * accelationForce * 1000.0f * Time.deltaTime;

                    audioSource[1].Stop();
                    if (!audioSource[2].isPlaying)
                        audioSource[2].Play();

                    fuel -= fuelDecreaseAmount * Time.deltaTime;
                }
                else
                {   // W, S 입력 안함
                    audioSource[2].Stop();
                    if (!audioSource[1].isPlaying)
                        audioSource[1].Play();

                    for (int i = 0; i < wheelMeshes.Length; i++)
                    {
                        wheelColliders[i].motorTorque = 0;
                        wheelColliders[i].brakeTorque = 0.001f;
                    }
                }
            }
            else
            {   // 최대 속력일 때
                for (int i = 0; i < wheelMeshes.Length; i++)
                    wheelColliders[i].motorTorque = 0;
            }
        }
        else
        {   // 운적석이 아니거나 기름이 없을 떄
            audioSource[1].Stop();
            audioSource[2].Stop();

            for (int i = 0; i < wheelMeshes.Length; i++)
            {
                wheelColliders[i].motorTorque = 0;
                wheelColliders[i].brakeTorque = 0.001f;
            }
        }

    }

    // 일시정지
    public override void KeyboardESC()
    {
        gameManager.PauseGame(!GameManager.isPause);
    }

    // 충돌시
    private void OnCollisionEnter(Collision collision)
    {
        if(vehicleVelocity > 15.0f)
        {
            health.Damage(10.0f * vehicleVelocity);
        }
    }


    // 안쓰는 키들
    public override void KeyboardR() { return; }
    public override void KeyboardZ() { return; }
    public override void KeyboardC() { return; }
    public override void LShift() { return; }
    public override void RMB() { return; }
    public override void LMB() { return; }
    public override void Keyboard3() { return; }
    public override void Keyboard4() { return; }
    public override void Keyboard5() { return; }
}
