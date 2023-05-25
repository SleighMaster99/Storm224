using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawnManager : MonoBehaviour
{
    private PoolManager poolManager;            // Pool Manager
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private float maxSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        poolManager = GameObject.Find("Pool Manager").GetComponent<PoolManager>();

        StartCoroutine(PeopleSpawn());
    }

    private IEnumerator PeopleSpawn()
    {
        while (!GameManager.scenarioManager.isScenarioComplete)
        {
            var people = poolManager.koreanMaleCtrlPool.Get();

            people.goalPoint[0] = startPoint;
            people.goalPoint[1] = endPoint;

            people.transform.position = startPoint.position;

            float waitTime = Random.Range(2.0f, maxSpawnTime);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
