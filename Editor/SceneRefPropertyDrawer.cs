using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SceneReference.Editor
{
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(SceneRef))]
	public class SceneReferencePropertyDrawer : PropertyDrawer
	{
		private struct BuildScene
		{
			public int buildIndex;
			public GUID assetGUID;
			public string assetPath;
			public EditorBuildSettingsScene scene;
		}

		private const string sceneAssetPropertyString = "sceneAsset";
		private const string scenePathPropertyString = "scenePath";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.serializedObject.isEditingMultipleObjects)
				return;

			var sceneAssetProperty = property.FindPropertyRelative(sceneAssetPropertyString);

			EditorGUI.BeginChangeCheck();

			sceneAssetProperty.objectReferenceValue = EditorGUI.ObjectField(
				position,
				label,
				sceneAssetProperty.objectReferenceValue,
				typeof(SceneAsset),
				false
			);

			if (EditorGUI.EndChangeCheck())
			{
				var buildScene = GetBuildScene(sceneAssetProperty.objectReferenceValue);

				if (buildScene.scene is null)
				{
					SerializedProperty serializedProperty = property.FindPropertyRelative(
						scenePathPropertyString
					);
					serializedProperty.stringValue = string.Empty;
				}
			}
		}

		private BuildScene GetBuildScene(Object sceneObject)
		{
			var entry = new BuildScene { buildIndex = -1, assetGUID = new GUID(string.Empty) };

			if (sceneObject is not SceneAsset)
				return entry;

			entry.assetPath = AssetDatabase.GetAssetPath(sceneObject);
			entry.assetGUID = new GUID(AssetDatabase.AssetPathToGUID(entry.assetPath));

			var scenes = EditorBuildSettings.scenes;
			for (var index = 0; index < scenes.Length; ++index)
			{
				if (!entry.assetGUID.Equals(scenes[index].guid))
					continue;

				entry.scene = scenes[index];
				entry.buildIndex = index;
				return entry;
			}

			return entry;
		}
	}
#endif
}
