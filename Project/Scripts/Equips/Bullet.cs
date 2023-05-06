using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    // Pool
    private PoolManager poolManager;                // Pool Manager
    private IObjectPool<Bullet> managedPool;        // pool

    private Vector3 bulletHoleScale;                // 탄흔 Scale
    private float damage;                           // 총알 데미지

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
    }

    // 총알 발사
    public void FireBullet(PoolManager poolManager, Vector3 bulletHoleScale, Vector3 dir, float fireForce, float bulletDamage)
    {
        this.poolManager = poolManager;
        this.bulletHoleScale = bulletHoleScale;
        damage = bulletDamage;

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().AddForce(dir * fireForce);
    }
    
    // 충돌시 Pool에 반납
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint hit = collision.contacts[0];       // 충돌 정보
        Debug.Log(collision.gameObject.name);

        // x탄흔 풀에서 가져오기
        var bullethole =  poolManager.bulletHolePool.Get();
        bullethole.transform.position = hit.point;
        bullethole.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        bullethole.transform.localScale = bulletHoleScale;

        if (this.gameObject.activeSelf)     // Exeption 방지 : Trying to release an object that has already been released to the pool
            managedPool.Release(this);      // 풀에 반납
    }
}
