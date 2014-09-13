using UnityEngine;

//Where all the physics & collision testing goes
public class DummyPhys : MonoBehaviour
{
	public GameObject m_master = null;
	public float m_force = 1.0f;
	public float m_torque = 1.0f;

	public Vector3 m_worldPos;
	public Quaternion m_worldRot;

	public Vector3 m_localToWorldPos;
	public Quaternion m_localToWorldRot;
	public Vector3 m_velocity;
	
	protected void OnValidate()
	{
		m_force = m_force > 1.0f ? 1.0f : m_force < 0.0f ? 0.0f : m_force;
		m_torque = m_torque > 1.0f ? 1.0f : m_torque < 0.0f ? 0.0f : m_torque;
	}

	public void Start()
	{
	}

	private void ChildIndependentRotation()
	{
		Transform parent = rigidbody.transform.parent;
		CharacterController cc = rigidbody.GetComponentInParent<CharacterController> ();
		rigidbody.transform.parent = null;
		cc.transform.rotation *= Quaternion.AngleAxis (45 * Time.deltaTime, Vector3.up);
		rigidbody.transform.parent = parent;
	}

	private void RelativeToMaster()
	{
	
		m_worldPos = gameObject.transform.position;
		m_localToWorldPos = gameObject.transform.parent.TransformDirection(gameObject.transform.localPosition) 
		              + gameObject.transform.parent.position;

		m_worldRot = gameObject.transform.rotation;
		m_localToWorldRot = gameObject.transform.parent.rotation * gameObject.transform.localRotation;

		
	}

	private void FollowMaster()
	{
		gameObject.rigidbody.velocity = Vector3.zero;
		Vector3 dv = m_master.transform.position - gameObject.transform.position;
		float f = (dv.magnitude / Time.fixedDeltaTime);
		gameObject.rigidbody.AddForce(f * dv.normalized, ForceMode.Impulse);
		
		Vector3 sCrossE_Z = Vector3.Cross (gameObject.transform.forward, 
		                                   m_master.transform.rotation * Vector3.forward);
		
		Vector3 sCrossE_X = Vector3.Cross (gameObject.transform.right,
		                                   m_master.transform.rotation * Vector3.right);
		
		Vector3 sCrossE_Y = Vector3.Cross (gameObject.transform.up, 
		                                   m_master.transform.rotation * Vector3.up);
		
		//make up for fragment of acceleration taken off by the delta time
		Vector3 wZ = sCrossE_Z.normalized * (Mathf.Asin (sCrossE_Z.magnitude) / Time.fixedDeltaTime); 
		Vector3 wX = sCrossE_X.normalized * (Mathf.Asin (sCrossE_X.magnitude) / Time.fixedDeltaTime);
		Vector3 wY = sCrossE_Y.normalized * (Mathf.Asin (sCrossE_Y.magnitude) / Time.fixedDeltaTime);
		
		Rigidbody rb = gameObject.rigidbody;
		Quaternion q = gameObject.transform.rotation * rb.inertiaTensorRotation;
		Vector3 angV = Quaternion.Inverse (q) * (wX + wZ + wY);
		Vector3 t = q * Vector3.Scale (rb.inertiaTensor, angV);
		
		this.rigidbody.AddTorque (t * m_torque, ForceMode.Impulse);
		this.rigidbody.maxAngularVelocity = t.magnitude * m_torque;
	}

	public float yaw, pitch, roll;

	public void OnCollisionEnter(Collision c)
	{
	}

	public void OnCollisionExit(Collision c)
	{
	}
	

	public void Update()
	{
		ChildIndependentRotation ();
		//GameObject.DestroyObject(gameObject.GetComponent<FixedJoint>());
		//this.rigidbody.AddForce(Vector3.down * (1.05f / Time.fixedDeltaTime));
		//this.rigidbody.AddTorque (Vector3.right * 10.0f);
	}

}
