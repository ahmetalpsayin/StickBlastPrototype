using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public interface IInputReceiver
{
    void OnInputDown(Vector2 screenPosition);
    void OnInputDrag(Vector2 screenPosition);
    void OnInputUp(Vector2 screenPosition);
}

