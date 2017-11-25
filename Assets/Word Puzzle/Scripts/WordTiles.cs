using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Rapresent a row of tiles used to compose a word.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BlinkImageColorUI))]
public class WordTiles : MonoBehaviour 
{
    public class WordTilesEventArgs : System.EventArgs
    {
        /// <summary>
        /// The word that was printed in the tile
        /// </summary>
        public string printedWord;

        /// <summary>
        /// The letter printed
        /// </summary>
        public string printedLetter;
    }

    public delegate void EventHandler(object sender, WordTilesEventArgs args);
    public static event EventHandler OnWordPrintCompleted;
    public static event EventHandler OnLetterPrinted;

    #region Inspector

    /// <summary>
    /// The time between letters being printed in the tile
    /// </summary>
    [Tooltip("The time between letters being printed in the tile")]
    public float letterPrintDelay = 0.5f;

    #endregion //Inspector

    /// <summary>
    /// The amount of tiles available
    /// </summary>
    public int TilesAmount
    {
        get
        {
            if(tilesText == null)
            {
                tilesText = GetComponentsInChildren<Text>(true);
            }

            if(tilesText.Length <= 0)
            {
                tilesText = GetComponentsInChildren<Text>(true);
            }

            return tilesText.Length;
        }
    }

    private WordTilesEventArgs eventArgs = new WordTilesEventArgs();

    private Text[] tilesText;
    private new AudioSource audio;
    private BlinkImageColorUI blinkImageColorUI;

    private WaitForSeconds wait_letterPrintDelay;

    #region Unity Engine & Events

    private void Awake()
    {
        tilesText = GetComponentsInChildren<Text>(true);
        audio = GetComponent<AudioSource>();
        blinkImageColorUI = GetComponent<BlinkImageColorUI>();

        wait_letterPrintDelay = new WaitForSeconds(letterPrintDelay);
    }

    private void Start()
    {
        // Clear all letters from tiles
        for(int i = 0; i < tilesText.Length; i++)
        {
            tilesText[i].text = string.Empty;
        }
    }

    #endregion //Unity Engine & Events

    /// <summary>
    /// Enter the entire word in the tiles 
    /// </summary>
    /// <param name="word">The word to enter</param>
    public void WriteWord(string word)
    {
        StartCoroutine(WriteWordRoutine(word.ToUpper()));
    }

    private IEnumerator WriteWordRoutine(string word)
    {
        for(int i = 0; i < word.Length; i++)
        {
            tilesText[i].text = word[i].ToString();
            eventArgs.printedLetter = word[i].ToString();
            if (OnLetterPrinted != null) OnLetterPrinted(this, eventArgs);
            yield return wait_letterPrintDelay;
        }

        eventArgs.printedWord = word;
        if (OnWordPrintCompleted != null) OnWordPrintCompleted(this, eventArgs);
    }

    /// <summary>
    /// Toggle blinking on this tileset
    /// </summary>
    /// <param name="toggle">The activate state of the blink</param>
    public void ToggleBlink(bool toggle)
    {
        blinkImageColorUI.enabled = toggle;
    }

}
