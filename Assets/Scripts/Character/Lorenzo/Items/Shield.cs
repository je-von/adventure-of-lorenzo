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
        mono.StartCoroutine(ActivateShield());
    }

    IEnumerator ActivateShield()
    {
        GameObject s = Lorenzo.GetInstance().shield;
        s.SetActive(true);

        yield return new WaitForSeconds(7f);

        s.SetActive(false);
    }
}
