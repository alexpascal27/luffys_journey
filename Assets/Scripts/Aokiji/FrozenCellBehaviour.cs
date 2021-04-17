using System;
using UnityEngine;

namespace Aokiji
{
    public class FrozenCellBehaviour : MonoBehaviour
    {
        private Animator _animator;
        private const float CellFreezeAnimationDurection = 1.625f;
        private float _remainingAnimationTime = 0f;
        private bool _freezing = true;
        private bool _unfreezing = false;
        [SerializeField] private float stillFrozenDuration = 0.5f;
        private float _remainingStillTime = 0f;
        private bool _isStill = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetBool("Freezing", true);
            _remainingAnimationTime = CellFreezeAnimationDurection;
        }

        private void Update()
        {
            // Stop freezing animation
            if (_freezing && _remainingAnimationTime <= 0)
            {
                _animator.SetBool("Freezing", false);
                _freezing = false;

                if (stillFrozenDuration > 0)
                {
                    _isStill = true;
                    _remainingStillTime = stillFrozenDuration;
                }
                else
                {
                    _unfreezing = true;
                    _animator.SetBool("Unfreezing", true);
                    _remainingAnimationTime = CellFreezeAnimationDurection;
                }
            }
            
            // Stop waiting and unfreeze
            if (_isStill && _remainingStillTime <= 0)
            {
                _isStill = false;
                
                _unfreezing = true;
                _animator.SetBool("Unfreezing", true);
                _remainingAnimationTime = CellFreezeAnimationDurection;
            }
            
            // Stop unfreezing and die
            if (_unfreezing && _remainingAnimationTime <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (_remainingStillTime > 0) _remainingStillTime -= Time.deltaTime;
            if (_remainingAnimationTime > 0) _remainingAnimationTime -= Time.deltaTime;
        }
    }
}