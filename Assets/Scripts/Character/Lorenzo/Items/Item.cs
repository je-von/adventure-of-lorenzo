using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public int quantity;
    public Sprite sprite;
    public MonoBehaviour mono;
    public Item(MonoBehaviour mono, Sprite sprite)
    {
        quantity = 1;
        Debug.Log(sprite.name);
        this.sprite = sprite;
        this.mono = mono;
    }

    public abstract void UseItem();
}
