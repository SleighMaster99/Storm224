using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
    // public Transform cameraSocket;              // 카메라 소켓
    //public Transform mainCamera;                // 플레이어 시점 카메라
    public Transform characterSeat;
    public Transform ridePosition;
    Rigidbody rigidbody;

    [SerializeField]
    private WheelCollider[] wheelColliders;
    [SerializeField]
    private GameObject[] wheelMeshes;

    public PlayerControlable drivePlayer;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void KeyboardR()
    {

    }
    public override void KeyboardZ()
    {

    }
    public override void KeyboardC()
    {

    }
    public override void LShift()
    {

    }
    public override void RMB()
    {

    }
    public override void LMB()
    {

    }
    //차량 탑승 상호작용
    public override void Interact()
    {
        StartCoroutine(GetOut());
    }

    //차량 하차  
    private IEnumerator GetOut()
    {
        var controller = FindObjectOfType<Controller>();
        var charBody = drivePlayer.GetComponent<Rigidbody>();
        var charCollider = drivePlayer.GetComponent<CapsuleCollider>();

        while(Vector3.Distance(drivePlayer.transform.position, ridePosition.position) > 0.1f)
        {
            yield return null;
            drivePlayer.transform.position += (ridePosition.position - drivePlayer.transform.position).normalized * Time.deltaTime * 10f;
        }

        charBody.isKinematic = false;
        charCollider.isTrigger = false;

        drivePlayer.transform.SetParent(null);

        controller.ChangeControlTarget(this,drivePlayer);
        drivePlayer = null;
    }


    //방향 전환
    public override void Rotate(Vector2 input)
    {
        if (mainCamera != null)
        {
            Vector3 camAngle = mainCamera.rotation.eulerAngles;
            float x = camAngle.x - input.y;

            if(x < 180f)
            {
                x = Mathf.Clamp(x, -1f, 70f);
            }
            else
            {
                x = Mathf.Clamp(x, 335f, 361f);
            }
            mainCamera.rotation = Quaternion.Euler(x, camAngle.y + input.x, camAngle.z);
        }
    }
    //Vehicle 움직이기
    public override void Move(Vector2 input)
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);
            wheelMeshes[i].transform.SetPositionAndRotation(position, quat);
        }

        wheelColliders[0].steerAngle = wheelColliders[1].steerAngle = input.x * 20f;
        //차량 미끄럼방지
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = input.y * (2000f / 4);
        }
        if (input.y == 0)   // 전진 중이 아닐 때
        {
            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].brakeTorque = 2000f / 2;
            }
        }
        else    // 키를 눌렀을 때
        {
            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].brakeTorque = 0; // 브레이크 해제
            }
        }
    }

    //차량 흔들림 방지
    public float downForceValue;

    void AddDownForce()
    {
        rigidbody.AddForce(-transform.up * downForceValue * rigidbody.velocity.magnitude);
    }


    // Start is called before the first frame update
}