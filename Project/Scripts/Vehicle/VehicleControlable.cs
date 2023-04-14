using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
    // public Transform cameraSocket;              // 카메라 소켓
    //public Transform mainCamera;                // 플레이어 시점 카메라
    
    public Transform[] characterSeat; // 좌석
    public Transform[] ridePosition; // 하차 위치

    Rigidbody rigidbody;

    [SerializeField]
    private WheelCollider[] wheelColliders;
    [SerializeField]
    private GameObject[] wheelMeshes;

    public PlayerControlable drivePlayer;
    public Transform currentSeat;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentSeat);
    }
    public override void Keyboard1()
    {
        MoveDriverSeat(characterSeat[0]);
    }
    public override void Keyboard2()
    {
        MoveDriverSeat(characterSeat[1]);
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
        GetOut();
    }
    void MoveDriverSeat(Transform seatPosition)
    {
        drivePlayer.transform.position = seatPosition.position;
        drivePlayer.transform.rotation = seatPosition.rotation;

        currentSeat = seatPosition;
    }
    public override void Spacebar()
    {
        Break();
    }
    public void Break()
    {
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].brakeTorque = 2000f / 2;
        }
    }
    //차량 하차 
    private void GetOut()
    {
        var controller = FindObjectOfType<Controller>();
        var charBody = drivePlayer.GetComponent<Rigidbody>();
        var charCollider = drivePlayer.GetComponent<CapsuleCollider>();

        if (characterSeat[0] == currentSeat)
            drivePlayer.transform.position = ridePosition[0].position;
        else
            drivePlayer.transform.position = ridePosition[1].position;
        
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
        
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = input.y * (2000f / 4);
        }

        if (input.y != 0)   // 전진 중이 아닐 때
        {
            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].brakeTorque = 0;
            }
        }
        if (characterSeat[1] == currentSeat)
        {

        }
        //else    // 키를 눌렀을 때
        //{
        //    for (int i = 0; i < 4; i++)
        //    {
        //        wheelColliders[i].brakeTorque = 0; // 브레이크 해제
        //    }
        //}
    }

    //차량 흔들림 방지
    public float downForceValue;

    void AddDownForce()
    {
        rigidbody.AddForce(-transform.up * downForceValue * rigidbody.velocity.magnitude);
        rigidbody.centerOfMass = new Vector3(0, -1, 0); //차 무게중심 낮추기
    }


    // Start is called before the first frame update
}
