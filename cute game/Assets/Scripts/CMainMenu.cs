using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CMainMenu : MonoBehaviour
{
    // 버튼 필드
    public Button buttonPlay; // 게임 시작 버튼
    public Button buttonQuit; // 게임 종료 버튼

    private AudioSource audioSource; // AudioSource 변수
    public AudioClip backgroundMusic; // 재생할 배경 음악

    // 메인 화면 이미지 GameObject 필드
    public GameObject mainMenuImage; // 메인 화면 이미지 GameObject

    void Awake()
    {
        // 버튼 클릭 이벤트 설정
        if (buttonPlay != null)
        {
            buttonPlay.onClick.AddListener(OnPlay);
        }
        else
        {
            Debug.LogWarning("Play button not assigned!");
        }

        if (buttonQuit != null)
        {
            buttonQuit.onClick.AddListener(OnQuit);
        }
        else
        {
            Debug.LogWarning("Quit button not assigned!");
        }

        // 초기 상태에서 비활성화는 하지 않음 (이것은 버튼 클릭 시 비활성화 처리)
    }

    void OnPlay()
    {
        // 게임 화면으로 이동 전에 UI 요소들을 비활성화
        Debug.Log("Starting Game...");

        if (mainMenuImage != null)
        {
            mainMenuImage.SetActive(false); // 메인 화면 이미지 비활성화
        }

        if (buttonPlay != null)
        {
            buttonPlay.gameObject.SetActive(false); // 게임 시작 버튼 비활성화
        }

        if (buttonQuit != null)
        {
            buttonQuit.gameObject.SetActive(false); // 게임 종료 버튼 비활성화
        }

        // 게임 시작: 지정한 게임 Scene으로 이동
        // "GameScene"을 실제 게임 Scene 이름으로 변경


        // AudioSource 컴포넌트 가져오기 또는 추가하기
            SceneManager.LoadScene("GameScene");
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // AudioClip을 AudioSource에 설정
            if (backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;

                // 반복 재생 설정
                audioSource.loop = true;

                // 자동으로 음악 재생
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("BackgroundMusic: AudioClip이 설정되지 않았습니다!");
            }
        }
    




void OnQuit()
    {
        // 게임 종료
        Debug.Log("Quit Game");
        Application.Quit();
    }
}


