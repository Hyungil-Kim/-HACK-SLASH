using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void StateEnter();
    void StateFixedUpdate();
    void StateUpdate();
    void StateExit();
}
