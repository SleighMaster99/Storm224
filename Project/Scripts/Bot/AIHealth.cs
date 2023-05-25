using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AIHealth : MonoBehaviour
{
    [Header("Health Property")]
    [SerializeField]
    private float maxHP;            // 최대 체력
    [SerializeField]
    private float initHP;           // 초기화 체력
    public float HP;               // 체력

    int guntype;
    public GameObject[] charObject;
    public GameObject ragdollObject;
    public GameObject removeObject;

    // Pool
    private PoolManager poolManager;                // Pool Manager

    // Start is called before the first frame update
    void Start()
    {
        HealthInitialize();
        poolManager = GameObject.Find("Pool Manager").GetComponent<PoolManager>();
    }

    // 초기화
    private void HealthInitialize() => HP = initHP;

    // 데미지 받기
    public void Damage(float damageAmount, RaycastHit hit)
    {
        HP -= damageAmount;

        if (hit.transform.tag == "ally" || hit.transform.tag == "enemy")
        {
            // 피 파티클 풀에서 가져오기
            var bloodParticleCtrl = poolManager.bloodParticlePool.Get();
            bloodParticleCtrl.transform.position = hit.point;
            bloodParticleCtrl.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }

        if (HP < 0)
            HP = 0;

        if (HP <= 0)
            changeRagdoll();
    }

    // 데미지 받기
    public void Damage(float damageAmount)
    {
        HP -= damageAmount;

        if (HP < 0)
            HP = 0;

        if (HP <= 0)
        {
            if (this.transform.CompareTag("ally") || this.transform.CompareTag("enemy"))
                changeRagdoll();
            else
                changeRagdollPeople();
        }
    }

    // 회복하기
    public void Heal(float healAmount)
    {
        HP += healAmount;

        if (HP > maxHP)
            HP = maxHP;
    }


    public void changeRagdoll()
    {
        ragdollObject.transform.position = charObject[0].transform.position;
        ragdollObject.transform.rotation = charObject[0].transform.rotation;
        //CopyAnimCharacterTransformToRagdoll(charObject.transform, ragdollObject.transform);
        charObject[0].gameObject.SetActive(false);
        ragdollObject.gameObject.SetActive(true);
    }

    public void changeRagdollPeople()
    {
        foreach(GameObject g in charObject)
            g.SetActive(false);

        ragdollObject.SetActive(true);
        this.GetComponent<KoreanMaleCtrl>().StopMoving();
    }

    private void CopyAnimCharacterTransformToRagdoll(Transform origin, Transform rag)
    {
        for (int i = 0; i < origin.transform.childCount; i++)
        {
            if(origin.transform.childCount != 0)
            {
                CopyAnimCharacterTransformToRagdoll(origin.transform.GetChild(i), rag.transform.GetChild(i));
            }
            rag.transform.GetChild(i).localPosition = origin.transform.GetChild(i).localPosition;
            rag.transform.GetChild(i).localRotation = origin.transform.GetChild(i).localRotation;
        }
    }
}
