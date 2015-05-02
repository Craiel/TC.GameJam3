using UnityEngine;
using System.Collections;
using Assets;

public class Tourette : MonoBehaviour {
	public  Assets.Scripts.Enemy.Color color;

	public int baseDamage;
	public int fireRange;
	public int aggroRange;
	public int rateOfFire;
	public int rotationSpeedCoefficient;

	public Transform target;

	private Transform aimBone;
	private Transform fireRotationBone;

	// Use this for initialization
	void Start () {
		// Sequence to Bone.012 - 0, 3
		aimBone = transform.GetChild (0).GetChild (3);
		fireRotationBone = aimBone.GetChild (0);

		System.Diagnostics.Trace.Assert (fireRange < aggroRange);
	}

	private Transform aggro() {
		if ((aimBone.transform.position - target.position).magnitude < aggroRange)
			return target;
		else
			return null;
	}

	private void fire() {
		// ??? Particles, summon bullets?
	}

	private void spinTheBarrel() {
		fireRotationBone.Rotate (0, fireRotationBone.rotation.y + rateOfFire, 0);
	}

	private void aimAt(Transform target) {
		Vector3 dir = target.position - transform.position;
		dir.Normalize ();
		//aimBone.RotateAround (aimBone.transform.position, new Vector3 (0, 1, 0), 10.0f * Vector3.Dot (new Vector3(0,0,1), dir));
		Debug.Log ("My position: " + aimBone.position + ", my target's position: " + target.position + ", direction I want to look at: " + dir.normalized);
//		aimBone.rotation.eulerAngles - 
		// FIGURE THIS SHIT OUT!
		aimBone.RotateAround (aimBone.position, dir, 1.0f);
	}

	private bool inFireRange(Transform target) {
		return false;
	}

	private bool aimingAt(Transform target) {
		return false;
	}

	// Update is called once per frame
	void Update () {
		Transform aggroTarget = aggro ();

		if (aggroTarget != null) {
			aimAt(aggroTarget);
			spinTheBarrel();
			if(inFireRange(aggroTarget) && aimingAt(aggroTarget))
				fire();
		}
	}
}
