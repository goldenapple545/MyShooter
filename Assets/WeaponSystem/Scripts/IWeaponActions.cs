using System.Collections.Generic;

public interface IWeaponActions
{
    void Shoot();
    bool Reload(int ammoCount);
}
