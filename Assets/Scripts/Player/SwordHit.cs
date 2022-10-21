using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Breakable"))
        {
            other.GetComponent<BreakableObject>().Hit();
        } 
        else if (other.CompareTag("Corruption"))
        {
            other.GetComponent<CorruptCoreController>().Hit();
        }
    }
}
