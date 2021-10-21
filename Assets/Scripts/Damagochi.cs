using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Damagochi : MonoBehaviour
{
    public new string name;

    int hungry
    {
        set
        {
            hungry += value;
            Mathf.Clamp(hungry, 0, 9);
        }
        get
        {
            return hungry;
        }
    }  //배고픔
    public enum HungryState
    {
        Very,
        Little,
        Normal,
        Enough,
        Full,
    }
    public HungryState hungryState;

    int intimacy {
        set
        {
            intimacy += value;
            Mathf.Clamp(intimacy, 0, 9);
        }
        get
        {
            return intimacy;
        }
    }   //애정도

    public enum Condition
    {
        Angry = 0,
        Bad = 3,
        Normal = 5,
        Good = 7,
        Happy= 10,
    }
    public Condition condition; //지금 기분

    public float moveSpeed;

    public void HungryChange(int value)
    {
        hungry += value;
        switch (hungry)
        {
            case 0:
            case 1:
                hungryState = HungryState.Very;
                break;
            case 2:
            case 3:
                hungryState = HungryState.Little;
                break;
            case 4:
            case 5:
                hungryState = HungryState.Normal;
                break;
            case 6:
            case 7:
                hungryState = HungryState.Enough;
                break;
            case 8:
            case 9:
                hungryState = HungryState.Full;
                break;
        }
    }

    public void IntimacyChange(int value)
    {
        intimacy += value;
        switch (intimacy)
        {
            case 0:
            case 1:
                condition = Condition.Angry;
                break;
            case 2:
            case 3:
                condition = Condition.Bad;
                break;
            case 4:
            case 5:
                condition = Condition.Normal;
                break;
            case 6:
            case 7:
                condition = Condition.Good;
                break;
            case 8:
            case 9:
                condition = Condition.Happy;
                break;
        }
    }


}
