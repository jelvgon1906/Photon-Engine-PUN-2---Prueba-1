using Photon.Pun;
using System;
using UnityEngine;

public class ControlPlayer : MonoBehaviourPunCallbacks
{
    public float speed = 10f;
    public float rotationSpeed;
    Rigidbody rb;
    Animator animator;
    Camera cam;
    [SerializeField] private GameObject ammo;
    [SerializeField] private GameObject outPosition;
    [SerializeField] private bool debugMode;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cam = this.gameObject.transform.Find("MainCamera").GetComponent<Camera>();  
        if (photonView.IsMine)
        {
            cam = Camera.main;
        }
        else
        {
            Destroy(cam);
        }
    }

    void Update()
    {
        //photon para que no mueva a todos los players
        if (debugMode) Move();
        else if (photonView.IsMine)
        { 
            Move();
            if (Input.GetButtonDown("Fire1"))
            {
                photonView.RPC("Shoot", RpcTarget.All);
                Fire();
            }
        
        }
        //colocar camara en personaje
        /*Camera.main.transform.SetParent(this.transform);
        Vector3 posCamara = new Vector3();
        posCamara.x = transform.position.x;
        posCamara.y = transform.position.y + 8.94f;
        posCamara.z = transform.position.z -3;
        Camera.main.transform.position = posCamara;*/



       
    }

    private void Fire()
    {
        Instantiate(ammo, outPosition.transform.position, outPosition.transform.rotation);
    }

    private void Move()
    {
        //mov
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        Vector3 velocidad = 
              transform.right * movX * speed  // (1,0,0) * 1/-1 * speed
            + transform.forward * movZ * speed  // (0,0,1) * 1/-1 * speed
            + transform.up * rb.velocity.y;// (0,1,0) * rb.velocity.y

        rb.velocity = velocidad;

        animator.SetFloat("Velocity", velocidad.magnitude);

        transform.Rotate(transform.up * Input.GetAxis("Mouse X") * rotationSpeed);

        velocidad = new Vector3(velocidad.x, 0, velocidad.z);
        animator.SetFloat("velocity", velocidad.magnitude);

        /*new Vector3();
        pos.x = transform.position.x + (movX * speed * Time.deltaTime);
        pos.y = transform.position.y;
        pos.z = transform.position.z + (movZ * speed * Time.deltaTime);

        transform.position = pos;*/

    }
}
