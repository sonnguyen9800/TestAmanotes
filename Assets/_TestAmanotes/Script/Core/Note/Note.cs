using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace TestAmanotes
{
    public class Note : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject[] _childNoteTiles;
        public float fallSpeed = 5f; // Speed at which the object falls
        [SerializeField] private LayerMask _layerToTap;

        private bool _runable;
        private bool _tapable;
        private bool _tapped;
        private Vector3 _targetPos;
        private HashSet<SpriteRenderer> _spriteRenderers = new();
        private void Awake()
        {
            foreach (var note in _childNoteTiles)
            {
                _spriteRenderers.Add(note.GetComponent<SpriteRenderer>());
            }
        }

        public HashSet<Vector3> GetAllPosition()
        {
          var data=  _childNoteTiles.Select(a => a.transform.position).ToHashSet();
          return data;
        }

        public void OnObjectSpawn()
        {
            _runable = true;
            _tapable = false;
            _tapped = false;
            
            
            _targetPos = transform.position;
            _targetPos.y = -10000;
        }

        public void OnObjectDisabled()
        {
            _runable = false;
            _tapable = false;
            _tapped = false;
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos,
                fallSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _layerToTap) != 0)
            {
                _tapable = true;
            }

        }

        private int CalculateScore()
        {
            return 100;
        }

        public void OnTap()
        {
            if (_tapped || !_tapable)
                return;
            _tapped = true;
            foreach (var note in _spriteRenderers)
            {
                note.DOFade(0, 0.2f);
            }
            ScoreManager.Instance.AddScore(CalculateScore());
        }
        
        
    }

}
