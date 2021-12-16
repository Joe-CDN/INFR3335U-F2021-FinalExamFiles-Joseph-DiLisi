using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;

public class OnlineMove : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public Transform head;
    public LayerMask groundMask;

    public float groundDistance = 0.4f;
    public Transform cam;
    public Joystick joystick;
    public PhotonView view;
    
    //public bool is3rdPerson = true;
    public static bool isGrabbing;
    Vector3 moveDir;

    float horizontal;
    float vertical;
    public float speed = 6f;
    public float gravity = -9.81f;

    Vector3 velocity;    
    public Vector3 camOffset;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool isGrounded;
    
    // Update is called once per frame
    void Update()
    {
        if(view.IsMine == true){
            //cam.position = new Vector3(cam.position.x, 3, this.transform.position.z - 1);
            //cam.rotation = Quaternion.Euler(40f, 0f, 0f);

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if(isGrounded && velocity.y < 0){
                velocity.y = -1f;
            }

            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);

                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }
        }
    }
    public void SetData(GameObject cameraParent) {
        Joystick[] joys = cameraParent.GetComponentsInChildren<Joystick>();
        foreach (Joystick joy in joys)
        {
            if (joy.name == "MoveControl")
                joystick = joy;
        }
        CinemachineVirtualCamera brain = cameraParent.GetComponentInChildren<CinemachineVirtualCamera>();
        brain.LookAt = head;
        //brain.Follow = this.transform; 
        brain.transform.position = new Vector3(0, 3, -1);
        cameraParent.transform.position = new Vector3(0, 4.5f, -1);
        cam = cameraParent.transform;
        cam.transform.position = new Vector3(0, 4.5f, -1);
        cam.transform.parent = this.transform;        
    }
}
