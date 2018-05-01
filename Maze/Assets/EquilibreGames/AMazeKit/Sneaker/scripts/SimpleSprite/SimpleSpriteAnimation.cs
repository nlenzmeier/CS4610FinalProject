using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SimpleSpriteAnimation : MonoBehaviour {

	[System.Serializable]
	public class SpriteData {
		public Vector2 coords;
		public Vector2 size;
		public Vector2 scale;
		public Vector2 offset;
	};
	
	[System.Serializable]
	public class SpriteDataList {
		public string name;
		public SpriteData[] spriteList;
		public bool loop;
		public float fps = 30;
	}
	
	public SpriteDataList[] animationList;
	
	// Material in which is taken the animation
	Material myMaterial;
	Vector2 textureSize;
	float nextUpdate;
	int animationIndex;
	int spriteIndex;
	float rate;

	void Awake() {
		// get material (shared ?)
		myMaterial = GetComponent<Renderer>().material;
		nextUpdate = 0;
		
		// Determine texture size
		textureSize = new Vector2(myMaterial.mainTexture.width, myMaterial.mainTexture.height);
	}
	
	void Start() {
		animationIndex = -1;
		if (animationList.Length > 0)
			SetAnimation(animationList[0].name);
		else
			enabled = false;
	}
	
	void Update() {
		if (Time.time > nextUpdate) {
			// Set texture scale offset accordingly
			SpriteDataList spriteAnim = animationList[animationIndex];
			SpriteData spriteInfo = spriteAnim.spriteList[spriteIndex++];
			//Debug.Log(System.DateTime.Now + " : " + transform.parent.parent.name + "/" + transform.parent.name + "/" + this.name + " > Set Sprite Coords " + spriteInfo.coords);
			if (!myMaterial.GetTextureScale("_MainTex").Equals(spriteInfo.scale))
				myMaterial.SetTextureScale  ("_MainTex", spriteInfo.scale);
			myMaterial.SetTextureOffset ("_MainTex", spriteInfo.offset);
			
			if (spriteIndex >= animationList[animationIndex].spriteList.Length) {
				if (animationList[animationIndex].loop)
					spriteIndex = 0;
				else {
					spriteIndex--;
					rate = 1; // or enabled = false; but change the FPS for the whole game
				}
			}
			
			nextUpdate = Time.time + rate;
		}
	}
	
	public void SetAnimation(string name) {
		string upperName = name.ToUpper();
		int newIndex = animationIndex;
		for (int i=0;i<animationList.Length;i++) {
			if (animationList[i].name.ToUpper().Equals(upperName))
				newIndex = i;
		}
		if (newIndex != animationIndex) {
			//Debug.Log(transform.parent.parent.name + "/" + transform.parent.name + "/" + this.name + " > Set Animation to " + name);
			// Update offset and scale and current animation rate
			spriteIndex = 0;
			animationIndex = newIndex;
			SpriteDataList spriteAnim = animationList[animationIndex];
			if (spriteAnim.fps <= 0)
				rate = 10;
			else
				rate = 1 / spriteAnim.fps;
			for (int i=0;i<spriteAnim.spriteList.Length;i++) {
				SpriteData spriteInfo = spriteAnim.spriteList[i];
				// Use texture size to determine sprite offsets (coords / texture size) and texture scale (sprite size / texture size)
				spriteInfo.scale = new Vector2(spriteInfo.size.x / textureSize.x, spriteInfo.size.y / textureSize.y);
				spriteInfo.offset = new Vector2(spriteInfo.coords.x / textureSize.x, 1 - (spriteInfo.coords.y+spriteInfo.size.y) / textureSize.y);
			}
			// Immediate update
			nextUpdate = Time.time;
		}
	}
}
