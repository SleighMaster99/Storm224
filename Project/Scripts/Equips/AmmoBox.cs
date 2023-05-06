using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField]
    private GameObject bullets;
    [SerializeField]
    private int ammo;

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

            return maxAmmo;
        }
        else
            return 0;
    }
}
