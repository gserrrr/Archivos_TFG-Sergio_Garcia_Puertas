using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float startTime;
    public float tiempoActual;

    public void EmpezarConteo() {
        startTime = Time.time;
    }

    void Update() {
        tiempoActual = Time.time - startTime;
    }
}
