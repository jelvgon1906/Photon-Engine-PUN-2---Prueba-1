﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ControlConexion : MonoBehaviourPunCallbacks
{
    #region Variables privadas
    #endregion

    #region Variables publicas
    public GameObject panelInicio;
    public GameObject panelBienvenida;
    public GameObject panelCreacionSala;
    public GameObject panelUnirSala;
    public GameObject panelSala;
    /*public GameObject btnConectar;*/
    public Button btnConectar;
    public TextMeshProUGUI txtEstado, txtInfoUser, txtCantidadJugadores, txtNombreSala, txtIsOpen;
    public TMP_InputField inputNickname, inputNombreSala, inputMaxJug, inputMinJug;

    public Button botonComenzarJuego; //Instancia del boton Conectar

    public GameObject elemJugador; //Cada uno de los botones que representa un jugador en la lista de sala
    public GameObject contenedorJugadores; //Contenedor que mantiene la lista de jugadores
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
        CambiarPanel(panelCreacionSala);
    }
    public void OnClickIrAUnirSala()
    {
        CambiarPanel(panelUnirSala);
    }
    public void OnClickVolver(GameObject LastPanel)
    {
        CambiarPanel(LastPanel);
        if(PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    public void OnClickVolverInicio(GameObject LastPanel)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
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
                opcionesSala.IsOpen = true;

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

    public void OnClickUnirseASala()
    {
        if (!string.IsNullOrEmpty(inputNombreSala.text))
        {
            PhotonNetwork.JoinRoom(inputNombreSala.text);
        }
        else
        {
            CambiarEstado("Introduzca un nombre correcto para la sala");
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

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        CambiarEstado("Desconectado por:" + cause.ToString());
        CambiarPanel(panelInicio);
    }

    /*public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        CambiarEstado("Sala creada correctamente");
        CambiarPanel(panelSala);
        if (PhotonNetwork.CurrentRoom.IsOpen)
        txtIsOpen.text = "Sala abierta" ;
        else txtIsOpen.text = "Sala cerrada";
        txtNombreSala.text = "Nombre: " + PhotonNetwork.CurrentRoom.Name;
        txtCantidadJugadores.text = "Jugadores: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        //Leemos info jugadores

        Player[] jugadores = PhotonNetwork.PlayerList;
        //Para cada jugador en jugadores
        foreach (Player player in jugadores)
        {
            //button
                *//*info = player.ActorNumber;
                 info += player.PhotonNetwork.Nickname;

                mensaje += info;
*//*
        }
        //mostrar mensaje
    }*/
    public override void OnCreatedRoom()
    {
        
        string mensaje = PhotonNetwork.NickName
            + " se ha conectado a "
            + PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.CurrentRoom.IsOpen)
            txtIsOpen.text = "Sala abierta";
        else txtIsOpen.text = "Sala cerrada";
        CambiarEstado(mensaje);
        CambiarPanel(panelSala);
        ActualizarPanelJugadores();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        CambiarEstado("Error al crear sala: " + message);
    }

    public override void OnJoinedRoom()
    {
        string mensaje = PhotonNetwork.NickName
        + " se ha unido a "
        + PhotonNetwork.CurrentRoom.Name;
        CambiarEstado(mensaje);
        CambiarPanel(panelSala);
        ActualizarPanelJugadores();
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CambiarEstado("No ha sido posible conectar a la sala: " + message);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ActualizarPanelJugadores();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ActualizarPanelJugadores();
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
        panelCreacionSala.SetActive(false);
        panelUnirSala.SetActive(false);
        panelSala.SetActive(false);

        panelObjetivo.SetActive(true);
    }

    private void ActualizarPanelJugadores()
    {
        //Actualizaci�n del nombre de sala y su capacidad
        txtNombreSala.text = PhotonNetwork.CurrentRoom.Name;
        txtCantidadJugadores.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        //Eliminamos todos los botones para empezar desde cero en cada actualización
        while (contenedorJugadores.transform.childCount > 0)
        {
            DestroyImmediate(contenedorJugadores.transform.GetChild(0).gameObject);
        }


        foreach (Player jugador in PhotonNetwork.PlayerList)
        {
            //Instanciamos un nuevo boton y lo colgamos del contenedor
            GameObject nuevoElemento = Instantiate(elemJugador);
            nuevoElemento.transform.SetParent(contenedorJugadores.transform, false);
            //Localizamos sus etiquetas y las actualizamos
            nuevoElemento.transform.Find("txtNombreJugador").GetComponent<TextMeshProUGUI>().text = jugador.NickName;
            //Equipo del jugador            
            /*object equipoJugador = jugador.CustomProperties["equipo"];
            String equipo = "";
            switch ((int)equipoJugador)
            {
                case 0:
                    equipo = "Rojo";
                    break;
                case 1:
                    equipo = "Azul";
                    break;
                case 2:
                    equipo = "Verde";
                    break;
            }
            nuevoElemento.transform.Find("txtNumActor").GetComponent<TextMeshProUGUI>().text = avatar;
           */
        }


        //Activaci�n de bot�n Comenzar Juego si el n�mero m�nimo de jugadores est� en la sala
        if (PhotonNetwork.CurrentRoom.PlayerCount >= int.Parse(inputMinJug.text)
            && PhotonNetwork.IsMasterClient)
        {
            botonComenzarJuego.gameObject.SetActive(true);
        }
        else
        {
            botonComenzarJuego.gameObject.SetActive(false);
        }

    }


    #endregion
}