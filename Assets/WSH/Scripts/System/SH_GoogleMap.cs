using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SH_GoogleMap : MonoBehaviour
{
    [SerializeField]
    string key;

    public SH_UI_GPSDebug gd;
    public GameObject playerMarker;
    SH_GPSEnemySpawner es;
    GraphicRaycaster gr;

    public Vector3 prevPlayerPos;
    public Vector3 currPlayerPos;
    public Vector3 playerMoveDir;
    public enum MapType
    {
        RoadMap,
        Satellite,
        Hybrid,
        Terrain
    }

    public RawImage rawImage;
    public float latitude;
    public float longitude;

    public int maxZoom;
    public int curZoom;

    public int mapScale;

    private void Start()
    {
        gd = FindObjectOfType<SH_UI_GPSDebug>();
        gr = FindObjectOfType<GraphicRaycaster>();
        es = FindObjectOfType<SH_GPSEnemySpawner>();
        StartCoroutine(LocationUpdate());
        playerMarker.transform.position = rawImage.transform.position;
    }

    public bool mapOn;
    IEnumerator LocationUpdate()
    {
        //if (!Input.location.isEnabledByUser)
        //    yield break;

        mapOn = false;
        Input.location.Start();

        if (Input.location.status == LocationServiceStatus.Failed)
            yield break;

        while (true)
        {
            prevPlayerPos = currPlayerPos;
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            currPlayerPos = new Vector3(latitude, longitude);
            playerMoveDir = prevPlayerPos - currPlayerPos;
            //EnemyMarkerUpdate();

            Maps(rawImage, latitude, longitude, curZoom, 2, MapType.Terrain);
            //gd.DebugUpdate(this);
            yield return new WaitForSeconds(3f);
            mapOn = true;
        }

        Input.location.Stop();
    }

    List<RaycastResult> results = new List<RaycastResult>();
    void EnemyMarkerUpdate()
    {
        for(int i = 0; i < es.enableEnemyMarkers.Count; ++i)
        {
            var temp = es.enableEnemyMarkers[0];
            temp.transform.position += playerMoveDir;
            PointerEventData ped = new PointerEventData(null);
            ped.position = temp.transform.position;
            results.Clear();
            gr.Raycast(ped, results);

            bool hasOn = false;
            foreach (var r in results)
            {
                if (r.gameObject.CompareTag("Map"))
                {
                    hasOn = true;
                    break;
                }
            }

            if (!hasOn)
            {
                if (temp.enemy.actionState != SH_ActionDamagochi.ActionState.Battle)
                {
                    Destroy(temp.enemy.gameObject);
                    Destroy(temp.gameObject);
                    es.enableEnemyMarkers.RemoveAt(0);
                }
            }
        }
        //foreach(var i in es.enableEnemyMarkers)
        //{
        //    i.transform.position += playerMoveDir;
        //    PointerEventData ped = new PointerEventData(null);
        //    ped.position = i.transform.position;
        //    results.Clear();
        //    gr.Raycast(ped, results);

        //    bool hasOn = false;
        //    foreach(var r in results)
        //    {
        //        if (r.gameObject.CompareTag("Map"))
        //        {
        //            hasOn = true;
        //            break;
        //        }
        //    }

        //    if (!hasOn)
        //    {
        //        Destroy(i.enemy.gameObject);
        //        Destroy(i);
        //    }
            
        //}
    }

    public void Zoom(bool pm)
    {
        if (pm)
        {
            curZoom++;
            if (maxZoom < curZoom)
                curZoom = maxZoom;
        }
        else
        {
            curZoom--;
            if (curZoom < 0)
                curZoom = 0;
        }
        int scale = 1;

        Maps(rawImage, latitude, longitude, curZoom, scale, MapType.RoadMap);
    }

    bool Maps(RawImage rawImage, float latitude, float longitude, int zoom, int scale, MapType mt)
    {
        try
        {
            int mw = Screen.width;
            int mh = Screen.height;

            string url = "https://maps.googleapis.com/maps/api/staticmap?" +
                "center=" + latitude + "," + longitude +
                "&zoom=" + zoom +
                "&size=" + 1920 + "x" + 1080 +
                "&scale=" + scale +
                "&maptype=" + mt +
                //"&markers=color:blue%7Clabel:S%7C" + latitude + "," + longitude +
                "&key=" + key;
            //Debug.Log("URL : " + url);

            WWW www = new WWW(url);
            int delay = 1000;
            int timer = 0;
            bool done = false;

            while (delay > timer)
            {
                System.Threading.Thread.Sleep(1);
                timer++;
                if (www.isDone)
                {
                    done = true;
                    break;
                }
            }

            if (!done)
                return false;

            if (rawImage == null)
                return false;

            rawImage.texture = www.texture;
            rawImage.SetNativeSize();
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        return true;
    }
}
