using UnityEngine;
using System.Collections;

public class respawnParticleEffect : MonoBehaviour {
	
	private Vector3[] triangle 	= new Vector3[3]{new Vector3(0,0.57f,0), new Vector3(.5f,-.287f,0), new Vector3(-.5f,-.287f,0)};
	private Vector3[] square 	= new Vector3[4]{new Vector3(-.5f,.5f,0), new Vector3(.5f,.5f,0), new Vector3(.5f,-.5f,0), new Vector3(-.5f,-.5f,0)};
	private Vector3[] pentagon	= new Vector3[5]{new Vector3(0,.87f,0), new Vector3(.835f,.27f,0), new Vector3(.52f,-.71f,0), new Vector3(-.52f,-.71f,0), new Vector3(-.835f,.27f,0)};
	private Vector3[] hexagon 	= new Vector3[6]{new Vector3(.63f, .9f, 0), new Vector3(1.03f,0,0), new Vector3(.63f, -.9f, 0), new Vector3(-.63f, .9f, 0), new Vector3(-1.03f,0,0), new Vector3(-.63f, .9f, 0)};
	private Vector3[] heptagon 	= new Vector3[7]{new Vector3(0,1.2f,0), new Vector3(.94f,.75f,0), new Vector3(1.175f,-.27f,0), new Vector3(.5275f,-1.064f,0), new Vector3(-.5275f,-1.064f,0), new Vector3(-1.175f,-.27f,0), new Vector3(-.94f,.75f,0)};
	private Vector3[] octagon 	= new Vector3[8]{new Vector3(.53f, 1.265f,0), new Vector3(1.265f, .53f, 0), new Vector3(1.265f, -.53f, 0), new Vector3(.53f, -1.265f,0), new Vector3(-.53f, -1.265f,0), new Vector3(-1.265f, -.53f, 0), new Vector3(-1.265f, .53f, 0), new Vector3(-.53f, 1.265f,0)};

	[SerializeField]
	private ParticleSystem projectileParticleEffect;

	[SerializeField]
	private int rotationCycles;

	// Use this for initialization
	void Start () {
			}
	
	// Update is called once per frame
	void Update () {

	}

	public void Init(int vertices, Color color){
		Vector3[] corners = new Vector3[0];
		switch (vertices){
		case 3:
			corners = triangle;
			break;
		case 4:
			corners = square;
			break;
		case 5:
			corners = pentagon;
			break;
		case 6:
			corners = hexagon;
			break;
		case 7:
			corners = heptagon;
			break;
		case 8:
			corners = octagon;
			break;
		default:
			break;
		}
		for (int i = 0; i < vertices; i++){
			ParticleSystem tmp = Instantiate(projectileParticleEffect,this.transform.position+corners[i], Quaternion.identity) as ParticleSystem;
			tmp.startColor = color;
			tmp.startSize *= vertices*vertices;
			tmp.transform.parent = this.transform;
		}
	}

	public float GetStartLifetime(){
		return projectileParticleEffect.startLifetime;
	}

	public int GetRotationCycles(){
		return rotationCycles;
	}

	public void Scale(float scaleFactor){
		this.transform.localScale = Vector3.one * scaleFactor;
	}

	public void Rotate(float rotationProgress){
		this.transform.Rotate(Vector3.forward * rotationProgress * Mathf.PI * rotationCycles);
	}
}
