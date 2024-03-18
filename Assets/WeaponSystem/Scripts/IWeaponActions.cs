using System.Collections.Generic;

public interface IWeaponActions
{
    bool Shoot();
    bool Reload(int ammoCount);
    bool OnSafe();
}
