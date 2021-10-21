using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPotion : Item
{
    public SkillPotion(MonoBehaviour mono) : base(mono, Resources.Load<Sprite>("SKILL_SPRITE"))
    {
    }

    public override void UseItem()
    {
        Lorenzo.GetInstance().skillPoints += 75;
    }
}
