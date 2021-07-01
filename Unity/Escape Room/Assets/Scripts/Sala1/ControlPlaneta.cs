using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class ControlPlaneta : NetworkBehaviour
{
    private List<GameObject> partes;
    [SerializeField] GameObject parpado;
    [SerializeField] GameObject corteza;
    [SerializeField] Transform posicionSpawnParpado;
    [SerializeField] int idPruebaCompletada;
    Timer temporizador;
    PeticionesApi peticiones;
    bool parpadoInstanciado = false;
    
    
    private void Start() {
        peticiones = GameObject.FindWithTag("Api").GetComponent<PeticionesApi>();
        temporizador = GameObject.FindWithTag("Timer").GetComponent<Timer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("PunteroPizarra")) {
            return;
        }
        //Destroy(corteza);
        corteza.SetActive(false);
        //Este script únicamente lo tendrá la lámpara con la que se pueda interactuar
        //Separar los objetos hijos
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject hijo = gameObject.transform.GetChild(i).gameObject;
            hijo.transform.parent = transform.parent;
            hijo.GetComponent<Rigidbody>().useGravity = true;
        }
        //g.AddComponent<NetworkObject>();
        if (IsServer) {
            RomperPlaneta();
        }
        
        
        //Destroy(posicionSpawnParpado.gameObject);
        posicionSpawnParpado.gameObject.SetActive(false);
        //Destroy(gameObject);
        gameObject.SetActive(false);
        PeticionesApi.Prueba pruebaCompletada = new PeticionesApi.Prueba(idPruebaCompletada, temporizador.tiempoActual);
        peticiones.tiempos.Add(pruebaCompletada);
    }


    void RomperPlaneta() {
        Debug.Log("Pre - Instancio parpado");
        if (!parpadoInstanciado) {
            parpadoInstanciado = true;
            
            GameObject g = Instantiate(parpado, posicionSpawnParpado.position, posicionSpawnParpado.rotation);
            g.GetComponent<NetworkObject>().Spawn();
        }
        Debug.Log("Post - Instancio parpado");
        
    }

}
