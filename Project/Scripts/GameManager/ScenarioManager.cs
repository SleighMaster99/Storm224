using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScenarioManager : MonoBehaviour
{
    public bool isScenarioComplete;                     // 시나리오 미션 완료 여부
    public string nextScenarioSceneName;                // 다음 씬
    public string currentScenarioName;                  // 일시정지 메뉴 제목
    public string currentScenarioContent;               // 일시정지 시나리오 미션
    public string[] missionContents;                    // 세부 임무 목록
    public string currentmission;                       // 현재 진행중인 세부 임무

    public abstract void EndScenario();                 // 시나리오 종료 조건 달성시
    public abstract void ChangeCurrentMission(int index);        // 현재 미션 바꾸기
}
