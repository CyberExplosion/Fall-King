using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif

public class QuicklyWiggleInteraction : IInputInteraction
{
    public float duration = 0.2f;

    public void Process(ref InputInteractionContext context)
    {
        if (context.timerHasExpired)
        {
            context.Canceled();
            return;
        }

        switch(context.phase)
        {
            case InputActionPhase.Waiting:
            //    if (context.control)
            //    {
            //        context.Started();
            //        context.SetTimeout(duration);
            //    }
            //    break;
            //case InputActionPhase.Started:
            //    if (context.Control.ReadValue<float>() != -1)
            //    {
            //        break;
            //    }
                context.Performed();
                break;
        }
    }

    public void Reset() {}
}