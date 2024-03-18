using System;

[Serializable]
public class Weapon: IWeaponActions
{
    public long ID;
    public bool isSafe;
    private int _ammoCount;

    public bool Shoot()
    {
        throw new NotImplementedException();
    }

    public bool Reload(int ammoCount)
    {
        throw new NotImplementedException();
    }

    public bool OnSafe()
    {
        throw new NotImplementedException();
    }
}
