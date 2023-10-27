using System;
using UnityEngine;

namespace BIL
{
	namespace Display
	{
		[ExecuteInEditMode]
		public class BarrelDistortion : MonoBehaviour
		{
			/** 
			 * Applies barrel distortion shader during post processing
			 * **/
			public Material material;

			public void RefreshMaterial(float k, float kcube)
			{
				material.SetFloat("kcube", kcube);
				material.SetFloat("k", k);
			}

			// Postprocess the image
			void OnRenderImage(RenderTexture source, RenderTexture destination)
			{
				Graphics.Blit(source, destination, material);
			}
		}
	}
}