
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayPlatformController : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private InputAction inputAction;

    void Start()
    {
        inputAction.Enable();
    }


    void Update()
    {
        if (IsPressingDown())
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformColider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformColider);
        yield return new WaitForSeconds(0.25f);

        Physics2D.IgnoreCollision(playerCollider, platformColider, false);
    }

    private bool IsPressingDown()
    {
        return inputAction.IsPressed();
    }

}
