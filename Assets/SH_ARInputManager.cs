using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SH_ARInputManager : MonoBehaviour
{
    public SH_Effect touchEffect;
    List<SH_Effect> enablePool = new List<SH_Effect>();
    List<SH_Effect> disablePool = new List<SH_Effect>();

    ARRaycastManager arRaycastManager;
    List<ARRaycastHit> hits;
    public RaycastHit hit;
    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        TouchInput();
        MouseInput();
    }

    void TouchEffect(Vector3 pos)
    {
        if (disablePool.Count == 0)
            return;

        disablePool[0].gameObject.transform.position = pos;
        disablePool[0].On();
    }

    void MouseInput()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector2 clickPos = Input.mousePosition;
        bool isOverUI = clickPos.IsPointOverUIObject();

        if (isOverUI)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            TouchEffect(hit.point);
        }
    }

    void TouchInput()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);

        Vector2 touchPosition = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            bool isOverUI = touchPosition.IsPointOverUIObject();

            if (!isOverUI && arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                TouchEffect(hitPose.position);
            }
        }
    }
}
