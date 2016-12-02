using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[ExecuteInEditMode]
public class ChunkUtilities : MonoBehaviour{

	static public void create(){

		//setting up new chunk object
		string newChunkName = chunkName + (useID ? "_" + chunkID.ToString() : "");
		GameObject newChunk = new GameObject (newChunkName);

		GameObject geometry = new GameObject ("Geometry");
		geometry.transform.parent = newChunk.transform;
		geometry.transform.localPosition = Vector3.zero;

		GameObject meshGEO = new GameObject ("Mesh");
		meshGEO.transform.parent = geometry.transform;
		meshGEO.transform.localPosition = Vector3.zero;

		GameObject colliderGEO = new GameObject ("Collider");
		colliderGEO.transform.parent = geometry.transform;
		colliderGEO.transform.localPosition = Vector3.zero;

		GameObject function = new GameObject ("Function");
		function.transform.parent = newChunk.transform;
		function.transform.localPosition = Vector3.zero;

		GameObject envronment = new GameObject ("Envrionment");
		envronment.transform.parent = newChunk.transform;
		envronment.transform.localPosition = Vector3.zero;

		//spawn geometry
		GameObject newMeshGEO = (GameObject)Instantiate(chunkMesh, Vector3.zero, Quaternion.identity) as GameObject;
		newMeshGEO.transform.parent = meshGEO.transform;
		newMeshGEO.transform.localPosition = Vector3.zero;

		GameObject newColliderGEO = (GameObject)Instantiate (chunkColliderMesh, Vector3.zero, Quaternion.identity) as GameObject;
		newColliderGEO.transform.parent = colliderGEO.transform;
		newColliderGEO.transform.localPosition = Vector3.zero;

		Vector3 chunkBounds = chunkSizeMesh.bounds.size;

		//create a spawn point
		if (hasSpawnPoint) {

			GameObject spawnPoint = new GameObject ("Spawn Socket");
			spawnPoint.transform.parent = function.transform;

			Vector3 socket = spawnPoint.transform.localPosition;
			socket = new Vector3 (0, 0, chunkSizeMesh.bounds.size.z - 5);
			spawnPoint.transform.localPosition = socket;

			//assign ref to chunk object
		}

		//create a trigger
		if (hasTrigger) {

			GameObject trigger = new GameObject ("Trigger");
			trigger.transform.parent = function.transform;

			trigger.AddComponent<BoxCollider> ();
			BoxCollider col = trigger.GetComponent<BoxCollider> ();
			col.isTrigger = true;
			trigger.AddComponent<Rigidbody> ();
			Rigidbody rb = trigger.GetComponent<Rigidbody> ();
			rb.useGravity = false;

			Vector3 triggerSize = trigger.transform.localScale;
			triggerSize = new Vector3 (chunkBounds.x + 5, 30, 5);
			trigger.transform.localScale = triggerSize;

			Vector3 triggerPosition = trigger.transform.localPosition;
			triggerPosition.y = triggerSize.y / 2;
			triggerPosition.z = (chunkBounds.z / 2) - 5;
			trigger.transform.localPosition = triggerPosition;
		}

		if (enviroSpawnPoints != null) {
			//assign all possible spawn points
		}

		//create new prefab here 
		createNewPrefab("Assets/NewPoolingTool/Chunks/" + newChunkName + ".prefab", newChunk);

		//delete prefab(scene) if value is false
		if (!previewChunk) {
			DestroyImmediate (newChunk);
		}
	}

	static void createNewPrefab(string in_path, GameObject in_go){

		//check if prefab exsists in folders
		//replace if it exsists
		//EditorUtility.DisplayDialog("Are you sure?", "Replace current prefab?", "Yes", "No");

		PrefabUtility.CreatePrefab (in_path, in_go);
	}

	static public string chunkName;
	static public bool useID;
	static public int chunkID;

	static public GameObject chunkMesh;
	static public GameObject chunkColliderMesh;
	static public Mesh chunkSizeMesh;

	static public bool hasSpawnPoint;
	static public bool hasTrigger;

	static public Transform enviroSpawnPoints;

	static public bool previewChunk;

}