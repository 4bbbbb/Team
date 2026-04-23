using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [Header("----- Camera Mode -----")]
    [SerializeField] private Vector3 followZoom = new Vector3(0, 3, -3);
    [SerializeField] private Vector3 followInit = new Vector3(0, 5.5f, -4.1f);
    [SerializeField] float sensitivity = 10f;

    private CinemachineFollow follow;

    private bool bZoom = false;

    private Vector2 scroll = Vector2.zero;

    private void Awake()
    {
        InputActionMap actionMap = playerInput.actions.FindActionMap("Player");

        follow = GetComponent<CinemachineFollow>();

        // Zoom
        {
            InputAction action = actionMap.FindAction("Zoom");
            action.performed += context => scroll = context.ReadValue<Vector2>();
            action.canceled += context => follow.FollowOffset = followInit;
        }
    }

    private void Update()
    {
        Zoom();
    }

    private Vector3 velocity;

    private void Zoom()
    {
        float scrollY = scroll.y;

        if (scrollY > 0.1f)
            follow.FollowOffset = Vector3.SmoothDamp(follow.FollowOffset, followZoom, ref velocity, 1 / sensitivity);

        else
            follow.FollowOffset = Vector3.SmoothDamp(follow.FollowOffset, followInit, ref velocity, 1 / sensitivity);

    }

    private void OnGUI()
    {
        GUI.color = Color.red;
        GUILayout.Label(scroll.ToString());
    }
}
