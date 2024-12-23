
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunFactoryManager
{
    public enum GunType
    {
        PISTOL, LASER
    }

    public GunType CurrentGunType { get; private set; }
    private Dictionary<GunType, Func<GunFactory>> factories;

    public GunFactoryManager()
    {
        CurrentGunType = GunType.PISTOL;
        factories = new Dictionary<GunType, Func<GunFactory>>
        {
            { GunType.PISTOL, () => PistolFactory.GetInstance() },
            { GunType.LASER, () => LaserGunFactory.GetInstance() }
        };
    }

    public GunFactory GetFactory(GunType type)
    {
        return factories.GetValueOrDefault(type, () => PistolFactory.GetInstance())();
    }

    public GunFactory Next()
    {
        GunType[] gunTypes = (GunType[])Enum.GetValues(typeof(GunType)).Cast<GunType>();
        int j = Array.IndexOf<GunType>(gunTypes, CurrentGunType) + 1;
        CurrentGunType = (gunTypes.Length == j) ? gunTypes[0] : gunTypes[j];
        return GetFactory(CurrentGunType);
    }

}
