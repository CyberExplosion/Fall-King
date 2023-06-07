using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif

public class QuicklyWiggleInteraction : IInputInteraction
{
    public float duration = 0.2f;

    /// <summary>
    /// Static constructor use to initialize data
    /// </summary>
    static QuicklyWiggleInteraction()
    {
        InputSystem.RegisterInteraction<QuicklyWiggleInteraction>();
    }

    /// <summary>
    /// Call when the game loads
    /// Call the static contructor as a side effect
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize() { }

    public void Process(ref InputInteractionContext context)
    {
        Debug.Log($"This thing works {context.control.ReadValueAsObject()}");

        if (context.timerHasExpired)
        {
            context.Canceled();
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                if ((int)context.control.ReadValueAsObject() == 1)
                {
                    context.Started();
                    context.SetTimeout(duration);
                }
                break;

            case InputActionPhase.Started:
                if ((int)context.control.ReadValueAsObject() == -1)
                    context.Performed();
                break;
        }

    }
    public void Reset() { }
}