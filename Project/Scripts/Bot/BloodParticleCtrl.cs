using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BloodParticleCtrl : MonoBehaviour
{
    // Pool
    private IObjectPool<BloodParticleCtrl> managedPool;         // Pool Manager

    [SerializeField]
    private float destroyTime;                                  // 삭제 시간

    private void OnEnable()
    {
        StartCoroutine("DestroyBulletHole");
    }

    // 풀에 반납
    private IEnumerator DestroyBulletHole()
    {
        yield return new WaitForSeconds(destroyTime);

        managedPool.Release(this);
    }

    public void SetManagedPool(IObjectPool<BloodParticleCtrl> pool)
    {
        managedPool = pool;
    }
}
