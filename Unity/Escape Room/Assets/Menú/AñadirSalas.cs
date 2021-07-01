using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AñadirSalas : MonoBehaviour
{
    [SerializeField] GameObject panel;
    string apiURL = "https://alumnes-ltim.uib.es/escaperoom/api.php";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Añadir());
    }

    

    IEnumerator Añadir() {
        
        UnityWebRequest www = UnityWebRequest.Get(apiURL + "?opcion=1&usuario=" + DBManager.usuario);
        yield return www.SendWebRequest();

        if (www.downloadHandler.text.StartsWith("ERROR")) {
            Debug.Log("7: consulta de los mejores tiempos fallida");

        } else {
            JObject salas = JObject.Parse(www.downloadHandler.text);
            for (int i = 0; i < (int)salas["cantSalas"]; i++) {
                GameObject g = Instantiate(panel);
                g.transform.SetParent(gameObject.transform,false);
                g.GetComponent<CrearPanel>().Crear((JObject)salas["partidas"][i]);
            }
        }
    }
}
