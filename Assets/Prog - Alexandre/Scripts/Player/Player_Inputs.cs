using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player_Inputs : MonoBehaviour
{
    [Header("Debug")]
    public bool jumpPressed = false;
    public bool attackPressed = false;
    public bool interactionPressed = false;
    public bool throwPressed = false;
    public float movement = 0f;
    public float menu = 0f;
    [SerializeField] private List<float[]> rumbleList = new List<float[]>();

    public static Player_Inputs instance;

    private void Awake()
    {
        if ( instance == null ) { instance = this; }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        Rumble();
    }

    void Rumble()
    {
        if (Gamepad.current == null) { return; }
        if (rumbleList.Count == 0) { Gamepad.current.SetMotorSpeeds(0, 0); return; }
        while (rumbleList[0][2] < Time.realtimeSinceStartup)
        {
            rumbleList.RemoveAt(0);
            if(rumbleList.Count == 0 ) { return; }
        }
        if (rumbleList.Count > 0)
        {
            Gamepad.current.SetMotorSpeeds(rumbleList[0][0], rumbleList[0][1]);
        }
    }

    public void AddRumble(Vector2 rumble, float time)
    {
        float[] list = { rumble.x, rumble.y, time + Time.realtimeSinceStartup };
        rumbleList.Add(list);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<float>();
    }

    public void Menu(InputAction.CallbackContext context)
    {
        menu = context.ReadValue<float>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        attackPressed = context.ReadValue<float>() > 0 ? true : false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValue<float>() > 0 ? true : false;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        interactionPressed = context.ReadValue<float>() > 0 ? true : false;
    }
    
    public void Throw(InputAction.CallbackContext context)
    {
        throwPressed = context.ReadValue<float>() > 0 ? true : false;
    }
}