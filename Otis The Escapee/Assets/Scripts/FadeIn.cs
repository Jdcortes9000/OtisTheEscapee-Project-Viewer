using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private bool fadeIn = true;
    [SerializeField] private Image imageToFade;
    [Range(0, 1)][SerializeField] private float maximumAlpha;
    
	void Start ()
	{
	    if (fadeIn)
	    {
	        Color temp = imageToFade.color;
	        temp.a = 0;
	        imageToFade.color = temp;
	    }
	    else
	    {
            Color temp = imageToFade.color;
            temp.a = maximumAlpha;
            imageToFade.color = temp;
        }
	}
    
	void Update ()
    {
	    if (imageToFade.color.a < maximumAlpha && fadeIn)
	    {
	        Color temp = imageToFade.color;
	        temp.a = Mathf.Lerp(temp.a, maximumAlpha, .1f);
	        imageToFade.color = temp;
	    }
        else if (imageToFade.color.a > 0 && !fadeIn)
        {
            Color temp = imageToFade.color;
            temp.a = Mathf.Lerp(temp.a, 0, .1f);
            imageToFade.color = temp;
        }
        else if (Math.Abs(imageToFade.color.a) < .0001f && !fadeIn)
        {
            gameObject.GetComponentInParent<GameObject>().SetActive(false);
        }
	}
}
