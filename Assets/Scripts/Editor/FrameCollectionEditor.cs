using System.IO;
using Showcase;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    [CustomEditor(typeof(FrameCollection))]
    public class FrameCollectionEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var element = new VisualElement();
            var path = GetPathProperty().stringValue;
            var text = new TextElement
            {
                text = string.IsNullOrEmpty(path) ? "null" : path
            };
            var button = new Button(OnClick)
            {
                text = "Select Video Path"
            };

            element.Add(text);
            element.Add(button);
            return element;
        }

        SerializedProperty GetPathProperty() => serializedObject.FindProperty("_selectedPath");

        void OnClick()
        {
            var path = EditorUtility.OpenFolderPanel("Select Video Path", "Assets/Art/Videos", "");
            GetPathProperty().stringValue = path;

            var filesProperty = serializedObject.FindProperty("_filePaths");
            var files = Directory.GetFiles(path, "*.jpg");
            Debug.Log(files.Length);
            filesProperty.arraySize = files.Length;
            for (var i = 0; i < files.Length; i++)
            {
                var split = files[i].Replace("\\", "/").Split('/');
                var key = split[^2] + "/" + split[^1];
                filesProperty.GetArrayElementAtIndex(i).stringValue = key;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
