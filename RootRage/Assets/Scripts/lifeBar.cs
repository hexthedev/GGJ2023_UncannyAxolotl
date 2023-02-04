using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeBar : MonoBehaviour
{
    public float totalhp;
    public float _damage = 0;
    private Transform Transform;
    private Vector3 originalSize;
    private float decreasePercentage = 0.3f;

    void Start()
    {
        Transform = GetComponent<Transform>();
        originalSize = Transform.localScale;
    }

    public void DoDamage(float damage)
    {
        _damage += damage;
        Shrink();
    }
    
    void Shrink()
    {
        float damageprct = _damage * originalSize.z / totalhp;
        Vector3 newSize = Transform.localScale- new Vector3(0,0,damageprct);
        Transform.localScale = newSize;

    }
}

