using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{
    public Shield() : base(Resources.Load<Sprite>("SHIELD_SPRITE"))
    {
    }

    public override void UseItem()
    {
        
    }
}
