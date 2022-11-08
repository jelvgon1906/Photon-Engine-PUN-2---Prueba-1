using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class ControlConexion : MonoBehaviourPunCallbacks
{
    #region Variables privadas
    #endregion

    #region Variables publicas
    public GameObject panelInicio;
    public GameObject panelBienvenida;
    public GameObject panelSala;
    /*public GameObject btnConectar;*/
    public Button btnConectar;
    public TextMeshProUGUI txtEstado, txtInfoUser;
    public TMP_InputField inputNickname, inputNombreSala, inputMaxJug, inputMinJug;
    #endregion


    private void Start()
    {
        CambiarPanel(panelInicio);
    }

    #region Eventos para botones


    public void OnClickConectarAServidor()
    {

        if (!(string.IsNullOrWhiteSpace(inputNickname.text)) || !(string.IsNullOrEmpty(inputNickname.text)))
        {


            if (!PhotonNetwork.IsConnected)
            {
                CambiarEstado("Conectando...");
                PhotonNetwork.ConnectUsingSettings();
                btnConectar.interactable = false;

            }
            else
            {
                CambiarEstado("Ya estas conectado.");
            }


        }
        else CambiarEstado("Nombre usuario incorrecto");

    }

    public void OnClickIrACrearSala()
    {
        CambiarPanel(panelSala);
    }

    public void OnClickCrearSala()
    {

        int min, max;
        min = int.Parse(inputMinJug.text);
        max = int.Parse(inputMaxJug.text);

        if (max >= min && min > 0)
        {
            if (!(string.IsNullOrWhiteSpace(inputNombreSala.text)) || !(string.IsNullOrEmpty(inputNombreSala.text)))
            {
                RoomOptions opcionesSala = new RoomOptions();
                opcionesSala.MaxPlayers = (byte)max;
                opcionesSala.IsVisible = true;
                opcionesSala.IsOpen = false;

                PhotonNetwork.CreateRoom(inputNombreSala.text, opcionesSala, TypedLobby.Default);
            }
            else
            {
                CambiarEstado("Nombre de sala incorrecto.");
            }
        }
        else
        {
            CambiarEstado("Numero de jugadores incorrecto.");
        }
    }
    #endregion

    #region Eventos propios de Photon

    public override void OnConnected()
    {
        CambiarEstado("Conectado a Photon");
        PhotonNetwork.NickName = inputNickname.text;
        txtInfoUser.text = PhotonNetwork.NickName;
        /*base.OnConnected();*/
        CambiarPanel(panelBienvenida);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        CambiarEstado("Sala creada correctamente");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        CambiarEstado("Sala creada correctamente");
    }


    #endregion

    #region Metodos privados
    /// <summary>
    /// Método que cambiará el mensaje de Estado
    /// de los paneles de introduccion al juego
    /// </summary>
    /// <param name="texto"></param>
    private void CambiarEstado(string texto)
    {
        txtEstado.text = texto;
    }

    private void CambiarPanel (GameObject panelObjetivo)
    {
        panelBienvenida.SetActive(false) ;
        panelInicio.SetActive(false);
        panelSala.SetActive(false);

        panelObjetivo.SetActive(true);
    }

    
    #endregion
}