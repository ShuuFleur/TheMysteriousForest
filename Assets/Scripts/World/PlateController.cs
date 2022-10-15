using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField] private Sprite spriteDesactivate;
    [SerializeField] private Sprite spriteActivate;
    [SerializeField] private Sprite spriteToggleDesactivate;
    [SerializeField] private Sprite spriteToggleActivate;
    [SerializeField] private GameObject assignedGameObject;
    private SpriteRenderer _spriteRenderer;

    public bool isActivate { get; private set; } = false;
    [SerializeField] private bool isToggle = false;
    
    private void Awake()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        if(isToggle) _spriteRenderer.sprite = spriteToggleDesactivate;
    }

    void SwitchState(bool value)
    {
        isActivate = value;
        if (!isToggle)
        {
            switch (isActivate)
            {
                case true:
                    _spriteRenderer.sprite = spriteActivate;
                    break;
                case false:
                    _spriteRenderer.sprite = spriteDesactivate;
                    break;
            }
        }
        else
        {
            switch (isActivate)
            {
                case true:
                    _spriteRenderer.sprite = spriteToggleActivate;
                    break;
                case false:
                    _spriteRenderer.sprite = spriteToggleDesactivate;
                    break;
            }
        }
        
        if(assignedGameObject == null) return;
        
        assignedGameObject.GetComponent<DoorController>().CheckPressurePlateState();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Block"))
        {
            SwitchState(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Block"))
        {
            if(isToggle) return;
            SwitchState(false);
        }
    }
}
