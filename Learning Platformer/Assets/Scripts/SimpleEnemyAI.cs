using UnityEngine;
using System.Collections;
using System;

public class SimpleEnemyAI : MonoBehaviour , ITakeDamage, PlayerRespawnListener {

    public float Speed;
    public float FireRate = 1;
    public Projectile Projectile;
    public GameObject DestroyedEffect;
    public Animator Animator;
    public Vector3 Offset;
    public AudioClip EnemyDestroySound;
    public int MaxHealth = 50;

    private PlayerController _controller;
    private Vector2 _direction;
    private Vector2 _startPosition;
    private float _canFireIn;

    public int Health { get; private set; }
    
    // Use this for initialization
	public void Start () {
        _controller = GetComponent<PlayerController>();
        _direction = new Vector2(-1, 0);
        _startPosition = transform.position;
        Health = MaxHealth;
    }
	
	// Update is called once per frame
	public void Update () {
        _controller.SetHorizontalForce(_direction.x * Speed);

        if((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight)){
            _direction = -_direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if ((_canFireIn -= Time.deltaTime) > 0)
            return;

        var rayCast = Physics2D.Raycast(transform.position, _direction, 10, 1 << LayerMask.NameToLayer("Player"));
        if (!rayCast)
            return;

        //var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
        var projectile = (Projectile)Instantiate(Projectile, (transform.position - Offset), transform.rotation);
        projectile.Initialise(gameObject, _direction, _controller.Velocity);
        _canFireIn = FireRate;

        Animator.SetFloat("Speed", Mathf.Abs(_direction.x) / Speed);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Instantiate(DestroyedEffect, transform.position, transform.rotation);
            SoundManager.Instance.PlayClip3D(EnemyDestroySound, transform.position);
            gameObject.SetActive(false);
        }
    }

    public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player)
    {
        _direction = new Vector2(-1, 0);
        Health = MaxHealth;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true); 
    } 
}
