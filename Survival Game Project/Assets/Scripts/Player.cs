using System.Collections;
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
        if(Input.GetMouseButton(0) && !attackCD)//if (animator.GetBool(animationStates[0]) && !attackCD)
        {
            StartCoroutine(Attack(null, AnimationStates.IS_MOVING));
            animator.SetTrigger(animationStates[AnimationStates.IS_ATTACKING]);
        }
    }
    public override void Movement(GameObject target)
    {
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W))
            { //north

                transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f);
                animator.SetBool(animationStates[0], true); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], true)
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f);
                animator.SetBool(animationStates[0], true); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], true)
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f);
                animator.SetBool(animationStates[0], true); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], true)
                spriteRenderer.flipX = true; //flip the sprite renderer
                attackHitBox.transform.localPosition = new Vector2(-attackHitBox.position.x, attackHitBox.position.y); //flip the attack hitbox
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
                animator.SetBool(animationStates[0], true); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], true)
                spriteRenderer.flipX = false;
                attackHitBox.transform.localPosition = new Vector2(attackHitBox.position.x, attackHitBox.position.y); //set right hitbox position
            }
        }
        else
        {
            animator.SetBool(animationStates[0], false); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], false)
        }
    }
    public override IEnumerator Attack(GameObject target, AnimationStates previousState)
    {
        if (Input.GetMouseButtonDown(0))
        {
            attackCD = true; //set cooldown to true
            animator.SetBool(animationStates[previousState], false); //set previous animation state
            attackHitBox.collider.enabled = true; //enable the collider
            yield return new WaitForSeconds(basicAttackDuration); //duration of the attack
            attackHitBox.collider.enabled = false;//disable collider
            animator.SetBool(animationStates[0], false); //set animation to idle
            yield return new WaitForSeconds(basicAttackCoolDown); //wait until next attack
            attackCD = false; //a new attack can occur

            float rng = Random.Range(0.0f, 1.0f);
        }
    }
}
