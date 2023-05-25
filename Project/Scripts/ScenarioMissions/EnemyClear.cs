using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClear : MonoBehaviour
{
    [SerializeField]
    private int changeIndex;

    [SerializeField]
    private GameObject movingEnemy;

    [SerializeField]
    private AIHealth[] enemyHealths;
    [SerializeField]
    private Health[] tankHealths;

    [SerializeField]
    private bool isAllDie;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealths = movingEnemy.GetComponentsInChildren<AIHealth>();
        tankHealths = movingEnemy.GetComponentsInChildren<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.scenarioManager.currentmission.Equals(GameManager.scenarioManager.missionContents[changeIndex-1]))
        {   // 현재 미션이 특정 미션일 때
            movingEnemy.SetActive(true);
        }

        if (movingEnemy.activeSelf && GameManager.scenarioManager.currentmission.Equals(GameManager.scenarioManager.missionContents[changeIndex-1]))
        {   // 현재 미션을 진행중일 때
            if (isAllDie)
            {
                GameManager.scenarioManager.ChangeCurrentMission(changeIndex);
                GameManager.gameManagerPlayerUI.MissionComplete();
                Debug.Log("isAllDie");
            }
            else
            {
                foreach (AIHealth ahp in enemyHealths)
                {
                    if (ahp.HP <= 0)
                        isAllDie = true;
                    else
                    {
                        isAllDie = false;
                        return;
                    }
                }

                foreach (Health ahp in tankHealths)
                {
                    if (ahp.HP <= 0)
                        isAllDie = true;
                    else
                    {
                        isAllDie = false;
                        return;
                    }
                }
            }
        }
    }

    private IEnumerator Clear()
    {
        GameManager.gameManagerPlayerUI.MissionComplete();
        GameManager.scenarioManager.ChangeCurrentMission(changeIndex);

        yield return null;
    }
}
