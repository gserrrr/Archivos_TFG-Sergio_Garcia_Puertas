
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json.Linq;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Inicialización : NetworkBehaviour
{
    //private NetworkVariable<string> codigoArmarioCreado = new NetworkVariable<string>();
    string codigoArmarioCreado; //4 números
    [SerializeField] TMP_Text numeroMonitorIzq1; // primeros 2 números de codigoArmario
    [SerializeField] TMP_Text numeroMonitorIzq2; // últimos 2 números de codigoArmario
    [SerializeField] KeypadLogic codigoArmario; //atributo del objeto candado

    
    string codigoPendrive;
    [SerializeField] TMP_Text numeroMonitorDch1;
    [SerializeField] TMP_Text numeroMonitorDch2;
    [SerializeField] TMP_Text numeroMonitorDch3;
    [SerializeField] TMP_Text numeroMonitorDch4;

    [SerializeField] KeypadLogic codigoPuertaSala2; //código para el candado de la puerta para entrar a la segunda sala

    [SerializeField] TMP_Text numeroPlaneta;
    int cantPlanetas = 8;
    [SerializeField] GameObject prefabPlanetaDestruible;
    [SerializeField] GameObject prefabPlanetaNoDestruible;

    [SerializeField] GameObject basuras;
    [SerializeField] GameObject prefabOjo;

    [SerializeField] GameObject xrRig;
    [SerializeField] Camera cam;

    // Start is called before the first frame update
    public void IniciarSala()
    {
        if (!IsServer) { //los parámetros de inicialización los creará y avisará a los clientes para que éstos actualicen sus variables
            return;
        }
        /*
        cam.GetComponent<NetworkObject>().Despawn(true);
        //creamos a los Players y los asignamos a los jugadores
        for (int i = 0; i < NetworkManager.Singleton.ConnectedClientsList.Count; i++) {
            GameObject jugador = Instantiate(xrRig,null,true);
            jugador.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.ConnectedClientsList[i].ClientId);
        }
        */
        //-------------------------ORDENADORES IZQUIERDA & ARMARIO-------------------------
        //generamos el número aleatorio para el candado del armario
        for (int i = 0; i<4; i++){
            int num = UnityEngine.Random.Range(0, 10);
            codigoArmarioCreado += num.ToString();
        }

        //le asignamos este número tanto al codelock del armario, como a las pistas de los monitores
        codigoArmario.setCodigoBueno(codigoArmarioCreado);
        numeroMonitorIzq1.text = codigoArmarioCreado.Substring(0, 2);
        numeroMonitorIzq2.text = codigoArmarioCreado.Substring(2, 2);

        //-------------------------PENDRIVE & PUERTA SALA 2-------------------------
        //asignamos el elemento random que aparecerá en el monitor de la derecha
        for (int i = 0; i < 4; i++)
        {
            int num = UnityEngine.Random.Range(0, 10);
            codigoPendrive += num.ToString();
        }
        numeroMonitorDch1.text = codigoPendrive[0].ToString();
        numeroMonitorDch2.text = codigoPendrive[1].ToString();
        numeroMonitorDch3.text = codigoPendrive[2].ToString();
        numeroMonitorDch4.text = codigoPendrive[3].ToString();

        //asignamos el código que se obtiene a la puerta
        codigoPuertaSala2.setCodigoBueno(codigoPendrive);

        //-------------------------PIZARRA & LÁMPARAS-------------------------
        //añadimos el número random a la ecuación
        int r = UnityEngine.Random.Range(1, cantPlanetas + 1);
        string ecuacion = numeroPlaneta.text += r.ToString();
        
        SpawnLamparasClientRpc(r);
        

        //-------------------------PAPELERAS-------------------------
        //de todas las papeleras que hay, cogemos una random y spawneamos ahí el ojo
        r = UnityEngine.Random.Range(0, basuras.transform.childCount - 1);
        GameObject hijo = basuras.transform.GetChild(r).gameObject;
        GameObject g = Instantiate(prefabOjo,hijo.transform.GetChild(1).position, hijo.transform.GetChild(1).rotation);
        //g.AddComponent<NetworkObject>();
        g.GetComponent<NetworkObject>().Spawn();
        SincronizarClientRpc(codigoArmarioCreado, codigoPendrive, ecuacion);
    }
    
    [ClientRpc]
    void SpawnLamparasClientRpc(int n) {
        //leemos el JSON con las 8 ubicaciones de las lámparas
        string jsonStringPath = File.ReadAllText("Assets/JSON/lamparas.json");
        JObject lamparas = JObject.Parse(jsonStringPath);
        Debug.Log("Planeta nº: " + n);

        for (int i = 0; i < cantPlanetas; i++) {
            GameObject planeta;
            if ((i + 1) == n)//es el planeta que se tiene que poder destruir
            {
                planeta = Instantiate(prefabPlanetaDestruible, new Vector3((float)lamparas["lamparas"][i]["position"]["x"], (float)lamparas["lamparas"][i]["position"]["y"], (float)lamparas["lamparas"][i]["position"]["z"]), transform.rotation);
                planeta.transform.localScale = new Vector3((float)lamparas["lamparas"][i]["scale"]["x"], (float)lamparas["lamparas"][i]["scale"]["y"], (float)lamparas["lamparas"][i]["scale"]["z"]);
            } else //el planeta no se tiene que poder destruir
                {
                planeta = Instantiate(prefabPlanetaNoDestruible, new Vector3((float)lamparas["lamparas"][i]["position"]["x"], (float)lamparas["lamparas"][i]["position"]["y"], (float)lamparas["lamparas"][i]["position"]["z"]), transform.rotation);
                planeta.transform.localScale = new Vector3((float)lamparas["lamparas"][i]["scale"]["x"], (float)lamparas["lamparas"][i]["scale"]["y"], (float)lamparas["lamparas"][i]["scale"]["z"]);
            }
            planeta.GetComponent<TexturaPlanetas>().ponerTexturas(i);
            //planeta.AddComponent<NetworkObject>();
            //planeta.GetComponent<NetworkObject>().Spawn();
            //planeta.GetComponent<TexturaPlanetas>().ponerTexturas(i);
        }
    }
    

    [ClientRpc]
    private void SincronizarClientRpc(string codigoArmarioLocal, string codigoPen, string ecuacion) {
        
        //le asignamos este número tanto al codelock del armario, como a las pistas de los monitores
        codigoArmario.setCodigoBueno(codigoArmarioLocal);
        numeroMonitorIzq1.text = codigoArmarioLocal.Substring(0, 2);
        numeroMonitorIzq2.text = codigoArmarioLocal.Substring(2, 2);

        //asignamos los números de la imagen que contiene el pendrive
        numeroMonitorDch1.text = codigoPen[0].ToString();
        numeroMonitorDch2.text = codigoPen[1].ToString();
        numeroMonitorDch3.text = codigoPen[2].ToString();
        numeroMonitorDch4.text = codigoPen[3].ToString();
        //asignamos el código que se obtiene a la puerta
        codigoPuertaSala2.setCodigoBueno(codigoPen);

        numeroPlaneta.text = ecuacion;

    }
    
}


