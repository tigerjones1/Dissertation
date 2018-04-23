using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour

{
    private const float SkinWidth = 0.02f;  //distance between character and object it is colliding with allowed
    private const int TotalHorizontalRays = 8;  //total amount of Rays(arrows) coming out from the character horizontally
    private const int TotalVerticalRays = 4;    //total amount of Rays(arrows) coming out from the character vertically

    private static readonly float SlopeLimitTangent = Mathf.Tan(75f * Mathf.Deg2Rad);

    public LayerMask PlatformMask; //objects the character is allowed to collide with
    public ControllerParameter DefaultParameters;   //allows for the controller parameters to be edited from the inspector

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
    public ControllerParameter Parameters { get { return overrideParameters ?? DefaultParameters; } }   //if overrideParameters is null it will return defaultParameters
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

    public void LateUpdate() //used to delay the calculations
    {
        jumpIn -= Time.deltaTime;
        velocity.y += Parameters.Gravity * Time.deltaTime;
        Move(Velocity * Time.deltaTime);    //move character per their velocity scaled by time

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

            if (Mathf.Abs(deltaMovement.x) > 0.01f) //checks if character is moving left or right
                MoveHorizontally(ref deltaMovement);

            MoveVertically(ref deltaMovement);  //constantly moving vertically(down) due to gravity
        }

        _transform.Translate(deltaMovement, Space.World);

        if (Time.deltaTime > 0)
            velocity = deltaMovement / Time.deltaTime;

        velocity.x = Mathf.Min(velocity.x, Parameters.MaxVelocity.x);   //clamping velocity to the max velocity defined in paramaters
        velocity.y = Mathf.Min(velocity.y, Parameters.MaxVelocity.y);

        if (State.IsMovingUpSlope)
            velocity.y = 0;
    }

    private void HandlePlatforms()
    {

    }

    private void CalculateRayOrigins() //invoked on every LateUpdate, pre-computes where the Rays will be shot out from
    {
        var size = new Vector2(boxCollider.size.x * Mathf.Abs(localScale.x), boxCollider.size.y * Mathf.Abs(localScale.y)) / 2; //calculates sizze of the character and divides it by 2
        var center = new Vector2(boxCollider.offset.x * localScale.x, boxCollider.offset.y * localScale.y); //centre of the character calculation

        rayCastTopLeft = transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        rayCastBottomRight = transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
        rayCastBottomLeft = transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);

    }

    private void MoveHorizontally(ref Vector2 deltaMovement)    //manipulates detlaMovement
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : -Vector2.right;
        var rayOrigin = isGoingRight ? rayCastBottomRight : rayCastBottomLeft;

        for (var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * verticalDistanceBetweenRays));
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);    //displays the rays on the character

            var rayCastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, PlatformMask);
            if (!rayCastHit)
                continue;

            if (i == 0 && HandleHorizontalSlope(ref deltaMovement, Vector2.Angle(rayCastHit.normal, Vector2.up), isGoingRight))
                break;

            deltaMovement.x = rayCastHit.point.x - rayVector.x; //constrains movement if character hits something
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

    private void MoveVertically(ref Vector2 deltaMovement) //invoked every frame as gravity is always acting upon the character
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