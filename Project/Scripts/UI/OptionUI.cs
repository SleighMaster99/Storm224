using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionUI : MonoBehaviour
{
    [Header("InputField")]
    [SerializeField]
    private TMP_InputField idleXText;
    [SerializeField]
    private TMP_InputField idleYText;
    [SerializeField]
    private TMP_InputField zoomXText;
    [SerializeField]
    private TMP_InputField zoomYText;

    [Header("Alert")]
    [SerializeField]
    private GameObject alertText;

    public float idleX;
    public float idleY;
    public float zoomX;
    public float zoomY;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("IdleX"))
        {
            idleX = PlayerPrefs.GetFloat("IdleX");
            idleY = PlayerPrefs.GetFloat("IdleY");
            zoomX = PlayerPrefs.GetFloat("ZoomX");
            zoomY = PlayerPrefs.GetFloat("ZoomY");
        }
        else
        {
            idleX = 1.5f;
            idleY = 1.0f;
            zoomX = 1.4f;
            zoomY = 0.9f;
        }
    }

    private void OnEnable()
    {
        SensitiveInitalize();
    }


    // 감도 초기화
    public void SensitiveInitalize()
    {
        if (PlayerPrefs.HasKey("IdleX"))
        {
            idleX = PlayerPrefs.GetFloat("IdleX");
            idleY = PlayerPrefs.GetFloat("IdleY");
            zoomX = PlayerPrefs.GetFloat("ZoomX");
            zoomY = PlayerPrefs.GetFloat("ZoomY");

            GameObject player = GameObject.Find("Player");
            if (player.GetComponent<PlayerControlable>() != null)
            {
                player.GetComponent<PlayerControlable>().rotateXSpeed = idleX;
                player.GetComponent<PlayerControlable>().rotateYSpeed = idleY;
            }
            else if (player.GetComponent<MuseumPlayerControlable>() != null)
            {
                player.GetComponent<MuseumPlayerControlable>().rotateXSpeed = idleX;
                player.GetComponent<MuseumPlayerControlable>().rotateYSpeed = idleY;
            }


            idleXText.text = idleX.ToString();
            idleYText.text = idleY.ToString();
            zoomXText.text = zoomX.ToString();
            zoomYText.text = zoomY.ToString();
        }
        else
        {
            idleXText.text = "1.5";
            idleYText.text = "1.0";
            zoomXText.text = "1.4";
            zoomYText.text = "0.9";
        }
    }

    // 감도 저장
    public void ButtonSaveSensitive()
    {
        idleX = float.Parse(idleXText.text);
        idleY = float.Parse(idleYText.text);
        zoomX = float.Parse(zoomXText.text);
        zoomY = float.Parse(zoomYText.text);

        GameObject player = GameObject.Find("Player");

        if (player.GetComponent<PlayerControlable>() != null)
        {
            player.GetComponent<PlayerControlable>().rotateXSpeed = idleX;
            player.GetComponent<PlayerControlable>().rotateYSpeed = idleY;
        }
        else if(player.GetComponent<MuseumPlayerControlable>() != null)
        {
            player.GetComponent<MuseumPlayerControlable>().rotateXSpeed = idleX;
            player.GetComponent<MuseumPlayerControlable>().rotateYSpeed = idleY;
        }

        PlayerPrefs.SetFloat("IdleX", idleX);
        PlayerPrefs.SetFloat("IdleY", idleY);
        PlayerPrefs.SetFloat("ZoomX", zoomX);
        PlayerPrefs.SetFloat("ZoomY", zoomY);

        PlayerPrefs.Save();

        StartCoroutine(AlertMessage());
    }

    // 감도 저장 되었다는 알림
    private IEnumerator AlertMessage()
    {
        alertText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        alertText.SetActive(false);
    }

    // 뒤로가기 버튼
    public void ButtonCancelOption()
    {
        this.gameObject.SetActive(false);
    }
}
