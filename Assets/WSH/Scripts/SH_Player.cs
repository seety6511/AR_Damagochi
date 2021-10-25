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
        arInputManager = FindObjectOfType<SH_ARInputManager>();
        //battlePanel = FindObjectOfType<SH_Panel_Battle>();
        controlDamagochi.owner = this;
    }

    private void Update()
    {
        arInputManager.MouseInput();
        arInputManager.TouchInput();
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

        if (arInputManager.hit.collider.CompareTag("Damagochi"))
        {
            if (controlDamagochi.AttackTo(arInputManager.hitDamagochi.GetComponent<SH_ActionDamagochi>()))
            {
                pinPointEffect.FollowTarget(arInputManager.hitDamagochi);
            }
            
        }
        else
        {
            pinPointEffect.Off();
            controlDamagochi.MoveTo(arInputManager.hit.point);
        }
    }
}
