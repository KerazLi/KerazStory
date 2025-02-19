using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* @Program:PlayerController.cs
* @Author: Keraz
* @Description: 主角的控制功能
* @Date: 2025年02月19日 星期三 19:37:47
*/


public class PlayerController : MonoBehaviour
{
    // Animator参数的哈希值，用于提高性能
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int InputX = Animator.StringToHash("InputX");
    private static readonly int InputY = Animator.StringToHash("InputY");
    
    // 角色的刚体组件，用于物理模拟
    private Rigidbody2D rb;
    
    // 移动速度的公共属性，允许在Unity编辑器中调整
    public float speed;
    
    // 存储每帧的输入值
    private float inputX;
    private float inputY;
    
    // 存储每帧的移动输入向量
    private Vector2 movementInput;
    
    // 动画控制器数组，用于更新动画状态
    private Animator[] animators;
    
    // 表示角色是否正在移动
    private bool isMoving;
    
    // 表示输入是否被禁用
    private bool inputDisable;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }
   
    private void OnEnable()
    {
        // 注册场景卸载前的事件处理函数
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        // 注册场景加载完成后的事件处理函数
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadedEvent;
        // 注册移动到指定位置的事件处理函数
        EventHandler.MoveToPosition += OnMoveToPosition;
    }

    /// <summary>
    /// 当移动到指定位置时调用此方法
    /// </summary>
    /// <param name="targetPosition">要移动到的目标位置</param>
    private void OnMoveToPosition(Vector3 targetPosition)
    {
        // 设置游戏对象的位置为指定的目标位置
        transform.position = targetPosition;
    }
    
    /// <summary>
    /// 在场景加载完成后调用此方法
    /// </summary>
    private void OnAfterSceneLoadedEvent()
    {
        // 场景加载完成后，启用输入
        inputDisable = false;
    }
    
    /// <summary>
    /// 在场景卸载前调用此方法
    /// </summary>
    private void OnBeforeSceneUnloadEvent()
    {
        // 场景卸载前，禁用输入
        inputDisable = true;
    }

    
    private void OnDisable()
    {
        // 取消订阅场景卸载前的事件处理函数
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        // 取消订阅场景加载完成后的事件处理函数
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadedEvent;
        // 取消订阅移动到指定位置的事件处理函数
        EventHandler.MoveToPosition -= OnMoveToPosition;
    }
    /// <summary>
    /// 在每帧更新时调用，处理玩家输入和动画状态切换。
    /// </summary>
    private void Update()
    {
        // 仅在玩家输入未被禁用时，处理玩家输入
        if (!inputDisable)
        {
            PlayerInput();
        }
        else
        {
            isMoving = false;
        }

        // 更新角色动画状态
        SwitchAnimation();
    }
    
    /// <summary>
    /// 在每帧固定更新时调用，主要用于角色移动更新。
    /// </summary>
    private void FixedUpdate()
    {
        // 仅在玩家输入未被禁用时，执行角色移动逻辑
        if (!inputDisable)
        {
            Movement();
        }
    }

    /// <summary>
    /// 处理玩家输入的函数
    /// </summary>
    private void PlayerInput()
    {
        // 获取玩家的水平和垂直输入
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    
        // 如果玩家同时在水平和垂直方向上有输入，减少输入值以限制对角移动速度
        if (inputY != 0 && inputX != 0)
        {
            inputX *= 0.6f;
            inputY *= 0.6f;
        }
    
        // 如果玩家按下左Shift键，减慢移动速度
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX *= 0.5f;
            inputY *= 0.5f;
        }
    
        // 创建一个包含输入方向的Vector2对象
        movementInput = new Vector2(inputX, inputY);
        // 判断玩家是否在移动
        isMoving = movementInput != Vector2.zero;
    }
    
    /// <summary>
    /// 更新玩家位置的函数
    /// </summary>
    private void Movement()
    {
        // 根据输入、速度和固定的时间间隔来移动玩家的位置
        rb.MovePosition(rb.position + movementInput * speed * Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// 切换动画状态的函数
    /// </summary>
    private void SwitchAnimation()
    {
        // 遍历所有动画控制器并更新它们的移动状态
        foreach (var animator in animators)
        {
            animator.SetBool(IsMoving, isMoving);
            // 如果玩家在移动，更新动画控制器的移动方向
            if (isMoving)
            {
                animator.SetFloat(InputX, inputX);
                animator.SetFloat(InputY, inputY);
            }
        }
    }
}
