using UnityEngine;
using System.Collections;

public class WaterKillzone : MonoBehaviour {
	
	#region triggers
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Deadly") Destroy(other.gameObject);
	}
	#endregion
}
