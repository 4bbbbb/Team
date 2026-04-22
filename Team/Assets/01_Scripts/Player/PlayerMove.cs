using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("----- Move -----")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float deadZone = 0.1f;

    private bool bRun;

    private Vector2 moveInput = Vector2.zero;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        Awake_BindInput();
    }

    private void Awake_BindInput()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputActionMap actionMap = playerInput.actions.FindActionMap("Player");

        // Move
        {
            InputAction action = actionMap.FindAction("Move");
            action.performed += context => moveInput = context.ReadValue<Vector2>();
            action.canceled += context => moveInput = Vector2.zero;
        }

        // Run
        {
            InputAction action = actionMap.FindAction("Sprint");
            action.performed += context => bRun = true;
            action.canceled += context => bRun = false;
        }
        
    }

    private Vector2 velocity;
    private Vector2 curMoveInput;

    private void Update()
    {
        // 현재 moveInput 값 (보간 처리)
        // sensitivity의 값을 더 빠르게 처리하기 위해 나눠주는 것
        curMoveInput = Vector2.SmoothDamp(curMoveInput, moveInput, ref velocity, 1f / sensitivity);

        Vector3 dir = Vector3.zero;

        // 뛰는 상태일 때 속도 변화
        float curSpeed = bRun ? runSpeed : walkSpeed;

        // input값이 deadZone보다 클 경우에만 움직임
        if (moveInput.magnitude > deadZone)
        {
            dir = (curMoveInput.x * Vector3.right) + (curMoveInput.y * Vector3.forward);
            dir = dir.normalized * curSpeed;
        }
        else
            dir = Vector3.zero;

        transform.Translate(dir * Time.deltaTime);
        animator.SetFloat("SpeedZ", dir.magnitude);
    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        GUILayout.Label(curMoveInput.ToString());
    }
}
