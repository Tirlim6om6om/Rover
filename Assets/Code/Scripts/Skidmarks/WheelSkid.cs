using System;
using UnityEngine;


namespace ITPRO.Rover.Skid
{
	[RequireComponent(typeof(WheelCollider))]
	public class WheelSkid : MonoBehaviour {

		// INSPECTOR SETTINGS

		[SerializeField]
		private Rigidbody rb;

		// END INSPECTOR SETTINGS
		private Skidmarks _skidmarksController;
		private WheelCollider _wheelCollider;
		private WheelHit _wheelHitInfo;
		private int _lastSkid = -1; // Array index for the skidmarks controller. Index of last skidmark piece this wheel used
		private float _lastFixedUpdateTime;

		private void Awake()
		{
			TryGetComponent(out _wheelCollider);
			_lastFixedUpdateTime = Time.time;
		}

		private void Start()
		{
			_skidmarksController = Skidmarks.instance;
		}

		private void FixedUpdate() {
			_lastFixedUpdateTime = Time.time;
		}

		private void LateUpdate() {
			if (_wheelCollider.GetGroundHit(out _wheelHitInfo))
			{
				if (_wheelHitInfo.collider.CompareTag("Terrain"))
				{
					Vector3 skidPoint = _wheelHitInfo.point + (rb.velocity * (Time.time - _lastFixedUpdateTime));
					_lastSkid = _skidmarksController.AddSkidMark(skidPoint, _wheelHitInfo.normal, 1, _lastSkid);
				}
				else
				{
					_lastSkid = -1;
				}
			}
			else {
				_lastSkid = -1;
			}
		}
	}

}
