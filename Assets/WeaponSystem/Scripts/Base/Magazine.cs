using UnityEngine;

public abstract class Magazine : MonoBehaviour, IMagazine
{
    [SerializeField] protected GameObject bulletPrefab;
    
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

    protected void ShowBullet(bool isVisible)
    {
        if (bulletPrefab)
            bulletPrefab.SetActive(isVisible);
    }

    public void SubstractOneBullet()
    {
        CurrentNumberOfBullets--;
        if (CurrentNumberOfBullets > 0)
        {
            ShowBullet(true);
        }
        else
        {
            ShowBullet(false);
        }
            
    }
}
