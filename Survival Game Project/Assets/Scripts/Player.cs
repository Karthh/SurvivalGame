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
        attackHitBox.transform = transform.GetChild(0);//get the position of the collider from the child
        attackHitBox.position = attackHitBox.transform.localPosition; //get the position of collider from the child
        attackHitBox.collider = transform.GetChild(0).GetComponent<BoxCollider2D>(); //get the collider from the child
        InitializeAnimationStates(); //set up animation states unique to player
    }

    // Update is called once per frame
    void Update()
    {
        bool checkForDeath = CheckForDeath(); //check for death
        if (!checkForDeath) //player is alive
        {
            Movement(null); //movement
            if (Input.GetMouseButton(0) && !attackCD)
            {
                StartCoroutine(Attack(null, AnimationStates.IS_MOVING)); //start attack
                animator.SetTrigger(animationStates[AnimationStates.IS_ATTACKING]); //set attack animation
            }
        }
        else //player is dead
        {
            animator.SetBool(animationStates[AnimationStates.IS_DEAD], true); //death animation trigger
            ChangeAnimationLayer(0);
            OnDeath(); //to do on death
            //Destroy(gameObject, 1.0f);
        }
        
    }
    /// <summary>
    /// Initializes the animationStates Dictionary
    /// </summary>
    public override void InitializeAnimationStates()
    {
        
        base.InitializeAnimationStates();
        animationStates.Add(AnimationStates.IS_ROLLING, "isRolling");
    }
    /// <summary>
    /// Player Movement method
    /// </summary>
    /// <param name="target">Where the entity should move to</param>
    public override void Movement(GameObject target)
    {
        if (Input.anyKey) //any keyboard input
        {
            if (Input.GetKey(KeyCode.W))
            { //north

                transform.Translate(0.0f, moveSpeed * Time.deltaTime, 0.0f); //move player up
                animator.SetBool(animationStates[AnimationStates.IS_MOVING], true); //set the animation
                ChangeAnimationLayer(1); //change the active animation layer
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f); //move player down
                animator.SetBool(animationStates[AnimationStates.IS_MOVING], true); //set the animation
                ChangeAnimationLayer(2); //change the active animation layer
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f); //move player left
                animator.SetBool(animationStates[AnimationStates.IS_MOVING], true); //set the animation
                spriteRenderer.flipX = true; //flip the sprite renderer
                attackHitBox.transform.localPosition = new Vector2(-attackHitBox.position.x, attackHitBox.position.y); //flip the attack hitbox
                ChangeAnimationLayer(0); //change the active animation layer
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f); //move player right
                animator.SetBool(animationStates[AnimationStates.IS_MOVING], true); //set the animation
                spriteRenderer.flipX = false; //unflip the sprite renderer
                attackHitBox.transform.localPosition = new Vector2(attackHitBox.position.x, attackHitBox.position.y); //set right hitbox position
                ChangeAnimationLayer(0); //change the active animation layer
            }
            if (Input.GetKey(KeyCode.Space))
            {
                animator.SetTrigger(animationStates[AnimationStates.IS_ROLLING]); //trigger roll animation
                //add roll method call here
            }
        }
        else
        {
            animator.SetBool(animationStates[AnimationStates.IS_MOVING], false); //set the animation to idle
        }
    }
    public override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
    }
    /// <summary>
    /// Player Attack Method
    /// </summary>
    /// <param name="target">The target this entity should target</param>
    /// <param name="previousState">The previous animation state</param>
    /// <returns></returns>
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
        }
    }
    /// <summary>
    /// Changes the target animation layer to the target index
    /// </summary>
    /// <param name="target">The target animation layer index</param>
    /// <param name="maxSize">The amount of layers in the animator</param>
    public void ChangeAnimationLayer(int target)
    {
        int size = animator.layerCount;
        for(int i = 0; i < size; i++)
        {
            if(i != target)
            {
                animator.SetLayerWeight(i, 0.0f);
            }
            else
            {
                animator.SetLayerWeight(i, 1.0f);
            }
        }
    }
}
