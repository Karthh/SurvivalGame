using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Small : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        bool checkForDeath = CheckForDeath();
        if (!checkForDeath)
        {
            base.Update(); //simple tracking
        }
        else
        {
            animator.SetBool(animationStates[AnimationStates.IS_DEAD], true);
            OnDeath();
        }
        
    }
}
