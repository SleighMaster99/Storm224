using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M18 : MonoBehaviour
{
    [Header("Grenade Components")]
    [SerializeField]
    private AudioSource audioSource;            // 오디오 소스

    [Header("Grenade Property")]
    [SerializeField]
    private GameObject grenadeBody;             // 투척류 Mesh
    [SerializeField]
    private GameObject smokeEffect;             // 폭발 효과(Particle)

    private float explosionWaitTime;            // 코킹 후 폭발까지 시간
    private float destroyTime;                  // 폭발 후 제거 시간
    public bool isCocking;                      // 코킹 여부

    [Header("Sounds")]
    [SerializeField]
    private AudioClip smokeSound;           // 폭발 소리

    public void InitializedGrenade(Transform spawnPoint, float explosionWaitTime, float destroyTime)
    {
        isCocking = false;

        this.transform.SetParent(spawnPoint);
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

        if (smokeEffect != null)
            smokeEffect.SetActive(true);

        if (smokeSound != null)
            audioSource.PlayOneShot(smokeSound);

        StartCoroutine("DestroyGrenade");
    }

    // 제거
    public IEnumerator DestroyGrenade()
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(this.gameObject);
    }
}
