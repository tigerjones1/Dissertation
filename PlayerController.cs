using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
/*{

    public float moveSpeed;
    public float jumpHeight;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool grounded;

    private bool doubleJumped;
    private bool facingRight = true;

    Transform playerGraphics;

    private Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        playerGraphics = transform.FindChild("PlayerIdle");
        if (playerGraphics == null)
        {
            Debug.LogError("no playerGraphics object as a child of the player found");
        }
    }

    void FixedUpdate()
    {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

    }

    // Update is called once per frame
    void Update()
    {

        if (grounded)
            doubleJumped = false;

        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.W) && !doubleJumped && !grounded)
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
            Jump();
            doubleJumped = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }

        anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        anim.SetBool("Ground", grounded);

        if (GetComponent<Rigidbody2D>().velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (GetComponent<Rigidbody2D>().velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    public void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = playerGraphics.localScale;
        theScale.x *= -1;
        playerGraphics.localScale = theScale;
    }
}*/

{
    private const float SkinWidth = 0.02f;
    private const int TotalHorizontalRays = 8;
    private const int TotalVerticalRays = 4;

    private static readonly float SlopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);

    public LayerMask PlatformMask;
    public ControllerParameter DefaultParameters;

    public ControllerState State { get; private set; }
    public Vector2 Velocity { get { return velocity; } }
    public bool canJump
    {
        get
        {
            if (Parameters.JumpRestriction == ControllerParameter.JumpBehaviour.canJumpAnywhere)
                return jumpIn <= 0;

            if (Parameters.JumpRestriction == ControllerParameter.JumpBehaviour.canJumpOnGround)
                return State.IsGrounded;

            return false;
        }
    }
    public bool HandleCollisions { get; set; }
    public ControllerParameter Parameters { get { return overrideParameters ?? DefaultParameters; } }
    public GameObject StandingOn { get; private set; }

    private Vector2 velocity;
    private Transform _transform;
    private Vector3 localScale;
    private BoxCollider2D boxCollider;
    private ControllerParameter overrideParameters;
    private Vector3 rayCastTopLeft,
        rayCastBottomRight,
        rayCastBottomLeft;
    private float jumpIn;

    private float verticalDistanceBetweenRays,
        HorizontalDistanceBetweenRays;

    public void Awake()
    {
        HandleCollisions = true;
        State = new ControllerState();
        _transform = transform;
        localScale = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();

        var colliderWidth = boxCollider.size.x * Mathf.Abs(transform.localScale.x) - (2 * SkinWidth);
        HorizontalDistanceBetweenRays = colliderWidth / (TotalVerticalRays - 1);

        var colliderHeight = boxCollider.size.y * Mathf.Abs(transform.localScale.y) - (2 * SkinWidth);
        verticalDistanceBetweenRays = colliderHeight / (TotalHorizontalRays - 1);
    }

    public void AddForce(Vector2 force)
    {
        velocity = force;
    }

    public void SetForce(Vector2 force)
    {
        velocity += force;
    }

    public void SetHorizontalForce(float x)
    {
        velocity.x = x;
    }

    public void SetVerticalForce(float y)
    {
        velocity.y = y;
    }

    public void Jump()
    {
        AddForce(new Vector2(0, Parameters.JumpMagnitude));
        jumpIn = Parameters.JumpFrequency;
    }

    public void LateUpdate()
    {
        jumpIn -= Time.deltaTime;
        velocity.y += Parameters.Gravity * Time.deltaTime;
        Move(Velocity * Time.deltaTime);

    }

    private void Move(Vector2 deltaMovement)
    {
        var wasGrounded = State.IsCollidingBelow;
        State.Reset();

        if (HandleCollisions)
        {
            HandlePlatforms();
            CalculateRayOrigins();

            if (deltaMovement.y < 0 && wasGrounded)
                HandleVerticalSlope(ref deltaMovement);

            if (Mathf.Abs(deltaMovement.x) > 0.01f)
                MoveHorizontally(ref deltaMovement);

            MoveVertically(ref deltaMovement);
        }

        _transform.Translate(deltaMovement, Space.World);

        if (Time.deltaTime > 0)
            velocity = deltaMovement / Time.deltaTime;

        velocity.x = Mathf.Min(velocity.x, Parameters.MaxVelocity.x);
        velocity.y = Mathf.Min(velocity.y, Parameters.MaxVelocity.y);

        if (State.IsMovingUpSlope)
            velocity.y = 0;
    }

    private void HandlePlatforms()
    {

    }

    private void CalculateRayOrigins()
    {
        var size = new Vector2(boxCollider.size.x * Mathf.Abs(localScale.x), boxCollider.size.y * Mathf.Abs(localScale.y)) / 2;
        var center = new Vector2(boxCollider.offset.x * localScale.x, boxCollider.offset.y * localScale.y);

        rayCastTopLeft = transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        rayCastBottomRight = transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
        rayCastBottomLeft = transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);

    }

    private void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? rayCastBottomRight : rayCastBottomLeft;

        for (var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!rayCastHit)
                continue;

            if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
                break;

            deltaMovement.x = rayCastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                State.IsCollidingRight = true;
            }
            else
            { 
                deltaMovement.x += SkinWidth;
            State.IsCollidingLeft = true;
        }

        if (rayDistance < SkinWidth + 0.001f)
            break;
    }
}

    private void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingUp = deltaMovement.y > 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var rayDirection = isGoingUp ? Vector2.up : -Vector2.up;
        var rayOrigin = isGoingUp ? rayCastTopLeft : rayCastBottomLeft;

        rayOrigin.x += deltaMovement.x;

        var standingOnDistance = float.MaxValue;
        for(var i = 0; i < TotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + (i * HorizontalDistanceBetweenRays), rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!rayCastHit)
                continue;

            if(!isGoingUp)
            {
                var verticalDistanceToHit = _transform.position.y - rayCastHit.point.y;
                if(verticalDistanceToHit < standingOnDistance)
                {
                    standingOnDistance = verticalDistanceToHit;
                    StandingOn = rayCastHit.collider.gameObject;
                }
            }
            deltaMovement.y = rayCastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingUp)
            {
                deltaMovement.y -= SkinWidth;
                State.IsCollidingAbove = true;
            }
            else
            {
                deltaMovement.y += SkinWidth;
                State.IsCollidingBelow = true;
            }
            if (!isGoingUp && deltaMovement.y > 0.001f)
                State.IsMovingUpSlope = true;

            if (rayDistance < SkinWidth + 0.001f)
                break;
        }
    }

    private void HandleVerticalSlope(ref Vector2 deltaMovement)
    {

    }

    private bool HandleHorizontalSlope(ref Vector2 deltaMovement, float angle, bool isGoingRight)
    {
        return false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var Parameters = other.gameObject.GetComponent<ControllerPhysicsVolume>();
        if (Parameters == null)
            return;

        overrideParameters = Parameters.Parameters;
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        var Parameters = other.gameObject.GetComponent<ControllerPhysicsVolume>();
        if (Parameters == null)
            return;

        overrideParameters = null;
    }
}