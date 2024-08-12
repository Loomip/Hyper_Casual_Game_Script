using UnityEngine;
using System.Collections;

public class EnemyJumpSMB : StateMachineBehaviour
{
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<EnemySkillState>().IsJump = true; // 점프 중임을 나타내는 플래그를 설정합니다.

        animator.GetComponent<EnemySkillState>().StartCoroutine("JumpCoroutine");
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<EnemyState>().Anima.SetFloat("vertical", 0f);

        // 점프가 종료되었음을 나타내는 플래그를 해제합니다.
        animator.GetComponent<EnemySkillState>().IsJump = false;
    }
}
