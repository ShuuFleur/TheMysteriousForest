using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private AudioSource _audioSource;
    private float time = 1f;
    private float elaspedTime;
    private Vector2 direction = Vector2.zero;
    private Vector2 lastPos;
    [SerializeField] private LayerMask layer;
    private bool isMoving = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!isMoving) return;
        
        
        
        elaspedTime += Time.deltaTime;
        _rb.position = Vector2.Lerp(lastPos, lastPos+(direction*0.16f), elaspedTime/time);

        if (elaspedTime >= time)
        {
            isMoving = false;
            _audioSource.Stop();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            print("MOVEIT");
            if(isMoving) return;
            lastPos = transform.position;
            direction = col.gameObject.GetComponent<PlayerController>().facingDirection;
            elaspedTime = 0;

            RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, 0.225f, layer);
            
            if(ray.collider != null) return;
            isMoving = true;
            
            _audioSource.Play();
        }
    }

}
