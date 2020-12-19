using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Hostile : StateMachineBehaviour
{
    float timer;
    PatrolBot bot;
    TurretAI script;
    BotManager bm;
    float targetRange = 15.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bot = animator.gameObject.GetComponent<PatrolBot>();
        script = animator.GetComponent<TurretAI>();
        bm = FindObjectOfType<BotManager>();
        bot.targetObject = bot.playerObject;
        timer = 0.0f;
        bm.AlertBots(bot.transform.position);
        Debug.Log("bm notified");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        timer += Time.deltaTime;
        bot.turret.transform.LookAt(bot.targetObject.transform.position);
        Debug.DrawLine(bot.transform.position, bot.targetObject.transform.position, Color.red);
        if (Vector3.Distance(bot.transform.position, bot.targetObject.transform.position) < targetRange && bot.canFire)
        {
            bot.Fire();
        }

        else if (!script.TargetIsVisible())
        {
            //Debug.Log(bot.name + " is now alert");
            animator.SetBool("Hostile", false);
            animator.SetBool("Alert", true);
        }

        else
        { 
            
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
