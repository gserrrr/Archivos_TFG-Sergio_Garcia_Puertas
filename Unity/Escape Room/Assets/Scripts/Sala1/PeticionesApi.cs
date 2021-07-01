using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MLAPI;
using MLAPI.Messaging;
using TMPro;
using UnityEngine.SceneManagement;

public class PeticionesApi : NetworkBehaviour
{
    public List<Prueba> tiempos = new List<Prueba>();
    public List<string> jugadores = new List<string>();
    [SerializeField] int idEscapeRoom;
    string apiURL = "https://alumnes-ltim.uib.es/escaperoom/api.php";
    [SerializeField] GameObject canvasFinal;
    [SerializeField] TMP_Text mensajeFinal;
    [SerializeField] ControlConexion controlConexion;
    /*
    private void Start() {
        if (IsClient) {
            AñadirUsuarioServerRpc(DBManager.usuario);
        }
    }
    public void AñadirHost() {
        jugadores.Add(DBManager.usuario);
    }

    [ServerRpc]
    void AñadirUsuarioServerRpc(string user) {
        jugadores.Add(user);
    }*/


    public void GuardarDatos() {
        //llamar a la api para crear una partida nueva y que nos devuelva el id
        //para ese id de partida crear todos los tiempos
        //crear todas las instancia de la relación partida_usuario
        if (IsHost) {
            StartCoroutine(Guardar());
            MensajeAlAcabarClientRpc(tiempos[tiempos.Count - 1].tiempo);
        }
        
    }

    IEnumerator Guardar() {
        //Consulta #2 -> añadir una nueva partida y devolver el ID
        WWWForm form = new WWWForm();
        form.AddField("opcion", 2); 
        form.AddField("idEscRoom", idEscapeRoom);
        UnityWebRequest www = UnityWebRequest.Post(apiURL, form);
        yield return www.SendWebRequest();

        string idPartida = www.downloadHandler.text;
        Debug.Log("idPartida: " + idPartida);

        //Consulta #3 -> añadir un nuevo tiempo a una partida
        for (int i = 0; i < tiempos.Count; i++) { 
            form = new WWWForm();
            form.AddField("opcion", 3);
            form.AddField("idPartida", idPartida);
            form.AddField("idPrueba", tiempos[i].num);
            form.AddField("tiempo", tiempos[i].tiempo.ToString().Replace(',','.'));
            Debug.Log(tiempos[i].num + " - " + tiempos[i].tiempo.ToString().Replace(',', '.'));
            StartCoroutine(PonerTiempos(form));
        }

        //Consulta #4 -> las relaciones entre los jugadores y la partida
        for (int i = 0; i < jugadores.Count; i++) {
            form = new WWWForm();
            form.AddField("opcion", 4);
            form.AddField("usuario", jugadores[i]);
            form.AddField("idPartida", idPartida);
            StartCoroutine(PonerUsuarioPartida(form));
        }

        //Le decimos a todos los usuario que envíen su json de posiciones
        PosicionesJugadoresClientRpc(idPartida);
    }

    IEnumerator PonerTiempos(WWWForm form) {
        UnityWebRequest request = UnityWebRequest.Post(apiURL, form);
        yield return request.SendWebRequest();
    }

    IEnumerator PonerUsuarioPartida(WWWForm form) {
        UnityWebRequest request = UnityWebRequest.Post(apiURL, form);
        yield return request.SendWebRequest();
        Debug.Log(request.downloadHandler.text);
    }

    /*
    //Para asegurar de que solo hay un objeto PeticionesApi
    private static PeticionesApi _instance;
    public static PeticionesApi Instance { get { return _instance; } }
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }*/


    public class Prueba {

        public int num;
        public float tiempo;
        public Prueba(int n, float t) {
            this.num = n;
            this.tiempo = t;
        }
    }

    [ClientRpc]
    void MensajeAlAcabarClientRpc(float t) {
        string tiempo = (t / 3600).ToString("00") + ":" + Mathf.Floor((t / 60)) .ToString("00") + ":" + (t % 60).ToString("00");
        //string tiempo = (t / 3600).ToString("00") + ":" + ((t % 3600) / 60).ToString("00") + ":" + ((t % 3600) % 60).ToString("00");
        mensajeFinal.text = tiempo;
        canvasFinal.SetActive(true);
        StartCoroutine(VolverMenu());
    }

    IEnumerator VolverMenu() {
        yield return new WaitForSeconds(5);
        controlConexion.Salir();
    }

    [ClientRpc]
    void PosicionesJugadoresClientRpc(string idPartida) {
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Jugador");
        for (int i=0; i < jugadores.Length; i++) {
            if (jugadores[i].GetComponent<NetworkObject>().IsLocalPlayer) {
                jugadores[i].GetComponent<Posicion_JSON>().EnviarJson(idPartida);
            }
        }
    }
}

