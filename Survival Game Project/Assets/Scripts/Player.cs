﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public float basicAttackDuration; //duration of basic attack
    public float basicAttackCoolDown; //duration of the basic attack cooldown
    public bool attackCD;

    public AttackHitBoxObject attackHitBox;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        attackHitBox.transform = transform.GetChild(0);
        attackHitBox.position = attackHitBox.transform.localPosition;
        attackHitBox.collider = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement(null);
        if(!attackCD)//if (animator.GetBool(animationStates[0]) && !attackCD)
        {
            StartCoroutine(Attack(null, AnimationStates.IS_MOVING));
        }
    }
    public override void Movement(GameObject target)
    {
        if(Input.GetKey(KeyCode.W)){ //north

            transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f);
            //animator.SetBool(animationStates[0], true); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], true)
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        // animator.SetBool(animationStates[0], false); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], false)

    }
    public override IEnumerator Attack(GameObject target, AnimationStates previousState)
    {
        if (Input.GetMouseButtonDown(0))
        {
            attackCD = true; //set cooldown to true
           // animator.SetBool(animationStates[previousState], false); //set previous animation state
            attackHitBox.collider.enabled = true; //enable the collider
            yield return new WaitForSeconds(basicAttackDuration); //duration of the attack
            attackHitBox.collider.enabled = false;//disable collider
           // animator.SetBool(animationStates[0], false); //set animation to idle
            yield return new WaitForSeconds(basicAttackCoolDown); //wait until next attack
            attackCD = false; //a new attack can occur
        }
        
    }
}
