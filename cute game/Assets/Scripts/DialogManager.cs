using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    
    public GameObject dialogueBox;        // 대화 상자 (활성/비활성용)
    public TextMeshProUGUI TestText;  // 커서 깜빡임용 텍스트

    private List<string> dialogues = new List<string>(); // 대화 목록
    private int index = 0;  // 현재 대사 인덱스
    private bool isTyping = false; // 텍스트 애니메이션 체크
    private bool isTypingCursor = false; // 커서 깜빡임 체크

    private string typingText;
    private char cursor_char = '|';

    void Start()
    {
        // 샘플 대사 추가
        dialogues.Add("병순이 : 안녕?\n내 이름은 병순이야.");
        dialogues.Add("곰순이 : 안녕.\n 내 이름은 곰순이야.");
        dialogues.Add("병순이: 어디를 그렇게 헐레벌떡 급하게 가는거니?");
        dialogues.Add("곰순이 : 그게 나는..");
        dialogues.Add("병순이 : 지금 당장 말해주지 않아도 돼.");
        dialogues.Add("두 동물은 서로 친구 사이이다.동물들의 마을에서 두 사람은 어릴적\n" +
            "같은 학교에서 만나 친구 사이를 돈독히 해왔다.");
        dialogues.Add("병순이 : 오늘도 카페일에 바쁘지?나도 도와줄께.");
        dialogues.Add("곰순이 : 응");
        dialogues.Add("곰순이는 카페 티어스의 사장이다.눈물이 날 정도로 맛있는 빵과\n" +
            "커피를 판다는 뜻에서 지은 이름이다.");


        RectTransform rt = TestText.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(800, rt.sizeDelta.y);



        ShowDialogue(); // 대화 시작
    }

    void Update()
    {
        // Enter 키를 누르면 다음 대화 출력
        if (Input.GetKeyDown(KeyCode.Return) && !isTyping)
        {
            NextDialogue();
        }
    }

    void ShowDialogue()
    {
        index = 0;
        dialogueBox.SetActive(true); // 대화창 활성화
        StartCoroutine(TypeText(dialogues[index])); // 첫 번째 대사 출력
    }

    void NextDialogue()
    {
        if (isTyping) return; // 현재 타이핑 중이라면 무시

        index++;

        if (index < dialogues.Count)
        {
            StartCoroutine(TypeText(dialogues[index])); // 다음 대사 출력
        }
        else
        {
            dialogueBox.SetActive(false); // 모든 대사 종료 시 창 닫기
        }
    }

    IEnumerator TypingCoroutine()
    {
        isTypingCursor = true;
        while (isTypingCursor)
        {
            if (TestText != null)
            {
                TestText.text = typingText + cursor_char; /*내가 한 부분: 커서 깜빡이는 기능 추가*/
                yield return new WaitForSeconds(0.25f); 
                TestText.text = typingText;
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        TestText.text = "";
        typingText = "";

        foreach (char letter in text)
        {
            TestText.text += letter;
            typingText += letter;
            yield return new WaitForSeconds(0.03f); // 한 글자씩 출력
        }

        isTyping = false;

        // 커서 깜빡임 시작
        if (TestText != null)
        {
            StartCoroutine(TypingCoroutine());
        }
    }
}

