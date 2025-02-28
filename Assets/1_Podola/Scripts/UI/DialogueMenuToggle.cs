using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween 플러그인 필요

public class DialogueMenuToggle : MonoBehaviour
{
    public Button toggleButton;        // 우측 상단 토글 버튼
    public Image toggleImage;          // 토글 버튼의 이미지 컴포넌트
    public Sprite offSprite;           // 접힌 상태 이미지
    public Sprite onSprite;            // 펼쳐진 상태 이미지
    public GameObject[] menuButtons;   // 펼쳐질 메뉴 버튼들
    public float animationDuration = 0.3f; // 애니메이션 지속 시간

    // 각 메뉴 버튼이 펼쳐질 목표 위치 (anchoredPosition)
    public Vector2[] openPositions;    

    private bool isOpen = false;

    void Start()
    {  
        toggleButton.onClick.AddListener(ToggleMenu);
        // 초기 상태: 메뉴 버튼 비활성화
        foreach (GameObject btn in menuButtons)
        {
            btn.SetActive(false);
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        toggleImage.sprite = isOpen ? onSprite : offSprite;

        if (isOpen)
        {
            // 펼칠 때: 메뉴 버튼들을 활성화 후 애니메이션 실행
            for (int i = 0; i < menuButtons.Length; i++)
            {
                GameObject btn = menuButtons[i];
                btn.SetActive(true);
                RectTransform rt = btn.GetComponent<RectTransform>();
                // 시작 위치를 토글 버튼 위치로 설정
                rt.anchoredPosition = toggleImage.rectTransform.anchoredPosition;
                // 목표 위치로 이동
                rt.DOAnchorPos(openPositions[i], animationDuration);
            }
        }
        else
        {
            // 접힐 때: 메뉴 버튼들을 토글 버튼 위치로 모은 후 비활성화
            for (int i = 0; i < menuButtons.Length; i++)
            {
                GameObject btn = menuButtons[i];
                RectTransform rt = btn.GetComponent<RectTransform>();
                // 토글 버튼 위치로 이동 후 비활성화
                rt.DOAnchorPos(toggleImage.rectTransform.anchoredPosition, animationDuration)
                  .OnComplete(() => { btn.SetActive(false); });
            }
        }
    }
}
