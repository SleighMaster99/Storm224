using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField]
    private GameObject bullets;
    [SerializeField]
    private int ammo;

    [Header("Sound Component")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip takeAmmoSound;

    void Start()
    {
        ammo = 5;
    }

    public int AddAmmo(int maxAmmo)
    {
        if (ammo > 0)
        {
            ammo--;

            if (ammo == 0)
                bullets.SetActive(false);

            audioSource.PlayOneShot(takeAmmoSound);

            return maxAmmo;
        }
        else
            return 0;
    }
}
