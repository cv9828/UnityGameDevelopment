using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Sirenix.OdinInspector;

public class WordPuzzle : MonoBehaviour 
{
    #region Inspector

    [InfoBox("You can use multiple database of words and it will be randomized at start to make different puzzles. \n Be sure to create words that are the same lenght of the tiles and with matching letters in cross tiles")]

    [InfoBox("There are more or not enough words in the puzzle to cover all", InfoMessageType.Error, "areWordAmountWrong")]
    [InfoBox("Some words in the database are longer or shorter then the tiles in the puzzle, use words that are the same lenght of the tiles", InfoMessageType.Error, "areWordLettersWrong")]

    [Required("There must be at least 1 word database")]
    [SerializeField]
    private WordsDatabase[] wordsDatabase;

    [SerializeField]
    private WordTiles[] wordTiles;

    [SerializeField]
    [ValueDropdown("GetSceneList")]
    private string puzzleCompleteScene;

    [Required()]
    [SerializeField]
    private InputField inputField;

    [Tooltip("The image component used to display")]
    [SerializeField]
    private Image hitImage;

    [SerializeField]
    private Text questionText;

    [BoxGroup("Sounds")]
    [SerializeField]
    private AudioClip wrongWordSound;

    [BoxGroup("Sounds")]
    [SerializeField]
    private AudioClip correctWordSound;

    #endregion //Inspector

    // Return true if there are more or less words then tiles in this puzzle
    private bool areWordAmountWrong
    {
        get
        {
            if (wordTiles == null) return true;

            for (int i = 0; i < wordsDatabase.Length; i++)
            {
                if (wordsDatabase[i].words == null)
                {
                    return true;
                }

                if (wordsDatabase[i].words.Length != wordTiles.Length)
                {
                    return true;
                }
            }

            return false;
        }
    }
    private bool areWordLettersWrong
    {
        get
        {
            if (wordTiles == null) return true;

            for (int i = 0; i < wordsDatabase.Length; i++)
            {
                for(int w = 0; w < wordsDatabase[i].words.Length; w++)
                {
                    if (wordsDatabase[i].words[w].word.Length != wordTiles[w].TilesAmount)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
    private string[] GetSceneList
    {
        get
        {
            return SceneUtility.GetSceneAllScenesNamesInBuild();
        }
    }

    private AudioSource audio;

    private int selectedDatabase;
    private int currentWord = 0;

    private bool isPuzzleCompleted;

    #region Unity Engine & Events

    private void Awake()
    {
        // Get all word tiles in the scene
        audio = GetComponent<AudioSource>();

        selectedDatabase = Random.Range(0, wordsDatabase.Length);
    }

    private void OnEnable()
    {
        inputField.onEndEdit.AddListener(delegate { InputField_OnSubmit(inputField); });
        WordTiles.OnWordPrintCompleted += WordTiles_OnWordPrintCompleted;
    }

    private void WordTiles_OnWordPrintCompleted(object sender, WordTiles.WordTilesEventArgs args)
    {
        if(isPuzzleCompleted)
        {
            PuzzleComplete();
        }
    }

    private void InputField_OnSubmit(InputField field)
    {
        // If the enterd text is the correct word
        if(wordsDatabase[selectedDatabase].words[currentWord].word.ToLower() == field.text.ToLower())
        {
            CorrectWord(field.text);
        }
        else
        {
            WrongWord();
        }
    }

    private void OnDisable()
    {
        WordTiles.OnWordPrintCompleted -= WordTiles_OnWordPrintCompleted;
        inputField.onEndEdit.RemoveAllListeners();
    }

    private void Start()
    {
        for(int i = 0; i < wordTiles.Length; i++)
        {
            wordTiles[i].ToggleBlink(false);
        }

        StartWord();
    }

    #endregion //Unity Engine & Events

    private void CorrectWord(string correctWord)
    {
        audio.PlayOneShot(correctWordSound);
        wordTiles[currentWord].WriteWord(correctWord);

        // Go to next word if there are still word puzzles
        if (currentWord < wordsDatabase[selectedDatabase].words.Length -1)
        {
            GoToNextWord();
        }
        else
        {
            isPuzzleCompleted = true;
        }
    }

    private void WrongWord()
    {
        audio.PlayOneShot(wrongWordSound);
    }

    private void GoToNextWord()
    {
        wordTiles[currentWord].ToggleBlink(false);
        currentWord++;
        StartWord();
    }

    private void StartWord()
    {
        WordsDatabase database = wordsDatabase[selectedDatabase];
        WordTiles wordTile = wordTiles[currentWord];

        wordTile.ToggleBlink(true);
        hitImage.sprite = database.words[currentWord].image;
        questionText.text = database.words[currentWord].question;
    }

    private void PuzzleComplete()
    {
        ScreenFader.instance.LoadLevel(puzzleCompleteScene);
    }

    [PropertyOrder(3)]
    [Button("Add all word tiles in scene")]
    private void AddAllWordsTileInScene()
    {
        wordTiles = FindObjectsOfType<WordTiles>();
    }

}
