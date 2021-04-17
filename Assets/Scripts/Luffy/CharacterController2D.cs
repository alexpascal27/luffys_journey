using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f; // How much to smooth out the movement
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true; // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private SpriteRenderer spriteRenderer;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}


	public void Move(float horizontalMove, float verticalMove)
	{
		MoveHorizontally(horizontalMove);
		MoveVertically(verticalMove);
	}

	private void MoveHorizontally(float move)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(move, m_Rigidbody2D.velocity.y);
		// And then smoothing it out and applying it to the character
		m_Rigidbody2D.velocity =
			Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

		// If the input is moving the player right and the player is facing left or vice versa
		if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight))
		{
			// ... flip the player.
			Flip();
		}
	}

	private void MoveVertically(float move)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, move);
		// And then smoothing it out and applying it to the character
		m_Rigidbody2D.velocity =
			Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
	}


private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		
		spriteRenderer.flipX = !spriteRenderer.flipX;
                
		int childCount = transform.childCount;
		for(int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!child.gameObject.CompareTag("UI"))
			{
				child.Rotate(0f, 180f, 0f);
			}
		}
	}
}
