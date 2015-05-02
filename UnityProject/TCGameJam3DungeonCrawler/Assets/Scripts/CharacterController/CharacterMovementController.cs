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

    [SerializeField]
    private float jumpCutSpeedLimit;

    [SerializeField]
    private float minimumJumpTriggerHeight;

    private Vector3 currentVelocity;

    private CharacterController characterController;

    private bool hasCharacterRequestedJumpWithinRange;

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

        if(this.characterController.isGrounded)
        {
            if (this.hasCharacterRequestedJumpWithinRange || Input.GetButtonDown("Jump"))
            {
                this.currentVelocity.y = this.jumpImpulseForce;
                this.hasCharacterRequestedJumpWithinRange = false;
            }
        }
        else
        {
            RaycastHit hitInfo;

            if(!Input.GetButton("Jump"))
            {
                if(this.currentVelocity.y > this.jumpCutSpeedLimit)
                {
                    this.currentVelocity.y = this.jumpCutSpeedLimit;
                }
            }
            else if(Physics.Raycast(new Ray(this.transform.position, -this.transform.up), this.minimumJumpTriggerHeight, LayerMask.NameToLayer("Default")))
            {   
                this.hasCharacterRequestedJumpWithinRange = true;
            }
        }

        if ((horizontalInput == 0f && this.characterController.isGrounded) || Mathf.Sign(horizontalInput) != Mathf.Sign(this.currentVelocity.x))
        {
            ApplyGroundFriction();
        }

        float horizontalAcceleration = horizontalInput * horizontalMovementSpeed * Time.deltaTime;

        this.currentVelocity += new Vector3(horizontalAcceleration, 0, 0);
        //float maxHorizontalSpeed = this.characterController.isGrounded ? this.maxGroundSpeed : this.maxAirHorizontalSpeed;
        float maxHorizontalSpeed = this.maxGroundSpeed;
        this.currentVelocity = new Vector3(Mathf.Clamp(this.currentVelocity.x, -maxHorizontalSpeed, maxHorizontalSpeed),
                                           Mathf.Clamp(this.currentVelocity.y, -this.maxVerticalSpeed, this.maxVerticalSpeed),
                                           0f);
    }

    private void ApplyGroundFriction()
    {
        if (this.currentVelocity.x > 0f)
        {
            this.currentVelocity.x -= this.groundFriction * Time.deltaTime;
            if(this.currentVelocity.x < 0f)
            {
                this.currentVelocity.x = 0f;
            }
        }
        else if (this.currentVelocity.x < 0)
        {
            this.currentVelocity.x += this.groundFriction * Time.deltaTime;
            if (this.currentVelocity.x > 0f)
            {
                this.currentVelocity.x = 0f;
            }
        }
    }
}