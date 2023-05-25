using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Components")]
    private Health playerHealth;
    private Health vehicleHealth;
    private VehicleControlable vehicleControlable;
    private ArtilleryControlable artilleryControlable;

    [Header("UI GameObject")]
    [SerializeField]
    private GameObject statusUI;
    [SerializeField]
    private GameObject equipUI;
    [SerializeField]
    private GameObject vehicleStatusUI;
    [SerializeField]
    private GameObject vehicleUI;
    [SerializeField]
    private GameObject artilleryBackground;
    [SerializeField]
    private GameObject artilleryStatusUI;
    [SerializeField]
    private GameObject artilleryUI;
    [SerializeField]
    private GameObject missionComplete;

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
    [SerializeField]
    private GameObject bloodImage;

    [Header("Message")]
    [SerializeField]
    private TMP_Text equipMessageText;

    [Header("Vehicle")]
    [SerializeField]
    private TMP_Text vehicleNameText;
    [SerializeField]
    private TMP_Text vehicleVelocityText;
    [SerializeField]
    private Slider fuelSlider;
    [SerializeField]
    private Slider vehicleHPSlider;
    private bool isVehicle;

    [Header("Artillery")]
    [SerializeField]
    private TMP_Text artilleryNameText;
    [SerializeField]
    private TMP_Text artilleryAmmoText;
    [SerializeField]
    private Image artilleryReloadImage;
    private bool isArtillery;

    private void Awake()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        hpSlider.value = playerHealth.HP;
        if (isVehicle)
        {
            vehicleHPSlider.value = vehicleHealth.HP;
            fuelSlider.value = vehicleControlable.fuel;
            vehicleVelocityText.text = ((int)vehicleControlable.vehicleVelocity * 4).ToString();
        }
        if (playerHealth.HP <= 35.0f)
            bloodImage.SetActive(true);
        else
            bloodImage.SetActive(false);
    }

    // 장비 장착시 UI 변경
    public void InitializeEquipUI(string equipName, int equipAmmo, int equipReloadedAmmo)
    {
        switch (equipName)
        {
            case "손":
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
            case "모르핀":
                equipIcon.sprite = equipSpriteIcons[6];
                aspect.aspectRatio = 1;
                break;
            case "붕대":
                equipIcon.sprite = equipSpriteIcons[7];
                aspect.aspectRatio = 1;
                break;
            case "탄약통":
                equipIcon.sprite = equipSpriteIcons[8];
                aspect.aspectRatio = 2;
                break;
            case "스패너":
                equipIcon.sprite = equipSpriteIcons[9];
                aspect.aspectRatio = 1;
                break;
            case "구급상자":
                equipIcon.sprite = equipSpriteIcons[10];
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

    // 차량 승하차시 UI 변경
    public void UpdateVehicleUI(bool isVehicle, VehicleControlable currentVehicleControlable, Health currentVehicleHealth)
    {
        this.isVehicle = isVehicle;

        if (this.isVehicle)
        {   // 차량 탑승
            vehicleControlable = currentVehicleControlable;
            vehicleHealth = currentVehicleHealth;

            vehicleHPSlider.maxValue = vehicleHealth.maxHP;

            vehicleNameText.text = currentVehicleControlable.vehicleName;

            statusUI.SetActive(false);
            equipUI.SetActive(false);
            vehicleUI.SetActive(true);
            vehicleStatusUI.SetActive(true);
        }
        else
        {   // 차량 하차
            vehicleControlable = null;
            vehicleHealth = null;

            vehicleNameText.text = null;

            statusUI.SetActive(true);
            equipUI.SetActive(true);
            vehicleUI.SetActive(false);
            vehicleStatusUI.SetActive(false);
        }
    }

    // 야포 사용 UI 변경
    public void UpdateArtilleryUI(bool isArtillery, ArtilleryControlable currentArtilleryControlable, Health currentArtilleryHealth)
    {
        this.isArtillery = isArtillery;

        if (this.isArtillery)
        {   // 야포 탑승
            artilleryControlable = currentArtilleryControlable;

            artilleryNameText.text = currentArtilleryControlable.artilleryName;
            artilleryAmmoText.text = artilleryControlable.ammo.ToString();

            statusUI.SetActive(false);
            equipUI.SetActive(false);
            artilleryBackground.SetActive(true);
            artilleryUI.SetActive(true);
            artilleryStatusUI.SetActive(true);
        }
        else
        {   // 야포 하차
            artilleryControlable = null;

            artilleryNameText.text = null;

            statusUI.SetActive(true);
            equipUI.SetActive(true);
            artilleryBackground.SetActive(false);
            artilleryUI.SetActive(false);
            artilleryStatusUI.SetActive(false);
        }
    }

    // 야포 Ammo UI 변경
    public void UpdateArtilleryAmmoUI(int ammo)
    {
        artilleryAmmoText.text = ammo.ToString();
        artilleryReloadImage.fillAmount = 0;

        StartCoroutine("ArtilleryReloadImage");
    }

    private IEnumerator ArtilleryReloadImage()
    {
        float timer = 0;

        while(timer <= 5.0f)
        {
            timer += Time.deltaTime;
            artilleryReloadImage.fillAmount += 0.2f * Time.deltaTime;

            yield return null;
        }
    }

    // 임무성공
    public void MissionComplete()
    {
        StartCoroutine(ActiveMissionComplete());
    }

    private IEnumerator ActiveMissionComplete()
    {
        missionComplete.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        missionComplete.SetActive(false);
    }
}
