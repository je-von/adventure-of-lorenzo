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
        Debug.Log("use health");
        if(Lorenzo.GetInstance().healthPoints + 200 > Lorenzo.GetInstance().maxHealth)
        {
            Lorenzo.GetInstance().healthPoints = Lorenzo.GetInstance().maxHealth;
        }
        else
        {
            Lorenzo.GetInstance().healthPoints += 200;

        }

    }
}
