using System;
using System.Collections;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    public AudioClip reloadSound;

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
        AudioSource.PlayClipAtPoint(reloadSound, transform.position);
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("Reloaded!");
        isReloading = false;
    }
}