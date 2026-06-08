using UnityEngine;
using UnityEngine.Windows;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    //protected string animBoolName;

    //protected Animator anim;

    // State Timer is used for changing state;
    protected float stateTimer;
    // Trigger is for checking is this state has been called;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine/*, string animBoolName*/)
    {
        this.stateMachine = stateMachine;
        //this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        //anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        //UpdateAnimationParameters();
    }

    public virtual void Exit()
    {
        //anim.SetBool(animBoolName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }

    //public virtual void UpdateAnimationParameters()
    //{ }
}
