using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DamagochiType
{
    Bear,
    Cat,
    Dove,
}
public class SH_HomePlateCreator : MonoBehaviour
{
    [Serializable]
    public struct HomePlateProps
    {
        public GameObject[] props;
        public float radius;
        public int instancePerOnce; //�ѹ� �����Ҷ� �󸶳� �е��� ���� ������ �Ǵ°�.
    }

    public HomePlateProps bear;
    public HomePlateProps cat;
    public HomePlateProps dove;
    public void CreateHomePlate(DamagochiType damaType, Vector3 centerPos)
    {
        switch (damaType)
        {
            case DamagochiType.Bear:
                Create(bear);
                break;
            case DamagochiType.Cat:
                Create(cat);
                break;
            case DamagochiType.Dove:
                Create(dove);
                break;
        }
    }

    void Create(HomePlateProps plate)
    {

    }
}
