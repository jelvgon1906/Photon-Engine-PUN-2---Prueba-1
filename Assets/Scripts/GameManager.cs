using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    GameObject miPersonaje;


    private void Awake()
    {
        /*miPersonaje = GetComponent<GameObject>();*/
        /*PhotonNetwork.Instantiate("Player", new Vector3(1.42f, 18.38f, -3.02681f)*//*(Random.Range(-10, 10), 10, Random.Range(-10, 10))*//*, Quaternion.identity);*/
        /*Camera.main.transform.SetParent(miPersonaje.transform);*/

        /*if (instance != null)
            DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);*/
        /*object valorHash = PhotonNetwork.LocalPlayer.CustomProperties["equipo"];*/
        object valor = PhotonNetwork.LocalPlayer.CustomProperties["equipo"];
        int equipo = (int)valor;
        if (equipo == 1)
        {
            miPersonaje = PhotonNetwork.Instantiate("PlayerRed", new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            miPersonaje = PhotonNetwork.Instantiate("PlayerBlue", new Vector3(0, 0, 0), Quaternion.identity);
        }

    }


    void Update()
    {

    }
}
