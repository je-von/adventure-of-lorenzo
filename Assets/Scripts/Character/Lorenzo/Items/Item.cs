using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public Sprite sprite;
    public Item(Sprite sprite)
    {
        Debug.Log(sprite.name);
        this.sprite = sprite;
    }

    public abstract void UseItem();
}
