using System;
using UnityEngine;
using System.Collections;

public class SoundAnimationControler : MonoBehaviour
{

    [SerializeField] private Color color;
    [SerializeField] public float strength;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = color;
        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    void Update()
    {
        if (strength - transform.localScale.x >= .2f)
        {
            transform.localScale = new Vector3(transform.localScale.x + .1f, transform.localScale.y + .1f, 0);
        }
        else if (Math.Abs(color.a) < .1f)
        {
            Destroy(gameObject);
        }
        else
        {
            color = new Color(color.r, color.b, color.g, color.a - .1f);
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
