using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public GameObject player;
    public float attackRange;
    public float distanceFromPlayer;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
        MoveTowards(player);
    }
    void MoveTowards(GameObject target)
    {
        if(distanceFromPlayer >= attackRange)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isIdle", false);
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

        }
        animator.SetBool("isMoving", false);
        animator.SetBool("isIdle", true);

        //transform.Translate(Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime));
    }
}
