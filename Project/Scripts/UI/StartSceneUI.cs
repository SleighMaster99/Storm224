using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField]
    private Image randomImage;
    [SerializeField]
    private Sprite[] images;

    [SerializeField]
    private Image randomBackgroundImage;
    [SerializeField]
    private Sprite[] backgroundImages;

    [SerializeField]
    private GameObject OptionUI;

    private Color imageColor;

    private void Awake()
    {
        imageColor = randomImage.color;
        imageColor.a = 0;

        randomImage.sprite = images[Random.Range(0, images.Length)];
        randomBackgroundImage.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];

        StartCoroutine(ShowRandomImage());
    }

    // RandomImage 점점 보여주기
    private IEnumerator ShowRandomImage()
    {
        while(randomImage.color.a < 1.0f)
        {
            imageColor.a += Time.deltaTime;
            randomImage.color = imageColor;

            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(HideRandomImage());
    }

    // RandomImage 점점 지우기
    private IEnumerator HideRandomImage()
    {
        while (randomImage.color.a > 0)
        {
            imageColor.a -= Time.deltaTime;
            randomImage.color = imageColor;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        randomImage.sprite = images[Random.Range(0, images.Length)];
        randomBackgroundImage.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];

        StartCoroutine(ShowRandomImage());
    }

    // 게임 시작 버튼
    public void ButtonGameStart()
    {
        SceneManager.LoadScene("MuseumReadyScene");
    }

    // 옵션 버튼
    public void ButtonOption()
    {
        OptionUI.SetActive(true);
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
