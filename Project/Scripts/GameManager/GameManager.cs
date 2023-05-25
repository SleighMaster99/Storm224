using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static bool isPause;

    [SerializeField]
    private GameObject pauseMenuUI;         // PauseUI GameObject
    
    public GameObject optionMenuUI;        // optionUI GameObject
    public GameObject gameOverUI;

    public PauseUI pauseUI;                 // PauseUI Script

    // Static Scripts
    public static ScenarioManager scenarioManager;
    public static PlayerUI gameManagerPlayerUI;
    public static OptionUI optionUI;

    private void Awake()
    {
        scenarioManager = this.GetComponent<ScenarioManager>();
        pauseUI = pauseMenuUI.GetComponent<PauseUI>();
        if(GameObject.Find("PlayerUI") != null)
        {
            Debug.Log("T");
            gameManagerPlayerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        }
        optionUI = optionMenuUI.GetComponent<OptionUI>();
        optionMenuUI.SetActive(true);
        optionMenuUI.SetActive(false);

        pauseUI.ChangeScenarioName(scenarioManager.currentScenarioName);
        pauseUI.ChangeScenarioContent(scenarioManager.currentScenarioContent);

        PauseGame(true);
        pauseMenuUI.SetActive(false);
    }

    // 게임 일시정지
    public void PauseGame(bool pause)
    {
        isPause = pause;

        if (isPause)
        {   // 게임진행 -> 일시정지
            pauseUI.ChangeMissionContent(scenarioManager.currentmission);

            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {   // 일시정지 -> 게임진행
            pauseMenuUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // 게임오버
    public void GameOver()
    {
        isPause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameOverUI.SetActive(true);
    }
}
