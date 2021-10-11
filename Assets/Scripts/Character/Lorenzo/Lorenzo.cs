using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lorenzo
{
    public int healthPoints, skillPoints;
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
        healthPoints = 1000;
        skillPoints = 200;
        coreItemCount = 0; // 0/9
    }

}
