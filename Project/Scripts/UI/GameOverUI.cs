using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("SceneName")]
    [SerializeField]
    private string retrySceneName;                      // 다시하기 선택시 불러올 씬 이름
    [SerializeField]
    private string selectScenarioSceneName;             // 시나리오 선택하기 선택시 불러울 씬 이름

    // 다시하기 버튼
    public void ButtonRetry()
    {
        SceneManager.LoadScene(retrySceneName);
    }

    // 시나리오 선택 버튼
    public void ButtonSelectScenario()
    {
        SceneManager.LoadScene(selectScenarioSceneName);
    }

    // 게임 종료 버튼
    public void ButtonExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
