using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    GameObject miPersonaje;


    private void Start()
    {
        
        PhotonNetwork.Instantiate("Player", new Vector3(1.42f, 18.38f, -3.02681f)/*(Random.Range(-10, 10), 10, Random.Range(-10, 10))*/, Quaternion.identity);
        Camera.main.transform.SetParent(miPersonaje.transform);

        /*if (instance != null)
            DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);*/
    }


    void Update()
    {

    }
}
