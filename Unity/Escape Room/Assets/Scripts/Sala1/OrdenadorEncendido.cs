using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrdenadorEncendido : MonoBehaviour
{
    bool estaEncendido = false;
    //[SerializeField] GameObject imagenMonitor;
    [Space]
    [SerializeField] UnityEvent seEnciende;
    [SerializeField] UnityEvent seApaga;
    bool siendoPulsada = false;
    

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Dedo") & !siendoPulsada){
            siendoPulsada = true;
            if (!estaEncendido){
                estaEncendido = true;
                seEnciende.Invoke();
            }
            else{
                estaEncendido = false;
                seApaga.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dedo")){
            siendoPulsada = false;
        }
    }
}
