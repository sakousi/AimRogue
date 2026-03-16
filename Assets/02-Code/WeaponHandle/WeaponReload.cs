using System;
using System.Collections;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    public float reloadTime = 2f;
    public bool isReloading = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("Reloaded!");
        isReloading = false;
    }
}