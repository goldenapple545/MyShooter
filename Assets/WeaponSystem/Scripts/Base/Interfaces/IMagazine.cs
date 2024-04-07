namespace WeaponSystem.Scripts.Base.Interfaces
{
    public enum MagazineType
    {
        Pistol,
        Revolver,
        Rifle,
        Shotgun
    }

    public interface IMagazine
    {
        MagazineType GetMagazineType();
        int GetNumberOfBullets();
        void ReloadMagazine();
        void SubstractOneBullet();
    }
}