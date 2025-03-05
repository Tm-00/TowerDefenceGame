using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 _playerMovementInput;
    private Vector2 _playerMouseInput;
    private float _xRot;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;

    private void Update()
    {
        _playerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        _playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));


        MovePlayer();
        MovePlayerCamera();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(_playerMovementInput);

        if (Input.GetKey(KeyCode.Space))
        {
            _velocity.y = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _velocity.y = -1f;
        }

        characterController.Move(MoveVector * speed * Time.deltaTime);
        characterController.Move(_velocity * speed * Time.deltaTime);

        _velocity.y = 0f;
    }

    private void MovePlayerCamera()
    {
        if (Input.GetMouseButton(1))
        {
            _xRot -= _playerMouseInput.y * sensitivity;
            transform.Rotate(0f, _playerMouseInput.x * sensitivity, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
        }
    }
}
