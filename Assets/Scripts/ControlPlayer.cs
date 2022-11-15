using Photon.Pun;
using UnityEngine;

public class ControlPlayer : MonoBehaviourPunCallbacks
{
    public float speed = 10f;
    void Start()
    {

    }

    void Update()
    {
        //photon para que no mueva a todos los players
        if (photonView.IsMine)
        {
            //colocar camara en personaje
            /*Camera.main.transform.SetParent(this.transform);
            Vector3 posCamara = new Vector3();
            posCamara.x = transform.position.x;
            posCamara.y = transform.position.y + 8.94f;
            posCamara.z = transform.position.z -3;
            Camera.main.transform.position = posCamara;*/

            //mov
            float movX = Input.GetAxis("Horizontal");
            float movZ = Input.GetAxis("Vertical");

            Vector3 pos = new Vector3();
            pos.x = transform.position.x + (movX * speed * Time.deltaTime);
            pos.y = transform.position.y;
            pos.z = transform.position.z + (movZ * speed * Time.deltaTime);

            transform.position = pos;
        }
    }
}
