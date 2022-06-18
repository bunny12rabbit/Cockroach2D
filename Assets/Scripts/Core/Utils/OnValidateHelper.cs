using System;
using System.Collections.Generic;

namespace Core.Utils
{
    /// <summary>
    /// Useful to perform action in OnValidate, CheckConsistency and Awake, that implicitly calls SendMessage, as it's not allowed.
    /// </summary>
    public static class OnValidateHelper
    {
        private static readonly List<Action> s_callbacks = new();

#if UNITY_EDITOR

        private static void Update()
        {
            foreach (var callback in s_callbacks)
            {
                callback?.Invoke();
            }

            UnityEditor.EditorApplication.delayCall -= Update;
            s_callbacks.Clear();
        }
#endif

        /// <summary>
        /// Execute given <paramref name="callback"/> in <seealso cref="UnityEditor.EditorApplication.delayCall"/>:
        /// <inheritdoc cref="UnityEditor.EditorApplication.delayCall"/>
        /// </summary>
        /// <param name="callback">Action to invoke at delayCall</param>
        public static void Execute(Action callback)
        {
            var isCallbackNotExist = !s_callbacks.Contains(callback);

#if UNITY_EDITOR
            if (s_callbacks.Count == 0)
                UnityEditor.EditorApplication.delayCall += Update;

            if (isCallbackNotExist)
                s_callbacks.Add(callback);
#endif
        }
    }
}