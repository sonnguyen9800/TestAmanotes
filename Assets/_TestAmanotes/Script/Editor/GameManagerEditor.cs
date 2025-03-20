using TestAmanotes;
using UnityEditor;
using UnityEngine;

namespace _TestAmanotes.Script.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class NoteSpawnerManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Get reference to the target script
            GameManager script = (GameManager)target;

            // Create a button
            if (GUILayout.Button("Start"))
            {
                script.StartGame();
            }

        }
    }
}