using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturaPlanetas : MonoBehaviour
{
    [SerializeField] List<GameObject> partesPlanetas;
    [SerializeField] List<Material> materiales;
    //string pathTexturas = "Assets/Materiales/Planetas";
    // Start is called before the first frame update
    public void ponerTexturas(int numPlaneta)
    {
        for (int i = 0; i<partesPlanetas.Count; i++)
        {
            //partesPlanetas[i].GetComponent<Renderer>().material = Resources.Load("Assets/Materiales/Planetas"+"planeta"+(i+1)+".mat",typeof(Material)) as Material;
            //partesPlanetas[i].GetComponent<MeshRenderer>().material = Resources.Load("Assets/Materiales/Planetas" + "/planeta" + (numPlaneta), typeof(Material)) as Material;
            Debug.Log(materiales.Count);
            partesPlanetas[i].GetComponent<MeshRenderer>().material = materiales[numPlaneta];
        }
    }

}
