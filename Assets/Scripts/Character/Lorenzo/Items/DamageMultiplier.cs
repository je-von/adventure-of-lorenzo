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
        int tempPrimary = Lorenzo.GetInstance().primaryWeapon.damage;
        int tempSecondary = Lorenzo.GetInstance().secondaryWeapon.damage;

        Lorenzo.GetInstance().primaryWeapon.damage *= 2;
        Lorenzo.GetInstance().secondaryWeapon.damage *= 2;

        new WaitForSeconds(5f);

        Lorenzo.GetInstance().primaryWeapon.damage = tempPrimary;
        Lorenzo.GetInstance().secondaryWeapon.damage = tempSecondary;

    }
}
