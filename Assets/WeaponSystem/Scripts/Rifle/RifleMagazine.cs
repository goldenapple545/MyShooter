using WeaponSystem.Scripts.Base;
using WeaponSystem.Scripts.Base.Interfaces;

namespace WeaponSystem.Scripts.Rifle
{
    public class RifleMagazine : Magazine
    {
        private void Start()
        {
            MagazineType = MagazineType.Rifle;
            NumberOfBullets = 30;
            CurrentNumberOfBullets = NumberOfBullets;
        }
    }
}

