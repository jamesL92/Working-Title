using System;
using System.Collections.Generic;
using UnityEngine;
using Working_Title.Assets.Scripts;

namespace ActionSystem {
  public class ActionManager: MonoSingleton<ActionManager> {
    private Stack<Action> actionStack = new Stack<Action>();
    private ActionFactory factory;

    protected override void Awake() {
      base.Awake();
      factory = new ActionFactory(this);
      GameManager.onTurnStart += ClearStack;
    }

    void Update() {
      if(Input.GetKeyUp(KeyCode.Backspace))
        UndoLastAction();
    }

    public void AddAction(string type) {
      if (actionStack.Count != 5){
        try {

          Action action = factory.CreateAction((ActionType)Enum.Parse(typeof(ActionType), type), GameManager.instance.currentPlayer);
          actionStack.Push(action);
          StartCoroutine(action.Perform());

        } catch(ArgumentException) {

          Debug.LogError(String.Format("{0} is not a valid action type", type));

        }
      } else { Debug.Log("No more actions left this turn"); }
    }

    public void UndoLastAction() {
      if(actionStack.Count > 0) {
        Action action = actionStack.Pop();
        action.Undo();
      } else {
        Debug.Log("There are no actions remaining in the stack.");
      }
    }
    public void ClearStack() {
      actionStack.Clear();
    }
  }
}