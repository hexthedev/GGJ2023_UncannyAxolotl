using UnityEngine;

public class Building : MonoBehaviour
{
    MeshRenderer mr;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material m)
    {
        mr.material = m;
    }
}
