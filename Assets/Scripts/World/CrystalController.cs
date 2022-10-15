using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField] private Sprite spriteDesactivate;
    [SerializeField] private Sprite spriteActivate;
    [SerializeField] private GameObject assignedGameObject;
    private SpriteRenderer _spriteRenderer;

    public bool isActivate { get; private set; } = false;

    private void Awake()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void SwitchState()
    {
        isActivate = !isActivate;
        switch (isActivate)
        {
            case true:
                _spriteRenderer.sprite = spriteActivate;
                break;
            case false:
                _spriteRenderer.sprite = spriteDesactivate;
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
