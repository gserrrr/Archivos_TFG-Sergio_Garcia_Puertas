using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IniciarSesion : MonoBehaviour
{
    [SerializeField] InputField campoUsuario;
    [SerializeField] InputField campoContrase�a;

    public Button botonIniciarSesion;
    //[SerializeField] UnityEvent sesionInciciada;

    //[SerializeField] TMP_Text nombreUsuario;

    [SerializeField] string menu;
    string loginURL = "https://alumnes-ltim.uib.es/escaperoom/login.php";

    public void CallIniciarSesion() {
        StartCoroutine(Login());
    }

    IEnumerator Login() {
        WWWForm form = new WWWForm();
        form.AddField("usuario", campoUsuario.text);
        form.AddField("contrase�a", campoContrase�a.text);
        UnityWebRequest www = UnityWebRequest.Post(loginURL, form);
        yield return www.SendWebRequest();

        if (www.downloadHandler.text.StartsWith("ERROR")) {
            Debug.Log(www.downloadHandler.text);
        } else {
            Debug.Log("Has iniciado sesi�n correctamente correctamente");
            DBManager.loggedIn = true;
            DBManager.usuario = campoUsuario.text;
            DBManager.id = www.downloadHandler.text;
            //nombreUsuario.text = DBManager.usuario;
            //sesionInciciada.Invoke();
            SceneManager.LoadScene(menu);
        }
    }
}
