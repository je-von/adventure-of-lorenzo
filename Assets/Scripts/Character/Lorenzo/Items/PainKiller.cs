using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainKiller : Item
{
    public PainKiller() : base(Resources.Load<Sprite>("PAINKILLER_SPRITE"))
    {
    }

    public override void UseItem()
    {
        int temp = Lorenzo.GetInstance().healthPoints;
        for(int i = 0; i < 5; i++)
        {
            new WaitForSeconds(1f);
            Lorenzo.GetInstance().healthPoints -= 90;
        }

        if(temp < Lorenzo.GetInstance().healthPoints)
        {
            Lorenzo.GetInstance().healthPoints = temp;
        }
    }
}
