using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArtilleryAmmo : MonoBehaviour
{
    private IObjectPool<ArtilleryAmmo> managedPool;         // pool
    private float damage;                                   // 포탄 데미지
    private float damageRange;                              // 피격 범위
    private float explosionForce;                           // 피격 범위

    [SerializeField]
    private GameObject explosionEffect;                     // 폭발 이펙트
    [SerializeField]
    private GameObject fireingLight;                        // 날아가는 동안 불빛 이펙트
    [SerializeField]
    private AudioSource audioSource;                        // 폭발 사운드
    [SerializeField]
    private float destroyTime;                              // 오브젝트 풀에 반납시간

    public void SetManagedPool(IObjectPool<ArtilleryAmmo> pool)
    {
        managedPool = pool;
    }

    // 포탄 발사
    public void FireBullet(Vector3 dir, float fireForce, float artilleryAmmoDamage, float damageRange, float explosionForce)
    {
        fireingLight.SetActive(true);
        damage = artilleryAmmoDamage;
        this.damageRange = damageRange;
        this.explosionForce = explosionForce;

        this.GetComponent<SphereCollider>().enabled = true;
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(dir * fireForce);
    }

    // 충돌시 Pool에 반납
    private void OnCollisionEnter(Collision collision)
    {
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        this.GetComponent<SphereCollider>().enabled = false;

        fireingLight.SetActive(false);
        explosionEffect.SetActive(true);

        if (!audioSource.isPlaying)
            audioSource.Play();

        // 폭발 범위 물체 콜라이더 얻기
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, damageRange);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            // 중력에 영향을 받는 오브젝트 날리기
            if (rigidbody != null)
                rigidbody.AddExplosionForce(explosionForce, this.transform.position, damageRange, explosionForce);

            if (hit.transform.CompareTag("enemy") || hit.transform.CompareTag("ally"))
            {   // 적일 때 피 파티클 및 데미지 처리
                AIHealth aihealth = hit.transform.GetComponent<AIHealth>();
                if(aihealth != null)
                    aihealth.Damage(damage);
            }
            else if (hit.transform.CompareTag("Vehicle"))
            {   // 차량일 때 데미지 처리
                Health health = hit.transform.GetComponent<Health>();
                health.Damage(damage);
            }
            else if (hit.transform.CompareTag("Player"))
            {   // 플레이어일 때 데미지 처리
                Health health = hit.transform.GetComponent<Health>();
                health.Damage(damage);
            }
        }

        StartCoroutine("DestroyAmmo");
    }

    private IEnumerator DestroyAmmo()
    {
        yield return new WaitForSeconds(destroyTime);

        explosionEffect.SetActive(false);

        managedPool.Release(this);      // 풀에 반납
    }
}
