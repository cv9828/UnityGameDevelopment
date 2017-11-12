using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public class ScreenFaderEventArgs : System.EventArgs
    {

    }

    public delegate void EventHandler(object sender, ScreenFaderEventArgs args);
    public event EventHandler OnScreenFadedToBlack;
    public event EventHandler OnScreenCleared;

    public static ScreenFader instance
    {
        get;
        private set;
    }

    #region Inspector

    /// <summary>
    /// The fade speed
    /// </summary>
    [Tooltip("The fade speed")]
    public float fadeSpeed = 1.5f;

    #endregion Insector

    private ScreenFaderEventArgs screenFaderEventArgs = new ScreenFaderEventArgs();

    /// <summary>
    /// Raw Image component used to fade screen
    /// </summary>
    private RawImage blackImage;

    private bool canFadeToBlack;
    private bool canFadeSound = true;

    #region Unity Engine & Events

    private void Awake()
    {
        Singleton();
        blackImage = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartScene();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    #endregion Unity Engine & Events

    /// <summary>
    /// Fade to clear at the start of a scene
    /// </summary>
    private void StartScene()
    {
        blackImage.enabled = true;
        blackImage.color = Color.black;
        StartCoroutine(FadeToClear());
        FadeSoundOn();
    }

    /// <summary>
    /// Fade to black and load a new level
    /// </summary>
    /// <param name="levelToLoad">The name of the level to load</param>
    public void LoadLevel(string levelToLoad)
    {
        if (canFadeToBlack)
        {
            canFadeToBlack = false;
            blackImage.enabled = true;
            blackImage.color = Color.clear;
            StartCoroutine(FadeToBlack(levelToLoad));
            FadeSoundOff();
        }
    }

    /// <summary>
    /// Fade to black and load a new level
    /// </summary>
    /// <param name="levelToLoad">The level to load index</param>
    public void LoadLevel(int levelToLoad)
    {
        if (canFadeToBlack)
        {
            canFadeToBlack = false;
            blackImage.enabled = true;
            blackImage.color = Color.clear;
            StartCoroutine(FadeToBlack(levelToLoad));
            FadeSoundOff();
        }
    }

    /// <summary>
    /// Fade to black and quit application
    /// </summary>
    public void QuitApp()
    {
        if (canFadeToBlack)
        {
            canFadeToBlack = false;
            blackImage.enabled = true;
            blackImage.color = Color.clear;
            StartCoroutine(FadeAndQuitApp());
            FadeSoundOff();
        }
    }

    /// <summary>
    /// Blink the screen by fading to black and then clearing 
    /// the screen
    /// </summary>
    /// <param name="blackDuration">The duration of the black screen</param>
    public void Blink(float blackDuration)
    {
        if (canFadeToBlack)
        {
            canFadeToBlack = false;
            blackImage.enabled = true;
            blackImage.color = Color.clear;
            StartCoroutine(BlinkRoutine(blackDuration));
        }
    }

    /// <summary>
    /// Fade the sound off
    /// </summary>
    public void FadeSoundOff()
    {
        if (!canFadeSound) return;

        StartCoroutine(FadeSoundOffRoutine());
        canFadeSound = false;
    }

    public void FadeSoundOn()
    {
        if (!canFadeSound) return;

        StartCoroutine(FadeSoundOnRoutine());
        canFadeSound = false;
    }

    private IEnumerator FadeSoundOnRoutine()
    {
        float lerp = 0;

        while (lerp < 1)
        {
            lerp += Time.deltaTime * fadeSpeed;
            AudioListener.volume = Mathf.Lerp(0, 1, lerp);
            yield return null;
        }

        canFadeSound = true;
    }

    private IEnumerator FadeSoundOffRoutine()
    {
        float lerp = 1;

        while(lerp > 0)
        {
            lerp -= Time.deltaTime * fadeSpeed;
            AudioListener.volume = Mathf.Lerp(0, 1, lerp);
            yield return null;
        }

        canFadeSound = true;
    }

    /// <summary>
    /// Blink the screen by fading to black and then clearing
    /// </summary>
    /// <param name="blackDuration">The duration of the black screen</param>
    /// <returns></returns>
    private IEnumerator BlinkRoutine(float blackDuration)
    {
        yield return StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(blackDuration);
        yield return StartCoroutine(FadeToClear());
    }

    /// <summary>
    /// Fade to black then quit the application
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAndQuitApp()
    {
        yield return StartCoroutine(FadeToBlack());
        Application.Quit();
    }

    /// <summary>
    /// Fade the screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeToBlack()
    {
        float fadeAmount = 0f;

        while (fadeAmount < 1)
        {
            fadeAmount += fadeSpeed * Time.deltaTime;
            blackImage.color = Color.Lerp(Color.clear, Color.black, fadeAmount);
            yield return null;
        }

        if (OnScreenFadedToBlack != null) OnScreenFadedToBlack(this, screenFaderEventArgs);
    }

    /// <summary>
    /// Fade the screen to black then load a level
    /// </summary>
    /// <param name="level">The name of the level to load</param>
    /// <returns></returns>
    private IEnumerator FadeToBlack(string level)
    {
        yield return StartCoroutine(FadeToBlack());

        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Fade the screen to black then load a level
    /// </summary>
    /// <param name="level">The index of the level to load</param>
    /// <returns></returns>
    private IEnumerator FadeToBlack(int level)
    {
        yield return StartCoroutine(FadeToBlack());

        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Clear the screen
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeToClear()
    {
        float fadeAmount = 0f;

        while (fadeAmount < 1)
        {
            fadeAmount += fadeSpeed * Time.deltaTime;
            blackImage.color = Color.Lerp(Color.black, Color.clear, fadeAmount);
            yield return null;
        }

        blackImage.enabled = false;
        canFadeToBlack = true;

        if (OnScreenCleared != null) OnScreenCleared(this, screenFaderEventArgs);
    }

    /// <summary>
    /// Create a singleton instance of this class
    /// </summary>
    protected void Singleton()
    {
        // First we check if there are any other instances conflicting
        if (instance != null && instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(this.gameObject);
        }
        else
        {
            // Here we save our singleton instance
            instance = this;
        }
    }
}
