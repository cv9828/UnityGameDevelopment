/*==============================================================================

==============================================================================*/

using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	#region Public Variables

	public static T Instance
    {
        get
        {
            // Check if the instance is null.
            if (_instance == null)
            {
                // Couldn't find the singleton in the scene, so make it.
                GameObject singleton = new GameObject(typeof(T).Name);
                _instance = singleton.AddComponent<T>();
            }

            return _instance;
        }
    }

    #endregion //Public Variables



    #region Private Variables

    private static T _instance;
	
	#endregion //Private Variables
	

	
	#region Unity Engine & Events
	
	public virtual void Awake()
    {
        CreateInstance();
    }
	
	#endregion //Unity Engine & Events
	
	
	
	#region Public Methods
	
	
	
	#endregion //Public Methods
	
	
	
	#region Private Methods
	
	private void CreateInstance()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
        }
    }
	
	#endregion //Private Methods
	
}
