using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    // Start is called before the first frame update
    public float baseDamage; //damage this hitbox should deal
    public string considersEnemy; //what the entity considers an enemy
    public float hitWeight;//how heavy the hit should beS
    void Start()
    {
        switch (gameObject.transform.root.tag)
        {
            case "Player":
                considersEnemy = "Enemy";
                break;
            case "Enemy":
                considersEnemy = "Player";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
