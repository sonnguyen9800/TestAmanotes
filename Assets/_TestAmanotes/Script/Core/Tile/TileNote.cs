using System;
using UnityEngine;

namespace TestAmanotes.Tile
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]

    public class TileNote : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer = null;

        public void SetColor(Color color)
        {
            _renderer.color = color;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            SetColor(Color.green);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            SetColor(Color.white);

        }
    }
}