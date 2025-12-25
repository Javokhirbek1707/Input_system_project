using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Game.Scripts.UI;

namespace Game.Scripts.LiveObjects
{
    public class Drone : MonoBehaviour
    {
        private enum Tilt
        {
            NoTilt, Forward, Back, Left, Right
        }

        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _speed = 5f;
        private bool _inFlightMode = false;
        [SerializeField]
        private Animator _propAnim;
        [SerializeField]
        private CinemachineVirtualCamera _droneCam;
        [SerializeField]
        private InteractableZone _interactableZone;

        private InputManagerScript _inputManager;

        public static event Action OnEnterFlightMode;
        public static event Action onExitFlightmode;

        private void Start()
        {
            _inputManager = GameObject.Find("InputManager").GetComponent<InputManagerScript>();
        }
        private void OnEnable()
        {
            InteractableZone.onZoneInteractionComplete += EnterFlightMode;
        }

        private void EnterFlightMode(InteractableZone zone)
        {
            if (_inFlightMode != true && zone.GetZoneID() == 4) // drone Scene
            {
                _propAnim.SetTrigger("StartProps");
                _droneCam.Priority = 11;
                _inFlightMode = true;
                OnEnterFlightMode?.Invoke();
                UIManager.Instance.DroneView(true);
                _interactableZone.CompleteTask(4);
                _inputManager.InitializeDroneInput();//Enable drone controls
            }
        }

        private void ExitFlightMode()
        {  
            _inputManager.DisableDroneControls();//Disable drone controls
            _droneCam.Priority = 9;
            _inFlightMode = false;
            UIManager.Instance.DroneView(false);
        }

        private void Update()
        {
            if (_inFlightMode)
            {
                //CalculateTilt();
                //CalculateMovementUpdate();

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _inFlightMode = false;
                    onExitFlightmode?.Invoke();
                    ExitFlightMode();
                }
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(transform.up * (9.81f), ForceMode.Acceleration);
            if (_inFlightMode)
                CalculateMovementFixedUpdate();
        }

        private void CalculateMovementFixedUpdate()
        {
            Debug.Log("Q/E rotation is working");//this for checking
        }

        public void CalculateMovementUpdate(float rotInput)
        {
            /*if (Input.GetKey(KeyCode.LeftArrow))
            {
                var tempRot = transform.localRotation.eulerAngles;
                tempRot.y -= _speed / 3;
                transform.localRotation = Quaternion.Euler(tempRot);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                var tempRot = transform.localRotation.eulerAngles;
                tempRot.y += _speed / 3;
                transform.localRotation = Quaternion.Euler(tempRot);
            }*/
            var temptRot = transform.localRotation.eulerAngles;
            temptRot.y += rotInput * (_speed / 3);
            transform.localRotation = Quaternion.Euler(temptRot);
        }

        public void CalculateMovementFixedUpdate(float direction)
        {

            /*if (Input.GetKey(KeyCode.Space))
            {
                _rigidbody.AddForce(transform.up * _speed, ForceMode.Acceleration);
            }
            if (Input.GetKey(KeyCode.V))
            {
                _rigidbody.AddForce(-transform.up * _speed, ForceMode.Acceleration);
            }*/

            _rigidbody.AddForce(transform.up * direction * _speed, ForceMode.Acceleration);
        }

        public void CalculateTilt(Vector2 tilt)
        {
            /*if (Input.GetKey(KeyCode.A)) 
                transform.rotation = Quaternion.Euler(00, transform.localRotation.eulerAngles.y, 30);
            else if (Input.GetKey(KeyCode.D))
                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, -30);
            else if (Input.GetKey(KeyCode.W))
                transform.rotation = Quaternion.Euler(30, transform.localRotation.eulerAngles.y, 0);
            else if (Input.GetKey(KeyCode.S))
                transform.rotation = Quaternion.Euler(-30, transform.localRotation.eulerAngles.y, 0);
            else 
                transform.rotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);*/

            float xRotation = 0;
            float zRotation = 0;

            xRotation = tilt.x * 30;

            zRotation = tilt.y * 30;


            transform.rotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y, zRotation);
        }

        private void OnDisable()
        {
            InteractableZone.onZoneInteractionComplete -= EnterFlightMode;
        }
    }
}
