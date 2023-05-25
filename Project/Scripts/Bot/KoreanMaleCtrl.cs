using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class KoreanMaleCtrl : MonoBehaviour
{
    [Header("애니메이션")]
    public Animator anim;

    [Header("순찰 위치")]
    public Transform[] goalPoint;   //순찰 포인트


    [Header("속도")]
    public float walkSpeed;
    public float RunSpeed;


    NavMeshAgent agent;
    int goalIndex;
    Transform nextPoint;
    bool isExplosion;
    bool isWalking;

    private IObjectPool<KoreanMaleCtrl> managedPool;         // pool

    public void SetManagedPool(IObjectPool<KoreanMaleCtrl> pool)
    {
        managedPool = pool;
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("isExplosion", false);
        anim.SetBool("isMove", false);
        agent.speed = walkSpeed;
        goalIndex = -1;
        isWalking = false;
        agent.enabled = false;

        StartCoroutine(MoveStart());
    }

    private IEnumerator MoveStart()
    {
        yield return new WaitForSeconds(0.2f);

        isWalking = true;
        agent.enabled = true;

        MoveTowards(goalPoint[1].transform.position); // 다음 목적지로 이동

        while (true)
        {
            if (Vector3.Distance(this.transform.position, goalPoint[1].transform.position) <= 2.0f) // 적 발견 하지 않고 목적지에 도착
                managedPool.Release(this);      // 풀에 반납

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (isWalking)
            NextPoint();
        */
        if (GameManager.scenarioManager.isScenarioComplete) // 폭발 하면
        {
            agent.speed = 0;
            anim.SetBool("isExplosion", true);
        }
    }

    void NextPoint() // 다음 목표 지점
    {
        if ((agent.remainingDistance <= agent.stoppingDistance)) // 적 발견 하지 않고 목적지에 도착
        {
            if (++goalIndex == goalPoint.Length) // 인덱스가 끝에 도달하면
            {
                managedPool.Release(this);      // 풀에 반납
            }
            else
                nextPoint = goalPoint[goalIndex]; // 다음 목적지 설정
            
            MoveTowards(nextPoint.position); // 다음 목적지로 이동
        }

        isWalking = false;
    }

    public void MoveTowards(Vector3 targetPosition) //이동
    {
        //Debug.Log("Move");
        anim.SetBool("isMove", true);
        agent.isStopped = false;
        agent.SetDestination(targetPosition); //타겟 위치로 이동
    }
    public void StopMoving()// 정지
    {
        isWalking = false;
        anim.SetBool("isMove", false);
        agent.ResetPath();
        agent.isStopped = true;
        agent.speed = 0;
        agent.velocity = Vector3.zero;
    }

    void reset()
    {
        anim.SetBool("isExplosion", false);
        anim.SetBool("isMove", false);
        agent.speed = walkSpeed;
        goalIndex = -1;
    }

}