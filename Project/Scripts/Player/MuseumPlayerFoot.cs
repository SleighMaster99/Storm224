using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumPlayerFoot : MonoBehaviour
{
    [Header("Player Foot Component")]
    [SerializeField]
    private MuseumPlayerControlable playerControlable;
    [SerializeField]
    private GameObject foot;

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
        StartCoroutine(MuseumFootSoundPlay());
    }

    // 발소리
    private IEnumerator MuseumFootSoundPlay()
    {
        while (true)
        {
            if (playerControlable.isMove && !playerControlable.isJump)
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
