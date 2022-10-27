using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

public class WolfController : AIController
{
    public Animator _animator;
    private bool rndBool
    {
        get { return (Random.value > 0.5f);  }
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Randomed",rndBool);
    }
    
    
}
