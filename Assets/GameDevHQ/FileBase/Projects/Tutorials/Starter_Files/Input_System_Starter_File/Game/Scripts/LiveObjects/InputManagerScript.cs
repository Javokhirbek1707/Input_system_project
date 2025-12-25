using Game.Scripts.LiveObjects;
using Game.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerScript : MonoBehaviour
{
    [SerializeField]
    private PlayerAction _input;
    [SerializeField]
    private Drone _drone;
    [SerializeField]
    private Forklift _forklift;

    void Start()
    {
        _input = new PlayerAction();
    }


    void Update()
    {
        //Drone tilt
        var tilt = _input.Drone.Move.ReadValue<Vector2>();
        _drone.CalculateTilt(tilt);

        //Drone rotation
        var rotInput = _input.Drone.Rotation.ReadValue<float>();
        _drone.CalculateMovementUpdate(rotInput);

        //Forklift movement
        var forkliftMove = _input.Forklift.Move.ReadValue<Vector2>();

        //forkfilt method here
        _forklift.CalcutateMovement(forkliftMove);
    }


    public void InitializeDroneInput()
    {
        //This method is called from within the Drone script when flight is enable 
        _input.Player.Disable();
        _input.Drone.Enable();
    }

    public void DisableDroneControls()
    {
        //This method is called from within the Drone script when flight is enable 
        _input.Drone.Disable();
        _input.Player.Enable();
    }

    private void FixedUpdate()
    {
        //Drone up and down
        var direction = _input.Drone.Vertical.ReadValue<float>();
        if(direction != 0)
            _drone.CalculateMovementFixedUpdate(-direction);//-direction to invert dir

        //Forklift lift using 
        var liftInput = _input.Forklift.LiftInput.ReadValue<float>();
        if (liftInput != 0)
        {
            _forklift.LiftRoutine(liftInput);
            Debug.Log(liftInput);
        }
    }

    public void InitializeForkliftInput()
    {
        _input.Player.Disable();
        _input.Forklift.Enable();

        _input.Forklift.Exit.performed += Exit_performed;
    }

    public void Exit_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("Exit Forklift");
        DisableDroneControls();
        _forklift.ExitDriveMode();
    }

    public void DisableForkliftControls()
    {
        _input.Player.Enable();
        _input.Forklift.Disable();
    }
}
