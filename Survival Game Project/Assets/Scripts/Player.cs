using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    public float basicAttackDuration; //duration of basic attack
    public float basicAttackCoolDown; //duration of the basic attack cooldown
    public bool attackCD;

    public float rollRangeValue;
    public float rollCDTimer;
    public bool rollCD;

    public AttackHitBoxObject attackHitBox;
    public bool interact;

    public Dictionary<string, int> inventory;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        attackHitBox.transform = transform.GetChild(0);//get the position of the collider from the child
        attackHitBox.position = attackHitBox.transform.localPosition; //get the position of collider from the child
        attackHitBox.collider = transform.GetChild(0).GetComponent<BoxCollider2D>(); //get the collider from the child
        inventory = new Dictionary<string, int>();
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
                rigidbody.velocity = Vector2.zero;
                ChangeAnimationLayer(1); //change the active animation layer
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0.0f, -moveSpeed * Time.deltaTime, 0.0f); //move player down
                animator.SetBool(animationStates[AnimationStates.IS_MOVING], true); //set the animation
                rigidbody.velocity = Vector2.zero;
                ChangeAnimationLayer(2); //change the active animation layer
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f); //move player left
                animator.SetBool(animationStates[AnimationStates.IS_MOVING], true); //set the animation
                spriteRenderer.flipX = true; //flip the sprite renderer
                attackHitBox.transform.localPosition = new Vector2(-attackHitBox.position.x, attackHitBox.position.y); //flip the attack hitbox
                rigidbody.velocity = Vector2.zero;
                ChangeAnimationLayer(0); //change the active animation layer
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f); //move player right
                animator.SetBool(animationStates[AnimationStates.IS_MOVING], true); //set the animation
                spriteRenderer.flipX = false; //unflip the sprite renderer
                attackHitBox.transform.localPosition = new Vector2(attackHitBox.position.x, attackHitBox.position.y); //set right hitbox position
                rigidbody.velocity = Vector2.zero;
                ChangeAnimationLayer(0); //change the active animation layer
            }
            if (Input.GetKey(KeyCode.Space) && !rollCD)
            {
                
                StartCoroutine(Roll());
                //add roll method call here
            }
            if (Input.GetKey(KeyCode.E))
            {
                //PlayerInteract();
                interact = true;
            }
            else
            {
                interact = false;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                PrintInventory();
            }


        }
        else
        {
            animator.SetBool(animationStates[AnimationStates.IS_MOVING], false); //set the animation to idle
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {

        /* if (collision.tag == "NPC" && interact == true)
         {
             NPC npc = collision.GetComponent<NPC>();
             Debug.Log(npc.npcMessage);
             //if (npc.canInteract)
             //{

                 //npc.canInteract = false;
             //}
             interact = false;
         } */
        
    }
    public void PlayerInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //interact = true;
        }
        else
        {
            // interact = false;
        }
    }
    public IEnumerator Roll()
    {
        animator.SetTrigger(animationStates[AnimationStates.IS_ROLLING]); //trigger roll animation
        rigidbody.velocity = Vector2.zero;
        objState.isDamageable = false;
        Debug.Log(objState.isDamageable);
        for (int i = 0; i < animator.layerCount; i++)
        {
            if (ReturnCurrentAnimationState(i))
            {
                switch (i)
                {
                    case 0:

                        if (spriteRenderer.flipX)
                        {
                            rigidbody.AddForce(Vector2.left * rollRangeValue, ForceMode2D.Force); //left
                        }
                        else
                        {
                            rigidbody.AddForce(Vector2.right * rollRangeValue, ForceMode2D.Force); //right
                        }
                        break;
                    case 1:
                        rigidbody.AddForce(Vector2.up * rollRangeValue, ForceMode2D.Force); //up
                        break;
                    case 2:
                        rigidbody.AddForce(Vector2.down * rollRangeValue, ForceMode2D.Force); //down
                        break;
                }
            }
        }

        rollCD = true;
        yield return new WaitForSeconds(rollCDTimer);
        rigidbody.velocity = Vector2.zero;
        rollCD = false;
        objState.isDamageable = true;
        Debug.Log(objState.isDamageable);

    }
    public override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        if (interact & col.TryGetComponent(out NPC npc) || col.tag == "NPC")
        {
            Debug.Log(npc.npcMessage);
        }
        if (col.tag == "Item" && col.TryGetComponent(out Item item))
        {
            if (!CheckInvForItem(item))
            {
                inventory.Add(item.itemName, 1); //add new item entry
                Debug.Log("Added: " + item.itemName + " to inventory. Count: " + inventory[item.itemName]);
                Destroy(col.gameObject);

            }
            else
            {
                inventory[item.itemName]++; //add +1 to item count
                Debug.Log("Added: " + item.itemName + " to inventory. Count: " + inventory[item.itemName]);
                Destroy(col.gameObject);
            }
            
        }
        if (col.tag == "Building")
    {
            Building buildingScript = col.GetComponent<Building>();
            if (!buildingScript.built)
            {
                buildingScript.Interact(inventory);
                UpdateInventory(buildingScript.GetRequiredItemKeys(), buildingScript.requiredAmounts);
            }
        }
    }
    /// <summary>
    /// Player Attack Method
    /// </summary>
    /// <param name="target">The target this entity should target</param>
    /// <param name="previousState">The previous animation state</param>
    /// <returns></returns>
    public override IEnumerator Attack(GameObject target, AnimationStates previousState)
    {
        if (Input.GetMouseButtonDown(0) && !animator.GetBool(animationStates[AnimationStates.IS_MOVING]))
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
                animator.SetLayerWeight(i, 0.0f); //turn off animation layer
            }
            else
            {
                animator.SetLayerWeight(i, 1.0f); //turn on animation layer
            }
        }
    }
    public bool ReturnCurrentAnimationState(int layerIndex)
    {
        if(animator.GetLayerWeight(layerIndex) == 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
       
        
    }
    void PrintInventory()
    {
        foreach(KeyValuePair<string ,int> item in inventory)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }
    bool CheckInvForItem(Item item)
    {
        foreach(string name in inventory.Keys)
        {
            if(item.itemName == name)
            {
                return true;
            }
        }
        return false;
    }
    void UpdateInventory(List<string> itemList, List<int> amounts)
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            if (inventory.ContainsKey(itemList[i]))
            {
                inventory[itemList[i]] = Mathf.Abs(amounts[i]);
                /*if (inventory[itemList[i]] - amounts[i] > 0)
                {
                    inventory[itemList[i]] -= amounts[i];
                }
                else if(amounts[i] > 0)
                {
                    inventory[itemList[i]] = 0;
                }*/
                /*if(amounts[i] == 0)
                {
                    inventory[itemList[i]] = 0;
                }
                else
                {
                    inventory[itemList[i]] -= Mathf.Abs(amounts[i]);
                }*/

            }
        }
    }
}
