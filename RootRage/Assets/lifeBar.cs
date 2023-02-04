using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeBar : MonoBehaviour
{
    public float totalhp;
    public float damage;
    private Transform Transform;
    private Vector3 originalSize;
    private float decreasePercentage = 0.3f;

    void Start()
    {
        Transform = GetComponent<Transform>();
        originalSize = Transform.localScale;
        InvokeRepeating("Shrink", 0, 2);
    }

    void Shrink()
    {
        float fdamage = (float)damage;
        float damageprct = damage * originalSize.z / totalhp;
        Vector3 newSize = Transform.localScale- new Vector3(0,0,damageprct);
        Transform.localScale = newSize;

    }
}

