namespace GameCreator.Transitions
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class TransitionAsyncLoading : TransitionAsyncBase
    {
        public RectTransform loadingBar;

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void Setup(AsyncOperation asyncOperation)
        {
            StartCoroutine(this.ShowProgress(asyncOperation));
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private IEnumerator ShowProgress(AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                this.loadingBar.localScale = new Vector3(progress, 1.0f, 1.0f);

                yield return null;
            }
        }
    }
}