using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SH_Player : MonoBehaviour
{
    public SH_ActionDamagochi controlDamagochi;
    public SH_Panel_Battle battlePanel;
    public SH_DamagochiPickEffect pinPointEffect;
    SH_ARInputManager arInputManager;

    private void Awake()
    {
        base.Awake();
        controlDamagochi.owner = this;
    }

    protected override void Update()
    {
        base.Update();
        MouseAction();
        TouchAction();
    }

    void TouchAction()
    {
        if (!touch)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (controlDamagochi == null)
            return;

        if (touchRayHit.collider.CompareTag("Damagochi"))
        {
            var hitDamagochi = touchRayHit.collider.gameObject.GetComponent<Damagochi>();
            if (controlDamagochi.AttackTo(hitDamagochi.GetComponent<SH_ActionDamagochi>()))
            {
                effectOff = true;
                pinPointEffect.FollowTarget(hitDamagochi);
            }
        }
        else
        {
            effectOff = false;
            pinPointEffect.Off();
            controlDamagochi.MoveTo(touchRayHitPoint);
        }
    }

    void MouseAction()
    {
        if (!click)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (controlDamagochi == null)
            return;

        if (mouseRayHit.collider == null)
            return;

        if (mouseRayHit.collider.CompareTag("Damagochi"))
        {
            var hitDamagochi = mouseRayHit.collider.gameObject.GetComponent<Damagochi>();
            if (controlDamagochi.AttackTo(hitDamagochi.GetComponent<SH_ActionDamagochi>()))
            {
                effectOff = true;
                pinPointEffect.FollowTarget(hitDamagochi);
            }
        }
        else
        {
            effectOff = false;
            pinPointEffect.Off();
            controlDamagochi.MoveTo(mouseRayHitPoint);
        }
    }
}
