using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioMissionTrigger : MonoBehaviour
{
    [SerializeField]
    private int changeIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") &&
            !GameManager.scenarioManager.currentmission.Equals(GameManager.scenarioManager.missionContents[changeIndex]))
        {
            GameManager.gameManagerPlayerUI.MissionComplete();
            GameManager.scenarioManager.ChangeCurrentMission(changeIndex);
        }   
    }
}
