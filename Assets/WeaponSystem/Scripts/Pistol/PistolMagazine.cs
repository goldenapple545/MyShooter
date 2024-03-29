public class PistolMagazine : Magazine
{
    private void Start()
    {
        MagazineType = MagazineType.Pistol;
        NumberOfBullets = 13;
        CurrentNumberOfBullets = NumberOfBullets;
    }
}
