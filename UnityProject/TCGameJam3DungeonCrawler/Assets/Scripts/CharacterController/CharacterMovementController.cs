using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovementController : MonoBehaviour
{
    [SerializeField]
    private float gravity;

    [SerializeField]
    private float maxVerticalSpeed;

    [SerializeField]
    private float maxGroundSpeed;

    [SerializeField]
    private float maxAirHorizontalSpeed;

    [SerializeField]
    private float jumpImpulseForce;

    [SerializeField]
    private float horizontalMovementSpeed;

    [SerializeField]
    private float groundFriction;

    private Vector3 currentVelocity;

    private CharacterController characterController;

    private void Start()
    {
        this.characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 acceleration = new Vector3(0, -this.gravity, 0);

        this.currentVelocity += acceleration * Time.deltaTime;
        if (this.currentVelocity.magnitude > this.maxVerticalSpeed)
        {
            this.currentVelocity = this.currentVelocity.normalized * this.maxVerticalSpeed;
        }

        HandleInput();

        this.characterController.Move(this.currentVelocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if( (this.characterController.collisionFlags & CollisionFlags.Above) != 0 ||
            (this.characterController.collisionFlags & CollisionFlags.Below) != 0)
        {
            this.currentVelocity.y = 0f;
        }
        
        if ((this.characterController.collisionFlags & CollisionFlags.Sides) != 0)
        {
            this.currentVelocity.x = 0f;
        }
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if(Mathf.Abs(horizontalInput) < 0.01f)
        {
            horizontalInput = 0f;
        }

        if(this.characterController.isGrounded)
        {
            if(Input.GetButton("Jump"))
            {
                this.currentVelocity.y = this.jumpImpulseForce;
            }

            if (horizontalInput == 0f)
            {
                ApplyGroundFriction();
            }
        }

        this.currentVelocity += new Vector3(horizontalInput * horizontalMovementSpeed * Time.deltaTime, 0, 0);
        float maxHorizontalSpeed = this.characterController.isGrounded ? this.maxGroundSpeed : this.maxAirHorizontalSpeed;
        this.currentVelocity = new Vector3(Mathf.Clamp(this.currentVelocity.x, -maxHorizontalSpeed, maxHorizontalSpeed),
                                           Mathf.Clamp(this.currentVelocity.y, -this.maxVerticalSpeed, this.maxVerticalSpeed),
                                           0f);
    }

    private void ApplyGroundFriction()
    {   
        if (this.currentVelocity.x > 0)
        {
            this.currentVelocity.x -= this.groundFriction * Time.deltaTime;
            if(this.currentVelocity.x < 0)
            {
                this.currentVelocity.x = 0;
            }
        }
        else if (this.currentVelocity.x < 0)
        {
            this.currentVelocity.x += this.groundFriction * Time.deltaTime;
            if (this.currentVelocity.x > 0)
            {
                this.currentVelocity.x = 0;
            }
        }
    }
}