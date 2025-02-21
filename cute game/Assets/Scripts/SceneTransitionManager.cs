using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public Image fadeImage; // 페이드 효과를 위한 UI Image (검은색)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ✅ 1. 일반 씬 전환
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // ✅ 2. 비동기 씬 전환 (로딩 화면 활용 가능)
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading Progress: " + (asyncLoad.progress * 100) + "%");
            yield return null;
        }
    }

    // ✅ 3. 현재 씬 다시 로드
    public void ReloadCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    // ✅ 4. 페이드 효과 적용 씬 전환
    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        float alpha = 0;
        fadeImage.gameObject.SetActive(true); // 페이드 이미지 활성화

        // 점점 어두워짐
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        // 점점 밝아짐
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // 페이드 이미지 비활성화
    }
}
