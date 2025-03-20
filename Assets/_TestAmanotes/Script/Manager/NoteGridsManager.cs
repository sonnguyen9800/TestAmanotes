using System;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace TestAmanotes
{
    public class NoteGridsManager : MonoSingleton<NoteGridsManager>
    {
        [SerializeField] private Tilemap _tileMap = null;

        
    }
}