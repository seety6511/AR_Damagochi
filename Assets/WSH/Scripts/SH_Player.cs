using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Player : MonoBehaviour
{
    //Camera cam;
    public SH_ActionDamagochi controlDamagochi;
    public SH_Panel_Battle battlePanel;
    SH_ARInputManager arInputManager;

    private void Awake()
    {
        //cam = Camera.main;
        arInputManager = FindObjectOfType<SH_ARInputManager>();
        battlePanel = FindObjectOfType<SH_Panel_Battle>();
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

        if (arInputManager.hit.collider.CompareTag("Damagochi"))
        {
            controlDamagochi.AttackTo(arInputManager.hit.collider.gameObject.GetComponent<SH_ActionDamagochi>());
        }
        else
        {
            controlDamagochi.MoveTo(arInputManager.hit.point);
        }
    }
}
