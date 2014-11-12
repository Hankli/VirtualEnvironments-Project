using UnityEngine;

namespace Tempest
{
	namespace Menu
	{
		public class ColouredTexture
		{
			public static Texture2D MakeTex( int width, int height, Color col )
			{
				Color[] pix = new Color[width * height];
				for( int i = 0; i < pix.Length; ++i )
				{
					pix[ i ] = col;
				}
				Texture2D result = new Texture2D( width, height );
				result.SetPixels( pix );
				result.Apply();
				return result;
			}
		}
	}
}
