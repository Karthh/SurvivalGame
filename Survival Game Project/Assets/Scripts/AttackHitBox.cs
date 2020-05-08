using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    // Start is called before the first frame update
    public float baseDamage; //damage this hitbox should deal
    public GameObject target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SeekTarget(Transform target, float lerpTime)
    {
        if(lerpTime == 0.0f)
        {
            transform.position = target.transform.position;
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, target.transform.position, lerpTime * Time.deltaTime);
        }
    }
}
