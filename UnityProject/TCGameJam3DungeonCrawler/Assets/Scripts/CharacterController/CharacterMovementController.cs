using UnityEngine;
using System.Collections;
using Assets.Scripts;

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

    [SerializeField]
    private float climbingSpeed;

    private Vector3 currentVelocity;

    private CharacterController characterController;

    private bool hasCharacterRequestedJumpWithinRange;

    private bool isClimbing;
    private bool startClimbing;
    private Ladder currentLadder;

    private int numLaddersClimbing;
    
    private void Start()
    {
        this.characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 acceleration = new Vector3(0, -this.gravity, 0);

        if(!this.isClimbing)
        {
            this.currentVelocity += acceleration * Time.deltaTime;
            if (this.currentVelocity.magnitude > this.maxVerticalSpeed)
            {
                this.currentVelocity = this.currentVelocity.normalized * this.maxVerticalSpeed;
            }
        }

        HandleInput();

        this.characterController.Move(this.currentVelocity * Time.deltaTime);
    }
    
    public void HandleLadderEntry(Ladder ladder)
    {
        this.startClimbing = true;
        this.currentLadder = ladder;

        this.numLaddersClimbing++;
    }

    public void HandleLadderExit(Ladder ladder)
    {
        numLaddersClimbing--;

        if(this.startClimbing)
        {
            this.startClimbing = false;
        }

        if (this.isClimbing && numLaddersClimbing == 0)
        {
            this.isClimbing = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        EnergyOrb orb = hit.gameObject.GetComponent<EnergyOrb>();
        if(orb != null)
        {
            orb.DeliverPayload(GetComponent<Player>());
        }

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
        if(this.startClimbing && Input.GetAxis("Vertical") > 0f)
        {
            this.startClimbing = false;
            this.isClimbing = true;
            this.currentVelocity = Vector3.zero;

            Bounds ladderBounds = this.currentLadder.GetComponent<BoxCollider>().bounds;

            float newHeight = this.characterController.isGrounded ? ladderBounds.center.y : this.transform.position.y;
            this.transform.position = new Vector3(ladderBounds.center.x, newHeight, this.transform.position.z);
        }

        if(this.isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            this.currentVelocity.y = verticalInput * this.climbingSpeed;
        }
       
        float horizontalInput = Input.GetAxis("Horizontal");

        if (this.characterController.isGrounded || this.isClimbing)
        {
            if (this.hasCharacterRequestedJumpWithinRange || Input.GetButtonDown("Jump"))
            {
                this.currentVelocity.y = this.jumpImpulseForce;
                this.hasCharacterRequestedJumpWithinRange = false;
            }
        }
        else
        {
            if (!Input.GetButton("Jump"))
            {
                if (this.currentVelocity.y > this.jumpCutSpeedLimit)
                {
                    this.currentVelocity.y = this.jumpCutSpeedLimit;
                }
            }
            else if (Physics.Raycast(new Ray(this.transform.position, -this.transform.up), this.minimumJumpTriggerHeight, LayerMask.NameToLayer("Default")))
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
    void onDestory()
    {
        Debug.Log("Player Died");
    }
}