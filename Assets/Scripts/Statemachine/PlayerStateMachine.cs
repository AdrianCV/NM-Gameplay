using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerBaseState _currentState;
    StateFactory _states;

    Camera _camera;

    CharacterInput _characterInput;
    CharacterController _characterController;
    Animator _animator;
    Rigidbody _rb;


    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _currentRunMovement;
    Vector3 _input;

    bool _isMovementPressed;
    bool _isrunPressed;
    float _rotationFactorPerFrame = 1.0f;
    float _groundedGravity = -0.05f;


    [SerializeField] float _turnSpeed = 900f;
    [SerializeField] float _walkSpeed = 3.0f;
    [SerializeField] float _runSpeed = 5.0f;

    [SerializeField] GameObject _susBox;
    [SerializeField] GameObject _notSusBox;

    bool _touchingSusItem;
    GameObject _susItem;
    bool _touchingNotSusItem;
    GameObject _notSusItem;



    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsRunPressed { get { return _isrunPressed; } }
    public float GroundedGravity { get { return _groundedGravity; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public Vector3 CurrentMovement { get { return _currentMovement; } }
    public Vector3 CurrentRunMovement { get { return _currentRunMovement; } }
    public Animator Animator { get { return _animator; } }
    public Transform Transform { get { return transform; } }
    public Rigidbody RB { get { return _rb; } }

    public Vector3 PlayerInput { get { return _input; } }
    public float RunMultiplier { get { return _runSpeed; } set { _runSpeed = value; } }
    public float WalkMultiplier { get { return _walkSpeed; } set { _walkSpeed = value; } }




    void Awake()
    {
        _characterInput = new CharacterInput();

        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        _camera = Camera.main;

        _states = new StateFactory(this);

        _currentState = _states.Idle();
        _currentState.EnterState();

        _characterInput.CharacterControls.Move.started += OnMovementInput;
        _characterInput.CharacterControls.Move.canceled += OnMovementInput;
        _characterInput.CharacterControls.Move.performed += OnMovementInput;

        _characterInput.CharacterControls.Run.started += OnRun;
        _characterInput.CharacterControls.Run.canceled += OnRun;
    }

    private void OnEnable()
    {
        _characterInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _characterInput.CharacterControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState();

        GatherInput();
        Look();
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    private void OnCollisionEnter(Collision other)
    {
        _currentState.CollisionEnter(this, other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "SusItem")
        {
            _touchingSusItem = true;
            _susItem = other.gameObject;
        }
        else if (other.tag == "NotSusItem")
        {
            _touchingNotSusItem = true;
            _notSusItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SusItem")
        {
            _touchingSusItem = false;
        }
        else if (other.tag == "NotSusItem")
        {
            _touchingNotSusItem = false;
        }
    }


    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x * _walkSpeed;
        _currentMovement.z = _currentMovementInput.y * _walkSpeed;

        _currentRunMovement.x = _currentMovementInput.x * _runSpeed;
        _currentRunMovement.z = _currentMovementInput.y * _runSpeed;

        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void OnRun(InputAction.CallbackContext context)
    {
        _isrunPressed = context.ReadValueAsButton();
    }

    void HandleRotation()
    {
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

        var skewedInput = matrix.MultiplyPoint3x4(_currentMovement);

        var relative = (transform.position + skewedInput) - transform.position;

        var rot = Quaternion.LookRotation(relative, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _rotationFactorPerFrame * Time.deltaTime);
    }

    void OldRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame);
        }
    }


    void HandleGravity()
    {
        if (_characterController.isGrounded)
        {
            float groundedGravity = -0.05f;
            _currentMovement.y = groundedGravity;
            _currentRunMovement.y = groundedGravity;
        }
        else
        {
            float gravity = -9.8f;
            _currentMovement.y += gravity;
            _currentRunMovement.y += gravity;
        }
    }

    #region NewRot
    private void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }
    #endregion
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}