using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    [Header("Player Foot Component")]
    [SerializeField]
    private PlayerControlable playerControlable;
    [SerializeField]
    private GameObject foot;
    [SerializeField]
    private GameObject rightFoot;

    // 착지 판정
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(7))
        {   // Layer 7 : Ground
            playerControlable.isJump = false;
        }
    }

    private void Start()
    {
        StartCoroutine("FootSoundPlay");
    }

    // 발소리
    private IEnumerator FootSoundPlay()
    {
        while (true)
        {
            if (playerControlable.isMove)
            {
                foot.SetActive(true);

                yield return new WaitForSeconds(playerControlable.footSoundTerm);

                foot.SetActive(false);
            }
            else
                yield return null;
        }
    }
}
