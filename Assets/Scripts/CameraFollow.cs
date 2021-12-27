using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Camera Settings")]
	public Transform playerTarget;
	public float Cameraspeed;
	public float minX, maxX;
	public float minY, maxY;

	private void FixedUpdate()
	{
		if (playerTarget != null)
		{
			Vector2 newCamPosition = Vector2.Lerp(transform.position, playerTarget.position, Time.deltaTime * Cameraspeed);
			float ClampX = Mathf.Clamp(newCamPosition.x, minX, maxX);
			float ClampY = Mathf.Clamp(newCamPosition.y, minY, maxY);
			transform.position = new Vector3(ClampX, ClampY, -10f);
		}

	}
}