using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Words Database", menuName = "Words Puzzle/Words Database")]
public class WordsDatabase : ScriptableObject 
{
    #region Inspector

    [Tooltip("A list of all puzzles available")]
    [InfoBox("Words in a puzzle must be of the correct lenght to match the puzzle tiles and ")]
    public Word[] words;

    #endregion //Inspector



    #region Unity Engine & Events



    #endregion //Unity Engine & Events

    [System.Serializable]
    public struct Word
    {
        [Tooltip("The word should in this puzzl")]
        public string word;

        [Tooltip("The question to show")]
        public string question;

        [Tooltip("An image that rappresent the word in the puzzle")]
        public Sprite image;
    }
	
}
