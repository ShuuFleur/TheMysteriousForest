using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{

    private Animator _animator;
    [SerializeField] private int hitForDestroy = 1;
    [SerializeField] private string animParameter = "Destroyed";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Hit()
    {
        
        hitForDestroy--;

        print(hitForDestroy);
        if (hitForDestroy == 0)
        {
            _animator.SetBool(animParameter, true);
        }
    }

    public void DestroySelf() 
    {
        Destroy(this.gameObject);
    }

}
