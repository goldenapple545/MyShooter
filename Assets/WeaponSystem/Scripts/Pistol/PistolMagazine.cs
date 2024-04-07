using WeaponSystem.Scripts.Base;
using WeaponSystem.Scripts.Base.Interfaces;

namespace WeaponSystem.Scripts.Pistol
{
    public class PistolMagazine : Magazine
    {
        private void Start()
        {
            MagazineType = MagazineType.Pistol;
            NumberOfBullets = 13;
            CurrentNumberOfBullets = NumberOfBullets;
        }
    }
}
