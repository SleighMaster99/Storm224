using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CartridgeCase : MonoBehaviour
{
    private IObjectPool<CartridgeCase> managedPool;         // Pool

    private float fireForce;
    private float destroyTime;                              // 탄피 제거 시간

    public void SetManagedPool(IObjectPool<CartridgeCase> pool)
    {
        managedPool = pool;
    }

    // 탄피 풀에서 가져오기
    public void RemoveCartridgeCase(Vector3 dir, float fireForce, float destroyTime)
    {
        this.fireForce = Random.Range(fireForce - 0.3f, fireForce);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().AddForce(dir * this.fireForce);
        this.destroyTime = destroyTime;

        StartCoroutine("RemoveObject");
    }

    // 탄피 풀에 반납
    private IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(destroyTime);

        managedPool.Release(this);
    }
}
