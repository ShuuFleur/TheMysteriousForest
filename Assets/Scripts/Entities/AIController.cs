using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public int baseHealth;
    public int baseAttack;
    public float moveSpeed;
    public float chaseRadius;
    public float attackRadius;
    public Transform target;
    public Transform homePos;

    


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if (Vector2.Distance(target.position, transform.position) <= chaseRadius && Vector2.Distance(target.position, transform.position) > attackRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}