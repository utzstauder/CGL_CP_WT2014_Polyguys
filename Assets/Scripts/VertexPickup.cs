using UnityEngine;
using System.Collections;

public class VertexPickup : MonoBehaviour {

	[SerializeField]
	[Range (3, 8)]
	private int vertex;

	public Transform mesh3;
	public Transform mesh4;
	public Transform mesh5;
	public Transform mesh6;
	public Transform mesh7;
	public Transform mesh8;

	private Transform[] meshes;

	[SerializeField]
	private ParticleSystem particlePrefab;
	private ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		meshes = new Transform[6]{mesh3, mesh4, mesh5, mesh6, mesh7, mesh8};
		DeactivateAllMeshes();
		ActivateMesh(vertex);
		particleSystem = Instantiate(particlePrefab, this.transform.position, this.transform.rotation) as ParticleSystem;
		particleSystem.startSize *= vertex;
		particleSystem.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void ActivateMesh(int vertices){
		if (vertices >= 3 && vertices <= 8)
			meshes[vertices-3].gameObject.SetActive(true);
	}
	
	private void DeactivateAllMeshes(){
		for (int i = 0; i < meshes.Length; i++){
			meshes[i].gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlatformerCharacter2D>().currentVertices < 8){
			other.GetComponent<PlatformerCharacter2D>().ChangeShape(other.GetComponent<PlatformerCharacter2D>().currentVertices+1);
			particleSystem.GetComponent<autoDestroy>().triggerDestroy(particleSystem.startLifetime);
			Destroy (this.gameObject);
		}
	}
}
