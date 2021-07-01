using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.XR.Interaction.Toolkit;

public class CambiarPropietario : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Coger")) { 
            Debug.Log("Cambiarpropietario.cs: He colisionado con una mano");
            if (gameObject.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.LocalClientId) {
                ObtenerOwnershipServerRpc(other.gameObject.GetComponentInParent<NetworkObject>().OwnerClientId);
                Debug.Log("Cambiarpropietario.cs: Soy el propietario de la mano");
            }
        } else {
            Debug.Log("Cambiarpropietario.cs: He colisionado con algo que no es la mano de un jugador");
        }
    }
    

    [ServerRpc]
    void ObtenerOwnershipServerRpc(ulong clientId) {
        Debug.Log("Cambiarpropietario.cs: Le he dado el ownership al cliente: " + clientId);
        gameObject.GetComponent<NetworkObject>().ChangeOwnership(clientId);
    }
}
