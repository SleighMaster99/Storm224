using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletHole : MonoBehaviour
{
    // Pool
    private IObjectPool<BulletHole> managedPool;        // Pool Manager

    [SerializeField]
    private Transform decal;                            // decal
    [SerializeField]
    private float setPosZ;                              // decal local position

    [SerializeField]
    private float destroyTime;                          // 삭제 시간

    private void Awake()
    {
        decal.position = new Vector3(0, 0, setPosZ);
    }

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

    public void SetManagedPool(IObjectPool<BulletHole> pool)
    {
        managedPool = pool;
    }
}
