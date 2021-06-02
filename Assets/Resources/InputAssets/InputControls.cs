using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputControls
{
    public static GameControls Instance { get; } = new GameControls();

    public static void Enable()
    {
        Instance.Enable();
    }

    public static SingleInputHandler CancelButton = new SingleInputHandler(Instance.InGameMenu.Cancel);
    public static SingleInputHandler SkipMoveButton = new SingleInputHandler(Instance.AbilityUsage.SkipMove);
}

public class SingleInputHandler
{
    private Action AlwaysExecuteQueue;
    private Func<bool> InterruptingActionsQueue;
    private Action DefaultActionsQueue;
    public SingleInputHandler(InputAction action)
    {
        action.performed += (context) => ActionTriggered();
    }

    private void ActionTriggered()
    {
        if(AlwaysExecuteQueue != null)
        {
            foreach (Action action in AlwaysExecuteQueue.GetInvocationList())
            {
                action.Invoke();
            }
        }

        if(InterruptingActionsQueue != null)
        {
            foreach (Func<bool> func in InterruptingActionsQueue.GetInvocationList())
            {
                if(func.Invoke())
                {
                    return;
                }
            }
        }

        if(DefaultActionsQueue != null)
        {
            foreach (Action action in DefaultActionsQueue.GetInvocationList())
            {
                action.Invoke();
            }
        }
    }

    public void AddInterrupting(Func<bool> func)
    {
        InterruptingActionsQueue += func;
    }

    public void Add(Action action)
    {
        DefaultActionsQueue += action;
    }

    public void AddAlways(Action action)
    {
        AlwaysExecuteQueue += action;
    }

    public void Remove(Action action)
    {
        AlwaysExecuteQueue -= action;
        DefaultActionsQueue -= action;
    }

    public void Remove(Func<bool> func)
    {
        InterruptingActionsQueue -= func;
    }
}
