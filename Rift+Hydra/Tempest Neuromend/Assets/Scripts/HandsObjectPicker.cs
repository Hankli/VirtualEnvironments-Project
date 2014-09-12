using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tempest
{
	namespace RazorHydra
	{

		public class HandsObjectPicker : MonoBehaviour
		{
			class EventHandler : MonoBehaviour
			{
				public Transform m_handle;
				
				private void OnJointBreak(float breakForce)
				{
					HandsObjectPicker p = m_handle.GetComponent<HandsObjectPicker> ();
					p.OnJointBreak (breakForce); //pass responsibility to parent
				}
			}

			public int m_exclusionMask;
			public float m_reachDistance;
			public float m_gripMaxForceResistance;
			public float m_gripMaxTorqueResistance;
			public float m_gripMinTriggerValue;
			public float m_gripMaxTriggerValue;
			public string m_gripJointName;
			public float m_gripDistance;

			private Hand m_hand;
			private GameObject m_gripJoint;
			private ConfigurableJoint m_gripConstraint;

			private int m_connectedLayerMask;
	
			private void Start()
			{
				m_hand= GetComponentInParent<Hand> ();
				m_gripJoint = GetJoint();
				m_gripConstraint = null;
				m_exclusionMask = 0;

			}

			public void OnJointBreak(float breakForce)
			{
				//joint is automatically removed from game
				//restore old object's settings
				m_gripConstraint.connectedBody.gameObject.layer = m_connectedLayerMask;
				m_gripConstraint.connectedBody.useGravity = true;

				//retrieve hand for application of extra velocity from throw
				m_gripConstraint.connectedBody.AddForce (m_hand.rigidbody.velocity, ForceMode.Impulse);
				m_gripConstraint.connectedBody.AddTorque (m_hand.rigidbody.angularVelocity, ForceMode.Impulse);

				//destroy dummy joint object attached to the constraint
				m_gripConstraint = null;
			}

			private GameObject GetJoint()
			{
				GameObject joint = null;
				foreach(Transform child in transform.GetComponentsInChildren<Transform>())
				{
					if(child.name.Equals (m_gripJointName))
					{
						joint = child.gameObject;
						break;
					}
				}
				return joint;
			}

			private void UpdateConnections()
			{
				m_gripConstraint.breakForce = (m_hand.TriggerValue * m_gripMaxForceResistance) / Time.fixedDeltaTime;
				m_gripConstraint.breakTorque = (m_hand.TriggerValue * m_gripMaxTorqueResistance) / Time.fixedDeltaTime; 
			}

			private void PickupScan()
			{
				//check if player is trying to grab something at all an if something is already grabbed
				if(m_hand.TriggerValue < m_gripMinTriggerValue ||
				   m_hand.TriggerValue > m_gripMaxTriggerValue || m_gripConstraint)
				{
					return;
				}

    			//check if the grabbing limb exists
				RaycastHit hit;
				int layer = ~(Physics.DefaultRaycastLayers << (m_gripJoint.layer | m_exclusionMask));
	
	     		//cast down palm's local y axis to check for objects within reach
				if( Physics.Raycast (m_gripJoint.transform.position, -m_gripJoint.transform.up, out hit, m_reachDistance, layer) )
				{
					Rigidbody hitRB = hit.rigidbody;
						
					if(hitRB && !hitRB.isKinematic)
					{
						//create dummy object that joins with picked object
						GameObject o = new GameObject();
					
						o.AddComponent<EventHandler>().m_handle = transform;
						o.transform.parent = m_gripJoint.transform;
						o.layer = m_gripJoint.layer;

						m_connectedLayerMask = hitRB.gameObject.layer; 
						hitRB.gameObject.layer = m_gripJoint.layer;
						hitRB.useGravity = false;
					
						//connect joints
						Rigidbody rb = o.AddComponent<Rigidbody>();
						rb.useGravity = false;
						rb.isKinematic = true;
					
						//configure joints that binds the picked object with dummy joint
						m_gripConstraint = gameObject.AddComponent<ConfigurableJoint>();

						m_gripConstraint.xMotion = ConfigurableJointMotion.Locked;
						m_gripConstraint.yMotion = ConfigurableJointMotion.Locked;
						m_gripConstraint.zMotion = ConfigurableJointMotion.Locked;

						m_gripConstraint.angularXMotion = ConfigurableJointMotion.Locked;
						m_gripConstraint.angularYMotion = ConfigurableJointMotion.Locked;
						m_gripConstraint.angularZMotion = ConfigurableJointMotion.Locked;

						m_gripConstraint.anchor = o.transform.position;
						m_gripConstraint.connectedAnchor = o.transform.position;

						m_gripConstraint.projectionDistance = m_gripDistance;
						m_gripConstraint.connectedBody = hitRB;
					} //end if
				}
			}

			private void Update()
			{
				PickupScan();

				if(m_gripConstraint)
				{
					UpdateConnections();
				}
			}

		}

	}
}