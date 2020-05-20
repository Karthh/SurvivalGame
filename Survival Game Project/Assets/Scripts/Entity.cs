using System.Collections;
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
    public Rigidbody2D rigidbody;
    #endregion
    public bool isDead = false;
    public Dictionary<AnimationStates, string> animationStates;
    public Queue<float> damageQueue;

    #region VFX
    float dissolveTimer = 1.0f;
    bool isDying = false;
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
        currentHealth = maxHealth; //health
        spriteRenderer = GetComponent<SpriteRenderer>(); //sprite renderer
        animator = GetComponent<Animator>(); //animator
        rigidbody = GetComponent<Rigidbody2D>(); //rigidbody
        InitializeAnimationStates(); //animation states dictionary
        animator.SetBool(animationStates[AnimationStates.IS_DEAD], false);
        isDead = false;
       
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
        if(col.tag == "Hitbox")
        {
            AttackHitBox hb = col.GetComponent<AttackHitBox>();
            if(hb.considersEnemy == gameObject.transform.tag) //hitbox is from an enemy
            {
                float damageFromHit = hb.baseDamage;
                TakeDamage(damageFromHit);
                animator.SetBool(animationStates[AnimationStates.IS_HURTING], true); //hurt animation trigger
                /*if (spriteRenderer.flipX)
                {
                    rigidbody.AddForce(Vector2.left * hb.hitWeight, ForceMode2D.Force); //add a small knockback force to the left
                }
                else
                {
                    rigidbody.AddForce(Vector2.right * hb.hitWeight, ForceMode2D.Force); //add a small knockback force to the right
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
            StartCoroutine(DissolveOnDeath(7.5f, 3.0f, Color.gray)); //dissolve entity on death
        }
        
    }
    #region VFX
    /// <summary>
    /// Dissolves Enitity on Death
    /// </summary>
    /// <param name="lerpTime">time to lerp</param>
    /// <param name="waitTime">time before dissolve effect</param>
    /// <param name="fadeColor">the color to fade to</param>
    /// <returns></returns>
    private IEnumerator DissolveOnDeath(float lerpTime, float waitTime, Color fadeColor)
    {
        isDying = true;
        yield return new WaitForSeconds(waitTime);
        if (isDying)
        {
            dissolveTimer -= Time.deltaTime; //decrement the timer
            if(dissolveTimer <= 0) //done dissolving
            {
                dissolveTimer = 0;
                isDying = false;
            }
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, fadeColor, lerpTime * Time.deltaTime); //lerp the color of the entity to the fade color
            Dissolve(); //use the dissolve shader
            yield return new WaitForSeconds(1f);
        }
        isDead = true;
        Destroy(gameObject, 0.1f); //destroy the game object

    }
    /// <summary>
    /// Dissolve Shader Code
    /// </summary>
    private void Dissolve()
    {
        Material dissolve = spriteRenderer.material;
        if (isDying)
        {
            dissolveTimer -= Time.deltaTime;
            if(dissolveTimer <= 0)
            {
                dissolveTimer = 0;
                isDying = false;
            }
            dissolve.SetFloat("_Fade", dissolveTimer);
        }
    }
    #endregion
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
