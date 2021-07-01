using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PuertaArmario : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable sePuedeAbrir;
    [SerializeField] LayerMask mascaraEverything;

    public void abrirPuerta()
    {
        sePuedeAbrir.interactionLayerMask = mascaraEverything;
    }

}
