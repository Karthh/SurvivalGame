﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AnimationStates
{
    IS_MOVING = 0,
    IS_ATTACKING = 1,
    IS_HURTING = 2,
    IS_DEAD = 3,
    IS_ROLLING = 4
}
public struct State
{
    public State(bool isDamageable)
    {
        this.isDamageable = isDamageable;
    }
    public bool isDamageable;

}
public abstract class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    public string enitityName; //the entities name
    public GameManager gameManager;
    #region Entity Stats
    public float maxHealth; //maximum health threshold
    public float currentHealth; //entities current health
    public float strength;
    public float magic;
    public float physicalDefense;
    public float magicalDefence;
    public float moveSpeed;
    public float resource;
    #endregion
    #region GameObject Components
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody;
    #endregion
    public bool isDead = false;
    public bool isDying = false;
    public Dictionary<AnimationStates, string> animationStates;
    public Queue<float> damageQueue;
    public State objState;

    #region VFX
    public EntityShaderManager shaderManager;
    #endregion
    public virtual void Start()
    {
        InitializeStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void InitializeStats()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        shaderManager = GetComponent<EntityShaderManager>();
        shaderManager.InitializeShaderManager();
        currentHealth = maxHealth; //health
        spriteRenderer = GetComponent<SpriteRenderer>(); //sprite renderer
        animator = GetComponent<Animator>(); //animator
        rigidbody = GetComponent<Rigidbody2D>(); //rigidbody
        InitializeAnimationStates(); //animation states dictionary
        animator.SetBool(animationStates[AnimationStates.IS_DEAD], false);
        isDead = false;
        objState = new State(true);
       
    }
    
    public virtual void InitializeAnimationStates()
    {
        animationStates = new Dictionary<AnimationStates, string>();
        animationStates.Add(AnimationStates.IS_MOVING, "isMoving");
        animationStates.Add(AnimationStates.IS_ATTACKING, "isAttacking");
        animationStates.Add(AnimationStates.IS_HURTING, "isHurting");
        animationStates.Add(AnimationStates.IS_DEAD, "isDead");
    }
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Hitbox" && objState.isDamageable)
        {
            AttackHitBox hb = col.GetComponent<AttackHitBox>();
            if(hb.considersEnemy == gameObject.transform.tag) //hitbox is from an enemy
            {
                float damageFromHit = hb.baseDamage;
                TakeDamage(damageFromHit);
                animator.SetBool(animationStates[AnimationStates.IS_HURTING], true); //hurt animation trigger
                /*if (spriteRenderer.flipX)
                {
                    StartCoroutine(AddForceOnHit(hb.hitWeight, 0.25f, Vector2.left)); //add a small knockback force to the left
                }
                else
                {
                    StartCoroutine(AddForceOnHit(hb.hitWeight, 0.25f, Vector2.right)); //add a small knockback force to the right
                }*/
                
            }
        }
    }
    public virtual void TakeDamage(float damageRecieved)
    {
        currentHealth -= CalculateDamageTaken(damageRecieved);
    }
    private float CalculateDamageTaken(float damageRecieved)
    {
        return damageRecieved;
    }
    public abstract void Movement(GameObject target);
    public abstract IEnumerator Attack(GameObject target, AnimationStates previousStates);
    /// <summary>
    /// Checks if the entity's current health is less than or equal to 0
    /// </summary>
    /// <returns>Returns true if current health is less than or equal to 0. Returns false if not</returns>
    public virtual bool CheckForDeath()
    {
        return (currentHealth <= 0);
    }
    public virtual void OnDeath()
    {
        rigidbody.velocity = Vector2.zero;
        if (!isDead)
        {
            shaderManager.SetMaterial(
                shaderManager.SetDissolveParams(
                    shaderManager.SetColorIntensity(Color.yellow, 2.3f), 100), spriteRenderer);
            StartCoroutine(shaderManager.DissolveOnDeath(7.5f, 3.0f, Color.gray, spriteRenderer)); //dissolve entity on death
        }
        
    }
    /// <summary>
    /// Applies a knockback force
    /// </summary>
    /// <param name="hitWeight"></param>
    /// <param name="t"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator AddForceOnHit(float hitWeight, float t, Vector3 direction)
    {
        rigidbody.AddForce(direction * hitWeight, ForceMode2D.Force);
        yield return new WaitForSeconds(t);
        rigidbody.velocity = Vector2.zero;
    }

}
