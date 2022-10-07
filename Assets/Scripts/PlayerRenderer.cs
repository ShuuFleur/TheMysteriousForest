using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aarthificial.Reanimation;

public class PlayerRenderer : MonoBehaviour
{
    private static class Drivers 
    {
        public const string IsMoving = "move";
    }

    private Reanimator _reanimator;
    private PlayerLocomotion _controller;

    private void Awake()
    {
        _reanimator = GetComponent<Reanimator>();
        _controller = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        _reanimator.Set(Drivers.IsMoving, _controller.isMoving);
    }
}
