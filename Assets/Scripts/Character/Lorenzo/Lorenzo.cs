using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lorenzo
{
    public GameObject shield;

    public bool hasRestarted = false;

    public int healthPoints, maxHealth, skillPoints, maxSkill;
    public List<Item> items;
    public int coreItemCount;
    public Weapon primaryWeapon, secondaryWeapon;

    public GameObject lorenzoObject;

    private static Lorenzo instance;

    public bool isInBasement;
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
        isInBasement = false;
    }

    public void restart()
    {
        instance = new Lorenzo();
        instance.hasRestarted = true;
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

            temp.quantity--;

            if(temp.quantity <= 0)
                items.RemoveAt(i - 1);
        }
    }

}
