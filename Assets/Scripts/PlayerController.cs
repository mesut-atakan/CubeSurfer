using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header ("Player Forward")]
    [Range (100, 500)] public float speed_forward = 250;
    [HideInInspector] public float playerSpeed;
    [HideInInspector] public Rigidbody rb;


    [Header ("Player Horizontal")]
    [Range(10, 150)] public float speed_horizontal = 20f;
    [HideInInspector] public float mouseSpeed;


    UISystem UIscript;



    private void Awake() {
        rb = this.gameObject.GetComponent<Rigidbody>();
        UIscript = GameObject.Find("Canvas").GetComponent<UISystem>();
    }


    private void FixedUpdate() {
        playerForward();
        playerHorizontal();
    }


    public void playerForward()
    {
        playerSpeed = speed_forward * Time.fixedDeltaTime;
        rb.velocity = Vector3.forward * playerSpeed;
    }

    public void playerHorizontal()
    {
        
        mouseSpeed = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * speed_horizontal;

        transform.Translate(mouseSpeed, 0, 0);
        var _pos = transform.position;

        _pos.x = Mathf.Clamp (transform.position.x, -2, 2);

        transform.position = _pos;
    }



    private void OnTriggerEnter(Collider other) 
    {
        if(this.gameObject.tag == "Player" && other.tag == "barrier")
        {
            UIscript.MenuSystem();
        }
    }
}
