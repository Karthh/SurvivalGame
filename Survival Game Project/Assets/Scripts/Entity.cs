using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AnimationStates
{
    IS_MOVING = 0,
    IS_ATTACKING = 1
}
public abstract class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    public string enitityName; //the entities name
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
    #endregion
    public Dictionary<AnimationStates, string> animationStates;
    public Queue<float> damageQueue;
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
        spriteRenderer = GetComponent<SpriteRenderer>(); //sprite renderer
        animator = GetComponent<Animator>(); //animator
        InitializeAnimationStates(); //animation states dictionary
        currentHealth = maxHealth; //health
    }
    
    public virtual void InitializeAnimationStates()
    {
        animationStates = new Dictionary<AnimationStates, string>();
        animationStates.Add(AnimationStates.IS_MOVING, "isMoving");
        animationStates.Add(AnimationStates.IS_ATTACKING, "isAttacking");
    }
    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Hitbox")
        {
            float damageFromHit = col.GetComponent<AttackHitBox>().baseDamage;
            TakeDamage(damageFromHit);
        }
        OnDeath();
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
    public virtual void OnDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(this, 1.0f);
        }
    }


    /*
    void InitializeStats(float health, float strength, float magic, float physicalDefense, float magicalDefense, float moveSpeed, float resource)
    {
        stats = new Dictionary<string, float>();
        stats.Add("Health", health);
        stats.Add("Strength", strength);
        stats.Add("Magic", magic);
        stats.Add("Physical Defense", physicalDefense);
        stats.Add("Magical Defense", magicalDefense);
        stats.Add("Movement Speed", moveSpeed);
        stats.Add("Resource", resource);
    }
    */
}
