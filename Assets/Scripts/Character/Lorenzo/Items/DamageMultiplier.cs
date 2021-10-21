using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMultiplier : Item
{
    public DamageMultiplier(MonoBehaviour mono) : base(mono, Resources.Load<Sprite>("DOUBLEDAMAGE_SPRITE"))
    {
    }

    public override void UseItem()
    {
        mono.StartCoroutine(DoubleDamage());
    }

    IEnumerator DoubleDamage()
    {
        int tempPrimary = Lorenzo.GetInstance().primaryWeapon.damage;
        int tempSecondary = Lorenzo.GetInstance().secondaryWeapon.damage;

        Lorenzo.GetInstance().primaryWeapon.damage *= 2;
        Lorenzo.GetInstance().secondaryWeapon.damage *= 2;

        yield return new WaitForSeconds(5f);

        Lorenzo.GetInstance().primaryWeapon.damage = tempPrimary;
        Lorenzo.GetInstance().secondaryWeapon.damage = tempSecondary;
    }
}
