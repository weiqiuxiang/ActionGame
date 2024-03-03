using System;
using Project.ActionGame;
using Project.System;
using Project.Utilities;
using UnityEngine;

namespace Project
{
    /// <summary>
    /// ActionGameのPresenter
    /// </summary>
    public class ActionGamePresenter : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        
        private ActionGameView view;
        private ActionGameModel model;
        private SceneAssetLoader loader;
        
        public void Prepare(ActionGameView view, ActionGameModel model, SceneAssetLoader loader)
        {
            this.view = view;
            this.model = model;
            this.loader = loader;
        }

        private void OpenCloseMenu()
        {
            
        }

        private void Pause()
        {
            playerController.Pause();
        }

        private void Resume()
        {
            playerController.Resume();
        }

        private void OnDestroy()
        {
            
        }
    }
}
