using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneTransitions
{

    public class GoToSceneController : BaseSceneController
    {

        [SerializeField] SceneRegistry scene;
        [SerializeField] bool useLoading;

        public void ToScene()
        {
            SceneManager.Instance.PushToScene(scene, this.transitionInfo, true, useLoading);

        }
    }
}


