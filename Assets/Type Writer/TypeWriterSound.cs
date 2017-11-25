using UnityEngine;
using System.Collections;

/// <summary>
/// Create sound from an old typewriter by taking a string
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TypeWriterSound : MonoBehaviour 
{
    #region Inspector

    [SerializeField]
    private AudioClip[] typeWriterSounds;

    #endregion //Inspector

    private AudioSource audio;

    #region Unity Engine & Events

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    #endregion //Unity Engine & Events

    /// <summary>
    /// Play a key press from a type writer
    /// </summary>
    /// <param name="letter">The letter to play</param>
    public void PlayKey(string letter)
    {
        audio.PlayOneShot(FindLetterSound(letter.ToLower()));
    }

    private AudioClip FindLetterSound(string letter)
    {
        for(int i = 0; i < typeWriterSounds.Length; i++)
        {
            if(typeWriterSounds[i].name.ToLower() == letter)
            {
                return typeWriterSounds[i];
            }
        }

        return typeWriterSounds[0];
    }


}
