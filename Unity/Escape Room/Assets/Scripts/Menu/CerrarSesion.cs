using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CerrarSesion : MonoBehaviour
{
    public void Logout() {
        DBManager.loggedIn = false;
        DBManager.usuario = "";
        DBManager.id = "";
        SceneManager.LoadScene("Iniciar Sesión");
    }
}
