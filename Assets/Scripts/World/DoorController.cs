using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    
    [SerializeField] private List<GameObject> listCrystals = new List<GameObject>();
    private bool bOpen;
    private Animator _animator;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private GameObject _camera;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _camera = transform.GetChild(0).gameObject;
    }
    
    public void CheckCrystalsState()
    {
        int activatedCrystal = 0;
        int crystalsNumber = listCrystals.Count;
        foreach (var crystal in listCrystals)
        {
            if (crystal.GetComponentInChildren<CrystalController>().isActivate == false)
            {
                activatedCrystal--;
                print(activatedCrystal + " / " + crystalsNumber);
            }
            else
            {
                activatedCrystal++;
                print(activatedCrystal + " / " + crystalsNumber);
            }
        }

        if (activatedCrystal == crystalsNumber)
        {
            SwitchState(true);
        }
        else
        {
            SwitchState(false);
        }
        
    }

    public void CheckPressurePlateState()
    {
        int activatedPlate = 0;
        int platesNumber = listCrystals.Count;
        foreach (var crystal in listCrystals)
        {
            if (crystal.GetComponentInChildren<PlateController>().isActivate == false)
            {
                activatedPlate--;
                print(activatedPlate + " / " + platesNumber);
            }
            else
            {
                activatedPlate++;
                print(activatedPlate + " / " + platesNumber);
            }
        }

        if (activatedPlate == platesNumber)
        {
            SwitchState(true);
        }
        else
        {
            SwitchState(false);
        }

    }
    
    void SwitchState(bool state)
    {
        bOpen = state;
        switch (bOpen)
        {
            case true:
                _animator.SetBool("Open", true);
                _collider.enabled = false;
                _spriteRenderer.sortingOrder = -1;
                _audioSource.PlayOneShot(sounds[0]);
                // StartCoroutine(delayDoorCam());
                break;
            case false:
                _animator.SetBool("Open", false);
                _collider.enabled = true;
                _spriteRenderer.sortingOrder = 0;
                _audioSource.PlayOneShot(sounds[1]);
                // StartCoroutine(delayDoorCam());
                break;
        }
        print(bOpen);
    }
    //
    // private IEnumerator delayDoorCam()
    // {
    //     _camera.SetActive(true);
    //     yield return new WaitForSeconds(1f);
    //     _camera.SetActive(false);
    //     StopCoroutine(delayDoorCam());
    // }
}
