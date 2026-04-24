using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("----- Move -----")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float sensitivity = 10f;
    [SerializeField] private float deadZone = 0.1f;

    [SerializeField] private GameObject footStepPrefab;
    [SerializeField] private Transform footPos;
    [SerializeField] private float footStepSpan = 1f;

    private bool bRun;
    Coroutine runRoutine;

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

    private void Reset()
    {
        //footPos = this.transform.Find("Player_FootStep");
        //Debug.Assert(footPos != null, "FootPos is null");
    }

    private Vector2 velocity;
    private Vector2 curMoveInput;
    private Vector3 lookDir;

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
            lookDir = dir.normalized;
            dir = dir.normalized * curSpeed;
        }
        else
            dir = Vector3.zero;

        // 이동
        transform.position += dir * Time.deltaTime;

        // 움직임 회전
        if (lookDir.magnitude > deadZone)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f / sensitivity);
        }

        // 뛸 때만 footStep 효과 생성
        if (runRoutine == null)
            runRoutine = StartCoroutine(RunRoutine());

        if (bRun == false)
        {
            StopCoroutine(RunRoutine());
            runRoutine = null;
        }

        // 애니메이션
        animator.SetFloat("SpeedZ", dir.magnitude);
    }

    // 뛸 때 footStep 생성 처리
    private IEnumerator RunRoutine()
    {
        while (bRun == true)
        {
            GameObject footStep = Instantiate(footStepPrefab, footPos.position, Quaternion.identity, footPos);
            Destroy(footStep, 0.5f);
            yield return new WaitForSeconds(footStepSpan);
        }
    }

    //private void OnGUI()
    //{
    //    GUI.color = Color.red;
    //    GUILayout.Label(curMoveInput.ToString());
    //}
}
