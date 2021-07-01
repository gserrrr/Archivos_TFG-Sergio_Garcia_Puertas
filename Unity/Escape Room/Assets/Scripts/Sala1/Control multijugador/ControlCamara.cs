using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class ControlCamara : NetworkBehaviour {
    public Camera cam;
    [SerializeField] float xP,yP,zP, xR, yR, zR;
    void Start() {

        if (!IsOwner) {
            cam.enabled = false;
            cam.GetComponent<AudioListener>().enabled = false;
            return;
        } else {
            transform.position = new Vector3(xP, yP, zP);
            transform.rotation = Quaternion.Euler(xR, yR, zR);
            
            AñadirJugadorServerRpc(DBManager.id);
            
        }
        
    }

    [ServerRpc]
    void AñadirJugadorServerRpc(string idUsuario) {
        GameObject.Find("GestorTiempos").GetComponent<PeticionesApi>().jugadores.Add(idUsuario);
    }
}
