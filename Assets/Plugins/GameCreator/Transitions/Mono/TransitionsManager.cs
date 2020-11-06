namespace GameCreator.Transitions
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.SceneManagement;
	using GameCreator.Core;
    using GameCreator.Core.Hooks;
    using GameCreator.Camera;

    [AddComponentMenu("Game Creator/Managers/TransitionsManager", 100)]
	public class TransitionsManager : Singleton<TransitionsManager> 
	{
		private const string CANVAS_ASSET_PATH = "GameCreator/Transitions/TransitionManager";

		private static readonly int ANIMATOR_TRIGGER_SHOW = Animator.StringToHash("Show");
		private static readonly int ANIMATOR_TRIGGER_HIDE = Animator.StringToHash("Hide");

		// PROPERTIES: ----------------------------------------------------------------------------

		private RectTransform container;

		// INITIALIZE: ----------------------------------------------------------------------------

		protected override void OnCreate ()
		{
			GameObject canvasPrefab = Resources.Load<GameObject>(CANVAS_ASSET_PATH);
			GameObject canvas = Instantiate<GameObject>(canvasPrefab, transform);

			this.container = canvas.transform.GetChild(0).GetComponent<RectTransform>();
		}

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject StartTransition(GameObject prefabTransition, string scene, LoadSceneMode mode, float duration)
		{
			this.StopAllCoroutines();
			this.CleanContainer();

			GameObject transition = Instantiate<GameObject>(prefabTransition, this.container);
            StartCoroutine(this.LoadScene(transition, scene, mode, duration));

			return transition;
		}

        public GameObject StartTransition(GameObject prefabTransition, CameraMotor motor, float duration)
        {
            this.StopAllCoroutines();
            this.CleanContainer();

            GameObject transition = Instantiate<GameObject>(prefabTransition, this.container);
            StartCoroutine(this.ChangeMotor(transition, duration, motor));

            return transition;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private IEnumerator LoadScene(GameObject transition, string scene, LoadSceneMode mode, float duration)
		{
			Animator animator = transition.GetComponentInChildren<Animator>();
			if (animator != null) 
			{
				animator.speed = 1.0f/duration;
				animator.SetTrigger(ANIMATOR_TRIGGER_SHOW);
			}

			WaitForSeconds waitForSeconds = new WaitForSeconds(duration);
			yield return waitForSeconds;

            TransitionAsyncBase asyncBase = transition.GetComponent<TransitionAsyncBase>();
            if (asyncBase == null) SceneManager.LoadScene(scene, mode);
            else
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, mode);
                asyncBase.Setup(asyncOperation);

                WaitUntil waitUntil = new WaitUntil(() => asyncOperation.isDone);
                yield return waitUntil;
            }

			if (animator != null) 
			{
				animator.SetTrigger(ANIMATOR_TRIGGER_HIDE);
				waitForSeconds = new WaitForSeconds(duration);
				yield return waitForSeconds;
			}

			this.CleanContainer();
		}

        private IEnumerator ChangeMotor(GameObject transition, float duration, CameraMotor motor)
        {
            Animator animator = transition.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.speed = 1.0f / duration;
                animator.SetTrigger(ANIMATOR_TRIGGER_SHOW);
            }

            WaitForSeconds waitForSeconds = new WaitForSeconds(duration + 0.1f);
            yield return waitForSeconds;

            CameraController cameraController = HookCamera.Instance.Get<CameraController>();
            if (cameraController != null) cameraController.ChangeCameraMotor(motor, 0f);

            if (animator != null)
            {
                animator.SetTrigger(ANIMATOR_TRIGGER_HIDE);
                waitForSeconds = new WaitForSeconds(duration);
                yield return waitForSeconds;
            }

            this.CleanContainer();
        }

        private void CleanContainer()
		{
			if (this.container == null) return;

			int numChildren = this.container.childCount;
			for (int i = numChildren - 1; i >= 0; --i)
			{
				GameObject child = this.container.GetChild(i).gameObject;
				child.SetActive(false);
				Destroy(child);
			}
		}
	}
}