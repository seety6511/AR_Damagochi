using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : Damagochi
{
    public static CatManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

}
