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
    List<ARRaycastHit> hits;
    public RaycastHit hit;
    public Damagochi hitDamagochi;
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
    void TouchEffect(Vector3 pos)
    {
        if (disablePool.Count == 0)
            return;

        disablePool[0].gameObject.transform.position = pos;
        disablePool[0].On();
    }

    public void MouseInput()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            TouchEffect(hit.point);
            if (hit.collider.CompareTag("Damagochi"))
            {
                hitDamagochi = hit.collider.gameObject.GetComponent<Damagochi>();
            }
            else
                hitDamagochi = null;
        }
    }

    public void TouchInput()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                TouchEffect(hitPose.position);
            }
        }
    }
}
