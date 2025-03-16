using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

/*
* @Program:TransitionManager.cs
* @Author: Keraz
* @Description:场景切换的功能
* @Date: 2025年02月19日 星期三 19:49:31
*/

namespace MFarm.Transition
{
    public class TransitionManager : MonoBehaviour
    {
        [SceneName]
        public string startSceneName = string.Empty;

        private CanvasGroup fadeCanvasGroup;
        private bool isFade;

        private void OnEnable()
        {
            EventHandler.TransitionEvent += OnTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.TransitionEvent -= OnTransitionEvent;
        }
        

        private IEnumerator Start()
        {
            fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return LoadSceneSetActive(startSceneName);
            EventHandler.CallAfterSceneLoadEvent();
        }


        private void OnTransitionEvent(string sceneToGo, Vector3 positionToGo)
        {
            if (!isFade)
            {
                StartCoroutine(Transition(sceneToGo, positionToGo));
            }
            
        }

        /// <summary>
        /// 场景切换
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="targetPosition">目标位置</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName, Vector3 targetPosition)
        {
            EventHandler.CallBeforeSceneUnloadEvent();
            yield return Fade(1f);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            yield return LoadSceneSetActive(sceneName);
            
            //移动人物坐标
            EventHandler.CallMoveToPosition(targetPosition);
            // 调用AfterSceneLoadEvent()
            EventHandler.CallAfterSceneLoadEvent();
            yield return Fade(0f);
            
            //Debug.Log("调用EventHandler.CallAfterSceneLoadEvent()");
        }

        /// <summary>
        /// 加载场景并设置为激活
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            
            SceneManager.SetActiveScene(newScene);
        }

        /// <summary>
        /// 场景加载的渐变
        /// </summary>
        /// <param name="targetAlpha">1是黑，0是透明</param>
        /// <returns>一个IEnumerator对象，用于控制渐变动画的帧更新。</returns>
        private IEnumerator Fade(float targetAlpha)
        {
            // 标记当前正在渐变，防止多个渐变同时进行
            isFade = true;
            // 设置当前渐变的CanvasGroup阻止光线投射，避免用户交互
            fadeCanvasGroup.blocksRaycasts = true;
            
            // 计算每帧透明度变化的速度，确保在设置的渐变持续时间内达到目标透明度
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Setting.fadeDuration;
            
            // 当当前透明度未接近目标透明度时，继续渐变
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                // 平滑地改变透明度，使用MoveTowards确保不会超过目标值
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                // 等待下一帧
                yield return null;
            }
            
            // 渐变完成后，允许光线投射，恢复用户交互
            fadeCanvasGroup.blocksRaycasts = false;
            // 标记当前不处于渐变状态
            isFade = false;
        }
    }
}