using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using System.Text;
using UnityEngine.UI;
using MLAPI.Transports.UNET;

public class ControlMultijugador : MonoBehaviour
{
    [SerializeField] InputField contraseñaInput;
    [SerializeField] InputField ip;
    [SerializeField] InputField puerto;
    [SerializeField] GameObject networkM;


    public void Cliente() {
        networkM.GetComponent<UNetTransport>().ConnectAddress = ip.text;
        networkM.GetComponent<UNetTransport>().ConnectPort = int.Parse(puerto.text);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(contraseñaInput.text);
        NetworkManager.Singleton.StartClient();
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback) {
        string contraseña = Encoding.ASCII.GetString(connectionData);
        bool conexionAprobada = contraseña == contraseñaInput.text;
        //ahora mismo todos spawnean en el mismo sitio, pero no debería haber problema porque no se chocan entre ellos ni nada
        callback(true, null, conexionAprobada, new Vector3(0, 0, 0), new Quaternion());
    }
}
