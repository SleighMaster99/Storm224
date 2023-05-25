using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumPlayerControlable : Controlable
{
    [Header("Components")]
    [SerializeField]
    private PauseUI pauseUI;                  // Pause UI Script
    [SerializeField]
    private GameManager gameManager;

    [Header("Speed Property")]
    public float moveSpeed;                     // Player 현재 움직임 속도
    [SerializeField]
    private float walkSpeed;                    // 걸을 때 속도
    [SerializeField]
    private float runSpeed;                     // 달릴 때 속도
    [SerializeField]
    private bool isRun = false;                 // 달리기 여부
    [SerializeField]
    private bool isLShiftDown = false;          // LShift 누름 여부
    public bool isMove = false;                 // 움직임 여부
    public float footSoundTerm;                 // 발소리 텀
    [SerializeField]
    private float walkTerm;
    [SerializeField]
    private float runTerm;

    [Header("Mouse Sensitive")]
    public float rotateXSpeed;                  // 마우스 가로 감도
    public float rotateYSpeed;                  // 마우스 수직 감도

    private Vector3 smoothRotation;
    private Vector3 currentVelocity;

    // 마우스 움직인 값
    private float rotateX;                      // 마우스 가로 움직임
    private float rotateY;                      // 마우스 세로 움직임

    [Header("Jump")]
    [SerializeField]
    private Rigidbody playerRigidbody;          // rigidbody
    [SerializeField]
    private float jumpPower;                    // 점프 AddForce 수치
    public bool isJump;                         // 점프 중인지 True : 점프중, False : 땅에 있음
    private bool isSpaceBar;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        moveSpeed = walkSpeed;

        rotateX = 0;
        rotateY = this.transform.eulerAngles.y;

        GameManager.optionUI.SensitiveInitalize();

        gameManager.PauseGame(false);
    }

    // 커서 On/off - Tab
    public override void KeyboardESC()
    {
        gameManager.PauseGame(!GameManager.isPause);
    }

    // 달리기 - LShift
    public override void LShift()
    {
        isLShiftDown = !isLShiftDown;
        
        if (isMove && isLShiftDown)
        {
            isRun = true;

            moveSpeed = runSpeed;
            footSoundTerm = runTerm;
        }
        else
        {
            isRun = false;

            moveSpeed = walkSpeed;
            footSoundTerm = walkTerm;
        }
    }

    // Player 움직임 - WASD
    public override void Move(Vector2 input)
    {
        Vector3 lookForward = new Vector3(mainCamera.forward.x, 0, mainCamera.forward.z).normalized;
        Vector3 lookRight = new Vector3(mainCamera.right.x, 0, mainCamera.right.z).normalized;
        Vector3 moveDir = lookForward * input.y + lookRight * input.x;
        moveDir = moveDir * moveSpeed * Time.deltaTime;

        if (input.magnitude != 0)
        {
            isMove = true;
            transform.position += moveDir;
        }
        else
        {
            isMove = false;
            isRun = false;
        }
    }

    // 마우스 움직임
    public override void Rotate(Vector2 input)
    {
        if (mainCamera != null)
        {
            float inputX = input.x * rotateXSpeed * 100 * Time.deltaTime;
            float inputY = input.y * rotateYSpeed * 100 * Time.deltaTime;
            rotateX += -inputY;
            rotateY += inputX;

            rotateX = Mathf.Clamp(rotateX, -70.0f, 50.0f);

            smoothRotation = Vector3.SmoothDamp(smoothRotation, new Vector3(rotateX, rotateY), ref currentVelocity, 0.1f);

            this.transform.eulerAngles = new Vector3(0, rotateY, 0);
            mainCamera.transform.eulerAngles = smoothRotation;
        }
    }

    // 점프 - SpaceBar
    public override void SpaceBar()
    {
        isSpaceBar = !isSpaceBar;

        if (isSpaceBar)
        {
            if (!isJump)
                playerRigidbody.AddForce(this.transform.up * jumpPower);

            isJump = true;
        }
    }

    public override void Interact() { }
    public override void Keyboard1() { }
    public override void Keyboard2() { }
    public override void Keyboard3() { }
    public override void Keyboard4() { }
    public override void Keyboard5() { }
    public override void KeyboardC() { }
    public override void KeyboardR() { }
    public override void KeyboardZ() { }
    public override void LMB() { }
    public override void RMB() { }
}
