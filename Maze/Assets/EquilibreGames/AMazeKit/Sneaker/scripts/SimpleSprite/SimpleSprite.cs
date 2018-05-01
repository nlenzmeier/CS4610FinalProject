using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class SimpleSprite : MonoBehaviour {

	/// <summary>
	/// Animation : the frame coordinates with a name
	/// </summary>
	[System.Serializable]
	public class SpriteData {
		public Vector2 coords;
		public Vector2 size;
		public Vector2 scale;
		public Vector2 offset;
	};
	
	[SerializeField]
	private SpriteData spriteInfo;
	
	// Material in which is taken the animation
	Material myMaterial;
	Vector2 textureSize;

	void Awake() {
		// get material
		//if (Application.isEditor && !Application.isPlaying)
			//myMaterial = renderer.sharedMaterial;
		//else
			myMaterial = GetComponent<Renderer>().sharedMaterial;
	}
	
	void Start() {
		UpdateSprite();		
	}
	
	void UpdateSprite() {
		// Determine texture size
		textureSize = new Vector2(myMaterial.mainTexture.width, myMaterial.mainTexture.height);
		
		// Use it to determine sprite offsets (coords / texture size) and texture scale (sprite size / texture size)
		spriteInfo.scale = new Vector2(spriteInfo.size.x / textureSize.x, spriteInfo.size.y / textureSize.y);
		spriteInfo.offset = new Vector2(spriteInfo.coords.x / textureSize.x, 1 - (spriteInfo.coords.y+spriteInfo.size.y) / textureSize.y);
		
		// Set texture scale offset accordingly
		myMaterial.SetTextureScale  ("_MainTex", spriteInfo.scale);
		myMaterial.SetTextureOffset ("_MainTex", spriteInfo.offset);

	}
}
