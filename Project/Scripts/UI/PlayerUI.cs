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

    private void Update()
    {
        hpSlider.value = playerHealth.HP;
    }

    public void InitializeEquipUI(string equipName, int equipAmmo, int equipReloadedAmmo)
    {
        switch (equipName)
        {
            case "M1 Garand":
                equipIcon.sprite = equipSpriteIcons[0];
                aspect.aspectRatio = 6;
                break;
            case "Arisaka 38":
                equipIcon.sprite = equipSpriteIcons[1];
                aspect.aspectRatio = 6;
                break;
            case "m3a1":
                equipIcon.sprite = equipSpriteIcons[2];
                aspect.aspectRatio = 2.5f;
                break;
            case "MK2":
                equipIcon.sprite = equipSpriteIcons[3];
                aspect.aspectRatio = 1;
                break;
            case "M18":
                equipIcon.sprite = equipSpriteIcons[4];
                aspect.aspectRatio = 1;
                break;
            case "Morphine":
                equipIcon.sprite = equipSpriteIcons[5];
                aspect.aspectRatio = 1;
                break;
            case "Bandage":
                equipIcon.sprite = equipSpriteIcons[6];
                aspect.aspectRatio = 1;
                break;
        }

        equipNameText.text = equipName;
        equipAmmoText.text = equipAmmo.ToString();
        equipReloadedAmmoText.text = equipReloadedAmmo.ToString();
    }

    public void UpdateEquipUI(int equipAmmo, int equipReloadedAmmo)
    {
        equipAmmoText.text = equipAmmo.ToString();
        equipReloadedAmmoText.text = equipReloadedAmmo.ToString();
    }

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
}
