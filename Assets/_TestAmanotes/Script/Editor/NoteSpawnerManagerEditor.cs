using _TestAmanotes.Script;
using TestAmanotes;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoteSpawnerManager))]
public class NoteSpawnerManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get reference to the target script
        NoteSpawnerManager script = (NoteSpawnerManager)target;

        // Create a button
        if (GUILayout.Button("Drop note (normal)"))
        {
           script.SpawnNoteDefault(Define.NoteType.Normal);
        }
        if (GUILayout.Button("Drop note (large)"))
        {
            script.SpawnNoteDefault(Define.NoteType.Large);
        }
    }
}