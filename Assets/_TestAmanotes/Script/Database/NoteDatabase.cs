using System;
using SisyphusFramework.ScriptableObject;
using SisyphusFramework.Utils;
using UnityEngine;

namespace _TestAmanotes
{
    [Serializable]
    public class NoteData
    {

        public GameObject Prefab;
        
    }
    [Serializable]
    public class NoteItem : ADataItem<NoteData>
    {
        
    }
    [CreateAssetMenu(menuName = "Note DB")]
    public class NoteDatabase : AScriptableDatabase<NoteItem, NoteData>
    {
        
    }
}