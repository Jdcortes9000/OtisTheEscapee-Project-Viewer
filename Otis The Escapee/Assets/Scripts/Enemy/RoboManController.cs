using UnityEngine;
using System.Collections;

public class RoboManController : BaseEnemyController {

    public override void Alert()
    {
        agent.speed = agent.speed * 1.5f;
        Damage_amount = Damage_amount * 1.5f;
        isAlert = false;
    }
}
