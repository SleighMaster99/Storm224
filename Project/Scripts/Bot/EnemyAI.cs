using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DC.Scanner;

public class EnemyAI : MonoBehaviour
{
    public TargetScanner scanner;
    Transform pos;
    [Header("속도")]
    public int runSpeed;
    public int walkSpeed;

    [Header("정찰병")]
    public bool isScout; // 정찰병
    public bool Kneel;
    [SerializeField]
    private float findRange;
    [SerializeField]
    private float attackRange;


    [Header("탐색 여부")]
    public bool isSearch;

    [Header("총기 종류 (1~6)")]
    public int weaponsTypes;

    [Header("총열 위치")]
    public GameObject gunPosition;

    [Header("애니메이션")]
    public Animator anim;

    [Header("사운드")]
    public AudioSource fire;

    [Header("파티클")]
    public ParticleSystem particleObject;

    [Header("명중률")]
    [SerializeField]
    private int hitPercent;


    [Header("순찰 위치")]
    public Transform[] goalPoint;   //순찰 포인트
    
    NavMeshAgent agent;             //navMesh
    int goalIndex;                  //순찰 포인트 배열의 인덱스 컨트롤
    Transform nextPoint;            //다음 순찰 포인트

    int damage;             //데미지
    float fireRate;         //연사 속도
    float reloadTime;       //장전 시간
    int magazineSize;       //한 탄창 갯수
    int maxMagazine;        //보유할수 있는 최대 탄창수
    int currentMagazine;    //남은 탄창
    int bulletsRemaining;       //잔여 총알 수
    bool isReloading = false;   //재장전 중인지 여부
    float nextShotTime;         //다음 발사 가능 시간
    bool isFire;
    int randomMotion;

    AIHealth aiHealth;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isSearch = false;
        WeaponsTypes(); // 무기 관련 변수 초기화 
        currentMagazine = maxMagazine; //현재 탄창수 초기화
        bulletsRemaining = magazineSize;    //보유탄창 수 초기화
        isFire = false;
        agent = GetComponent<NavMeshAgent>();
        aiHealth = this.GetComponent<AIHealth>();

        if (isScout)
        {
            anim.SetBool("isMove", true);
            agent.speed = walkSpeed;
            goalIndex = -1;                      //목적지 배열 인덱스 초기화
        }
        else if (!isScout)
        {
            anim.SetBool("isMove", false);
            if (Kneel)
                anim.SetBool("Kneel", true);
            else
                anim.SetBool("Kneel", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(aiHealth.HP > 0)
        {
            if (isScout)
            {
                Search(); // 탐색               
                NextPoint(); // 이동
            }
            else if (!isScout)
                Wait();
        }
    }



    public void Search()
    {
        pos = scanner.GetNearestTarget(); // 가장 가까운 적의 정보를 가져옴
        //정찰병이고 적 미식별 
        if (pos == null && isScout && isSearch )
        {
            scanner.ViewRadius = findRange; // 탐지 범위 원래대로
            isSearch = false;        // 탐색 중 아님
            isFire = false;
            anim.SetInteger("nextAnim", 2);
            anim.SetBool("Run", false);
        }

        if (pos != null)    // 적이 있으면 
        {
            scanner.ViewRadius = findRange + 2.0f; // 탐지 범위 증가

            if (!isSearch)
            {
                randomMotion = Random.Range(0,2);
                if (weaponsTypes == 2 || weaponsTypes == 4)  // 저격 총일 때 발견시 사격
                {
                    if(randomMotion == 0)
                        anim.SetInteger("nextAnim", 0);
                    else if(randomMotion == 1)
                        anim.SetInteger("nextAnim", 3);
                    isFire = true;
                    //StopMoving();
                }
                else if (isScout && (weaponsTypes != 2 && weaponsTypes != 4)) // 일반 총일 때 발견시 돌격
                {
                    anim.SetBool("Run", true);
                    agent.speed = runSpeed;
                    agent.SetDestination(pos.position);
                }
            }
            if(isScout && !isFire &&(Vector3.Distance(transform.position, pos.position) <= scanner.AlertRadius)) {
                // 돌격 시 일정 거리 이상 되면 멈추고 사격
                if (randomMotion == 0)
                    anim.SetInteger("nextAnim", 0);
                else if (randomMotion == 1)
                    anim.SetInteger("nextAnim", 3);
                isFire = true;
                StopMoving();
                agent.speed = walkSpeed;
            }
            isSearch = true; // 탐색 중
            //Debug.Log("found");

            Vector3 directionToPlayer = (pos.position - transform.position);//회전 방향

            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer.normalized);//정규화

            this.transform.rotation = lookRotation; //회전
            transform.Rotate(new Vector3(0f, 35f, 0f), Space.World);


            if (isFire)
            {
                RaycastHit hit = new RaycastHit();
                Vector3 d = (directionToPlayer).normalized; // 정규화
                float di = Vector3.Distance(transform.position, pos.position); // 목표물 간의 거리 
                Physics.Raycast(gunPosition.transform.position, d, out hit, di + 3.0f); //레이 발사
                Debug.DrawRay(gunPosition.transform.position, d * (di + 3.0f), Color.yellow); //레이 그리기

                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Kneel Fire Rifle") ||
                    this.anim.GetCurrentAnimatorStateInfo(0).IsName("Kneel Rifle Aiming Idle") ||
                    this.anim.GetCurrentAnimatorStateInfo(0).IsName("Rifle Aiming Idle") ||
                    this.anim.GetCurrentAnimatorStateInfo(0).IsName("Firing Rifle")) // 사격 자세 애니메이션 일때만
                {
                    if (this.gameObject.tag == "enemy") // 탐색을 하는 오브젝트가 적 일경우
                    {

                        if (hit.collider != null && (hit.collider.tag == "Player" || hit.collider.tag == "ally"))
                        {// 아군 및 플레이어 만 공격

                            // 공격 가능 체크
                            if (Time.time > nextShotTime)
                            {
                                //총알이 없거나 장전 중이 아님
                                if (bulletsRemaining > 0 && !isReloading)
                                {
                                    fire.Play();
                                    particleObject.Play();
                                    anim.SetInteger("nextAnim", 1);
                                    AttackAlly(hit);

                                }
                            }
                        }
                    }
                    else if (this.gameObject.tag == "ally") // 탐색을 하는 오브젝트가 아군 일경우
                    {
                        if (hit.collider != null && hit.collider.tag == "enemy")
                        {// 적만 공격

                            // 공격 가능 체크
                            if (Time.time > nextShotTime)
                            {
                                //총알이 없거나 장전 중이 아님
                                if (bulletsRemaining > 0 && !isReloading)
                                {
                                    fire.Play();
                                    particleObject.Play();
                                    anim.SetInteger("nextAnim", 1);
                                    AttackEnemy(hit);

                                }
                            }
                        }
                    }
                }
            }
        }
    }


    void NextPoint() // 다음 목표 지점
    { 
        if (!isSearch && (agent.remainingDistance <= agent.stoppingDistance)) // 적 발견 하지 않고 목적지에 도착
        {
            goalIndex++; // 다음 목적지로 변경
            if (goalIndex == goalPoint.Length) // 인덱스가 끝에 도달하면
            {
                goalIndex = 0; // 다음 목적지 초기화
            }
            nextPoint = goalPoint[goalIndex]; // 다음 목적지 설정
            MoveTowards(nextPoint.position); // 다음 목적지로 이동
        }
    }

    public void MoveTowards(Vector3 targetPosition) //이동
    {
        anim.SetBool("isMove", true); 
        agent.isStopped = false;    
        agent.SetDestination(targetPosition); //타겟 위치로 이동
    }

    void StopMoving()// 정지
    {
        anim.SetBool("isMove", false);
        agent.ResetPath();
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }

    public void AttackAlly(RaycastHit hit)  //아군 및 플레이어 공격
    {
        //Debug.Log("Attack");
        nextShotTime = Time.time + fireRate;

        int random = Random.Range(0, 100); // 명중률 0~6타격 6~9 빗나감 (명중률 70%)

        if ((hit.collider.tag == "ally" ) && (random < hitPercent))
        {                 
            AIHealth aiHealth = hit.collider.GetComponent<AIHealth>();
            if (aiHealth != null)
            {
                aiHealth.Damage(damage, hit);
            }
            
        }
        else if ((hit.collider.tag == "Player" || hit.collider.tag == "Vehicle") && (random < hitPercent))
        {
            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
            {
                health.Damage(damage);
            }

        }

        bulletsRemaining--;
        //Debug.Log("남은 탄알"+ bulletsRemaining);
        if (bulletsRemaining <= 0)
        {
            StartCoroutine(Reload());
        }
    }
    public void AttackEnemy(RaycastHit hit) // 적 공격
    {
        //Debug.Log("Attack");
        nextShotTime = Time.time + fireRate;

        int random = Random.Range(0, 100); // 명중률 0~6타격 6~9 빗나감 (명중률 70%)

        if (hit.collider.tag == "enemy" && random < hitPercent)
        {
            AIHealth aiHealth = hit.collider.GetComponent<AIHealth>();
            if (aiHealth != null)
            {
                aiHealth.Damage(damage, hit);
            }

        }

        bulletsRemaining--;
        //Debug.Log("남은 탄알"+ bulletsRemaining);
        if (bulletsRemaining <= 0)
        {

            StartCoroutine(Reload());
        }
    }
    IEnumerator Reload()
    {
        //Debug.Log("Reload");
        //탄창 보유 여부
        if (currentMagazine > 0)
        {
            //장전중 검사
            if (isReloading)
            yield break;

            isReloading = true;

            //장전 시간 만큼 딜레이
            yield return new WaitForSeconds(reloadTime);

        
            //탄창 초기화
            bulletsRemaining = magazineSize;
            
            //탄창 소비
            currentMagazine--;
            isReloading = false;
            //Debug.Log("Reload Finish");
        }
    }

    

    public void WeaponsTypes()
    {     
        switch (weaponsTypes)
        {
            case 1:
                // test Rifle
                damage = 60;
                fireRate = 2.3f;
                reloadTime = 2;
                magazineSize = 5;
                maxMagazine = 5;
                break;

            case 2:
                //M1 Garand
                damage = 20;
                fireRate = 3.0f;
                reloadTime = 3;
                magazineSize = 8;
                maxMagazine = 10;
                break;

            case 3:
                //M3
                damage = 7;
                fireRate = 0.5f;
                reloadTime = 2;
                magazineSize = 30;
                maxMagazine = 5;
                break;

            case 4:
                //Mosin Nagant
                damage = 25;
                fireRate = 4.0f;
                reloadTime = 2;
                magazineSize = 5;
                maxMagazine = 5;
                break;

            case 5:
                //DP-28
                damage = 20;
                fireRate = 0.4f;
                reloadTime = 2;
                magazineSize = 47;
                maxMagazine = 3;
                break;

            case 6:
                //PPSh-41
                damage = 7;
                fireRate = 0.5f;
                reloadTime = 2;
                magazineSize = 50;
                maxMagazine = 3;
                break;
        }
    }
    private void OnDrawGizmos()
    {
        scanner.ShowGizmos();
    }

    void Wait()
    {
        pos = scanner.GetNearestTarget(); // 가장 가까운 적의 정보를 가져옴
                                          // 
        if (pos == null && isSearch)
        {
            anim.SetBool("Run", false);
            scanner.ViewRadius = findRange; // 탐지 범위 원래대로
            isSearch = false;        // 탐색 중 아님
            isFire = false;
            anim.SetInteger("nextAnim", 2);
        }

        if (pos != null)    // 적이 있으면 
        {
            scanner.ViewRadius = findRange + 2.0f; // 탐지 범위 증가
            if (!isSearch)
            {
                randomMotion = Random.Range(0, 2);
                if (weaponsTypes == 2 || weaponsTypes == 4)  // 저격 총일 때 발견시 사격
                {
                    if (randomMotion == 0)
                        anim.SetInteger("nextAnim", 0);
                    else if (randomMotion == 1)
                        anim.SetInteger("nextAnim", 3);
                    isFire = true;
                }
                if (weaponsTypes != 2 && weaponsTypes != 4)  // 저격 총이 아닐 때 발견시 이동
                {
                    anim.SetBool("Run", true);
                    agent.speed = runSpeed;
                    agent.SetDestination(pos.position);
                }
            }

            if (!isFire && (Vector3.Distance(transform.position, pos.position) <= scanner.AlertRadius))
            {
                // 돌격 시 일정 거리 이상 되면 멈추고 사격
                if (randomMotion == 0)
                    anim.SetInteger("nextAnim", 0);
                else if (randomMotion == 1)
                    anim.SetInteger("nextAnim", 3);
                isFire = true;
                StopMoving();
                agent.speed = walkSpeed;
            }

            isSearch = true; // 탐색 중

            Vector3 directionToPlayer = (pos.position - transform.position);//회전 방향

            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer.normalized);//정규화
            this.transform.rotation = lookRotation; //회전
            transform.Rotate(new Vector3(0f, 35f, 0f), Space.World);


            if (isFire)
            {
                RaycastHit hit = new RaycastHit();
                Vector3 d = (directionToPlayer).normalized; // 정규화
                float di = Vector3.Distance(transform.position, pos.position); // 목표물 간의 거리 
                Physics.Raycast(gunPosition.transform.position, d, out hit, di + 3.0f); //레이 발사
                Debug.DrawRay(gunPosition.transform.position, d * (di + 3.0f), Color.yellow); //레이 그리기

                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Kneel Fire Rifle") ||
                    this.anim.GetCurrentAnimatorStateInfo(0).IsName("Kneel Rifle Aiming Idle") ||
                    this.anim.GetCurrentAnimatorStateInfo(0).IsName("Rifle Aiming Idle") ||
                    this.anim.GetCurrentAnimatorStateInfo(0).IsName("Firing Rifle")) // 사격 자세 애니메이션 일때만
                {
                    if (this.gameObject.tag == "enemy") // 탐색을 하는 오브젝트가 적 일경우
                    {

                        if (hit.collider != null && (hit.collider.tag == "Player" || hit.collider.tag == "ally"))
                        {// 아군 및 플레이어 만 공격

                            // 공격 가능 체크
                            if (Time.time > nextShotTime)
                            {
                                //총알이 없거나 장전 중이 아님
                                if (bulletsRemaining > 0 && !isReloading)
                                {
                                    fire.Play();
                                    particleObject.Play();
                                    anim.SetInteger("nextAnim", 1);
                                    AttackAlly(hit);

                                }
                            }
                        }
                    }
                    else if (this.gameObject.tag == "ally") // 탐색을 하는 오브젝트가 아군 일경우
                    {
                        if (hit.collider != null && hit.collider.tag == "enemy")
                        {// 적만 공격

                            // 공격 가능 체크
                            if (Time.time > nextShotTime)
                            {
                                //총알이 없거나 장전 중이 아님
                                if (bulletsRemaining > 0 && !isReloading)
                                {
                                    fire.Play();
                                    particleObject.Play();
                                    anim.SetInteger("nextAnim", 1);
                                    AttackEnemy(hit);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
