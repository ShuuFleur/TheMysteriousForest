using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;

public class PlayerRenderer : MonoBehaviour
{

    private PlayerLocomotion _controller;
    private Animator _animator;

    public Vector2 direction;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {

        switch (_controller.movementInput)
        {
            case Vector2 v when v.Equals(Vector2.up):
                direction = new Vector2(0, 1);
                break;
            case Vector2 v when v.Equals(Vector2.down):
                direction = new Vector2(0, -1);
                break;
            case Vector2 v when v.Equals(Vector2.left):
                direction = new Vector2(-1, 0);
                break;
            case Vector2 v when v.Equals(Vector2.right):
                direction = new Vector2(1, 0);
                break;
        }

        _animator.SetFloat("X", direction.x);
        _animator.SetFloat("Y", direction.y);
        _animator.SetFloat("Speed", _controller.movementInput.magnitude);

    }
}
