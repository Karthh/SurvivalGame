using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackHitBoxObject
{
    public Transform transform;
    public Vector3 position;
    public BoxCollider2D collider;
};
public struct Attack
{
    public float duration;
    public float cooldownDuration;
    public bool isCD;
}
public abstract class Enemy : Entity
{
    public GameObject target; //the target GameObject
    public float attackRange; //the range in which this unit can attack
    public float distanceFromTarget; //the distance from the target object
    public float activeRange; //the range from the target in which this unit is active
    public float attackDuration; //duration of enemy's attack
    public float attackCoolDown; //duration of the enemy's attack cooldown
    public bool attackCD;
    public int itemCount;

    private bool isFlipped;

    public AttackHitBoxObject attackHitBox;
    public List<GameObject> dropTable;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        attackHitBox.transform = transform.GetChild(0);
        attackHitBox.position = attackHitBox.transform.localPosition;
        attackHitBox.collider = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {

        distanceFromTarget = Vector2.Distance(transform.position, target.transform.position); //calculate distance from target
        if (distanceFromTarget <= activeRange) //check to see if the target is in active range
        {
            Movement(target);
        }
        else
        {
            //to do outside range
            //turn character 'off' code here
        }
        Debug.DrawLine(transform.position, target.transform.position, Color.red);

    }
    public override void Movement(GameObject target)
    {
        if(distanceFromTarget > attackRange) //moving towards target
        {
            animator.SetBool(animationStates[0], true); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], true)
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime); //move enemy to target

        }else if(distanceFromTarget <= attackRange && !attackCD) //attack
        {
            animator.SetTrigger(animationStates[AnimationStates.IS_ATTACKING]);
            StartCoroutine(Attack(target, 0));
        }
        else //idle
        {
            animator.SetBool(animationStates[0], false); //set the animation, can also write animator.SetBool(animationStates[AnimationState.IS_MOVING], false)
        }
        isFlipped = false; //change sprite direction
        if (transform.position.x < target.transform.position.x)
        {
            isFlipped = true;
            if (isFlipped)
            {
                spriteRenderer.flipX = true; //flip the sprite renderer
                attackHitBox.transform.localPosition = new Vector2(-attackHitBox.position.x, attackHitBox.position.y); //flip the attack hitbox
            }
            
        }
        else
        {
            isFlipped = false;
            spriteRenderer.flipX = false;
            attackHitBox.transform.localPosition = new Vector2(attackHitBox.position.x, attackHitBox.position.y); //set right hitbox position
        }
       
        
    }
    public override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
    }
    public override IEnumerator Attack(GameObject target, AnimationStates previousState)
    {
        attackCD = true; //set cooldown to true
        animator.SetBool(animationStates[previousState], false); //set previous animation state
        attackHitBox.collider.enabled = true; //enable the collider
        yield return new WaitForSeconds(attackDuration); //duration of the attack
        attackHitBox.collider.enabled = false;//disable collider
        animator.SetBool(animationStates[0], false); //set animation to idle
        yield return new WaitForSeconds(attackCoolDown); //wait until next attack
        attackCD = false; //a new attack can occur
    }
    public virtual GameObject ConfigureDropTable()
    {
        //Item item = list.GetComponent<Item>();
        float dropRange = Random.Range(0.0f, 1.0f);
        if (dropRange < 0.5)
        {
            return dropTable[0];
        }
        else
        {
            return dropTable[1];
        }
    }
    public override bool CheckForDeath()
    {
        bool spawnDrop = false;
        if (base.CheckForDeath() == true)
        {
            spawnDrop = true;
            if (spawnDrop == true)
            {
                for (int i = 0; i < itemCount; itemCount++){
                    Instantiate(ConfigureDropTable(), transform.position, Quaternion.identity);
                }
                
            }
            return true;
            spawnDrop = false; //comment out if left unfinished
        }
        return false;
    }
}
