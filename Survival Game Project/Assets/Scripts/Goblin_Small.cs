﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Small : Enemy
{
    // Start is called before the first frame update
    public float dashRange;
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        bool checkForDeath = CheckForDeath();
        if (!checkForDeath)
        {
            base.Update(); //simple tracking
        }
        else
        {
            animator.SetBool(animationStates[AnimationStates.IS_DEAD], true);
            OnDeath();
        }
        
    }
    public override IEnumerator Attack(GameObject target, AnimationStates previousState)
    {
        float tempMS = moveSpeed;
        moveSpeed = 0; //stop moving
        attackCD = true; //set cooldown to true
        animator.SetBool(animationStates[previousState], false); //set previous animation state
        transform.position = Vector2.Lerp(transform.position, target.transform.position, tempMS / 2.0f); //dash to target
        attackHitBox.collider.enabled = true; //enable the collider
        yield return new WaitForSeconds(attackDuration); //duration of the attack
        moveSpeed = tempMS; //start moving
        attackHitBox.collider.enabled = false;//disable collider
        animator.SetBool(animationStates[0], false); //set animation to idle
        yield return new WaitForSeconds(attackCoolDown); //wait until next attack
        attackCD = false; //a new attack can occur
    }
}
