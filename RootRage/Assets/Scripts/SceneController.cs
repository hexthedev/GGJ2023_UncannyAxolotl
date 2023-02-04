using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject building;
    public GameObject homeAgent;
    public GameObject awayAgent;
    public Transform homeBase;
    public Transform awayBase;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Instantiate(building);
        };
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject instance = Instantiate(homeAgent) as GameObject; 
            instance.GetComponent<MoveTo>().home = homeBase;
            instance.GetComponent<MoveTo>().goal = awayBase;
        };
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject instance = Instantiate(awayAgent) as GameObject;
            instance.GetComponent<MoveTo>().home = awayBase;
            instance.GetComponent<MoveTo>().goal = homeBase;
        };
    }
    
}
