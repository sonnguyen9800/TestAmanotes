using System;
using UnityEngine;

namespace TestAmanotes.Tile
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileNote : MonoBehaviour
    {
        
        [SerializeField] private SpriteRenderer _renderer = null;
        
        private Vector3Int cellPos = Vector3Int.down;
        private TileDataHub _hub;
        private bool _didSetup;
        private byte _id;
        public void Setup(Vector3Int cellPosition)
        {
            cellPos = cellPosition;
            _hub = DataHubManager.Instance.TileDataHub;
            _id = _hub.Add(new DataItem
            {
                IsPlaced = false,
                Position = cellPos
            });
            _didSetup = true;
        }
        public void SetColor(Color color)
        {

            _renderer.color = color;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_didSetup)
                return;
            SetColor(Color.green);
            _hub.Edit(_id, item => item.IsPlaced, true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_didSetup)
                return;
            SetColor(Color.white);
            _hub.Edit(_id, item => item.IsPlaced, false);

        }
    }
}