using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int InputX = Animator.StringToHash("InputX");
    private static readonly int InputY = Animator.StringToHash("InputY");
    private Rigidbody2D rb;
    public float speed;
    private float inputX;
    private float inputY;
    private Vector2 movementInput;
    private Animator[] animators;
    private bool isMoving;
    private bool inputDisable;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }
    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
        
    }

    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputDisable = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        
    }
    private void Update()
    {
        if (inputDisable==false)
        {
            PlayerInput();
        }
        
        SwitchAnimation();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        if (inputY!=0&&inputX!=0)
        {
            inputX *= 0.6f;
            inputY *= 0.6f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX *= 0.5f;
            inputY *= 0.5f;
        }
        movementInput = new Vector2(inputX, inputY);
        isMoving = movementInput != Vector2.zero;
    } 
    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.fixedDeltaTime);
    }

    private void SwitchAnimation()
    {
        foreach (var animator in animators)
        {
            animator.SetBool(IsMoving, isMoving);
            if (isMoving)
            {
                animator.SetFloat(InputX, inputX);
                animator.SetFloat(InputY, inputY);
            }
        }
    }
}
