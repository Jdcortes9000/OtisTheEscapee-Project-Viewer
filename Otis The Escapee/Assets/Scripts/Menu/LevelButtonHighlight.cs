using UnityEngine;
using System.Collections;

public class LevelButtonHighlight : MonoBehaviour {

    public GameObject manager;

    void OnMouseEnter()
    {
        if (this.gameObject.tag == "L1")
        {
            manager.GetComponent<LevelSelectManager>().Animate = true;
            manager.GetComponent<LevelSelectManager>().index = 0;
        }
        else if (this.gameObject.tag == "L2")
        {
            manager.GetComponent<LevelSelectManager>().Animate = true;
            manager.GetComponent<LevelSelectManager>().index = 1;
        }
        else if (this.gameObject.tag == "L3")
        {
            manager.GetComponent<LevelSelectManager>().Animate = true;
            manager.GetComponent<LevelSelectManager>().index = 2;
        }
    }
}
