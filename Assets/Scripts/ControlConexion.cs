using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlConexion : MonoBehaviourPunCallbacks
{
    #region Constantes
    //constantes equipos
    public const int SIN_EQUIPO = 0;
    public const int ROJO = 1;
    public const int AZUL = 2;
    #endregion
    #region Variables privadas
    #endregion

    #region Variables publicas
    public GameObject panelInicio;
    public GameObject panelBienvenida;
    public GameObject panelCreacionSala;
    public GameObject panelUnirSala;
    public GameObject panelSala;
    public GameObject panelSeleccionEquipo;
    /*public GameObject btnConectar;*/
    public Button btnConectar;
    public TextMeshProUGUI txtEstado, txtInfoUser, txtCantidadJugadores, txtNombreSala, txtIsOpen;
    public TMP_InputField inputNickname, inputNombreSala, inputMaxJug, inputMinJug, inputUnirSala;

    public Button botonComenzarJuego; //Instancia del boton Conectar

    public GameObject elemJugador; //Cada uno de los botones que representa un jugador en la lista de sala
    public GameObject contenedorJugadores; //Contenedor que mantiene la lista de jugadores
    #endregion


    private void Start()
    {
        CambiarPanel(panelInicio);
        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(this.gameObject);
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
    /// <summary>
    /// Asigna al local player un valor en un id "equipo"
    /// de la tabla hash, con el equipo seleccionado
    /// </summary>
    public void OnClickEquipoRojo()
    {
        SeleccionEquipo(ROJO);
    }
    public void OnClickEquipoAzul()
    {
        SeleccionEquipo(AZUL);
    }
    public void SeleccionEquipo(int equipo)
    {
        playerProperties["equipo"] = equipo;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        CambiarEstado("Equipo seleccionado" + (equipo==ROJO ? "rojo":"azul"));
        CambiarPanel(panelBienvenida);
    }
    public void OnClickSeleccionarEquipo()
    {
        CambiarPanel(panelSeleccionEquipo);

        /*btnConectar.interactable = true;*/
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
        if (PhotonNetwork.CurrentRoom != null)
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
                /*opcionesSala.CustomRoomProperties(min, max);*/
                /*opcionesSala.IsOpen = false;*/

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
        if (!string.IsNullOrEmpty(inputUnirSala.text))
        {
            PhotonNetwork.JoinRoom(inputUnirSala.text);
        }
        else
        {
            
            CambiarEstado("Introduzca un nombre correcto para la sala" + inputUnirSala.text);
        }
    }

    public void OnClickComenzarPartida()
    {
        SceneManager.LoadScene("GAME");
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

        playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties.Add("equipo", SIN_EQUIPO);//Valor 0 indica Sin Equipo
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        CambiarEstado("Desconectado por:" + cause.ToString());
        CambiarPanel(panelInicio);
        btnConectar.interactable = true;
        txtInfoUser.text = "No user";
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
        Debug.Log("hi!");
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
    /// 

    
    private void CambiarEstado(string texto)
    {
        txtEstado.text = texto;
    }


    private void CambiarPanel(GameObject panelObjetivo)
    {
        panelBienvenida.SetActive(false);
        panelInicio.SetActive(false);
        panelCreacionSala.SetActive(false);
        panelUnirSala.SetActive(false);
        panelSala.SetActive(false);
        panelSeleccionEquipo.SetActive(false);

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
            object equipoJugador = jugador.CustomProperties["equipo"];
            string equipo = "";
            switch ((int)equipoJugador)
            {
                case SIN_EQUIPO:
                    equipo = "Sin equipo";
                    break;
                case ROJO:
                    equipo = "Rojo";
                    break;
                case AZUL:
                    equipo = "Azul";
                    break;
            }
            nuevoElemento.transform.Find("txtEquipo").GetComponent<TextMeshProUGUI>().text = equipo;

        }


        //Activaci�n de bot�n Comenzar Juego si el n�mero m�nimo de jugadores est� en la sala
        if (PhotonNetwork.CurrentRoom.PlayerCount >= int.Parse(inputMinJug.text) && PhotonNetwork.IsMasterClient)
        {
            botonComenzarJuego.gameObject.SetActive(true);
        }
        else
        {
            botonComenzarJuego.gameObject.SetActive(false);
        }

    }

    ExitGames.Client.Photon.Hashtable playerProperties;

    #endregion
}