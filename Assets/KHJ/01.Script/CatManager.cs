using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : Damagochi
{
    public static CatManager instance;
    KHJ_SceneMngr mngr;
    public bool isMoving;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        mngr = KHJ_SceneMngr.instance;
    }
    private void Update()
    {
        SetState();
        if (isMoving)
        {
            GetComponent<SceneAnimatorController>().SetAnimatorString("isWalking");
        }




    }

    public void MoveTo()
    {

    }


    void SetState()
    {
        if(hungryState == HungryState.Little)
        {
            mngr.IntimacyImg.sprite = mngr.ImmoSprites[5];
            return;
        }
        if(hungryState == HungryState.Very)
        {
            mngr.IntimacyImg.sprite = mngr.ImmoSprites[6];
            return;
        }

        switch (condition)
        {
            case Condition.Happy:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[0];
                return;
            case Condition.Good:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[1];
                return;
            case Condition.Normal:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[2];
                return;
            case Condition.Bad:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[3];
                return;
            case Condition.Angry:
                mngr.IntimacyImg.sprite = mngr.ImmoSprites[4];
                return;
        }
    }
}
