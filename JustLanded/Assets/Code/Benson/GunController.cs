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

    private MovementController movementController;

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
        movementController = GetComponent<MovementController>();
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
            if (movementController.IsFacingRight())
            {
                bulletInstance = Instantiate(bullet, bulletSpawnPoint.position, pistol.transform.rotation);
            }
            else
            {
                var rotation = pistol.transform.rotation.eulerAngles;
                Debug.Log(rotation);
                var eulerRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z - 180);
                //var eulerRotation = Quaternion.Euler(rotation.x, rotation.y -180, rotation.z);
                bulletInstance = Instantiate(bullet, bulletSpawnPoint.position, eulerRotation);
            }
        }
    }

    private bool CanShootAnotherBullet()
    {
        return (Time.time - lastShotTime) > secondBetweenShots;
    }
    private void UpdateGunRotation()
    {

        var aimAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Debug.Log("Aiming at " + direction + " with aiming angle " + aimAngle);
        //if (isFlipped)
        if (movementController.IsFacingRight())
        {
            if (direction.x >= 0)
            {
                pistol.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
            }
            //aimAngle -= 180;
        }
        else
        {
            if (direction.x <= 0)
            {
                pistol.transform.rotation = Quaternion.Euler(0, 0, aimAngle - 180);
            }
            //pistol.transform.localScale = -pistol.transform.localScale;
        }


        // Vector3 localScale = new Vector3(0.5f, pistol.transform.localScale.y, 1f);

        // //Debug.Log("angle " + aimAngle);
        // // if (!isFlipped)
        // // {
        // if (aimAngle != 0)
        // {
        //     if (aimAngle > 90 || aimAngle < -90)
        //     {
        //         localScale.y = -0.5f;
        //     }
        //     else
        //     {
        //         localScale.y = 0.5f;
        //     }
        // }
        //}
        //pistol.transform.localScale = localScale;
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
