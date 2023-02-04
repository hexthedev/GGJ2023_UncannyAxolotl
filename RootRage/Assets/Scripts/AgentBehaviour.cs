using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

// 1) Make it see if there are buildings or enemies in front [needs to know own player and enemy player]
// 2) Make it target enemies before building follow them until killed

public class AgentBehaviour : MonoBehaviour
{
    public int Team;
    public AgentBehaviour TargetAgent;
    public Building TargetBuilding;
    public Transform EnemyBase;
    

    public int AgentHealth = 100;
    public int AgentDamage = 20;
    public float attackInterval = 2;
    TimerBehaviour damageTimer;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
    }

    void Update()
    {
        // find target
        if (TargetAgent == null)
        {
            var hits = Physics.SphereCastAll(
                transform.position,
                1f,
                transform.forward,
                2
            );

            var agents = hits
                .Select(a => a.collider.gameObject.GetComponent<AgentBehaviour>())
                .Where(a => a != null)
                .Where(a => a.Team != Team);

            if (agents.Any())
                TargetAgent = agents.First();
            else
            {
                var buildings = hits
                    .Select(h => h.collider.gameObject.GetComponent<Building>())
                    .Where(b => b != null)
                    .Where(b => b.Team != Team);

                if (buildings.Any())
                    TargetBuilding = buildings.First();
            }
        }

        if (TargetAgent != null)
        {
            float distance = (TargetAgent.transform.position - transform.position).magnitude;

            if (distance < 1)
            {
                agent.isStopped = true;
                // do damage to the other agent
                startAttackEnemy(attackInterval, TargetAgent);

            }
            else
            {
                agent.destination = TargetAgent.transform.position;
                agent.isStopped = false;
            }
            
            // TRY ATTACK
        }
        else
        {
            if (TargetBuilding != null)
            {
                float distance = (TargetBuilding.transform.position - transform.position).magnitude;

                if (distance < 1)
                    agent.isStopped = true;
                else
                {
                    agent.destination = TargetBuilding.transform.position;
                    agent.isStopped = false;
                }
                // TRY ATTACK  
            }
        }

        if (TargetAgent == null && TargetBuilding == null)
        {
            agent.destination = EnemyBase.transform.position;
            agent.isStopped = false;
        }
    }

    void takeDamage(int damageAmount)
    {
        AgentHealth -= damageAmount;
        if (AgentHealth <= 0)
        {
            Destroy(this);
        }
    }
    
    void startAttackEnemy(float interval, AgentBehaviour targetAgent)
    {
        if(damageTimer != null)
            Destroy(damageTimer);
        
        damageTimer = gameObject.AddComponent<TimerBehaviour>();
        damageTimer.Interval = interval;
        damageTimer.Do = Attack;
        
        void Attack()
        {
            if (targetAgent)
            {
                TargetAgent.takeDamage(this.AgentDamage);
            }
        }
        
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawSphere(transform.position + transform.forward * 2, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}