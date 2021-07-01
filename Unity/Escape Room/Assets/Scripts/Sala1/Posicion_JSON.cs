using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.Networking;
using System;

public class Posicion_JSON : NetworkBehaviour {
    string jsonPosicion = "[";
    [SerializeField] Camera cam;
    string urlApiAnalisis = "https://alumnes-ltim.uib.es/escaperoom/apiAnalisis.php";
    void Start() {

        if (IsOwner) {
            StartCoroutine(GuardarPosicion());
        }

    }

    IEnumerator GuardarPosicion() {
        while (true) {
            jsonPosicion += "{\"x\":" + (Math.Truncate(10 * cam.transform.position.x) / 10).ToString().Replace(',', '.') + ",\"y\":" + (Math.Truncate(10 * cam.transform.position.z) /10).ToString().Replace(',', '.') + "},";
            yield return new WaitForSeconds((float)0.25);
        }
    }

   
    public void EnviarJson(string idPartida) {
        //Arreglamos el json
        StopCoroutine(GuardarPosicion());
        jsonPosicion = jsonPosicion.Substring(0, jsonPosicion.Length - 1);
        jsonPosicion += "]";

        
        //Enviamos el formulario
        WWWForm formulario = new WWWForm();
        formulario.AddField("opcion", 1);
        formulario.AddField("idPartida", idPartida);
        formulario.AddField("idUsuario", DBManager.id);
        formulario.AddField("tipoDatos", 1); //tipoDatos -> posición
        formulario.AddField("datos", jsonPosicion);
        Debug.Log(jsonPosicion);
        StartCoroutine(Enviar(formulario));
    }

    IEnumerator Enviar(WWWForm f) {
        UnityWebRequest www = UnityWebRequest.Post(urlApiAnalisis, f);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
    }
}
