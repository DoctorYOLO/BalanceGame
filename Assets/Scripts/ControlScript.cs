using UnityEngine;
using System.Collections;

public class ControlScript : MonoBehaviour {

	public float stopStart = 1.5f, speed = 5f, rotationSpeed = 100f, heightPlayer = 1f;
	private float mag, angleToTarget;
	private Vector3 dir;
	private Vector3 target = new Vector3 ();
	private Vector3 lastTarget = new Vector3 ();
	private bool walk;
	Ray ray;
	RaycastHit hit;

	public PhotonView name;
	public GameObject graphics;
	public GameObject me;

	public bool isPause = false;

	// Use this for initialization
	void Awake () 
	{
		name.RPC ("updateName", PhotonTargets.AllBuffered, PhotonNetwork.playerName);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Input.GetButtonDown ("Fire1")) 
		{
			if (Physics.Raycast (ray, out hit, 100)) 
			{
				if (hit.collider.CompareTag ("Ground")) 
				{
					target = hit.point;
				}
			}
				
		}
		if (Input.GetKeyDown (KeyCode.Tab)) {
			isPause = true;
		}
		if(Input.GetKeyUp (KeyCode.Tab)){
			isPause = false;
		}
		LookAtThis ();
		MoveTo ();
	}

	void OnGUI(){
		if (isPause) {
			GUILayout.BeginArea (new Rect(Screen.width/2 - 250, Screen.height/2 - 250, 500,500));
			foreach(PhotonPlayer pl in PhotonNetwork.playerList){
				GUILayout.Box (pl.name);
			}
			GUILayout.EndArea ();
		}
	}

	private void CalculateAngle (Vector3 temp)
	{
		dir = new Vector3 (temp.x, transform.position.y, temp.z) - transform.position;
		angleToTarget = Vector3.Angle (dir, transform.forward);
	}

	private void LookAtThis ()
	{
		if (target != lastTarget) {
			CalculateAngle (target);
			if (angleToTarget > 3)
				transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (dir), rotationSpeed * UnityEngine.Time.deltaTime);
		}
	}

	private void MoveTo ()
	{
		if (target != lastTarget) {
			if ((transform.position - target).sqrMagnitude > heightPlayer + 0.1f) {
				if (!walk) {
					walk = true;
				}
				mag = (transform.position - target).magnitude;
				transform.position = Vector3.MoveTowards (transform.position, target, mag > stopStart ? speed * UnityEngine.Time.deltaTime : Mathf.Lerp (speed * 0.5f, speed, mag / stopStart) * UnityEngine.Time.deltaTime);
				//ray = new Ray (transform.position, -Vector3.up);
				//if (Physics.Raycast (ray, out hit, 1000.0f)) {
					//transform.position = new Vector3 (transform.position.x, hit.point.y + heightPlayer, transform.position.z);
				//}
			} else {
				lastTarget = target;
				if (walk) {
					walk = false;
				}
			}
		}
	}



}
