using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NTC.Pool;
using UnityEngine;

namespace TestAmanotes
{
    public class Note : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameConfigSO _config = null;
        [SerializeField] private GameObject[] _childNoteTiles;
        [SerializeField] private LayerMask _layerToTap;

        private float _timeAtStart;
        private bool _runable;
        private bool _tapable;
        private bool _tapped;
        private Vector3 _targetPos;
        private HashSet<SpriteRenderer> _spriteRenderers = new();
        private double _timeInstantiated;
        private float _initialY;
        [SerializeField]
        private float _speedFall = 0;
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

        private void Start()
        {
           // OnObjectSpawn();
        }

        private void OnObjectSpawn()
        {
            _runable = true;
            _tapable = false;
            _tapped = false;
            foreach (var note in _spriteRenderers)
            {
                note.color = Color.white;
            }
            
            _targetPos = transform.position;
            _targetPos.y = NoteSpawnerManager.Instance.TapLineTransform.position.y;
            _timeAtStart = 0;
            
            _initialY = transform.position.y;
            _timeInstantiated = SongManager.GetAudioSourceTime();
            
            //CalculateSpeed();
            _spriteRenderers.ToList().ForEach(a => a.color = Color.white);
            
            
        }

        private void OnObjectDisabled()
        {
            _runable = false;
            _tapable = false;
            _tapped = false;
            _timeAtStart = 0;
            _timeInstantiated = 0;
            
            
        }
        
        private float t;
        private double timeSinceInstantiated;
        private void Update()
        {
            if (!_runable)
                return;
            timeSinceInstantiated = SongManager.GetAudioSourceTime() - _timeInstantiated;
            t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

            transform.position = Vector3.Lerp(
                new Vector3(transform.position.x, _initialY, transform.position.z), 
                new Vector3(transform.position.x, _targetPos.y + _config.NoteEndPointCalibrate, transform.position.z), 
                t
            );
            

        }

        private void OnMouseDown()
        {
            //OnTap();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _layerToTap) != 0)
            {
                Debug.Log("Note is tappable");
                _tapable = true;
                _timeAtStart = Time.time;
            }

        }

        public void OnTap()
        {
            if (_tapped || !_tapable)
                return;
            _tapped = true;
            foreach (var note in _spriteRenderers)
            {
                note.DOColor(Color.yellow, 0.0f);
            }
            float currentTime = Time.time;
            var (score, type) = GameManager.Instance.CalculateScore(gameObject.transform.position, currentTime - _timeAtStart);
            ScoreManager.Instance.AddScore(score);
            GameManager.Instance.SpawnText(type, transform.position);

        }


        public void OnSpawn()
        {
            OnObjectSpawn();
        }

        public void OnDespawn()
        {
            OnObjectDisabled();
        }
    }

}
