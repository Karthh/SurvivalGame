using System.Collections;
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
        //float tempMS = moveSpeed;
       // moveSpeed = 0; //stop moving
        attackCD = true; //set cooldown to true
        animator.SetBool(animationStates[previousState], false); //set previous animation state
        Vector2 uTargetDirection = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized; //create a unit vector in the direction of the target
        Debug.DrawLine(transform.position, uTargetDirection);
        rigidbody.AddForce(uTargetDirection * dashRange, ForceMode2D.Force); //dash towards target
        attackHitBox.collider.enabled = true; //enable the collider
        yield return new WaitForSeconds(attackDuration); //duration of the attack
        rigidbody.velocity = Vector2.zero;
        //moveSpeed = tempMS; //start moving
        attackHitBox.collider.enabled = false;//disable collider
        animator.SetBool(animationStates[0], false); //set animation to idle
        yield return new WaitForSeconds(attackCoolDown); //wait until next attack
        attackCD = false; //a new attack can occur
    }
}
