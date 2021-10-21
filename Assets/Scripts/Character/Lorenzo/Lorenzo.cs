using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lorenzo
{
    public int healthPoints, maxHealth, skillPoints, maxSkill;
    public List<Item> items;
    public int coreItemCount;
    public Weapon primaryWeapon, secondaryWeapon;

    public GameObject lorenzoObject;

    private static Lorenzo instance;
    public static Lorenzo GetInstance()
    {
        if (instance == null)
            instance = new Lorenzo();

        return instance;
    }

    private Lorenzo()
    {
        items = new List<Item>();
        maxHealth = healthPoints = 1000;
        skillPoints = maxSkill = 200;
        coreItemCount = 0; // 0/9
    }

    public bool DecreaseSkillPoint(int point)
    {
        if(point <= this.skillPoints)
        {
            this.skillPoints -= point;
            return true;
        }

        return false;
    }

    public void UseInventoryItem(int i)
    {
        if (items.Count >= i)
        {
            var temp = items[i - 1];
            temp.UseItem();

            items.RemoveAt(i - 1);
        }
    }
}
