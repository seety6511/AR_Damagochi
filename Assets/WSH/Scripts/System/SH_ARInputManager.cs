using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class SH_ARInputManager : MonoBehaviour
{
    public SH_Effect touchEffect;
    List<SH_Effect> enablePool = new List<SH_Effect>();
    List<SH_Effect> disablePool = new List<SH_Effect>();

    ARRaycastManager arRaycastManager;
    public List<ARRaycastHit> hits;
    public RaycastHit hit;

    public bool touch;
    public Vector2 touchPos;
    public Touch touchData;

    public bool click;
    public Vector2 mousePos;

    public bool touchOff;
    public bool clickOff;
    public bool effectOff;

    public bool arHit;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        hits = new List<ARRaycastHit>();
        for(int i = 0; i < 6; ++i)
        {
            var e = Instantiate(touchEffect);
            enablePool.Add(e);
            e.Set(enablePool, disablePool);
            e.Off();
        }
    }

    protected virtual void Update()
    {
        TouchInput();
        ClickInput();
        InputEffect();
    }

    public bool ARRayCast(Ray ray)
    {
        arHit = false;
        if(arRaycastManager.Raycast(ray, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            if (hits.Count != 0)
                arHit = true;
            return arHit;
        }
        return arHit;
    }

    public bool ARRayCast(Ray ray, ref List<ARRaycastHit> hits)
    {
        arHit = false;
        if (arRaycastManager.Raycast(ray, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            if (hits.Count != 0)
                arHit = true;
            return arHit;
        }
        return arHit;
    }

    void InputEffect()
    {
        if (effectOff)
            return;

        if (click)
        {
            TouchEffect(mouseRayHitPoint);
        }

        if (touch)
        {
            TouchEffect(touchPos);
        }
    }
    void TouchEffect(Vector3 pos)
    {
        if (disablePool.Count == 0)
            return;

        disablePool[0].gameObject.transform.position = pos;
        disablePool[0].On();
    }

    public RaycastHit mouseRayHit;
    public Vector3 mouseRayHitPoint;
    void ClickInput()
    {
        if (clickOff)
        {
            click = false;
            mousePos = Vector2.zero;
            return;
        }

        mousePos = Input.mousePosition;
        var mouseRay = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(mouseRay, out mouseRayHit))
            mouseRayHitPoint = mouseRayHit.point;
        else
        {
            mouseRayHit = new RaycastHit();
            mouseRayHitPoint = Vector3.zero;
        }

        if (Input.GetMouseButtonDown(0))
            click = true;
        else
            click = false;
    }

    public RaycastHit touchRayHit;
    public Vector3 touchRayHitPoint;
    void TouchInput()
    {
        if (touchOff)
        {
            touch = false;
            touchData = new Touch();
            touchPos = Vector2.zero;
            return;
        }

        if (Input.touchCount == 0)
        {
            touch = false;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        if (Physics.Raycast(ray, out touchRayHit))
            touchRayHitPoint = touchRayHit.point;
        else
        {
            touchRayHit = new RaycastHit();
            touchRayHitPoint = Vector3.zero;
        }
        ARRayCast(ray);
        touch = true;
        touchData = Input.GetTouch(0);
        touchPos = touchData.position;
    }
}
