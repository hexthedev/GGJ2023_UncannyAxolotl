// MoveTo.cs
using UnityEngine;
using UnityEngine.AI;
    
public class MoveTo : MonoBehaviour {
       
    public Transform goal;
    public Transform home;
    public Transform current;
    private NavMeshAgent agent;
    
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        
        current = goal;
        
        GotoNextPoint();
    }
    
    void GotoNextPoint() {
        if (current.position == home.position)
        {
            current = goal; 
        }
        else
        {
            current = home;
        }
        agent.destination = current.position;

    }
    
    void Update () {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}
