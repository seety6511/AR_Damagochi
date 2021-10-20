using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Player : MonoBehaviour
{
    Camera cam;
    public Damagochi controlDamagochi;
    SH_ARInputManager arInputManager;

    private void Start()
    {
        cam = Camera.main;
        arInputManager = FindObjectOfType<SH_ARInputManager>();
    }

    private void Update()
    {
        MouseAction();
    }

    public void MouseAction()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (controlDamagochi == null)
            return;

        controlDamagochi.MoveTo(arInputManager.hit.point);
    }
}
