using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainKiller : Item
{
    public PainKiller(MonoBehaviour mono) : base(mono, Resources.Load<Sprite>("PAINKILLER_SPRITE"))
    {
    }

    public override void UseItem()
    {
        mono.StartCoroutine(DecreaseHealthPoints());
    }

    IEnumerator DecreaseHealthPoints()
    {
        int temp = Lorenzo.GetInstance().healthPoints;
        Lorenzo.GetInstance().healthPoints += 450;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f);
            Lorenzo.GetInstance().healthPoints -= 90;
        }

        if (temp < Lorenzo.GetInstance().healthPoints)
        {
            Lorenzo.GetInstance().healthPoints = temp;
        }
    }
}
