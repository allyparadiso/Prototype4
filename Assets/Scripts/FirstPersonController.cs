using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.8f;

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float lookSensitivity = 0.1f;
    [SerializeField] private float topClamp = 85f;
    [SerializeField] private float bottomClamp = -85f;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private float _cinemachineTargetPitch;
    private float _verticalVelocity;

    public GameObject center;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        center.SetActive(true);
    }
    private void Update()
    {
        Movement();
    }
    private void LateUpdate()
    {
        Rotation();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }
    private void Movement()
    {
        if (_controller.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2;
        }
        Vector3 moveDirection = transform.forward * _moveInput.y + transform.right * _moveInput.x;

        _verticalVelocity += gravity * Time.deltaTime;
        moveDirection.y = _verticalVelocity;

        _controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    private void Rotation()
    {
        if (_lookInput.sqrMagnitude >= 0.01f)
        {
            _cinemachineTargetPitch -= _lookInput.y * lookSensitivity;
            _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, bottomClamp, topClamp);
            cameraTarget.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, 0f);

            transform.Rotate(Vector3.up * (_lookInput.x * lookSensitivity));
        }
    }
}
