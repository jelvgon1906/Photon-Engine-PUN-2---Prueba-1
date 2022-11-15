using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    private void Start()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(-10, 10), 10, Random.Range(-10, 10)), Quaternion.identity);
        instance = this;
        if (instance != null)
            DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }


    void Update()
    {

    }
}
