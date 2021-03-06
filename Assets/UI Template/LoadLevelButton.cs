﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Sirenix.OdinInspector;

/// <summary>
/// Fade the screen and load a new level
/// </summary>
public class LoadLevelButton : MonoBehaviour, IPointerClickHandler 
{
    #region Inspector

    [ValueDropdown("GetAllScenes")]
    [Tooltip("The name of the level to load when this button is clicked!")]
    public string levelName;

    #endregion //Inspector

    private string[] GetAllScenes
    {
        get
        {
            return SceneUtility.GetSceneAllScenesNamesInBuild();
        }
    }

    #region Unity Engine & Events

    public void OnPointerClick(PointerEventData eventData)
    {
        ScreenFader.instance.LoadLevel(levelName);
    }

    #endregion //Unity Engine & Events

}
