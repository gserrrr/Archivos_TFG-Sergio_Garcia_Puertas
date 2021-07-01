using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using MLAPI.Messaging;
using MLAPI;

public class PuertaPrincipal : NetworkBehaviour
{
    [SerializeField] private XRGrabInteractable sePuedeAbrir;
    [SerializeField] LayerMask mascaraEverything;

    [ServerRpc]
    public void AbrirPrimeraPuertaServerRpc() {
        AbrirPrimeraPuertaClientRpc();
    }

    [ClientRpc]
    void AbrirPrimeraPuertaClientRpc() {
        sePuedeAbrir.interactionLayerMask = mascaraEverything;
    }
}
