using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Alert : StateMachineBehaviour
{
    float timer;
    float rotationMax = 0.30f;
    float OscillationModifier = 3.0f;
    float targetRange = 3.0f;
    Transform tempPos;

    PatrolBot bot;
    TurretAI script;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        //Acquire script links
        bot = animator.gameObject.GetComponent<PatrolBot>();
        script = animator.GetComponent<TurretAI>();

        timer = 0.0f;
        
        tempPos = bot.playerObject.transform;
        bot.tempTarget.transform.position = tempPos.position;
        bot.targetObject = bot.tempTarget;
        bot.tempTarget.transform.parent = null;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        timer += Time.deltaTime;

        Debug.DrawLine(bot.transform.position, bot.targetObject.transform.position, Color.yellow);
        if (script.TargetIsVisible())
        {
            Debug.Log(bot.name + " is now hostile");
            animator.SetBool("Alert", false);
            animator.SetBool("Hostile", true);
        }
        else if (Vector3.Distance(bot.transform.position, bot.targetObject.transform.position) < targetRange)
        {
            animator.SetBool("Alert", false);
            animator.SetBool("Hostile", false);
            //Debug.Log(bot.name + " is returning to patrol");
        }
        else
        {
            float rotationValue = Mathf.Cos(timer) * rotationMax;
            bot.turret.transform.Rotate(0, rotationValue / OscillationModifier, 0);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bot.tempTarget.transform.parent = bot.gameObject.transform;
    }

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
