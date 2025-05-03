using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FadeCanvasController : MonoBehaviour
{
    [Header("Scene & Timing")]
    [SerializeField] private string sceneToLoad = "Town";         
    [SerializeField] private float fadeDuration = 1f;             
    [SerializeField] private float loadingAnimationTime = 3f;     

    [Header("UI References")]
    [SerializeField] private Image fadeImage;                     
    [SerializeField] private TMP_Text loadingText;                

    private bool isAnimatingDots = false;                         

    private void Awake()
    {
        
        Color fadeColor = fadeImage.color;
        fadeColor.a = 0f;
        fadeImage.color = fadeColor;

        
        Color textColor = loadingText.color;
        textColor.a = 0f;
        loadingText.color = textColor;
    }

    
    public void StartFade()
    {
        StartCoroutine(FadeOutAndLoad());
    }

    private IEnumerator FadeOutAndLoad()
    {
        
        float elapsedTime = 0f;
        Color startFadeColor = fadeImage.color;
        Color endFadeColor = new Color(startFadeColor.r, startFadeColor.g, startFadeColor.b, 1f); 

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = Color.Lerp(startFadeColor, endFadeColor, t);
            yield return null;
        }

        
        float textFadeTime = 0.5f;
        float textElapsed = 0f;
        Color startTextColor = loadingText.color;
        Color endTextColor = new Color(startTextColor.r, startTextColor.g, startTextColor.b, 1f); 

        while (textElapsed < textFadeTime)
        {
            textElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(textElapsed / textFadeTime);
            loadingText.color = Color.Lerp(startTextColor, endTextColor, t);
            yield return null;
        }

        
        isAnimatingDots = true;
        StartCoroutine(AnimateLoadingDots());

        
        yield return new WaitForSeconds(loadingAnimationTime);

        
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator AnimateLoadingDots()
    {
        string baseText = "Loading";
        int dotCount = 0;

        while (isAnimatingDots)
        {
            dotCount = (dotCount + 1) % 4; 
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(0.4f); 
        }
    }

    private void OnDisable()
    {
        isAnimatingDots = false; 
    }
}
