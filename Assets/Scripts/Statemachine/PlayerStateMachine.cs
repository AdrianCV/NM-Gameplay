using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerBaseState _currentState;
    StateFactory _states;

    Camera _camera;

    CharacterInput _characterInput;
    CharacterController _characterController;
    Animator _animator;
    Rigidbody _rb;
    AudioSource _audioSource;


    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _currentRunMovement;
    Vector3 _input;

    bool _isMovementPressed;
    bool _isrunPressed;
    float _rotationFactorPerFrame = 1.0f;
    float _groundedGravity = -0.05f;

    bool _wheelIsOpen;

    [SerializeField] bool _grounded;


    [SerializeField] float _turnSpeed = 900f;
    [SerializeField] float _walkSpeed = 3.0f;
    [SerializeField] float _runSpeed = 5.0f;
    [SerializeField] float _jumpForce = 400f;

    [SerializeField] float _fallMultiplier = 2.5f;
    [SerializeField] float _lowJumpMultiplier = 2f;

    [SerializeField] float _groundCheckDistance;

    [SerializeField] float _radius;

    [SerializeField] LayerMask _layer;

    [SerializeField] Material _currentMat;

    [SerializeField] GameObject _death;
    [SerializeField] GameObject _gameOver;
    [SerializeField] GameObject _win;
    [SerializeField] ColorWheelController _colorWheelController;

    [SerializeField] AudioClip _paintSound;
    [SerializeField] AudioClip _jumpSound;


    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsRunPressed { get { return _isrunPressed; } }
    public bool Grounded { get { return _grounded; } }
    public bool WheelIsOpen { get { return _wheelIsOpen; } set { _wheelIsOpen = value; } }
    public float GroundedGravity { get { return _groundedGravity; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public Vector3 CurrentMovement { get { return _currentMovement; } }
    public Vector3 CurrentRunMovement { get { return _currentRunMovement; } }
    public Material CurrentMaterial { get { return _currentMat; } set { _currentMat = value; } }
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
        _audioSource = GetComponent<AudioSource>();

        _camera = Camera.main;

        _states = new StateFactory(this);

        _currentState = _states.Idle();
        _currentState.EnterState();

        _characterInput.CharacterControls.Move.started += OnMovementInput;
        _characterInput.CharacterControls.Move.canceled += OnMovementInput;
        _characterInput.CharacterControls.Move.performed += OnMovementInput;

        _characterInput.CharacterControls.Run.started += OnRun;
        // _characterInput.CharacterControls.Run.canceled += OnRun;

        _characterInput.CharacterControls.Jump.started += OnJump;
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

        GroundCheck();
        JumpGravity();
        GatherInput();
        Look();
        Paint();
    }

    void Paint()
    {
        if (Input.GetMouseButtonDown(0) && _currentMat != null && !_wheelIsOpen)
        {
            Collider[] _colliders = Physics.OverlapSphere(transform.position, _radius, _layer);

            foreach (Collider col in _colliders)
            {
                if (col.transform.position.x > transform.position.x || col.transform.position.z > transform.position.z)
                {
                    col.GetComponent<Renderer>().sharedMaterial = _currentMat;

                    if (_paintSound != null)
                    {
                        _audioSource.PlayOneShot(_paintSound);
                    }
                }
            }
        }
    }

    void JumpGravity()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void GroundCheck()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, _groundCheckDistance))
        {
            _grounded = true;
        }
        else
        {
            _grounded = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    void FixedUpdate()
    {
        _currentState.FixedUpdateState();
    }

    private void OnCollisionEnter(Collision other)
    {
        _currentState.CollisionEnter(this, other);

        if (other.gameObject.tag == "Death")
        {
            // Do Game Over
            _death.GetComponent<DeathMove>().Speed = 0;
            _gameOver.SetActive(true);
        }
        else if (other.gameObject.tag == "Win")
        {
            // Do Win
            _death.GetComponent<DeathMove>().Speed = 0;
            _win.SetActive(true);
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
        _isrunPressed = !_isrunPressed; //context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (_grounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

            if (_jumpSound != null)
            {
                _audioSource.PlayOneShot(_jumpSound);
            }
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