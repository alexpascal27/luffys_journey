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
    [SerializeField] private BoxCollider2D[] boxCollider2Ds;    
        
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

            SetGameObjectScale(3f);
            Vector3 currentGameObjectPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(currentGameObjectPosition.x - 0.2f,
                currentGameObjectPosition.y + 0.8f, currentGameObjectPosition.z);
            
            aflameAnimating = true;
            _ability1Animator.SetBool("Flame" , true);
            remainingCellAflameAnimationPeriod = cellAflameAnimationPeriod;
            // Enable collider
            foreach (BoxCollider2D flameCollider in boxCollider2Ds)
            {
                flameCollider.enabled = true;
            }
        }
        // Finished cell blazing
        if (aflameAnimating && remainingCellAflameAnimationPeriod <= 0)
        {
            aflameAnimating = false;
            _ability1Animator.SetBool("Flame", false);
            foreach (BoxCollider2D flameCollider in boxCollider2Ds)
            {
                flameCollider.enabled = false;
            }
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

    private void SetGameObjectScale(float scale)
    {
        Transform tempParentTransform = gameObject.transform.parent;
        gameObject.transform.parent = null;
        gameObject.transform.localScale = new Vector3(scale, scale);
        gameObject.transform.parent = tempParentTransform;
    }
}
