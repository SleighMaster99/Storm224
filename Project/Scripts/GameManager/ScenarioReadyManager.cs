using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScenarioReadyManager : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField]
    private GameObject loadingUI;                               // Loading UI
    [SerializeField]
    private GameObject explainUI;                               // explain UI

    [Header("Data")]
    [SerializeField]
    private Sprite[] loadingPhotoImageDatas;                    // 로딩 배경 이미지 데이터
    [SerializeField]
    private Sprite[] loadingPhotoBackgroundImageDatas;          // 로딩 배경 테두리 데이터
    [SerializeField]
    [TextArea]
    private string explainTextData;                             // 설명 텍스트 데이터
    [SerializeField]
    private string nextSceneName;                               // 다음 씬 이름

    [Header("UI")]
    [SerializeField]
    private Image loadingPhotoImage;                            // 로딩 배경 이미지
    [SerializeField]
    private Image loadingPhotoBackgroundImage;                  // 로딩 배경 이미지 테두리
    [SerializeField]
    private Image loadingImage;                                 // 로딩 이미지
    [SerializeField]
    private TMP_Text explainText;                               // 설명 텍스트

    [Header("Audio Source")]
    [SerializeField]
    private AudioSource audioSource;                            // Audio Source

    [Header("Variable")]
    [SerializeField]
    private float expaintTextSpeed;                             // 설명 텍스트 속도
    [SerializeField]
    private float endLoadingStartTime;                          // Scenario1Scene 이동 로딩 시작하기까지 시간

    [Header("Test Button")]
    [SerializeField]
    private bool isChange;                                      // 랜덤 배경 이미지 테스트 버튼

    private void Awake()
    {
        loadingPhotoImage.sprite = loadingPhotoImageDatas[Random.Range(0, loadingPhotoImageDatas.Length)];
        loadingPhotoBackgroundImage.sprite = loadingPhotoBackgroundImageDatas[Random.Range(0, loadingPhotoBackgroundImageDatas.Length)];

        loadingUI.SetActive(true);

        StartCoroutine(Loading());
    }

    // SceneLoadingStart
    private IEnumerator Loading()
    {
        float timer = 0;
        float loadingTime = Random.Range(4.0f, 7.0f);

        while(timer < loadingTime)
        {
            timer += Time.deltaTime;
            loadingImage.fillAmount = timer / loadingTime;

            yield return null;
        }

        loadingUI.SetActive(false);
        explainUI.SetActive(true);

        StartCoroutine(Typing());
    }

    // Scenario1ReadyScene Explain Typing Text
    private IEnumerator Typing()
    {
        for (int i = 0; i < explainTextData.Length; i++)
        {
            if (i != 0)
            {
                if (explainTextData[i - 1].ToString().Equals("."))
                    yield return new WaitForSeconds(expaintTextSpeed + 1.0f);
                else
                {
                    audioSource.Play();
                    explainText.text = explainTextData.Substring(0, i + 1);
                }
            }
            else
            {
                audioSource.Play();
                explainText.text = explainTextData.Substring(0, i + 1);
            }

            yield return new WaitForSeconds(expaintTextSpeed);
        }

        yield return new WaitForSeconds(endLoadingStartTime);

        SceneManager.LoadScene(nextSceneName);
    }

    // SceneLoadingEnd
    private IEnumerator EndLoading()
    {
        float timer = 0;
        float loadingTime = Random.Range(4.0f, 7.0f);

        explainUI.SetActive(false);
        loadingUI.SetActive(true);

        while (timer < loadingTime)
        {
            timer += Time.deltaTime;
            loadingImage.fillAmount = timer / loadingTime;

            yield return null;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    private void Update()
    {
        // 랜덤 이미지 테스트
        //TestChangeImage();
    }

    // 랜덤 이미지 테스트
    private void TestChangeImage()
    {
        if (isChange)
        {
            loadingUI.SetActive(true);
            explainUI.SetActive(false);

            loadingPhotoImage.sprite = loadingPhotoImageDatas[Random.Range(0, loadingPhotoImageDatas.Length)];
            loadingPhotoBackgroundImage.sprite = loadingPhotoBackgroundImageDatas[Random.Range(0, loadingPhotoBackgroundImageDatas.Length)];

            StartCoroutine(Loading());
        }

        isChange = false;
    }
}
