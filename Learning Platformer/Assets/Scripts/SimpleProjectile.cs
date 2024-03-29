﻿using UnityEngine;
using System.Collections;

public class SimpleProjectile : Projectile, ITakeDamage {

    public int Damage;
    public GameObject DestroyedEffect;
    public int PointsToGivePlayer;
    public float TimeToLive;
    public AudioClip ProjectileDestroySound;

    public void Update()
    {
        if((TimeToLive -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
            return;
        }
        transform.Translate(Direction * ((Mathf.Abs(InitialVelocity.x) + Speed) * Time.deltaTime), Space.World);
    } 

    public void TakeDamage(int damage, GameObject instigator)
    {
        if(PointsToGivePlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if(projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameMaster.Instance.AddPoints(PointsToGivePlayer);
                FloatingText.Show(string.Format("+{0}!", PointsToGivePlayer), "CorrectAnswerText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50)); 
            }
        }
        DestroyProjectile();
    }

    protected override void OnCollideOther(Collider2D other)
    {
        DestroyProjectile();
    }

    protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (DestroyedEffect != null)
        {
            Instantiate(DestroyedEffect, transform.position, transform.rotation);

            SoundManager.Instance.PlayClip3D(ProjectileDestroySound, transform.position);
            Destroy(gameObject);
        }
    }

}
