using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable
{
    bool IsAggroed { get; set; }
    bool IsInAttackDistance { get; set; }

    void SetAggroStatus(bool isAggroed);
    void SetAttackDistanceBool(bool isInAttackDistance);
}
