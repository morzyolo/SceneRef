#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;

namespace SceneReference
{
	[System.Serializable]
	public class SceneRef : ISerializationCallbackReceiver
	{
#if UNITY_EDITOR
		[SerializeField] private Object sceneAsset;
		private bool IsValidSceneAsset
		{
			get
			{
				if (!sceneAsset)
					return false;

				return sceneAsset is SceneAsset;
			}
		}
#endif

		[SerializeField]
		private string scenePath = string.Empty;

		public string ScenePath
		{
			get
			{
#if UNITY_EDITOR
				return GetScenePathFromAsset();
#else
				return scenePath;
#endif
			}
			set
			{
				scenePath = value;
#if UNITY_EDITOR
				sceneAsset = GetSceneAssetFromPath();
#endif
			}
		}

		public static implicit operator string(SceneRef sceneReference)
		{
			return sceneReference.ScenePath;
		}

		public void OnBeforeSerialize()
		{
#if UNITY_EDITOR
			HandleBeforeSerialize();
#endif
		}

		public void OnAfterDeserialize()
		{
#if UNITY_EDITOR
			EditorApplication.update += HandleAfterDeserialize;
#endif
		}

#if UNITY_EDITOR
		private SceneAsset GetSceneAssetFromPath()
		{
			return string.IsNullOrEmpty(scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
		}

		private string GetScenePathFromAsset()
		{
			return sceneAsset == null ? string.Empty : AssetDatabase.GetAssetPath(sceneAsset);
		}

		private void HandleBeforeSerialize()
		{
			if (!IsValidSceneAsset && !string.IsNullOrEmpty(scenePath))
			{
				sceneAsset = GetSceneAssetFromPath();
				if (sceneAsset == null)
					scenePath = string.Empty;

				EditorSceneManager.MarkAllScenesDirty();
			}
			else
			{
				scenePath = GetScenePathFromAsset();
			}
		}

		private void HandleAfterDeserialize()
		{
			EditorApplication.update -= HandleAfterDeserialize;
			if (IsValidSceneAsset)
				return;

			if (string.IsNullOrEmpty(scenePath))
				return;

			sceneAsset = GetSceneAssetFromPath();
			if (!sceneAsset)
				scenePath = string.Empty;

			if (!Application.isPlaying)
				EditorSceneManager.MarkAllScenesDirty();
		}
#endif
	}
}
