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
        HideMAybe();
    }

    void HideMAybe()
    {
        if (_damage == 0)
            GetComponent<MeshRenderer>().enabled = false;
        else
            GetComponent<MeshRenderer>().enabled = true;
    }

    void Shrink()
    {
        float damageprct = (totalhp-_damage) * originalSize.z / totalhp;
        Vector3 newSize = originalSize - new Vector3(0,0,damageprct);
        Transform.localScale = newSize;

    }
}

