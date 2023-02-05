using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

// 1) Make it see if there are buildings or enemies in front [needs to know own player and enemy player]
// 2) Make it target enemies before building follow them until killed

public class AgentBehaviour : MonoBehaviour
{
    public Bullet bullet;
    public int Team;
    public AgentBehaviour TargetAgent;
    public Building TargetBuilding;
    public Transform EnemyBase;

    public UnitConfig UnitConfig;
    public float currentHP;
    TimerBehaviour damageTimer;

    private NavMeshAgent agent;
    public TMP_Text text;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHP = UnitConfig.HP;
        agent.speed = Mathf.Min(1, UnitConfig.MoveSpeed / 100);
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
    }

    void FindTarget()
    {
        if (TargetAgent != null || TargetBuilding != null) { return; }

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
        {
            TargetAgent = agents.First();
            return;
        }

        var buildings = hits
            .Select(h => h.collider.gameObject.GetComponent<Building>())
            .Where(b => b != null)
            .Where(b => b.Team != Team);

        if (buildings.Any())
            TargetBuilding = buildings.First();
    }

    bool CanIAttack(AgentBehaviour target)
    {
        float distance = (target.transform.position - transform.position).magnitude;

        return distance < Mathf.Max(1f, UnitConfig.Range);
    }

    bool CanIAttack(Building target)
    {
        float distance = (target.transform.position - transform.position).magnitude;

        return distance < Mathf.Max(2f, UnitConfig.Range);
    }

    void MoveTowards(AgentBehaviour target)
    {
        agent.destination = target.transform.position;
        agent.isStopped = false;
    }

    void MoveTowards(Building target)
    {
        agent.destination = target.transform.position;
        agent.isStopped = false;
    }

    void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
            return;
        }
        text.text = $"{currentHP}";
        if (damageTimer != null)
        {
            return;
        }

        // find target
        FindTarget();

        if (TargetAgent != null)
        {
            if (CanIAttack(TargetAgent))
            {
                StartAttack(TargetAgent);
            }
            else
            {
                MoveTowards(TargetAgent);
            }
            return;
        }

        if (TargetBuilding != null)
        {
            if (CanIAttack(TargetBuilding))
            {
                StartAttack(TargetBuilding);
            }
            else
            {
                MoveTowards(TargetBuilding);
            }
            return;
        }

        if (EnemyBase != null)
        {
            agent.destination = EnemyBase.transform.position;
            agent.isStopped = false;
        }
    }

    void StartAttack(Building target)
    {
        agent.isStopped = true;
        damageTimer = gameObject.AddComponent<TimerBehaviour>();
        damageTimer.Interval = UnitConfig.attackInterval;
        damageTimer.Do = Attack;

        void Attack()
        {
            Debug.Log("Deal damage to a building");

            if (target != null)
            {
                Bullet bullet = Instantiate(this.bullet);
                bullet.startPosition = transform.position;
                bullet.endPosition = target.transform.position;
            }
            
            if (target == null)
            {
                Destroy(damageTimer);
                return;
            }
            target.currentHP -= UnitConfig.Damage;
        }
    }

    void StartAttack(AgentBehaviour target)
    {
        agent.isStopped = true;
        damageTimer = gameObject.AddComponent<TimerBehaviour>();
        damageTimer.Interval = UnitConfig.attackInterval;
        damageTimer.Do = Attack;

        void Attack()
        {
            Debug.Log("Deal damage to an agent");


            if (target != null)
            {
                Bullet bullet = Instantiate(this.bullet);
                bullet.startPosition = transform.position;
                bullet.endPosition = target.transform.position;
            }

            if (target == null)
            {
                Destroy(damageTimer);
                return;
            }
            target.currentHP -= UnitConfig.Damage;
        }

    }


    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawSphere(transform.position + transform.forward * 2, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}