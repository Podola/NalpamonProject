using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public InputActionReference moveAction; // 이동 액션
    public InputActionReference interactAction; // 상호작용 액션
    public LayerMask interLayer; // 상호작용 가능한 오브젝트의 레이어
    public float interRange = 2f; // 상호작용 범위

    private Vector2 moveInput; // 이동 입력 값
    private GameObject currentInteractable; // 현재 상호작용 가능한 오브젝트
    private GameObject currentInterKey; // 현재 활성화된 InterKey
    public UnityEvent interactEvent; // f키 눌렸을때 이벤트 따로 추가 필요

    private void OnEnable()
    {
        if (moveAction == null || interactAction == null)
        {
            Debug.LogError("InputActionReference가 할당되지 않았습니다.");
            return;
        }
        // 액션 활성화 및 콜백 등록
        moveAction.action.Enable();
        interactAction.action.Enable();

        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
        interactAction.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        // 액션 비활성화 및 콜백 해제
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
        interactAction.action.performed -= OnInteract;

        moveAction.action.Disable();
        interactAction.action.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // 이동 입력 값 저장
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // F 키를 눌렀을 때 실행될 로직
        if (currentInteractable != null)
        {
            Debug.Log("F 키를 누름");
            // 여기에 상호작용 로직을 추가
        }
    }

    private void Update()
    {
        // 이동 처리
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
        // 상호작용 가능한 오브젝트 감지
        DetectInteractable();
    }

    private void DetectInteractable()
    {
        // 레이캐스트를 사용해 상호작용 가능한 오브젝트 감지
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + new Vector3(0, 1.5f, 0), interRange, Vector2.right, 0f, interLayer);

        if (hit.collider != null)
        {
            // 새로운 오브젝트가 감지된 경우
            if (currentInteractable != hit.collider.gameObject)
            {
                // 이전 오브젝트의 InterKey 비활성화
                if (currentInterKey != null)
                {
                    currentInterKey.SetActive(false);
                }

                // 새로운 오브젝트 저장 및 InterKey 활성화
                currentInteractable = hit.collider.gameObject;
                currentInterKey = FindInterKey(currentInteractable);

                if (currentInterKey != null)
                {
                    currentInterKey.SetActive(true);
                }
            }
        }
        else
        {
            // 감지된 오브젝트가 없는 경우
            if (currentInterKey != null)
            {
                currentInterKey.SetActive(false);
                currentInteractable = null;
                currentInterKey = null;
            }
        }
    }

    private GameObject FindInterKey(GameObject interactable)
    {
        // InterKey를 찾기 위해 모든 자식 오브젝트를 탐색
        foreach (Transform child in interactable.transform)
        {
            if (child.name == "InterKey")
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1.5f, 0), interRange);
    }
}