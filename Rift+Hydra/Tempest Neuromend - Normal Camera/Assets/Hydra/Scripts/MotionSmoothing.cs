
using UnityEngine;

namespace Tempest
{
	namespace RazorHydra
	{
		public class MotionSmoother
		{
			public static Quaternion CalculateOrientation(Plugin.sixenseControllerData[] cd)
			{
				//return a quaternion that is an average of all sampled rotation data
				//http://wiki.unity3d.com/index.php/Averaging_Quaternions_and_Vectors
				
				float dot = 0.0f;
				Vector4 v = Vector4.zero;
				
				for(int i=0; i<cd.Length; i++)
				{
					if(i > 0)
					{
						dot = -cd[i - 1].rot_quat[0] * -cd[i].rot_quat[0] + -cd[i - 1].rot_quat[1] * -cd[i].rot_quat[1] + 
							   cd[i - 1].rot_quat[2] * cd[i].rot_quat[2] + cd[i - 1].rot_quat[3] * cd[i].rot_quat[3];
						
						if(dot < 0.0f)
						{
							cd[i].rot_quat[0] = -cd[i].rot_quat[0];
							cd[i].rot_quat[1] = -cd[i].rot_quat[1];
							cd[i].rot_quat[2] = -cd[i].rot_quat[2];
							cd[i].rot_quat[3] = -cd[i].rot_quat[3];
						}
					}
					
					float a = 1.0f / (i + 1);
					
					v.x -= cd[i].rot_quat[0] * a;
					v.y -= cd[i].rot_quat[1] * a;
					v.z += cd[i].rot_quat[2] * a;
					v.w += cd[i].rot_quat[3] * a;
				}
				
				dot = v.w * v.w + v.x * v.x + v.y * v.y + v.z * v.z;
				
				float invMagnitude = 1.0f / Mathf.Sqrt (v.w * v.w + v.x * v.x + v.y * v.y + v.z * v.z);
				v *= invMagnitude;
				
				return new Quaternion (v.x, v.y, v.z, v.w);
			}
			
			public static Vector3 CalculatePosition(float smoothFactor, Plugin.sixenseControllerData[] cd)
			{
				//use simple exponential smoothing for positional input data
				//x(0) = sample[0]
				//s(i) = a * x(i) + (1.0f - a) * s(i-1)
				
				Vector3 s_0 = new Vector3(cd[0].pos[0], cd[0].pos[1], -cd[0].pos[2]);
				Vector3 s_t = s_0;
				
				for(int i = 1; i < cd.Length; i++)
				{
					s_t = smoothFactor * s_0 + (1.0f - smoothFactor) * s_t;
					s_0 = new Vector3(cd[i].pos[0], cd[i].pos[1], -cd[i].pos[2]);
				}
				
				return s_t;
			}
		}
	}
}

