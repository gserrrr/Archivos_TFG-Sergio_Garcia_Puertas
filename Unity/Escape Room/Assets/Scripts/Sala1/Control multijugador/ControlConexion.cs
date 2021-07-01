using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MLAPI;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;

public class ControlConexion : MonoBehaviour
{
    [SerializeField] InputField contraseñaInput;
    [SerializeField] TMP_Text error;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject botonSalir;
    [SerializeField] GameObject botonStart;
    [SerializeField] TMP_Text tuIp;

    private void Start() {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        if (NetworkManager.Singleton.IsClient) {
            panel.SetActive(false);
        }
    }

    private void OnDestroy() {
        if (NetworkManager.Singleton == null) { //para evitar errores cuando cierras el juego sin haberte desconectado
            return;
        }

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    public void Host() {
        tuIp.text += LocalIPAddress();
        tuIp.gameObject.SetActive(true);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(new Vector3(0, 0, 0), new Quaternion(), true);
    }
    
    public string LocalIPAddress() {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                localIP = ip.ToString();
                break;
            }
        }
        Debug.Log(localIP);
        return localIP;
    }

    public void Salir() {
        if (NetworkManager.Singleton.IsHost) {
            NetworkManager.Singleton.StopHost();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }else if (NetworkManager.Singleton.IsClient) {
            NetworkManager.Singleton.StopClient();
        }
        SceneManager.LoadScene("Menú");

    }

    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback) {
        string contraseña = Encoding.ASCII.GetString(connectionData);
        bool conexionAprobada = contraseña == contraseñaInput.text;
        callback(true, null, conexionAprobada, new Vector3(0,0,0), new Quaternion());
    }

    //Este método se llama en el server (cada vez que alguien se une) 
    //y también lo llama el cliente que se conecta, pero no el resto de clientes
    private void HandleClientConnected(ulong clientId) {
        Debug.Log("Ha entrado un usuario nuevo");
        if (clientId == NetworkManager.Singleton.LocalClientId) { //si el cliente que se quiere unir somos nosotros
            panel.SetActive(false);
            botonSalir.SetActive(true);
        }
    }

    private void HandleClientDisconnect(ulong clientId) {
        if (clientId == NetworkManager.Singleton.LocalClientId) { //si el cliente que se quiere desconectar somos nosotros
            SceneManager.LoadScene("Menú");
        }
    }

    //Esto hace falta porque si somos el host no se lactiva el OnClientConnectedCallback()
    //Por tanto si el que se ha conectado es el host, queremos que también se ejecute lo otro ya que
    //el host también juega (como los clientes normales)
    private void HandleServerStarted() {
        if (NetworkManager.Singleton.IsHost) {
            HandleClientConnected(NetworkManager.Singleton.LocalClientId);
        }
    }
}
