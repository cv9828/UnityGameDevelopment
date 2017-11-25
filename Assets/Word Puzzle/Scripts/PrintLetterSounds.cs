using UnityEngine;
using System.Collections;

public class PrintLetterSounds : MonoBehaviour 
{
    #region Inspector



    #endregion //Inspector

    private TypeWriterSound typeWriterSound;

    #region Unity Engine & Events

    private void Awake()
    {
        typeWriterSound = GetComponent<TypeWriterSound>();
    }

    private void OnEnable()
    {
        WordTiles.OnLetterPrinted += WordTiles_OnLetterPrinted;
    }

    private void WordTiles_OnLetterPrinted(object sender, WordTiles.WordTilesEventArgs args)
    {
        typeWriterSound.PlayKey(args.printedLetter);
    }

    private void OnDisable()
    {
        WordTiles.OnLetterPrinted -= WordTiles_OnLetterPrinted;
    }

    #endregion //Unity Engine & Events

}
