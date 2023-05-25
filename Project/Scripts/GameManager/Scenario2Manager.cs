using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenario2Manager : ScenarioManager
{
    [SerializeField]
    private GameObject lastObject;
    [SerializeField]
    private GameObject[] targets;

    private void Awake()
    {
        currentmission = missionContents[0];
    }

    // 다음 미션으로 변경
    public override void ChangeCurrentMission(int changeindex)
    {
        if (changeindex <= (missionContents.Length - 1))
            currentmission = missionContents[changeindex];
        else
            EndScenario();

        if (changeindex == (missionContents.Length - 1))
            lastObject.SetActive(true);

        if(changeindex == 1)
        {
            targets[0].SetActive(false);
            targets[1].SetActive(true);
        }
        else if(changeindex == 3)
        {
            targets[1].SetActive(false);
            targets[2].SetActive(true);
        }
    }

    // 시나리오 임무 성공시 : 시나리오 2 레디 씬으로 변경
    // 시나리오 임무 실패시 : 게임오버 UI 보여주기
    public override void EndScenario()
    {
        StartCoroutine(ChangeScenario2Scene());
    }

    private IEnumerator ChangeScenario2Scene()
    {
        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene(nextScenarioSceneName);
    }
}
