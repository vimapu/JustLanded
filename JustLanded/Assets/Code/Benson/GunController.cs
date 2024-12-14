using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{


    [SerializeField] InputAction inputAction;
    [SerializeField] GameObject pistol;
    [SerializeField] float secondBetweenShots;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawnPoint;

    private GameObject bulletInstance;

    private Vector2 direction;

    private bool isFlipped;
    private bool isInputPressed = false;
    private float lastShotTime;
    private bool isGunActive = false;

    // Start is called before the first frame update
    void Start()
    {
        inputAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        direction = inputAction.ReadValue<Vector2>();
        isInputPressed = inputAction.IsPressed();
        if (isFlipped)
        {
            // direction = -direction;
        }
    }

    void FixedUpdate()
    {
        if (isGunActive)
        {
            if (isInputPressed)
            {
                UpdateGunRotation();
            }
            if (Gamepad.current.rightTrigger.isPressed)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if (lastShotTime != null && CanShootAnotherBullet())
        {
            lastShotTime = Time.time;
            // instantiate bullet and shoot

            bulletInstance = Instantiate(bullet, bulletSpawnPoint.position, pistol.transform.rotation);
        }
    }

    private bool CanShootAnotherBullet()
    {
        return (Time.time - lastShotTime) > secondBetweenShots;
    }
    private void UpdateGunRotation()
    {
        var aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (isFlipped)
        {
            aimAngle -= 180;
        }
        pistol.transform.rotation = Quaternion.Euler(0, 0, aimAngle);

        Vector3 localScale = new Vector3(0.5f, pistol.transform.localScale.y, 1f);

        Debug.Log("angle " + aimAngle);
        // if (!isFlipped)
        // {
        if (aimAngle != 0)
        {
            if (aimAngle > 90 || aimAngle < -90)
            {
                localScale.y = -0.5f;
            }
            else
            {
                localScale.y = 0.5f;
            }
        }
        //}
        pistol.transform.localScale = localScale;
        //Debug.Log("Direction " + direction + " localScale " + localScale);

    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        //pistol.transform.localScale = new Vector3(0.5f, -pistol.transform.localScale.y, 1f);
    }

    public void ActivateGun()
    {
        isGunActive = true;
    }

    public void DeactivateGun()
    {
        isGunActive = false;
    }
}
