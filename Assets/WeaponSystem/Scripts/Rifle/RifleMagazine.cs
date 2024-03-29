public class RifleMagazine : Magazine
{
    private void Start()
    {
        MagazineType = MagazineType.Rifle;
        NumberOfBullets = 30;
        CurrentNumberOfBullets = NumberOfBullets;
    }
}

