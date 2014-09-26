using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		public class HandIdleAnimationState : State<Hand>
		{
			public void Enter(Hand hand)
			{
			}
			
			public void Execute(Hand hand)
			{
				if ( hand.AnimationInfo.GetBool("Fist") == false &&  
				    hand.AnimationInfo.GetBool("Point") == false )
				{
					hand.AnimationInfo.SetBool("Idle", true);
				}
				else
				{
					hand.AnimationInfo.SetBool("Idle", false);
				}
			}
			
			public void Exit(Hand hand) 
			{
			}
		}

		public class HandPointAnimationState : State<Hand>
		{
			private float m_fLastTriggerVal;
			
			public void Enter(Hand hand)
			{
				m_fLastTriggerVal = 0.0f;
			}
			
			public void Execute(Hand hand)
			{
				// Fist with right or left
				float fTriggerVal = Mathf.Lerp( m_fLastTriggerVal, hand.Controller.Trigger, 0.1f );
				m_fLastTriggerVal = fTriggerVal;
				
				if ( fTriggerVal > 0.01f )
				{
					hand.AnimationInfo.SetBool( "Point", true );
				}
				else
				{
					hand.AnimationInfo.SetBool( "Point", false );
				}
			}
			
			public void Exit(Hand hand) 
			{
				hand.AnimationInfo.SetBool ("Point", false);
			}
		}

		
		public class HandClenchAnimationState : State<Hand>
		{
			private float m_fLastTriggerVal;
			
			public void Enter(Hand hand)
			{
				m_fLastTriggerVal = 0.0f;
			}
			
			public void Execute(Hand hand)
			{
				// Fist with right or left
				float fTriggerVal = Mathf.Lerp( m_fLastTriggerVal, hand.Controller.Trigger, 0.1f );
				m_fLastTriggerVal = fTriggerVal;
				
				if ( fTriggerVal > 0.01f )
				{
					hand.AnimationInfo.SetBool( "Fist", true );
				}
				else
				{
					hand.AnimationInfo.SetBool( "Fist", false );
				}
				
				hand.AnimationInfo.SetFloat("FistAmount", fTriggerVal);
			}
			
			public void Exit(Hand hand)
			{
				hand.AnimationInfo.SetBool ("Fist", false);
			}
		}

	}
}
