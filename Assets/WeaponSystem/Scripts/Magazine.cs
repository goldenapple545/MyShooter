using UnityEngine;

public abstract class Magazine : MonoBehaviour, IMagazine
{
    protected MagazineType MagazineType;
    protected int NumberOfBullets;
    protected int CurrentNumberOfBullets;

    public MagazineType GetMagazineType()
    {
        return MagazineType;
    }

    public int GetNumberOfBullets()
    {
        return CurrentNumberOfBullets;
    }

    public void ReloadMagazine()
    {
        CurrentNumberOfBullets = NumberOfBullets;
    }

    public void SubstractOneBullet()
    {
        if (CurrentNumberOfBullets > 0)
            CurrentNumberOfBullets--;
    }
}
