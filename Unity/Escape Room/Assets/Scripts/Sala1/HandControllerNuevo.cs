using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

public class HandControllerNuevo : MonoBehaviour
{
    
    [SerializeField] InputActionReference controllerActionGrip;
    [SerializeField] InputActionReference controllerActionTrigger;

    private Animator handAnimator;

    private void Awake()
    {
        controllerActionGrip.action.performed += GripPress;
        controllerActionTrigger.action.performed += TriggerPress;

        handAnimator = GetComponent<Animator>();
    }
    private void OnDestroy() {
        controllerActionGrip.action.performed -= GripPress;
        controllerActionTrigger.action.performed -= TriggerPress;
    }

    private void GripPress(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Grip", obj.ReadValue<float>());
    }

    private void TriggerPress(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Señalar", obj.ReadValue<float>());
    }


    
}
