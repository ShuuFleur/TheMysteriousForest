using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CorruptCoreController : MonoBehaviour
{
    private Volume effect;

    [SerializeField] private int hitForDestroy = 1;
    private float elaspedTime;
    private float time = 1f;

    private bool breaked = false;

    private void Awake() {
        effect = GetComponentInChildren<Volume>();
    }

    private void Update() {
        if(!breaked) return;
        if(elaspedTime >= 1f) DestroySelf();

        elaspedTime += Time.deltaTime;
        effect.weight = Mathf.Lerp(1, 0, elaspedTime/time);

    }

    // Update is called once per frame
    public void Hit()
    {
        hitForDestroy--;

        if (hitForDestroy == 0) breaked = true;
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
