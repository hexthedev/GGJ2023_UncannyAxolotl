using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRotator : MonoBehaviour
{
    readonly Vector3 Left = new Vector3(0, -1, 0);
    readonly Vector3 Right = new Vector3(0, 1, 0);
           
    public float speed;

    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = transform.rotation * Quaternion.Euler(Left * (speed * Time.deltaTime));
        }   
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = transform.rotation * Quaternion.Euler(Right * (speed * Time.deltaTime));
        }   
    }
}
