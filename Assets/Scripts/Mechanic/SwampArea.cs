using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                Player player = collision.GetComponent<Player>();
                if (player != null)
                {
                    player.moveSpeed = player.moveSpeed - 5f;
                    player.jumpPower = player.jumpPower - 5f;
                    player.Animator.speed = 0.5f;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                Player player = collision.GetComponent<Player>();
                if (player != null)
                {
                    player.moveSpeed = player.moveSpeed + 5f;
                    player.jumpPower = player.jumpPower + 5f;
                    player.Animator.speed = 1f;
                }
            }
        }
    }
}
