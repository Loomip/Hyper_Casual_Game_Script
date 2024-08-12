using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockdownDeathSmb : StateMachineBehaviour
{
    [SerializeField] private float nuckBackSize;

    float time;

    Vector3 backward;
    Quaternion rotation;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        backward = -animator.transform.forward;

        animator.GetComponent<NavMeshAgent>().updateRotation = false;

        animator.applyRootMotion = false;

        // 타이머 초기화
        time = 0f; 
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;

        if (time < 0.7f)
        {
            //animator.GetComponent<NavMeshAgent>().velocity = backward * nuckBackSize;
            animator.GetComponent<NavMeshAgent>().velocity = backward * nuckBackSize;
        }
        else
        {
            animator.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            animator.GetComponent<NavMeshAgent>().updateRotation = true;
            animator.GetComponent<EnemyKnockdownState>().IsKnockDown = false;

            if (animator.GetComponent<EnemyState>().Health.Hp <= 0)
            {
                animator.GetComponent<NavMeshAgent>().isStopped = true;
            }
        }

        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
