using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DC.Scanner;

public class TankCtrl : MonoBehaviour
{
    public TargetScanner scanner;
    Transform pos;

    [Header("탐지 거리")]
    public float viewRange;

    [Header("속도")]
    public int Speed;

    [Header("포탑")]
    public GameObject turret;

    [Header("포신 위치")]
    public GameObject gunPosition;

    [Header("포탄")]
    public GameObject bulletPrefab;

    [Header("사운드")]
    public AudioSource moveAudioSource;
    public AudioSource fireAudioSource;

    AudioSource audioSource;

    [Header("파티클")]
    public ParticleSystem particleObject;

    [Header("순찰 위치")]
    public Transform[] goalPoint;   //순찰 포인트

    [Header("연사 속도")]
    public float fireRate;         //연사 속도

    [Header("Components")]
    [SerializeField]
    private Health health;          // HP
    private PoolManager poolManager;            // Pool Manager
    public Roll_Wheels[] roll_wheels;

    [Header("Tank")]
    [SerializeField]
    private float fireForce;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float damageRange;
    [SerializeField]
    private float explosionForce;
    public bool isWait;

    NavMeshAgent agent;             //navMesh
    int goalIndex;                  //순찰 포인트 배열의 인덱스 컨트롤
    Transform nextPoint;            //다음 순찰 포인트
    bool isSearch;
    bool isFire;
    bool isDie;

    float nextShotTime;         //다음 발사 가능 시간

    private void Awake()
    {
        poolManager = GameObject.Find("Pool Manager").GetComponent<PoolManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        goalIndex = -1;
        isSearch = false;
        isFire = false;
        isDie = false;
        roll_wheels[0].isMove = false;
        roll_wheels[1].isMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsDestroy();

        if (!isDie)
        {
            if (!isSearch && !isWait)
                NextPoint();

            if (isWait)
            {
                moveAudioSource.Stop();
                agent.enabled = false;
            }

            Search();
        }
    }

    public void Search()
    {
        pos = scanner.GetNearestTarget(); // 가장 가까운 적의 정보를 가져옴
        //정찰병이고 적 미식별 
        if (pos == null && isSearch)
        {
            scanner.ViewRadius = viewRange; // 탐지 범위 원래대로
            isSearch = false;        // 탐색 중 아님
            isFire = false;

        }
        if (pos != null)    // 적이 있으면 
        {
            if (!isSearch)
            {
                scanner.ViewRadius = viewRange + 2.0f;
                StopMoving();
                isFire = true;
                isSearch = true;
            }
            Vector3 directionToPlayer = (pos.position - this.transform.position);//회전 방향

            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer.normalized);//정규화

            turret.transform.rotation = lookRotation; //회전


            if (isFire)
            {
                if (Time.time > nextShotTime)
                {
                    makeBullet();
                }
            }
        }
    }

    public void makeBullet()
    {
        nextShotTime = Time.time + fireRate;
        particleObject.Play();
        fireAudioSource.Play();
        var artilleryAmmo = poolManager.artilleryAmmoPool.Get();

        artilleryAmmo.transform.position = gunPosition.transform.position;
        artilleryAmmo.FireBullet(gunPosition.transform.forward, fireForce, damage, damageRange, explosionForce);
    }

    void NextPoint() // 다음 목표 지점
    {
        if (!(moveAudioSource.isPlaying))
        {
            moveAudioSource.Play();
        }
        if ((agent.remainingDistance <= agent.stoppingDistance)) // 적 발견 하지 않고 목적지에 도착
        {
            goalIndex++; // 다음 목적지로 변경
            if (goalIndex == goalPoint.Length) // 인덱스가 끝에 도달하면
            {
                goalIndex = 0; // 다음 목적지 초기화
            }
            nextPoint = goalPoint[goalIndex]; // 다음 목적지 설정
            //Debug.Log("GoalIndex : " + goalIndex);
            MoveTowards(nextPoint.position); // 다음 목적지로 이동
        }
    }

    public void MoveTowards(Vector3 targetPosition) //이동
    {
        //Debug.Log("Move");
        roll_wheels[0].isMove = true;
        roll_wheels[1].isMove = true;
        agent.enabled = true;
        agent.SetDestination(targetPosition); //타겟 위치로 이동
    }

    public void StopMoving()// 정지
    {
        roll_wheels[0].isMove = false;
        roll_wheels[1].isMove = false;
        moveAudioSource.Stop();
        agent.enabled = false;
    }

    [SerializeField]
    private GameObject dieEffect1;
    [SerializeField]
    private GameObject dieEffect2;
    private void IsDestroy()
    {
        if(health.HP <= 0)
        {
            isDie = true;
            dieEffect1.SetActive(true);
            dieEffect2.SetActive(true);
            agent.enabled = false;
            roll_wheels[0].isMove = false;
            roll_wheels[1].isMove = false;
        }
    }

    private void OnDrawGizmos()
    {
        scanner.ShowGizmos();
    }
}