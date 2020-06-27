using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Vector3 direction;
    public Animator animator;
    public float lifeTime;
    void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction.x * Time.deltaTime * speed, direction.y * Time.deltaTime * speed, 0f);
        //transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if(animator != null)
        {
            animator.SetTrigger("onHit");
            Destroy(gameObject, 0.1f);
        }
       
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(animator != null)
        {
            animator.SetTrigger("onHit");
        }
        
    }
}
