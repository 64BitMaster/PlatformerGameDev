using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParallaxCamera:MonoBehaviour {
	public delegate void ParallaxCameraDelegate(float deltaMovement);
	public ParallaxCameraDelegate onCameraTranslate;
	private float oldPosition;
    public float FOV = 60.0f;
    
    
    private void Awake()
    {

    }
    void Start() {
		oldPosition = transform.position.x;
        
	}
	void Update() {
		if (transform.position.x != oldPosition) {
			if (onCameraTranslate != null) {
				float delta = oldPosition - transform.position.x;
				onCameraTranslate(delta);
			}
			oldPosition = transform.position.x;
		}
        Camera.main.fieldOfView = FOV;
	}
}
