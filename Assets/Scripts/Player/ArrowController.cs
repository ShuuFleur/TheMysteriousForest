using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound;    
    
    private Rigidbody2D rb;
    private Collider2D c;
    private Animator animator;
    private AudioSource audioSource;

    private float speed = 2;
    private bool isMoving = true;
    private Vector2 velocity;
    private Vector2 stickPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Setup(Vector2 velocity, Vector3 direction)
    {
        this.velocity = velocity;
        transform.rotation = Quaternion.Euler(direction);
    }

    private void Update()
    {
        if (!isMoving) return;

        switch (velocity)
        {
            case Vector2 v when v.Equals(Vector2.up):
                stickPos = new Vector2(0, 0.05f);
                break;
        }

        rb.velocity = velocity * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Projectile Obstacle"))
        {
            transform.position = transform.position + (Vector3)stickPos;
            DisableCollision();
            animator.SetTrigger("Obstacled");
            audioSource.PlayOneShot(hitSound);
            Destroy(this.gameObject, 2);
        } else {
            Destroy(this.gameObject);
        }

    }

    public void DisableCollision()
    {
        c.enabled = false;
        isMoving = false;
        rb.velocity = Vector2.zero;
    }
}
