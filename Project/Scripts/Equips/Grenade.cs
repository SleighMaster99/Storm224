using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Grenade Components")]
    [SerializeField]
    private AudioSource audioSource;            // 오디오 소스

    [Header("Grenade Property")]
    [SerializeField]
    private GameObject grenadeBody;             // 투척류 Mesh
    [SerializeField]
    private GameObject explosionEffect;         // 폭발 효과(Particle)
    [SerializeField]
    private float damage;                       // 데미지

    private float grenadeEffectForce;           // 투척 폭발 힘
    private float grenadeEffectUpperForce;      // 투척 폭발 위쪽 힘
    private float grenadeEffectRadius;          // 투척 폭발 범위
    private float explosionWaitTime;            // 코킹 후 폭발까지 시간
    private float destroyTime;                  // 폭발 후 제거 시간
    public bool isCocking;                      // 코킹 여부

    [Header("Sounds")]
    [SerializeField]
    private AudioClip explosionSound;           // 폭발 소리

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, grenadeEffectRadius);
    }

    public void InitializedGrenade(Transform spawnPoint, float grenadeEffectForce, float grenadeEffectUpperForce, float grenadeEffectRadius, float explosionWaitTime, float destroyTime)
    {
        isCocking = false;

        this.transform.SetParent(spawnPoint);
        this.grenadeEffectForce = grenadeEffectForce;
        this.grenadeEffectUpperForce = grenadeEffectUpperForce;
        this.grenadeEffectRadius = grenadeEffectRadius;
        this.explosionWaitTime = explosionWaitTime;
        this.destroyTime = destroyTime;
    }

    // 던지기
    public void Throw(Vector3 throwPosition, Vector3 throwDirection, float throwForce)
    {
        this.transform.position = throwPosition;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce);
    }


    // 안전손잡이 제거(코킹)
    public void Cocking()
    {
        isCocking = true;
        StartCoroutine("GrenadeExplosion");
    }

    // 폭발
    private IEnumerator GrenadeExplosion()
    {
        yield return new WaitForSeconds(explosionWaitTime);

        this.GetComponent<Rigidbody>().useGravity = false;
        grenadeBody.SetActive(false);

        if (explosionEffect != null)
            explosionEffect.SetActive(true);

        if (explosionSound != null)
            audioSource.PlayOneShot(explosionSound);

        // 폭발 범위 물체 콜라이더 얻기
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, grenadeEffectRadius);

        foreach(Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            // 중력에 영향을 받는 오브젝트 날리기
            if (rigidbody != null)
                rigidbody.AddExplosionForce(grenadeEffectForce, this.transform.position, grenadeEffectRadius, grenadeEffectUpperForce);

            if (hit.transform.CompareTag("enemy") || hit.transform.CompareTag("ally"))
            {   // 적일 때 피 파티클 및 데미지 처리
                AIHealth aihealth = hit.transform.GetComponent<AIHealth>();
                if (aihealth != null)
                    aihealth.Damage(damage);
            }
            else if (hit.transform.CompareTag("Vehicle") || hit.transform.CompareTag("Player"))
            {   // 차량일 때 데미지 처리
                Health health = hit.transform.GetComponent<Health>();
                health.Damage(damage);
            }
        }

        StartCoroutine("DestroyGrenade");
    }

    // 제거
    public IEnumerator DestroyGrenade()
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(this.gameObject);
    }
}
