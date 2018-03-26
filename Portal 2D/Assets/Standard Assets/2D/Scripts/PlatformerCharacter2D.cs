using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
		[SerializeField] private float m_DashSpeed = 80f;                    // dash speed 
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		private bool wall_check;
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
		private Transform m_WallCheck; // a position marking where to check for walls
		public LayerMask walllayermask;
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
		private Transform char_arm;

		private bool can_double_jump = false;
		private bool is_dashing = false;
		private int timer = 0;
		private bool wall_sliding = false;

        private void Awake()
        {
            // Setting up references.
			char_arm=transform.Find("char_arm");
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
			m_WallCheck = transform.Find("WallCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
			m_Grounded = false;
			wall_check = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);
			if (m_Grounded && wall_sliding) {
				m_Anim.SetBool ("WallSlide", false);
			}

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
			m_Anim.SetFloat("hSpeed",Math.Abs(m_Rigidbody2D.velocity.x));


			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				is_dashing = true;
				timer = 13;
			}
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

				if (is_dashing)
				{
					Vector3 v = m_Rigidbody2D.velocity;
					v.y = 0;
					m_Rigidbody2D.velocity = v;
					if (m_FacingRight)
						m_Rigidbody2D.AddForce(new Vector2(m_DashSpeed, 0f));
					else
						m_Rigidbody2D.AddForce(new Vector2(-1 * m_DashSpeed, 0f));
				}

				if (timer > 0) --timer;   // will decriment timer for a 5 second dash

				if (timer <= 0) is_dashing = false;

                // Move the character


                float vx = m_Rigidbody2D.velocity.x;

                float responsiveness = 0.5f;

                if (Math.Abs(vx - move * m_MaxSpeed) < responsiveness)
                    vx = move * m_MaxSpeed;
                else
                {
                    if (vx < move * m_MaxSpeed)
                        vx += responsiveness;
                    else
                        vx -= responsiveness;
                }
                

                m_Rigidbody2D.velocity = new Vector2(vx, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }

				if (jump && !wall_sliding)
				{



					if (m_Grounded && !wall_sliding)
					{
						m_Grounded = false;
						m_Anim.SetBool("Ground", false);
						Vector3 v = m_Rigidbody2D.velocity;
						v.y = 0;
						m_Rigidbody2D.velocity = v;
						m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
						can_double_jump = true;
					}
					else
					{
						if (can_double_jump)
						{
							m_Grounded = false;
							m_Anim.SetBool("Ground", false);
							Vector3 v = m_Rigidbody2D.velocity;
							v.y = 0;
							m_Rigidbody2D.velocity = v;
							m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
							can_double_jump = false;
						}
					}

				}

				if (!m_Grounded)
				{
					wall_check = Physics2D.OverlapCircle(m_WallCheck.position, 0.1f, walllayermask);
					if (wall_check) {
						handle_wall_slide (jump);

					} else {
						m_Anim.SetBool ("WallSlide", false);
					}

				}

				if (m_Grounded && wall_sliding) {
					wall_sliding = false;
				}
            }
/*            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }*/
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
			theScale = char_arm.localScale;
			theScale.x *= -1;
			char_arm.localScale = theScale;
        }

		public void handle_wall_slide(bool jump)
		{
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -1);
			wall_sliding = true;
			m_Anim.SetBool ("WallSlide", true);


			if (m_FacingRight)
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{

					Vector3 v = m_Rigidbody2D.velocity;
					v.y = 0;
					m_Rigidbody2D.velocity = v;
					m_Rigidbody2D.AddForce(new Vector2(-1750, m_JumpForce * 1.8f));
				}


			}
			else if (!m_FacingRight)
			{

				if (Input.GetKeyDown(KeyCode.Space))
				{

					Vector3 v = m_Rigidbody2D.velocity;
					v.y = 0;
					m_Rigidbody2D.velocity = v;
					m_Rigidbody2D.AddForce(new Vector2(1750, m_JumpForce * 1.8f));
				}

			}

			// Flip(); 

		}
    }
}
