using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolMagazine : Magazine
{
    private void Start()
    {
        MagazineType = MagazineType.Pistol;
        NumberOfBullets = 13;
        CurrentNumberOfBullets = NumberOfBullets;
    }
}
