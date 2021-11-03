using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WarriorController : EnemyController
{
    public override IEnumerator MoveEnemy()
    {
        animator.SetBool("isWalking", true);
        return base.MoveEnemy();
    }
}
