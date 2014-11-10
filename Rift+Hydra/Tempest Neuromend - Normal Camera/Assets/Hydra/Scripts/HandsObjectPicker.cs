using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tempest
{
	namespace RazorHydra
	{
		public class HandsObjectPicker : MonoBehaviour
		{
			class JointBreakEventPasser : MonoBehaviour
			{
				public Transform m_target;
				
				private void OnJointBreak(float breakForce)
				{
					HandsObjectPicker p = m_target.GetComponent<HandsObjectPicker> ();
					p.OnJointBreak (breakForce); //pass responsibility to parent
				}
			}

			
			public string m_gripJointName;

			private float m_pullReach = 0.8f;
			private float m_pullRadius = 0.1f;

			private float m_gripTriggerValue = 0.3f;
			private float m_gripBreakResistance = 50000.0f;

			private Hand m_hand;
			private GameObject m_gripJoint;
			private ConfigurableJoint m_gripConstraint;

			private int m_connectedLayerMask;

			private void Start()
			{
				m_hand = GetComponentInParent<Hand> ();
				m_gripJoint = GetJoint();
				m_gripConstraint = null;

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

				ThrowableObject throwableObj = m_gripConstraint.connectedBody.GetComponent<ThrowableObject>();
				if(throwableObj != null)
				{
					throwableObj.SelfDestruct();
				}

				//destroy dummy joint object attached to the constraint
				GameObject.Destroy (m_gripConstraint.gameObject);
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
				m_gripConstraint.breakForce = m_hand.TriggerValue * m_gripBreakResistance;
				m_gripConstraint.breakTorque = m_hand.TriggerValue * m_gripBreakResistance;
			}

			private void PickupScan()
			{
				//check if player is trying to grab something at all an if something is already grabbed
				if(m_hand.TriggerValue < m_gripTriggerValue || m_gripConstraint != null)
				{
					return;
				}

    			//check if the grabbing limb exists
				RaycastHit hit;
				int layer = ~(Physics.DefaultRaycastLayers << m_gripJoint.layer);
	
	     		//cast down palm's local y axis to check for objects within reach
				Ray ray = new Ray (m_gripJoint.transform.position, -m_gripJoint.transform.up);

				if(Physics.SphereCast (ray, m_pullRadius, out hit, m_pullReach, layer) )
				{
					Rigidbody hitRB = hit.rigidbody;

					if(hitRB && !hitRB.isKinematic)
					{
						//create dummy object that joins with picked object
						GameObject o = new GameObject("Holding Dummy");

						//add component in dummy object to pass joint break event back to parent
						o.AddComponent<JointBreakEventPasser>().m_target = transform;
					
						//pull the object closer to hand
						CapsuleCollider collider = GetComponent<CapsuleCollider> ();
						Vector3 holdPoint = collider.ClosestPointOnBounds(hit.point);

						hitRB.transform.Translate(holdPoint - hit.point, Space.World);

						//turn off collision between hand and object and turn off gravity of held object
						m_connectedLayerMask = hitRB.gameObject.layer; 
						hitRB.gameObject.layer = m_gripJoint.layer;
						hitRB.useGravity = false;

						//connect joints
						Rigidbody rb = o.AddComponent<Rigidbody>();
						rb.useGravity = false;
						rb.isKinematic = true;
					
						o.transform.parent = m_gripJoint.transform;
						o.layer = m_gripJoint.layer;

						//configure joints that binds the picked object with dummy joint
						m_gripConstraint = o.AddComponent<ConfigurableJoint>();

						m_gripConstraint.xMotion = ConfigurableJointMotion.Locked;
						m_gripConstraint.yMotion = ConfigurableJointMotion.Locked;
						m_gripConstraint.zMotion = ConfigurableJointMotion.Locked;

						m_gripConstraint.angularXMotion = ConfigurableJointMotion.Locked;
						m_gripConstraint.angularYMotion = ConfigurableJointMotion.Locked;
						m_gripConstraint.angularZMotion = ConfigurableJointMotion.Locked;

						//set anchors
						m_gripConstraint.anchor = m_gripJoint.transform.position;
						m_gripConstraint.connectedAnchor = hitRB.position;
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


			private void OnGUI()
			{
			}
		}

	}
}