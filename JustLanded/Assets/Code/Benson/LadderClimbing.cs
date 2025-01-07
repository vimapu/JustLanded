// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class LadderClimbing : MonoBehaviour
// {

//     private bool canClimb;
//     private float vertical;
//     private float speed = 8f;
//     [SerializeField] InputAction climbingAction;

//     [SerializeField] Rigidbody2D rigidbody;

//     private float gravityScale;

//     // Start is called before the first frame update
//     void Start()
//     {
//         climbingAction.Enable();
//         canClimb = false;
//         gravityScale = GetComponent<Rigidbody2D>().gravityScale;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (canClimb)
//         {
//             vertical = climbingAction.ReadValue<float>();
//         }
//     }

//     void FixedUpdate()
//     {
//         if (canClimb)
//         {
//             rigidbody.velocity = new Vector2(rigidbody.velocity.x, vertical * speed);
//         }
//     }

//     public void EnableClimbing()
//     {
//         canClimb = true;
//         rigidbody.gravityScale = 0f;
//     }

//     public void DisableClimbing()
//     {
//         canClimb = false;
//         rigidbody.gravityScale = gravityScale;
//     }



// }
