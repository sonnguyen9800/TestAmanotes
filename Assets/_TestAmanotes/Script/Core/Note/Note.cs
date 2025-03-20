using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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

        public void OnObjectSpawn()
        {
            _runable = true;
            _tapable = false;
            _tapped = false;
            
            
            _targetPos = transform.position;
            _targetPos.y = NoteSpawnerManager.Instance.TapLineTransform.position.y;
            _timeAtStart = 0;
            
            _initialY = transform.position.y;
            _timeInstantiated = SongManager.GetAudioSourceTime();
            
            CalculateSpeed();
            _spriteRenderers.ToList().ForEach(a => a.color = Color.white);
            
        }

        public void OnObjectDisabled()
        {
            _runable = false;
            _tapable = false;
            _tapped = false;
            _timeAtStart = 0;
            _timeInstantiated = 0;
        }


        private void CalculateSpeed()
        {
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - _timeInstantiated;
            float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
            
            float totalLerpTime = SongManager.Instance.noteTime * 2;
            Vector3 startPos = new Vector3(transform.position.x, _initialY, transform.position.z);
            Vector3 targetPos = new Vector3(transform.position.x, _targetPos.y, transform.position.z);
            if (t == 0)
                _speedFall = 0;
            else
            {
                _speedFall = Vector3.Distance(startPos, targetPos) / t;

            }

        }

        private void Update()
        {
            if (!_runable)
                return;
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - _timeInstantiated;
            float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));


            transform.localPosition = Vector3.Lerp(Vector3.up * _initialY,
                Vector3.up * _targetPos.y, t); 

            // transform.localPosition += Vector3.down * _speedFall * Time.deltaTime;

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
                note.DOColor(Color.yellow, 0.2f);
                note.DOFade(0, 0.2f);

            }

            float currentTime = Time.time;
            int score = GameManager.Instance.CalculateScore(gameObject.transform.position, currentTime - _timeAtStart);
            ScoreManager.Instance.AddScore(score);
        }
        
        
    }

}
