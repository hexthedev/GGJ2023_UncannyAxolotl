using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float curTime = 0;
    public float endTime = 0;

    public Vector3 startPosition;
    public Vector3 endPosition;
    
    void Update()
    {
        curTime += Time.deltaTime;
        transform.position = Vector3.Slerp(startPosition, endPosition, curTime/endTime);
        
        if(curTime >= endTime)
            Destroy(gameObject);
    }
}
