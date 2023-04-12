using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private IObjectPool<Bullet> managedPool;        // Pool

    private float damage;                           // 총알 데미지

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
    }

    // 총알 발사
    public void FireBullet(Vector3 dir, float fireForce, float bulletDamage)
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().AddForce(dir * fireForce);
        damage = bulletDamage;
    }

    
    // 충돌시 Pool에 반납
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        managedPool.Release(this);
    }
}
