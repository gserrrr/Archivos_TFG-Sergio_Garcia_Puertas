using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Registrar : MonoBehaviour{
    [SerializeField] InputField campoUsuario;
    [SerializeField] InputField campoContrase�a;

    public Button botonRegistrarse;
    string registerURL = "https://alumnes-ltim.uib.es/escaperoom/register.php";

    public void CallRegistrar(){
        StartCoroutine(Registrarse());
    }

    IEnumerator Registrarse(){
        WWWForm form = new WWWForm();
        form.AddField("usuario", campoUsuario.text);
        form.AddField("contrase�a", campoContrase�a.text);
        UnityWebRequest www = UnityWebRequest.Post(registerURL, form);
        yield return www.SendWebRequest();

        if (www.downloadHandler.text != "0") {
            Debug.Log(www.downloadHandler.text);
        } else {
            Debug.Log("Te has registrado correctamente");
        }
    }

    /*public void ComprobarInputs() {
        botonRegistrarse.interactable = (campoUsuario.text.Length <= 15 && campoContrase�a.text.Length <= 4);
    }*/
}
