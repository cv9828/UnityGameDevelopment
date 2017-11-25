using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;

/// <summary>
/// Blink a UI image component between 2 diffent color
/// </summary>
public class BlinkImageColorUI : MonoBehaviour 
{
    #region Inspector

    public Color color1 = Color.white;
    public Color color2 = Color.black;
    public AnimationCurve blinkCurve;
    public float blinkSpeed = 2f;

    [SerializeField]
    private Image[] images;

    #endregion //Inspector

    private Color[] initialColors;
    private float lerp;

    #region Unity Engine & Events

    private void Awake()
    {
        initialColors = new Color[images.Length];
        for (int i = 0; i < initialColors.Length; i++)
        {
            if (images[i] != null)
            {
                initialColors[i] = images[i].color;
            }
            else
            {
                initialColors[i] = Color.white;
            }
        }
    }

    private void Update()
    {
        if(lerp <= 1)
        {
            lerp += Time.deltaTime * blinkSpeed;

            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = Color.Lerp(color1, color2, blinkCurve.Evaluate(lerp));
            }
        }
        else
        {
            lerp = 0;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] != null)
            {
                images[i].color = initialColors[i];
            }
        }
    }

    #endregion //Unity Engine & Events

    [Button()]
    private void AddAllImagesInChildren()
    {
        var imagesInChildren = GetComponentsInChildren<Image>();
        images = new Image[imagesInChildren.Length];
        imagesInChildren.CopyTo(images, 0);
    }

}
