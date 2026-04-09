using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flock_Logic
{
	public class Flock : MonoBehaviour
	{
		[SerializeField] private float speed = 2.1f;
		[SerializeField] private float rotationSpeed = 4.1f;

		Vector3 averageHeaging;
		Vector3 averagePosition;

		[SerializeField] private bool randSpeed = false;
		[SerializeField] private float maxSpeed = 5.1f;
		[SerializeField] private float minSpeed = 0.1f;

		[SerializeField] float neighbourDistance = 2.0f;
		[SerializeField] float avoidingDistance = 1.0f;

		bool turning = false;
		// Start is called before the first frame update
		void Start()
		{
			if (randSpeed)
			{
				speed = Random.Range(maxSpeed, minSpeed);
			}
		}

		// Update is called once per frame
		void Update()
		{
			if (transform.position.x >= GlobalFlock.tankSizeXS || transform.position.x <= -GlobalFlock.tankSizeXS || transform.position.y >= GlobalFlock.tankSizeYS || transform.position.y <= -GlobalFlock.tankSizeYS || transform.position.z >= GlobalFlock.tankSizeZS || transform.position.z <= -GlobalFlock.tankSizeZS)
			{
				turning = true;
			}
			else
			{
				turning = false;
			}

			if (turning)
			{
				Vector3 direction = Vector3.zero - transform.position;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

				speed = Random.Range(maxSpeed, minSpeed);
			}
			else
			{
				if (Random.Range(minSpeed, maxSpeed) < 1)
				{
					ApplyRules();
				}
			}

			transform.Translate(0, 0, speed * Time.deltaTime);
		}

		private void ApplyRules()
		{
			GameObject[] gos;
			gos = GlobalFlock.allFish;

			Vector3 vcentre = Vector3.zero;
			Vector3 vavoid = Vector3.zero;
			float gSpeed = .1f;

			Vector3 goalPos = GlobalFlock.goalPos;

			float dist;

			int groupSize = 0;
			foreach (GameObject go in gos)
			{
				if (go != this.gameObject)
				{
					dist = Vector3.Distance(go.transform.position, this.transform.position);
					if (dist <= neighbourDistance)
					{
						vcentre += go.transform.position;
						groupSize++;

						if (dist < avoidingDistance)
						{
							vavoid = vavoid + (this.transform.position - go.transform.position);
						}

						Flock anotherFlock = go.GetComponent<Flock>();
						gSpeed = gSpeed + anotherFlock.speed;
					}
				}
			}

			if (groupSize > 0)
			{
				vcentre = vcentre / groupSize + (goalPos - this.transform.position);
				speed = gSpeed / groupSize;

				Vector3 direction = (vcentre + vavoid) - transform.position;
				if (direction != Vector3.zero)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
				}
			}
		}
	}
}
