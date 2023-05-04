using UnityEngine;
using System.Collections;

/// <summary>
/// Creates wandering behaviour for a CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Wander : MonoBehaviour
{
	public float speed = 1;
	public float directionChangeInterval = 1;
	public float maxHeadingChange = 30;
	public GameObject footprint;

	CharacterController controller;
	float heading;
	Vector3 targetRotation;

  	void Start()
    {
        // start a repeating timer that triggers every 2 seconds
        InvokeRepeating("SpawnFootprint", 2.0f, 2.0f);

    }

	void Awake ()
	{
		controller = GetComponent<CharacterController>();

		// Set random initial rotation
		heading = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0, heading, 0);

		StartCoroutine(NewHeading());
	}

	void Update ()
	{
		transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
		var forward = transform.TransformDirection(Vector3.forward);
		controller.SimpleMove(forward * speed);
	}

	 void SpawnFootprint()
    {
        // get the current position of the deer NPC
        Vector3 deerPos = transform.position;

        // instantiate a new footprint game object at the deer's position
		Quaternion footprintRot = Quaternion.Euler(0.0f, 270.0f, 0.0f);

        GameObject newFootprint = Instantiate(footprint, deerPos + new Vector3(0.0f, -1.0f, 0.0f), footprintRot);

        // adjust the footprint's position so it appears slightly above the ground
    }

	/// <summary>
	/// Repeatedly calculates a new direction to move towards.
	/// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
	/// </summary>
	IEnumerator NewHeading ()
	{
		while (true) {
			NewHeadingRoutine();
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}

	/// <summary>
	/// Calculates a new direction to move towards.
	/// </summary>
	void NewHeadingRoutine ()
	{
		var floor = transform.eulerAngles.y - maxHeadingChange;
		var ceil  = transform.eulerAngles.y + maxHeadingChange;
		heading = Random.Range(floor, ceil);
		targetRotation = new Vector3(0, heading, 0);
	}
}