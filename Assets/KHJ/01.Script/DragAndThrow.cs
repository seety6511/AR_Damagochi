using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndThrow : MonoBehaviour
{

    [SerializeField]
    private float throwSpeed = 35f;
    private float speed;
    private float lastMouseX, lastMouseY;

    public bool thrown, holding;

    private Rigidbody _rigidbody;
    private Vector3 newPosition;
    public GameObject initialPos;
    public Transform catballpos;
    public Transform bearballpos;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Reset();
    }

    void Update()
    {
        if(KHJ_SceneMngr.instance.nowPet == Pet.cat)
        {
            initialPos = catballpos.gameObject;
        }
        else if(KHJ_SceneMngr.instance.nowPet == Pet.bear)
        {
            initialPos = bearballpos.gameObject;
        }

        if (holding)
            OnTouch();

        if (thrown)
        {
            CatManager.instance.actionState = CatManager.ActionState.isPlaying;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform == transform)
                {
                    holding = true;
                    transform.SetParent(null);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (lastMouseY < Input.mousePosition.y)
            {
                //CatManager.instance.IntimacyChange(3);
                CatManager.instance.condition = Damagochi.Condition.Happy;
                KHJ_SceneMngr.instance.currH += 12;
                ThrowBall(Input.mousePosition);
            }
        }

        if (Input.GetMouseButton(0))
        {
            lastMouseX = Input.mousePosition.x;
            lastMouseY = Input.mousePosition.y;
        }
    }


    public void Cancel()
    {
        CancelInvoke("Reset");
        CatManager.instance.ResetDestination();
        CatManager.instance.actionState = CatManager.ActionState.Idle;
        transform.position = initialPos.transform.position;
        newPosition = transform.position;
        thrown = holding = false;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, 200f, 0f);
    }
    public void Reset()
    {
        CatManager.instance.ResetDestination();
        CatManager.instance.actionState = CatManager.ActionState.isWaiting;
        CancelInvoke();
        transform.position = initialPos.transform.position;
        newPosition = transform.position;
        thrown = holding = false;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, 200f, 0f);
        transform.SetParent(Camera.main.transform);
    }

    void OnTouch()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane * 27.5f;

        newPosition = Camera.main.ScreenToWorldPoint(mousePos);

        transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, 50f * Time.deltaTime);
    }

    void ThrowBall(Vector2 mousePos)
    {
        _rigidbody.useGravity = true;

        float differenceY = (mousePos.y - lastMouseY) / Screen.height * 100;
        speed = throwSpeed * differenceY;

        float x = (mousePos.x / Screen.width) - (lastMouseX / Screen.width);
        x = Mathf.Abs(Input.mousePosition.x - lastMouseX) / Screen.width * 100 * x;

        Vector3 direction = new Vector3(x, 0f, 1f);
        direction = Camera.main.transform.TransformDirection(direction);

        _rigidbody.AddForce((direction * speed / 2f) + (Vector3.up * speed));

        holding = false;
        thrown = true;

        Invoke("Reset", 5.0f);
    }
}
