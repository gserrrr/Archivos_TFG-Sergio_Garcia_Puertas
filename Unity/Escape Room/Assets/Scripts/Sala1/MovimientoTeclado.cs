using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

public class MovimientoTeclado : NetworkBehaviour
{
    [SerializeField] InputAction inputs;
    [SerializeField] float velocidad;
    // Start is called before the first frame update


    void Start()
    {
        inputs.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Vector2 mov = inputs.ReadValue<Vector2>();
        //Debug.Log(mov.x + ", " + mov.y);
        gameObject.transform.position += transform.TransformDirection(mov.x * velocidad * Time.deltaTime,0, mov.y * velocidad * Time.deltaTime);
    }
}
