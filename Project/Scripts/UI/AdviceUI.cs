using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdviceUI : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform);
    }
}
