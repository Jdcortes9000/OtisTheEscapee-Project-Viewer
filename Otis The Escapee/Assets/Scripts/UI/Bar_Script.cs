using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class Bar_Script : MonoBehaviour
{
    /// <summary>
    /// Color the Bar starts as when full
    /// </summary>
    [SerializeField] private Color StartingColor = Color.white;
    /// <summary>
    /// Color the Bar ends with when empty
    /// </summary>
    [SerializeField] private Color EndingColor = Color.black;
    /// <summary>
    /// The percentage the bar starts at when instantiated
    /// </summary>
    [SerializeField] private float InitialAmountFilled = 100f;
    /// <summary>
    /// The current percentage of the Bar
    /// </summary>
    [SerializeField] protected float AmountFilled;
    /// <summary>
    /// The current image of the bar
    /// </summary>
    private Image sprite;

    void Start()
    {
        AmountFilled = InitialAmountFilled;
        sprite = GetComponent<Image>();
        EventManager.PassStrings += ParseStrings;
        sprite.color = StartingColor;
    }

    void Update()
    {
        sprite.color = Color.Lerp(EndingColor, StartingColor, AmountFilled/100);
        gameObject.transform.localScale = new Vector3(AmountFilled / 100, transform.localScale.y, transform.localScale.x);
    }

    public virtual void ParseStrings(string[] values)
    {
        
    }

    void OnDestory()
    {
        EventManager.PassStrings -= ParseStrings;
    }
}
