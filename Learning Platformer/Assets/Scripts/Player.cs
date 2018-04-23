using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ITakeDamage
{

    public bool isFacingRight;  //checks whether the player is facing right or not
    private PlayerController controller;    //keeps track of the characterController component on this object
    private float normalisedHorizontalSpeed;    //a value either -1 or 1 that determins if the player is moving left or right

    public int Damage = 10;
    public float MaxSpeed = 8;  //max speed player can move
    public float SpeedAccelerationOnGround = 10f;   //used to determine how quicky the speed can change on ground
    public float SpeedAccelerationInAir = 5f;   //used to determine how quickly the speed can change in air
    public int MaxHealth = 100; 
    public GameObject OuchEffect;
    public Projectile Projectile;
    public float FireRate;
    public Transform ProjectileFireLocation;
    public GameObject FireProjectileEffect;
    public Animator Animator;
    public AudioClip PlayerHitSound;
    public AudioClip PlayerShotShound;

    public int Health { get; private set; }
    public bool IsDead { get; private set; }

    private float _canFireIn;

    public void Awake()
    {
        controller = GetComponent<PlayerController>();  //assigns the PlayerCOntroller component to controller
        isFacingRight = transform.localScale.x > 0; //allows the ability to make player face left or right at the start of a level
        Health = MaxHealth;
    }


    public void Update()
    {
        _canFireIn -= Time.deltaTime;

        if (!IsDead)
            HandleInput();  //changes normalisedHorizontalSpeed based on Key Input

        var movementFactor = controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;  //movementFactor will be set to either Speed OnGround or InAir

        if (IsDead)
            controller.SetHorizontalForce(0);
        else
            controller.SetHorizontalForce(Mathf.Lerp(controller.Velocity.x, normalisedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));    //sets the x value of the players velocity to a linear interpolation between the max speed player should move and current velocity using the movementFactor * by the deltaTime

        Animator.SetBool("Ground", controller.State.IsGrounded);
        Animator.SetFloat("Speed", Mathf.Abs(controller.Velocity.x) / MaxSpeed);
    }

    public void FinishLevel()
    {
        enabled = false;
        controller.enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Kill()
    {
        controller.HandleCollisions = false;
        GetComponent<Collider2D>().enabled = false;
        IsDead = true;
        Health = 0;

        controller.SetForce(new Vector2(0, 20));
    }

    public void RespawnAt(Transform spawnPoint)
    {
        if (!isFacingRight)
            Flip();

        IsDead = false;
        GetComponent<Collider2D>().enabled = true;
        controller.HandleCollisions = true;
        Health = MaxHealth;

        transform.position = spawnPoint.position;

    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        SoundManager.Instance.PlayClip3D(PlayerHitSound, transform.position);
        FloatingText.Show(string.Format("-{0}", damage), "PlayerTakeDamageText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));

        Instantiate(OuchEffect, transform.position, transform.rotation);
        Health -= damage;

        if (Health <= 0)
            LevelManager.Instance.KillPlayer();

    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            normalisedHorizontalSpeed = 1;  //move player to the right
            if (!isFacingRight) //if player is facing left then flip to face the right
                Flip();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            normalisedHorizontalSpeed = -1; //move player to the left
            if (isFacingRight)  //if the player i facing right then flip to face the left
                Flip();
        }
        else
        {
            normalisedHorizontalSpeed = 0;  //if no key is pressed then keep the player in the same spot
        }
        if (controller.canJump && Input.GetKeyDown(KeyCode.W))
        {
            controller.Jump();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        if (_canFireIn > 0)
            return;
        ;
        if (FireProjectileEffect != null)
        {
            var effect = (GameObject) Instantiate(FireProjectileEffect, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            effect.transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
            effect.transform.parent = transform;
            Destroy(effect, 0.08f);

            SimpleEnemyAI enemy = gameObject.GetComponent<SimpleEnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage, OuchEffect);    
            }
        }

        var direction = isFacingRight ? Vector2.right : -Vector2.right;

        var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        projectile.Initialise(gameObject, direction, controller.Velocity);

        _canFireIn = FireRate;

        SoundManager.Instance.PlayClip3D(PlayerShotShound, transform.position);
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.x);    //flips the player around by negating the x component of the scale
        isFacingRight = transform.localScale.x > 0; //sets isFacingRight to true if the player is actually facing the right
    }
}
