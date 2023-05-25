using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private TMP_Text scenarioNameText;
    [SerializeField]
    private TMP_Text scenarioContentText;
    [SerializeField]
    private TMP_Text missionContentText;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ButtonResume()
    {
        gameManager.PauseGame(false);
    }

    public void ButtonOption()
    {
        gameManager.optionMenuUI.SetActive(true);
    }

    public void ButtonMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ButtonExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Pause 메뉴 시나리오 제목 변경
    public void ChangeScenarioName(string scenarioName)
    {
        this.scenarioNameText.text = scenarioName;
    }

    // Pause 메뉴 시나리오 명령 변경
    public void ChangeScenarioContent(string scenarioContent)
    {
        this.scenarioContentText.text = scenarioContent;
    }

    // Pause 메뉴 시나리오 작전 내용 변경
    public void ChangeMissionContent(string missionContent)
    {
        this.missionContentText.text = missionContent;
    }
}
