using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Health playerHealth;

    [Header("Equip Icons")]
    [SerializeField]
    private Image equipIcon;
    [SerializeField]
    private AspectRatioFitter aspect;
    [SerializeField]
    private Sprite[] equipSpriteIcons;

    [Header("Equip Texts")]
    [SerializeField]
    private TMP_Text equipNameText;
    [SerializeField]
    private TMP_Text equipAmmoText;
    [SerializeField]
    private TMP_Text equipReloadedAmmoText;

    [Header("Health")]
    [SerializeField]
    private Slider hpSlider;

    [Header("Body Status")]
    [SerializeField]
    private Image bodyStatusImage;
    [SerializeField]
    private Sprite[] bodyStatusIcons;

    [Header("Message")]
    [SerializeField]
    private TMP_Text equipMessageText;

    private void Awake()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        hpSlider.value = playerHealth.HP;
    }

    // 장비 장착시 UI 변경
    public void InitializeEquipUI(string equipName, int equipAmmo, int equipReloadedAmmo)
    {
        switch (equipName)
        {
            case "Hand":
                equipIcon.sprite = equipSpriteIcons[0];
                aspect.aspectRatio = 1;
                break;
            case "M1 Garand":
                equipIcon.sprite = equipSpriteIcons[1];
                aspect.aspectRatio = 6;
                break;
            case "Arisaka 38":
                equipIcon.sprite = equipSpriteIcons[2];
                aspect.aspectRatio = 6;
                break;
            case "m3a1":
                equipIcon.sprite = equipSpriteIcons[3];
                aspect.aspectRatio = 2.5f;
                break;
            case "MK2":
                equipIcon.sprite = equipSpriteIcons[4];
                aspect.aspectRatio = 1;
                break;
            case "M18":
                equipIcon.sprite = equipSpriteIcons[5];
                aspect.aspectRatio = 1;
                break;
            case "Morphine":
                equipIcon.sprite = equipSpriteIcons[6];
                aspect.aspectRatio = 1;
                break;
            case "Bandage":
                equipIcon.sprite = equipSpriteIcons[7];
                aspect.aspectRatio = 1;
                break;
            case "탄약통":
                equipIcon.sprite = equipSpriteIcons[8];
                aspect.aspectRatio = 2;
                break;
        }

        equipNameText.text = equipName;
        equipAmmoText.text = equipAmmo.ToString();
        equipReloadedAmmoText.text = equipReloadedAmmo.ToString();
    }

    // 탄 소모시 UI 변경
    public void UpdateEquipUI(int equipAmmo, int equipReloadedAmmo)
    {
        equipAmmoText.text = equipAmmo.ToString();
        equipReloadedAmmoText.text = equipReloadedAmmo.ToString();
    }

    // 플레이어 상태 변화시 UI 변경
    public void UpdateBodyStatusUI(int bodyStatus)
    {
        switch (bodyStatus)
        {
            case 0:
                bodyStatusImage.sprite = bodyStatusIcons[0];
                break;
            case 1:
                bodyStatusImage.sprite = bodyStatusIcons[1];
                break;
            case 2:
                bodyStatusImage.sprite = bodyStatusIcons[2];
                break;
        }
    }

    // 텍스트 보여주기
    public void ShowEquipMessage(string message, float showingTime)
    {
        equipMessageText.text = message;

        StartCoroutine("ShowingEquipMessage", showingTime);
    }
    
    // showingTime 동안 텍스트 보여주기
    private IEnumerator ShowingEquipMessage(float showingTime)
    {
        equipMessageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(showingTime);

        equipMessageText.gameObject.SetActive(false);
    }
}
