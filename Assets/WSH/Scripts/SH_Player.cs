using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SH_Player : SH_ARInputManager
{
    public SH_ActionDamagochi controlDamagochi;
    public SH_Panel_Battle battlePanel;
    public SH_DamagochiPickEffect pinPointEffect;

    protected override void Awake()
    {
        controlDamagochi.owner = this;
    }

    private void Update()
    {
        MouseInput();
        TouchInput();
        MouseAction();
    }

    public void MouseAction()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (controlDamagochi == null)
            return;

        if (hit.collider.CompareTag("Damagochi"))
        {
            if (controlDamagochi.AttackTo(hitDamagochi.GetComponent<SH_ActionDamagochi>()))
            {
                pinPointEffect.FollowTarget(hitDamagochi);
            }
            
        }
        else
        {
            pinPointEffect.Off();
            controlDamagochi.MoveTo(hit.point);
        }
    }
}
