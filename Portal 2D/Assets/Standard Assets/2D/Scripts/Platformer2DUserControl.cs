using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
		private bool crouch=false;

		public RectTransform time_bar;
		private float cur_time;
		private float max_time=100f;


        private void Awake()
        {
			cur_time = 100f;
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
			if (Input.GetKeyDown(KeyCode.CapsLock)){
				crouch=!crouch;
			}
			if (crouch&&cur_time>0f) {
				Time.timeScale = 0.5f;
				cur_time -= 0.25f;
			} else {
				crouch = false;
				Time.timeScale = 1.0f;
			}
			if (!crouch && cur_time < max_time) {
				cur_time += 0.1f;
			}
			time_bar.sizeDelta = new Vector2 (cur_time, time_bar.sizeDelta.y);
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, false, m_Jump);
            m_Jump = false;
        }
    }
}
