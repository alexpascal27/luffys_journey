using System;
using UnityEngine;

public class CellAflame : MonoBehaviour
{
    [SerializeField] public float periodBetweenWarningAndAflame;
    private float remainingWait = 0f;
    private bool waiting = false;
    [SerializeField] public float warningAnimationPeriod;
    private float remainingWarningAnimationPeriod = 0f;
    private bool warningAnimating = false;
    [SerializeField] public float cellAflameAnimationPeriod;
    private bool aflameAnimating = false;
    private float remainingCellAflameAnimationPeriod = 0f;
    
    [SerializeField] private Animator _ability1Animator;

    private void Start()
    {
        remainingWarningAnimationPeriod = warningAnimationPeriod;
        warningAnimating = true;
        _ability1Animator.SetBool("Warning" , true);
    }

    private void Update()
    {
        // Warning finished
        if (warningAnimating && remainingWarningAnimationPeriod <= 0)
        {
            warningAnimating = false;
            _ability1Animator.SetBool("Warning" , false);

            waiting = true;
            remainingWait = periodBetweenWarningAndAflame;
        }
        // Waiting between warning and flame finished
        if (waiting && remainingWait <= 0)
        {
            waiting = false;

            aflameAnimating = true;
            _ability1Animator.SetBool("Flame" , true);
            remainingCellAflameAnimationPeriod = cellAflameAnimationPeriod;
        }
        // Finished cell blazing
        if (aflameAnimating && remainingCellAflameAnimationPeriod <= 0)
        {
            aflameAnimating = false;
            _ability1Animator.SetBool("Flame", false);
            
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (warningAnimating)
        {
            remainingWarningAnimationPeriod -= Time.deltaTime;
        }

        if (aflameAnimating)
        {
            remainingCellAflameAnimationPeriod -= Time.deltaTime;
        }

        if (waiting)
        {
            remainingWait -= Time.deltaTime;
        }
    }
}
