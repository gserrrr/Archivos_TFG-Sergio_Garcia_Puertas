using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CandadoFinal : MonoBehaviour
{
    [SerializeField] private bool trianguloPuesto = false;
    [SerializeField] private bool parpadoPuesto = false;
    [SerializeField] private bool ojoPuesto = false;
    [SerializeField] private XRGrabInteractable sePuedeAbrir;
    [SerializeField] LayerMask mascaraEverything;
    [SerializeField] Rigidbody rbPuerta;
    bool completada = false;
    [SerializeField] int idPruebaCompletada;
    Timer temporizador;
    PeticionesApi peticiones;

    private void Start() {
        peticiones = GameObject.FindWithTag("Api").GetComponent<PeticionesApi>();
        temporizador = GameObject.FindWithTag("Timer").GetComponent<Timer>();
    }

    public bool ParpadoPuesto { get => parpadoPuesto; set => parpadoPuesto = value; }
    public bool TrianguloPuesto { get => trianguloPuesto; set => trianguloPuesto = value; }
    public bool OjoPuesto { get => ojoPuesto; set => ojoPuesto = value; }

    public void comprobarCompletado()
    {
        if (trianguloPuesto && parpadoPuesto && ojoPuesto && !completada)
        {
            //rbPuerta.isKinematic = false;
            sePuedeAbrir.interactionLayerMask = mascaraEverything;
            completada = true;
            PeticionesApi.Prueba pruebaCompletada = new PeticionesApi.Prueba(idPruebaCompletada, temporizador.tiempoActual);
            peticiones.tiempos.Add(pruebaCompletada);
            peticiones.GuardarDatos();
            //sonido de abierto
        }
    }
}
