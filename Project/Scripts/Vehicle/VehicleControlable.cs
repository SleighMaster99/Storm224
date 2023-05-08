using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
    // public Transform cameraSocket;              // 카메라 소켓
    //public Transform mainCamera;                // 플레이어 시점 카메라

    public float Vehicle_Speed;

    
    public Transform[] characterSeat; // 좌석
    public Transform[] ridePosition; // 하차 위치

    Rigidbody rigidbody;

    public AudioSource audioSource; // 차량 오디오 소스
    public AudioClip[] audioClip; //효과음을 저장할 배열 변수 선언

    public enum audio {Boarding,Moving, brake}; //오디오 타입 선언


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
        for (int i = 0; i < wheelMeshes.Length; i++)
        {
            wheelColliders[i].brakeTorque = 2000f / 2;
        }
        PlaySound(audio.brake);
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
        audioSource.Stop(); //차량에서 내릴 때 사운드 종료
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
        if (characterSeat[0] == currentSeat)
        {
            for (int i = 0; i < wheelMeshes.Length; i++)
            {

            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);
            wheelMeshes[i].transform.SetPositionAndRotation(position, quat);
             }

            wheelColliders[0].steerAngle = wheelColliders[1].steerAngle = input.x * 20f;
        
            for (int i = 0; i < wheelMeshes.Length; i++)
            {
                wheelColliders[i].motorTorque = input.y * (Vehicle_Speed / 4);
            }

            if (input.y != 0)   // 전진 중일 때
            {
                 for (int i = 0; i < wheelMeshes.Length; i++)
                 {
                    wheelColliders[i].brakeTorque = 0;
                 }
                if (audioSource.GetComponent<AudioSource>().isPlaying) return; //현재 사운드를 플레이 중이면 사운드 더 진행하지 않기
                else audioSource.GetComponent<AudioSource>().PlayOneShot(audioClip[1]); //플레이 중이 아니라면 1회만 플레이
                PlaySound(audio.Moving);
            }

        }

    }
    public void PlaySound(audio type) //효과음 플레이 함수
    {
        switch(type)
        {
            case audio.Boarding:
                audioSource.clip = audioClip[0]; 
                break;
            case audio.Moving: //움직일때 audioClip의 0번째 사운드 재생
                audioSource.clip = audioClip[1]; 
                break;
            case audio.brake:
                audioSource.clip = audioClip[2];//브레이크를 할 때 audioClip의 1번째 사운드 재생
                break;
        }
        audioSource.Play();
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
