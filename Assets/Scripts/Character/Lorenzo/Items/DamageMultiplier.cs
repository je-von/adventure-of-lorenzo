using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplier : Item
{
    public DamageMultiplier() : base(Resources.Load<Sprite>("DOUBLEDAMAGE_SPRITE"))
    {
    }

    public override void UseItem()
    {
        
    }
}
