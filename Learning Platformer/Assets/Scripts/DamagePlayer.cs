using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour {

    public int DamageToGive = 10;

    private Vector2
        lastPositon,
        velocity;

    public void LateUpdate()
    {
        velocity = (lastPositon - (Vector2)transform.position) / Time.deltaTime;
        lastPositon = transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.TakeDamage(DamageToGive, gameObject);
        var controller = player.GetComponent<PlayerController>();
        var totalVelocity = controller.Velocity + velocity;

        controller.SetForce(new Vector2(
            -1 * Mathf.Sign(totalVelocity.x) * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 6, 10, 40),
            -1 * Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 2, 0, 15)));

        }
    }

