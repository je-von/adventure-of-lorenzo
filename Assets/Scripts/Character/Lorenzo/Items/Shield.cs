using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{
    public Shield(MonoBehaviour mono) : base(mono, Resources.Load<Sprite>("SHIELD_SPRITE"))
    {
    }

    public override void UseItem()
    {
        
    }
}
