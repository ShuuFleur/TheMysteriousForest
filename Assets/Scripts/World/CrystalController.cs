using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private Sprite spriteDesactivate;
    [SerializeField] private Sprite spriteActivate;
    [SerializeField] private GameObject assignedGameObject;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    public bool isActivate { get; private set; } = false;

    private void Awake()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        _audioSource = GetComponentInParent<AudioSource>();
    }

    private void SwitchState()
    {
        isActivate = !isActivate;
        switch (isActivate)
        {
            case true:
                _spriteRenderer.sprite = spriteActivate;
                _audioSource.PlayOneShot(sounds[0]);
                break;
            case false:
                _spriteRenderer.sprite = spriteDesactivate;
                _audioSource.PlayOneShot(sounds[1]);
                break;
        }
        if(assignedGameObject == null) return;
        
        assignedGameObject.GetComponent<DoorController>().CheckCrystalsState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitBox"))
        {
            SwitchState();
        }
    }
}
