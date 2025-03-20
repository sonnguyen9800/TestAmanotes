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
        private Vector3Int _currentCell;

        
        [SerializeField]
        private LayerMask _layerMask;
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name);
            if (((1 << other.gameObject.layer) & _layerMask) == 0) return;

            Vector3Int newCell = _tileMap.WorldToCell(other.transform.position);

            // If player enters a new tile, update the cell
            if (newCell == _currentCell) return;
            _currentCell = newCell;
            Debug.Log($"Player entered tile: {_currentCell}");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log(other.name);

            if (!other.gameObject.CompareTag("Player")) return;

            Debug.Log($"Player exited tile: {_currentCell}");
        }
        
        
    }
}