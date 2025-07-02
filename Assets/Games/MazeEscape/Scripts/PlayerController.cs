using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("수치 설정")]
    public float moveSpeed = 10f;
    public float mouseSensitivity = 1f;
    public float gravity = 9.8f;
    public float lookXLimit = 60f;

    [Header("컴포넌트 설정")]
    public CharacterController controller;
    public Camera playerCamera;
    
    private float rotationX = 0;
    private float verticalVelocity = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0, mouseX, 0);

        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 move = transform.TransformDirection(
            new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))
        ) * moveSpeed;

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}
