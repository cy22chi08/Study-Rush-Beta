using UnityEngine;
using TMPro;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float dashMultiplier = 2.0f;
    public float maxStamina = 3.0f;
    public float staminaRegenRate = 0.5f;
    public float mouseSensitivity = 2.0f;
    public Transform cameraTransform; // Assign in Inspector
    public TMP_Text staminaText;

    private float currentStamina;
    private bool isDashing;
    private CharacterController controller;
    private float verticalRotation = 0f;
    private bool isLooking = false;  // Left-click to look
    private bool isShiftLocked = false; // Shift Lock mode

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaUI();

        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleDash();
        UpdateStaminaUI();
        HandleShiftLock();
    }

    private void HandleMovement()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        float speed = isDashing ? moveSpeed * dashMultiplier : moveSpeed;

        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    private void HandleLook()
    {
        if (isShiftLocked)
        {
            // If Shift Lock is ON, always rotate camera
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
        else
        {
            // Normal Look (Only when Left Mouse is held)
            if (Input.GetMouseButtonDown(0))
            {
                isLooking = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isLooking = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (isLooking)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

                transform.Rotate(Vector3.up * mouseX);

                verticalRotation -= mouseY;
                verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
                cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            }
        }
    }

    private void HandleShiftLock()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isShiftLocked = !isShiftLocked;

            if (isShiftLocked)
            {
                // Enable Shift Lock (Hide Cursor & Center Camera)
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                // Disable Shift Lock (Allow normal view)
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void HandleDash()
    {
        bool isMoving = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;

        if (Input.GetKey(KeyCode.LeftControl) && isMoving && currentStamina > 0 && !isShiftLocked)
        {
            isDashing = true;
            currentStamina -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
            }
        }
    }

    private void UpdateStaminaUI()
    {
        if (staminaText != null)
        {
            staminaText.text = "Stamina: " + Mathf.Round(currentStamina * 10) / 10;
        }
    }
}