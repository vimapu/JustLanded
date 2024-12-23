using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunFactory : GunFactory
{
    private static LaserGunFactory factory;

    private GameObject bullet;
    private Sprite sprite;

    public static LaserGunFactory GetInstance()
    {
        if (factory == null)
        {
            factory = new LaserGunFactory();
        }
        return factory;
    }

    private LaserGunFactory()
    {
        bullet = (GameObject)Resources.Load("bullets/laserGun", typeof(GameObject));
        sprite = (Sprite)Resources.Load("gun/lasertGunSprite", typeof(Sprite));
    }
    public GameObject GetBulletPrefab()
    {
        return this.bullet;
    }

    public Sprite GetGunSprite()
    {
        return this.sprite;
    }
}
