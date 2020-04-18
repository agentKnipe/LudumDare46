using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWorldController : MonoBehaviour{
    public Animator Animator;
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start(){
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update(){

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        Look(Input.GetAxis("Horizontal"));

        moveDirection *= speed;

        // Move the controller
        if(moveDirection != Vector3.zero) {
            //Look(moveDirection);
            characterController.Move(moveDirection * Time.deltaTime);
            Animator.SetBool("IsWalking", true);
        }
        else {
            Animator.SetBool("IsWalking", false);
        }
    }

    private void Look(float direction) {
        if (direction > 0) { // Look to the right
            transform.localScale = new Vector3(1.5f, transform.localScale.y, transform.localScale.z);
        }
        else if (direction < 0){ // look to the left
            transform.localScale = new Vector3(-1.5f, transform.localScale.y, transform.localScale.z);
        }
    }
}
