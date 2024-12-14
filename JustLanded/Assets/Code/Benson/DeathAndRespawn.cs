using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAndRespawnController : MonoBehaviour
{

    Rigidbody2D rigidbody;
    Collider2D collider;
    private bool isDead = false;
    private Vector2 respawnPosition;
    private BashController bashController;

    [SerializeField] float respawnJumpSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        bashController = GetComponent<BashController>();
        SetRespawnPosition(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
    }

    public void SetRespawnPosition(Vector2 position)
    {
        respawnPosition = position;
    }

    private void Jump()
    {
        //rigidbody.velocity = new Vector2(rigidbody.velocity.x, respawnJumpSpeed);
        rigidbody.velocity = new Vector2(0, respawnJumpSpeed);
    }

    public void Die()
    {
        isDead = true;
        collider.enabled = false;
        StartCoroutine(Respawn());
    }

    public void Kill(IKillable killer)
    {
        if (bashController.IsBashing())
        {
            killer.Kill();
        }
        else
        {
            Die();
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.2f);
        transform.position = (Vector2)respawnPosition;
        collider.enabled = true;
        isDead = false;
        Jump();
        //Debug.Log("Respawning from position " + transform.position);
    }
}
