using UnityEngine;

		public class CameraDepth : MonoBehaviour
		{
			public bool RenderDepth = true;

			void OnEnable()
			{
				SetCameraDepth();
			}

			void OnValidate()
			{
				SetCameraDepth();
			}

			void SetCameraDepth()
			{
				var cam = GetComponent<Camera>();
				if (RenderDepth)
					cam.depthTextureMode |= DepthTextureMode.Depth;
				else
					cam.depthTextureMode &= ~DepthTextureMode.Depth;
			}
		}
