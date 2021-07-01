using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrearPanel : MonoBehaviour
{
    [SerializeField] TMP_Text titulo, dificultad, mejorTiempo, numIntentos;
    [SerializeField] Button host;
    
    public void Crear(JObject datosPartida) {
        titulo.text = datosPartida["escaperoom_nombre"].ToString();
        dificultad.text = datosPartida["escaperoom_dificultad"].ToString();
        if (datosPartida["num_intentos"].ToString() != "") { //ha jugado mínimo una partida y tenemos los datos
            mejorTiempo.text = datosPartida["mejor_tiempo"].ToString();
            numIntentos.text = datosPartida["num_intentos"].ToString();
        } else {
            mejorTiempo.text = "-";
            numIntentos.text = "0";
        }
        
        host.onClick.AddListener(CambiarEscena); 
    }

    void CambiarEscena() {
        SceneManager.LoadScene(titulo.text);
    }

}
