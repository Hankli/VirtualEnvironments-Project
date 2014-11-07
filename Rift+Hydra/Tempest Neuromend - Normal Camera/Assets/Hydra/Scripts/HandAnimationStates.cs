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

		public class HandClenchAnimationState : State<Hand>
		{
			private float m_fLastTriggerVal;
			private float m_interpolateFist;
			
			public void Enter(Hand hand)
			{
				m_interpolateFist = 0.2f;
				m_fLastTriggerVal = 0.0f;
			}
			
			public void Execute(Hand hand)
			{
				float fTriggerVal = Mathf.Lerp( m_fLastTriggerVal, hand.TriggerValue, m_interpolateFist);
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
