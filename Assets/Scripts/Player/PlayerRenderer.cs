using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;

public class PlayerRenderer : MonoBehaviour
{

    private PlayerController _controller;
    private Animator _animator;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {

        _animator.SetFloat("X", _controller.facingDirection.x);
        _animator.SetFloat("Y", _controller.facingDirection.y);
        _animator.SetFloat("Speed", _controller.movementInput.magnitude);

    }
}
