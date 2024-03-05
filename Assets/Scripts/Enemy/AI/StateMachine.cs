using UnityEngine;

public enum EnemyState
{
    Idle,
    Patrolling,
    Chasing,
    Attacking,
    Dead
}

public abstract class StateMachine
{
    abstract public void Enter(EnemyController agent);
    abstract public void Exit(EnemyController agent);
    abstract public StateMachine Update(EnemyController agent);
}

public class IdleState : StateMachine
{
    public IdleState(EnemyController agent)
    {
        Enter(agent);
    }
    public override void Enter(EnemyController agent)
    {
        agent.CurrentState = EnemyState.Idle;
        agent.ResetNav();
    }

    public override void Exit(EnemyController agent)
    {
    }

    public override StateMachine Update(EnemyController agent)
    {
        if (agent.Target != null)
        {
            Exit(agent);
            return new ChasingState(agent);
        }
        else if (agent.CanPatrol())
        {
            Exit(agent);

            return new PatrollingState(agent);
        }
        else
            return this;
    }
}

public class PatrollingState : StateMachine
{
    public PatrollingState(EnemyController agent)
    {
        Enter(agent);
    }
    public override void Enter(EnemyController agent)
    {
        agent.CurrentState = EnemyState.Patrolling;
    }

    public override void Exit(EnemyController agent)
    {
    }

    public override StateMachine Update(EnemyController agent)
    {
        if (agent.Target != null)
        {
            Exit(agent);
            return new ChasingState(agent);
        }
        else if (agent.ContPatrol())
        {
            // Has Arrived
            Exit(agent);
            return new IdleState(agent);

        }
        else return this;
    }
}

public class ChasingState : StateMachine
{
    public ChasingState(EnemyController agent)
    {
        Enter(agent);
    }
    public override void Enter(EnemyController agent)
    {
        agent.CurrentState = EnemyState.Chasing;
    }

    public override void Exit(EnemyController agent)
    {
    }

    public override StateMachine Update(EnemyController agent)
    {
        if (agent.Target != null)
        {
            if (agent.CanAttack())
            {
                Exit(agent);
                return new AttackingState(agent);
            }
            else
            {
                agent.StartChase();
                return this;
            }
        }
        else
        {
            Exit(agent);
            return new IdleState(agent);
        }
    }
}

public class AttackingState : StateMachine
{
    public AttackingState(EnemyController agent)
    {
        Enter(agent);
    }
    public override void Enter(EnemyController agent)
    {
        agent.CurrentState = EnemyState.Attacking;
    }

    public override void Exit(EnemyController agent)
    {
    }

    public override StateMachine Update(EnemyController agent)
    {
        if (agent.CanAttack())
        {
            agent.Attack();
            return this;
        }
        else
        {
            Exit(agent);
            return new ChasingState(agent);
        }
    }
}
