using System;

namespace SKit
{
    public class MonoRunner : Singleton<MonoRunner>
    {
        #region Private Variables
        private Action updateActions, fixedUpdateActions, lateUpdateActions = null;
        #endregion

        #region Public Methods
        public void AddUpdate(Action action)
        {
            updateActions += action;
        }

        public void AddFixedUpdate(Action action)
        {
            fixedUpdateActions += action;
        }

        public void AddLateUpdate(Action action)
        {
            lateUpdateActions += action;
        }

        public void RemoveUpdate(Action action)
        {
            updateActions -= action;
        }

        public void RemoveFixedUpdate(Action action)
        {
            fixedUpdateActions -= action;
        }

        public void RemoveLateUpdate(Action action)
        {
            lateUpdateActions -= action;
        }
        #endregion

        #region Unity Methods
        private void Update()
        {
            updateActions?.Invoke();
        }

        private void FixedUpdate()
        {
            fixedUpdateActions?.Invoke();
        }

        private void LateUpdate()
        {
            lateUpdateActions?.Invoke();
        }
        #endregion
    }
}