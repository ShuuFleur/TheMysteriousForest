using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public int baseHealth;
    public int baseAttack;
    public float chaseRadius;
    public float attackRadius;
    public float distanceOffset;
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
        if(Vector2.Distance(target.position, transform.position) <= chaseRadius) transform.position = Vector2.MoveTowards(transform.position, target.position, attackRadius);
    }
}