using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolFactory : GunFactory
{

    private static PistolFactory factory;

    private GameObject bullet;
    private Sprite sprite;

    public static PistolFactory GetInstance()
    {
        if (factory == null)
        {
            factory = new PistolFactory();
        }
        return factory;
    }


    private PistolFactory()
    {
        bullet = (GameObject)Resources.Load("bullets/pistol", typeof(GameObject));
        sprite = (Sprite)Resources.Load("gun/pistolSprite", typeof(Sprite));
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
