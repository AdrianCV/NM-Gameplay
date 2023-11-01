using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace StateMachine
{
    public class PlayerStateMachine : MonoBehaviour
    {
        PlayerBaseState _currentState;
        StateFactory _states;

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
        bool _isRunPressed;
        float _rotationFactorPerFrame = 1.0f;
        float _groundedGravity = -0.05f;

        bool _wheelIsOpen;

        bool _isFadingIn;
        bool _isFadingOut;

        float _fadeStartTime;
        float _fadeStartVolume;

        [SerializeField] float _fadeDuration;
        [SerializeField] float _maxDistance;

        [SerializeField] Volume _volume;
        Vignette _vignette;
        ColorAdjustments _colorAdjustments;

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

        [SerializeField] AudioClip[] _paintSounds;
        [SerializeField] AudioClip _jumpSound;
        [SerializeField] AudioClip _deathSound;
        [SerializeField] AudioClip _winSound;
    
    
        static readonly int Death = Animator.StringToHash("Death");
        static readonly int Jump = Animator.StringToHash("Jump");
        static readonly int RunAttack = Animator.StringToHash("RunAttack");
        static readonly int Attack1 = Animator.StringToHash("Attack1");


        public PlayerBaseState CurrentState { get => _currentState;
            set => _currentState = value;
        }
        public CharacterController CharacterController => _characterController;
        public bool IsMovementPressed => _isMovementPressed;
        public bool IsRunPressed => _isRunPressed;
        public bool Grounded => _grounded;
        public bool WheelIsOpen { get => _wheelIsOpen;
            set => _wheelIsOpen = value;
        }
/*
    public float GroundedGravity => _groundedGravity;
*/
        public float CurrentMovementY { get => _currentMovement.y;
            set => _currentMovement.y = value;
        }
        public Vector3 CurrentMovement => _currentMovement;
        public Vector3 CurrentRunMovement => _currentRunMovement;
        public Material CurrentMaterial { get => _currentMat;
            set => _currentMat = value;
        }
        public Animator Animator => _animator;
        public Transform Transform => transform;
        public Rigidbody Rb => _rb;

        public Vector3 PlayerInput => _input;
        public float RunMultiplier { get => _runSpeed;
            set => _runSpeed = value;
        }
        public float WalkMultiplier { get => _walkSpeed;
            set => _walkSpeed = value;
        }




        void Awake()
        {
            _characterInput = new CharacterInput();

            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();

            if (_volume != null)
            {
                _volume.profile.TryGet(out _vignette);
                _volume.profile.TryGet(out _colorAdjustments);
            }

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
            // ScaryDeath();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void Paint()
        {
            if (!Input.GetMouseButtonDown(0) || _currentMat == null || _wheelIsOpen) return;
            const int maxColliders = 5;
            var colliders = new Collider[maxColliders];
            var size = Physics.OverlapSphereNonAlloc(transform.position, _radius, colliders, _layer);

            foreach (var col in colliders)
            {
                if (!(Vector3.Dot(col.transform.position, Transform.forward) > 0)) continue;

                if (col.GetComponent<PaintedObject>().CurrentMaterial != null)
                {
                    col.GetComponent<PaintedObject>().CurrentMaterial = _currentMat;
                }


                _animator.SetTrigger(IsMovementPressed ? RunAttack : Attack1);
    
                if (_paintSounds != null)
                {
                    _audioSource.PlayOneShot(_paintSounds[Random.Range(0, _paintSounds.Length)]);
                }
            }
        }

        void JumpGravity()
        {
            switch (_rb.velocity.y)
            {
                case < 0:
                    _rb.velocity += Vector3.up * (Physics.gravity.y * (_fallMultiplier - 1) * Time.deltaTime);
                    break;
                case > 0:
                    _rb.velocity += Vector3.up * (Physics.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime);
                    break;
            }
        }

        void GroundCheck()
        {
            _grounded = Physics.Raycast(transform.position + new Vector3(0, 1, 0), -Vector3.up, _groundCheckDistance);
        }

        void ScaryDeath()
        {
            if (_vignette != null)
            {
                var audioSourceVolume = _audioSource.volume;
                _vignette.intensity.value = audioSourceVolume;
                _colorAdjustments.saturation.value = audioSourceVolume * -100;
            }

            const int maxHits = 10;
            var hit = new RaycastHit[maxHits];
        
            var size = Physics.SphereCastNonAlloc(transform.position, 15, new Vector3(0, 0, 0), hit, 0, 7);

            // print(hit.point);

            var closestPoint = Vector3.positiveInfinity;

            foreach (var point in hit)
            {
                if (Vector3.Distance(transform.position, point.point) < Vector3.Distance(transform.position, closestPoint))
                {
                    closestPoint = point.point;
                }
            }



            var distance = Vector3.Distance(transform.position, closestPoint);
            float volume;

            print(distance);

            if (distance > _maxDistance)
            {
                if (!_isFadingOut)
                {
                    _isFadingOut = true;
                    _isFadingIn = false;
                    _fadeStartTime = Time.time;
                    _fadeStartVolume = _audioSource.volume;
                }

                var fadeElapsedTime = Time.time - _fadeStartTime;
                var fadePercentage = Mathf.Clamp01(fadeElapsedTime / _fadeDuration);
                volume = Mathf.Lerp(_fadeStartVolume, 0f, fadePercentage);
            }
            else
            {
                if (!_isFadingIn)
                {
                    _isFadingIn = true;
                    _isFadingOut = false;
                    _fadeStartTime = Time.time;
                    _fadeStartVolume = _audioSource.volume;
                }

                var fadeElapsedTime = Time.time - _fadeStartTime;
                var fadePercentage = Mathf.Clamp01(fadeElapsedTime / _fadeDuration);
                volume = Mathf.Lerp(_fadeStartVolume, 1f, fadePercentage);
            }

            _audioSource.volume = volume;
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

            if (other.gameObject.CompareTag("Death"))
            {
                // Do Game Over
                _death.GetComponent<DeathMove>().Speed = 0;
                _gameOver.SetActive(true);
                _audioSource.PlayOneShot(_deathSound);
                _animator.SetTrigger(Death);
                this.enabled = false;
            }
            else if (other.gameObject.CompareTag("Win"))
            {
                // Do Win
                _death.GetComponent<DeathMove>().Speed = 0;
                _win.SetActive(true);
                _audioSource.PlayOneShot(_winSound);
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
            _isRunPressed = !_isRunPressed; //context.ReadValueAsButton();
        }

        void OnJump(InputAction.CallbackContext context)
        {
            if (_grounded)
            {
                _animator.SetTrigger(Jump);
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
        private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 225, 0));
        public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
    }
}