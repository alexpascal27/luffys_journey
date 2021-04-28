
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    // Animation
    [SerializeField] private Animator _animator;
    [SerializeField] private float punchAnimationTime = 1f;
    private float currentPunchAnimationTime = 0f;
    public bool currentlyPunching = false;
    [SerializeField] private BoxCollider2D punchBoxCollider2D;
    
    // Movement
    public CharacterController2D controller;
    
    [SerializeField][Range(0f,1000f)]private float runSpeed = 50f;
    float _horizontalMove = 0f;
    float _verticalMove = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            if (currentPunchAnimationTime <= 0)
            {
                _animator.SetBool("Attacking", true);
                currentPunchAnimationTime = punchAnimationTime;
                currentlyPunching = true;
                if(!punchBoxCollider2D.enabled)punchBoxCollider2D.enabled = true;
                _rb.velocity = new Vector2(0,0);
            }
        }
        
        if (currentlyPunching && currentPunchAnimationTime <= 0)
        {
            _animator.SetBool("Attacking", false);
            currentlyPunching = false;
            if(punchBoxCollider2D.enabled) punchBoxCollider2D.enabled= false;
        }
        
        _horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        _verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
    }

    void FixedUpdate()
    {
        if (!currentlyPunching)
        {
            controller.Move(_horizontalMove * Time.fixedDeltaTime, _verticalMove*Time.fixedDeltaTime);
            
            if ((_horizontalMove != 0 || _verticalMove != 0))
            {
                _animator.SetBool("Running", true);
            }
            else
            {
                _animator.SetBool("Running", false);
            }
        }
        else
        {
            _animator.SetBool("Running", false);
        }
        
        
        
        if (currentPunchAnimationTime > 0)
        {
            currentPunchAnimationTime -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject collisionGameObject = other.gameObject;
        if (collisionGameObject.CompareTag("FinishedLevelTrigger"))
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            // If we just existed level 3
            if (nextIndex > 2)
            {
                Debug.Log("You win!");
                // Load win screen
                SceneManager.LoadScene(4);
            }
            else
            {
                SceneManager.LoadScene(6);
            }
        }
    }
}
