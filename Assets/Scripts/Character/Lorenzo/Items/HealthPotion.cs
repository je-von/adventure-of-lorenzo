using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    public HealthPotion() : base(Resources.Load<Sprite>("HEALTH_SPRITE"))
    {
    }

    public override void UseItem()
    {
        Lorenzo.GetInstance().healthPoints += 200;
    }
}
