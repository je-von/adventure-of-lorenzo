using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public float healthPoints, maxHealth;
    public float shootingInterval;
    public int bulletDamage;
    public int bulletsReload;
    public int respawnTime;
    public float itemPossibilty;
    public Enemy( int healthPoints, float shootingInterval, int bulletDamage, int bulletsReload, int respawnTime, float itemPossibilty)
    {
        this.healthPoints = this.maxHealth = healthPoints;
        this.shootingInterval = shootingInterval;
        this.bulletDamage = bulletDamage;
        this.bulletsReload = bulletsReload;
        this.respawnTime = respawnTime;
        this.itemPossibilty = itemPossibilty;
    }
}
