using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DisableScriptsAtStart : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] scriptsToDisable;
        [SerializeField] private float disableTime;
        private float _remainingTime;
        private bool _scriptsDisabled = true;

        private void Start()
        {
            _remainingTime = disableTime;
            
            // Disable
            EnableDisableScripts(false);
        }

        private void Update()
        {
            if (_remainingTime <= 0 && _scriptsDisabled)
            {
                _scriptsDisabled = false;
                // Enable Scripts
                EnableDisableScripts(true);
            }
        }

        private void FixedUpdate()
        {
            if (_remainingTime > 0) _remainingTime -= Time.deltaTime;
        }

        private void EnableDisableScripts(bool enable)
        {
            foreach (MonoBehaviour script in scriptsToDisable)
            {
                script.enabled = enable;
            }
        }
    }
}