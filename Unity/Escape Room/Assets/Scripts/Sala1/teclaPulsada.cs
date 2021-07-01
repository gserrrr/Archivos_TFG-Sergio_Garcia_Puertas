using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class teclaPulsada : MonoBehaviour
{
    public UnityEvent pulsada;
    bool siendoPulsada = false;
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Dedo") & !siendoPulsada){
            pulsada.Invoke();
            siendoPulsada = true;
            Debug.Log("he pulsado una tecla");
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Dedo"))
        {
            siendoPulsada = false;
        }
    }
}
