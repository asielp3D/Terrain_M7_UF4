using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSController : MonoBehaviour
{
   //variables para velocidad, salto y gravedad
    private CharacterController _controller;

    private float _horizontal;
    private float _vertical;


    [SerializeField] private float _playerSpeed = 5;

    [SerializeField] private float _jumpHeight = 1;

    private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    TPSController _script;


    //variables para rotacion
    private float turnSmoothVelocity;

    [SerializeField] float turnSmoothTime = 0.1f;

    //variables para sensor

    [SerializeField] private Transform _sensorPosition;

    [SerializeField] private float _sensorRadius = 0.2f;

    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;

    private Transform _camera;
     private Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main.transform;
        _script = GetComponent<TPSController>();

    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if(Input.GetButton("Fire2"))
        {
            AimMovement();
        }
        else
        {
            Movement();
        }
       
        
        Jump();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelZ", 0);
        _animator.SetFloat("VelZ", direction.magnitude);

        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

           

            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }

        
    }

    void AimMovement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

         _animator.SetFloat("VelX", _horizontal);
         _animator.SetFloat("VelZ", _vertical);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;

        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camera.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        
        if(direction != Vector3.zero)
        {
            
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }

        
    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = -4;
            _animator.SetBool("Jump" , false);
        }
        
         if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
            _animator.SetBool("Jump" , true);
        }
        _playerGravity.y += _gravity * Time.deltaTime;

        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider deathCollider)
    {
        if(deathCollider.gameObject.layer == 7)
        {
            _script.enabled = false;

        }
    }
}
