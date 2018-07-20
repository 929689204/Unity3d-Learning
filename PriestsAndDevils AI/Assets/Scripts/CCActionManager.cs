using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback
{

    public FirstController sceneController;

    protected new void Start()
    {
        sceneController = (FirstController)SSDirector.GetInstance().currentSceneController;
        sceneController.actionManager = this;
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
    }

    public void MoveAction(GameObject gameObject, Vector3 destination)
    {
        this.RunAction(gameObject, CCMoveToAction.GetSSAction(destination, 1), this);
    }

    #region ISSActionCallback implementation
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null)
    {
        sceneController.someObjectHandling = false;

    }
    #endregion
}