using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lorenzo
{
    public int healthPoints, skillPoints;
    //public List<

    public static Lorenzo instance;
    public static Lorenzo GetInstance()
    {
        if (instance == null)
            instance = new Lorenzo();

        return instance;
    }

    private Lorenzo()
    {

    }

}
