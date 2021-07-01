using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KeypadLogic : MonoBehaviour
{
    [SerializeField] TMP_Text codigoActual;
    [SerializeField] string codigoBueno;
    [SerializeField] UnityEvent abrirPuerta;
    bool completada = false;
    [SerializeField] int idPruebaCompletada;
    Timer temporizador;
    PeticionesApi peticiones;

    private void Start() {
        peticiones = GameObject.FindWithTag("Api").GetComponent<PeticionesApi>();
        temporizador = GameObject.FindWithTag("Timer").GetComponent<Timer>();
    }

    public void botonNumericoPulsado(int num){
        codigoActual.text = codigoActual.text.Substring(1) + num.ToString();
    }

    public void borrarUltimo()
    {
        codigoActual.text = "-" + codigoActual.text.Substring(0, 3);
    }

    public void comprobarNumero()
    {
        if (codigoActual.text.Equals(codigoBueno) && !completada)
        {
            abrirPuerta.Invoke();
            completada = true;
            PeticionesApi.Prueba pruebaCompletada = new PeticionesApi.Prueba(idPruebaCompletada, temporizador.tiempoActual);
            peticiones.tiempos.Add(pruebaCompletada);
            //sonido de candado abierto
        }
    }

    public void setCodigoBueno(string codigo)
    {
        codigoBueno = codigo;
    }
}
