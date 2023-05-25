using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPositionUI : MonoBehaviour
{
    [SerializeField]
    private EquipManager equipManager;
    [SerializeField]
    private PlayerControlable playerControlable;
    [SerializeField]
    private GameObject playerUICanvas;

    private GameManager gameManager;

    void Awake()
    {
        playerControlable = GameObject.FindWithTag("Player").GetComponent<PlayerControlable>();
        equipManager = GameObject.FindWithTag("Player").GetComponent<EquipManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // 보병
    public void ButtonSelectPositionInfantry()
    {
        equipManager.SelectPosition(1);
        this.gameObject.SetActive(false);
        playerUICanvas.SetActive(true);
        playerControlable.PlayerInitialize();
        equipManager.CurrentEquipChanger(1);
        gameManager.PauseGame(false);
    }

    // 정비병
    public void ButtonSelectPositionMechanic()
    {
        equipManager.SelectPosition(2);
        this.gameObject.SetActive(false);
        playerUICanvas.SetActive(true);
        playerControlable.PlayerInitialize();
        equipManager.CurrentEquipChanger(1);
        gameManager.PauseGame(false);
    }

    // 의무병
    public void ButtonSelectPositionMedic()
    {
        equipManager.SelectPosition(3);
        this.gameObject.SetActive(false);
        playerUICanvas.SetActive(true);
        playerControlable.PlayerInitialize();
        equipManager.CurrentEquipChanger(1);
        gameManager.PauseGame(false);
    }
}
