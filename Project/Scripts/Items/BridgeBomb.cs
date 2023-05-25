using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBomb : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField]
    private GameObject[] bombInstallBoxes;              // 폭탄 설치 박스
    [SerializeField]
    private GameObject[] bombs;                         // 폭탄 외형
    [SerializeField]
    private GameObject[] explosionParticles;            // 폭발 파티클
    [SerializeField]
    private GameObject stick;                           // bomber stick
    [SerializeField]
    private GameObject stickMovePoint;                  // bomber stick move point
    [SerializeField]
    private GameObject[] smokeParticles;                // 연기 파티클

    [Header("Rigidboyd")]
    [SerializeField]
    private Rigidbody[] bridgeRigidbodys;               // 폭발한 다리 Rigidbody

    [Header("Variable")]
    [SerializeField]
    private bool[] isInstalls;                          // 폭탄 설치 여부
    [SerializeField]
    private float explosionRadius;                      // 폭발 범위
    [SerializeField]
    private float explosionForce;                       // 폭발 위력
    [SerializeField]
    private float explosionUpperForce;                  // 폭발 Up Vector 위력

    [Header("AudioSource")]
    [SerializeField]
    AudioSource closeBombAudioSource;
    [SerializeField]
    AudioSource farBombAudioSource;
    [SerializeField]
    AudioSource bomberActiveAudioSource;

    private void Awake()
    {
        isInstalls = new bool[12];
        
        /*
        // 폭파 테스트 폭탄 설치
        foreach (GameObject boxes in bombInstallBoxes)
        {
            InstallBomb(boxes.transform.parent.position);
        }
        */
    }

    // 폭탄 설치
    public void InstallBomb(Vector3 hitPosition)
    {
        for (int i = 0; i < isInstalls.Length; i++)
        {
            if (bombs[i].transform.parent.position.Equals(hitPosition))
            {
                bombs[i].SetActive(true);
                isInstalls[i] = true;
            }
        }
    }

    // 설치된 폭탄 폭파
    public void ActiveBomber() => StartCoroutine("Booooooooomb");

    // 2초뒤 폭파 효과
    private IEnumerator Booooooooomb()
    {
        StartCoroutine("MoveBomberStick");

        yield return new WaitForSeconds(2.0f);

        ExplodeBomb();
    }

    private IEnumerator MoveBomberStick()
    {
        while(stick.transform.position != stickMovePoint.transform.position)
        {
            stick.transform.position = Vector3.MoveTowards(stick.transform.position, stickMovePoint.transform.position, 1.0f * Time.deltaTime);

            yield return null;
        }

        bomberActiveAudioSource.Play();
    }

    // 설치된 폭탄 폭파 기능
    private void ExplodeBomb()
    {
        // 폭탄이 설치된 다리 물리법칙 작용
        for (int i = 0; i < bridgeRigidbodys.Length; i++)
        {
            if (isInstalls[i*4] && isInstalls[i*4+1] && isInstalls[i*4+2] && isInstalls[i*4+3])
            {
                bridgeRigidbodys[i].useGravity = true;
                bridgeRigidbodys[i].constraints = RigidbodyConstraints.None;

                smokeParticles[i * 2].SetActive(true);
                smokeParticles[i * 2 + 1].SetActive(true);
            }
        }

        // 폭발 효과
        for (int i = 0; i < isInstalls.Length; i++)
        {
            if (isInstalls[i])
            {   // 폭탄이 설치되었을 때
                explosionParticles[i].SetActive(true);

                Collider[] colldiers = Physics.OverlapSphere(bombs[i].transform.position, explosionRadius);
                foreach (Collider hit in colldiers)
                {
                    Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
                    // 중력에 영향을 받는 오브젝트 날리기
                    if (rigidbody != null)
                        rigidbody.AddExplosionForce(explosionForce, bombs[i].transform.position, explosionRadius, explosionUpperForce);

                    Health health = hit.GetComponent<Health>();
                    if (health != null)
                        health.Damage(5000.0f);

                    AIHealth health2 = hit.GetComponent<AIHealth>();
                    if (health2 != null)
                        health2.Damage(5000.0f);
                }

                bombs[i].SetActive(false);
                bombInstallBoxes[i].SetActive(false);
            }
        }

        closeBombAudioSource.Play();
        StartCoroutine("FarBombSound");

        // 시나리오 종료
        foreach(bool install in isInstalls)
        {
            if (!install)
                break;
            else
                GameManager.scenarioManager.isScenarioComplete = true;
        }
        GameManager.scenarioManager.EndScenario();
    }

    // 1.5초뒤 멀리서 들리는 폭탄 효과
    private IEnumerator FarBombSound()
    {
        yield return new WaitForSeconds(1.5f);

        farBombAudioSource.Play();
    }

    /*
    // 폭파 테스트 폭파
    [SerializeField]
    private bool isBomb;
    private void Update()
    {
        if (isBomb)
        {
            StartCoroutine("Booooooooomb");
            isBomb = false;
        }  
    }
    */
}
