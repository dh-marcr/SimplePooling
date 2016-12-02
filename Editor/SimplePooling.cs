using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class SimplePooling : EditorWindow
{
	[MenuItem ("Window/SimplePooling")]
	public static void ShowWindow ()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow bar = EditorWindow.GetWindow (typeof(SimplePooling));

		#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		bar.title = "SimplePooling";
		#else
		bar.titleContent = new GUIContent ("SimplePooling");
		#endif
		bar.minSize = new Vector2 (200, 500);
		bar.maxSize = new Vector2 (350, 2000);
	}

	void OnInspectorUpdate ()
	{
		Repaint ();
	}

	void OnGUI ()
	{

		Texture settingsIcon = Resources.Load ("SettingsGear_Icon") as Texture;
		Vector2 settingButtonSize = new Vector2 (25, 19);

		GUILayout.BeginHorizontal ();

		if (GUILayout.Button (settingsIcon, GUILayout.Width (settingButtonSize.x), GUILayout.Height (settingButtonSize.y))) {
			showSettings = !showSettings;
			if (showSettings) {
				lastToolbar = toolbar;
				toolbar = poolToolbarCount + 1;
			} else {
				toolbar = lastToolbar;
			}
			settingsToolbar = 3;
		}

		string[] menuOptions = new string[poolToolbarCount];

		for (int x = 0; x < poolToolbarCount; x++) {

			menuOptions [x] = toolbarNames [x];
		}

		toolbar = GUILayout.Toolbar (toolbar, menuOptions);

		GUILayout.EndHorizontal ();

		if (showSettings) {
			GUILayout.BeginVertical ();

			GUILayout.Space (10);

			string[] toolProps = new string[]{ "Options", "Prefabs" };

			toolPropertiesBar = GUILayout.Toolbar (toolPropertiesBar, toolProps);

			switch (toolPropertiesBar) {
			case 0:

				GUILayout.Space (5);
				GUILayout.BeginVertical ("Box");
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();

				GUILayout.Label ("Toolbar Options Count");

				GUI.backgroundColor = Color.green;

				if (GUILayout.Button ("+", GUILayout.Width (20), GUILayout.Height (18))) {
					toolbarNames.Add ("");
					poolToolbarCount++;
				}

				GUI.backgroundColor = Color.red;

				if (GUILayout.Button ("-", GUILayout.Width (20), GUILayout.Height (18)) && poolToolbarCount > 0) {
					toolbarNames.RemoveAt (toolbarNames.Count - 1);
					poolToolbarCount--;
				}

				GUI.backgroundColor = Color.white;

				poolToolbarCount = EditorGUILayout.IntField (poolToolbarCount, GUILayout.Width (40));

				GUILayout.EndHorizontal ();

				GUILayout.Space (10);

				if (toolbarNames.Count == poolToolbarCount) {
					for (int i = 0; i < poolToolbarCount; i++) {
						GUILayout.BeginHorizontal ();

						GUILayout.Label ("Option " + (i + 1));
						toolbarNames [i] = EditorGUILayout.TextField (toolbarNames [i], GUILayout.Width (120));

						GUILayout.EndHorizontal ();
					}
				}

				GUILayout.EndVertical ();

				break;

			case 1:

				GUILayout.BeginHorizontal ("Box");
				GUILayout.Label ("Chunks Folder");
				if (GUILayout.Button (Resources.Load ("folder_icon") as Texture, GUILayout.Width (27), GUILayout.Height (20))) {
					chooseChunkPath = EditorUtility.OpenFolderPanel ("Chunks Folder", "", "");
					chooseChunkPath = properPath (chooseChunkPath);
				}
				chooseChunkPath = EditorGUILayout.TextField (chooseChunkPath);
				GUILayout.EndHorizontal ();

				GUILayout.BeginHorizontal ("Box");
				GUILayout.Label ("Spawnable Objects Folder");
				if (GUILayout.Button (Resources.Load ("folder_icon") as Texture, GUILayout.Width (27), GUILayout.Height (20))) {
					chooseSpawnableObjectPath = EditorUtility.OpenFolderPanel ("Spawnable Objects Folder", "", "");
					chooseSpawnableObjectPath = properPath (chooseSpawnableObjectPath);
				}
				chooseSpawnableObjectPath = EditorGUILayout.TextField (chooseSpawnableObjectPath);
				GUILayout.EndHorizontal ();

				prefabToolbar = GUILayout.Toolbar (prefabToolbar, new string[]{ "Chunks", "Spawnable Objects" });

				List<GameObject> folderObjects = new List<GameObject> ();

				switch (prefabToolbar) {

				case 0:
					DirectoryInfo chunkDir = new DirectoryInfo (chooseChunkPath);
					FileInfo[] chunkInfo = chunkDir.GetFiles ("*.*");

					folderObjects = getFolderObjects (chooseChunkPath, ".prefab", chunkInfo);

					for (int i = 0; i < folderObjects.Count; i++) {

						GUILayout.BeginHorizontal ();
						GameObject newChunk = folderObjects [i];
						folderObjects [i] = (GameObject)EditorGUILayout.ObjectField (newChunk, typeof(GameObject), false);

						GUILayout.EndHorizontal ();
					}

					if (GUILayout.Button ("Open Chunk Creator")) {

						CreateChunk chunkCreator = (CreateChunk)EditorWindow.GetWindow (typeof(CreateChunk));
						#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
						window.title = "Chunk Creator";
						#else
						chunkCreator.titleContent = new GUIContent ("Chunk Creator");
						#endif
						chunkCreator.minSize = new Vector2 (350, 185);
						chunkCreator.maxSize = new Vector2 (350, 185);
						chunkCreator.Show ();
					}
					break;

				case 1:
					DirectoryInfo spawnableDir = new DirectoryInfo (chooseSpawnableObjectPath);
					FileInfo[] spawnableInfo = spawnableDir.GetFiles ("*.*");

					folderObjects = getFolderObjects (chooseSpawnableObjectPath, ".prefab", spawnableInfo);

					for (int i = 0; i < folderObjects.Count; i++) {

						GUILayout.BeginHorizontal ();
						GameObject newSpawnable = folderObjects [i];
						folderObjects [i] = (GameObject)EditorGUILayout.ObjectField (newSpawnable, typeof(GameObject), false);

						GUILayout.EndHorizontal ();
					}
					break;
				}
				break;
			}

			GUILayout.EndVertical ();
		}
			
		switch (toolbar) {

		case 0:
			showSettings = false;
			GUILayout.Label ("This is stuff for tab1");
			break;

		case 1:
			showSettings = false;
			GUILayout.Label ("This is stuff for tab2");
			break;

		case 2:
			showSettings = false;
			GUILayout.Label ("This is stuff for tab3");
			break;

		case 3:
			showSettings = false;
			GUILayout.Label ("This is stuff for tab4");
			break;

		case 4:
			showSettings = false;
			GUILayout.Label ("This is stuff for tab5");
			break;
		}
	}

	List<GameObject> getFolderObjects(string in_path, string in_extension, FileInfo[] in_fileInfo){

		List<GameObject> folderObjects = new List<GameObject> ();

		foreach (FileInfo f in in_fileInfo) {
			if (f.Extension == in_extension) {
				string objectPath = in_path + f.Name;
				folderObjects.Add((GameObject)AssetDatabase.LoadAssetAtPath(objectPath, typeof(GameObject)));
			}
		}
		return folderObjects;
	}

	string properPath(string in_path){

		string newPath = in_path;

		string[] pathParts = in_path.Split ('/');

		newPath = "";
		bool startAdding = false;

		for (int i = 0; i < pathParts.Length; i++) {
			if (pathParts[i] == "Assets") {
				startAdding = true;
			}

			if (startAdding) {
				newPath += pathParts[i] + "/";
			}
		}

		return newPath;
	}

	//main toolbar
	int toolbar;
	int lastToolbar;

	//tool properties
	int toolPropertiesBar;

	//settings section
	bool showSettings;
	int settingsToolbar;
	List<string> toolbarNames = new List<string> ();
	int poolToolbarCount;

	string chooseChunkPath;

	string chooseSpawnableObjectPath;

	int prefabToolbar;
}

public class CreateChunk : EditorWindow{

	void OnGUI(){

		GUILayout.BeginHorizontal ();
		GUILayout.EndHorizontal ();

		GUILayout.Space (5);
		GUILayout.BeginVertical ("Box");

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Chunk Name");
		chunkName = EditorGUILayout.TextField (ChunkUtilities.chunkName);
		ChunkUtilities.chunkName = chunkName;

		if (ChunkUtilities.useID) {
			GUILayout.Label ("ID");
			chunkID = EditorGUILayout.IntField (ChunkUtilities.chunkID);
			ChunkUtilities.chunkID = chunkID;
		} else {
			GUILayout.Label ("Use ID");
			useID = EditorGUILayout.Toggle (ChunkUtilities.useID);
			ChunkUtilities.useID = useID;
		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Chunk Mesh");
		chunkMesh = (GameObject)EditorGUILayout.ObjectField (ChunkUtilities.chunkMesh, typeof(GameObject), false);
		ChunkUtilities.chunkMesh = chunkMesh;
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Chunk Collider Mesh");
		colliderMesh = (GameObject)EditorGUILayout.ObjectField (ChunkUtilities.chunkColliderMesh, typeof(GameObject), false);
		ChunkUtilities.chunkColliderMesh = colliderMesh;
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Chunk Size Mesh");
		sizeMesh = (Mesh)EditorGUILayout.ObjectField (ChunkUtilities.chunkSizeMesh, typeof(Mesh), false);
		ChunkUtilities.chunkSizeMesh = sizeMesh;
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Spawn Point");
		GUILayout.Space (231);
		hasSpawnPoint = EditorGUILayout.Toggle (ChunkUtilities.hasSpawnPoint);
		ChunkUtilities.hasSpawnPoint = hasSpawnPoint;
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Trigger");
		GUILayout.Space (255);
		hasTrigger = EditorGUILayout.Toggle (ChunkUtilities.hasTrigger);
		ChunkUtilities.hasTrigger = hasTrigger;
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Environment Spawn Points");
		enviroSpawnPoints = (Transform)EditorGUILayout.ObjectField (ChunkUtilities.enviroSpawnPoints, typeof(Transform), false);
		ChunkUtilities.enviroSpawnPoints = enviroSpawnPoints;
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Preview Chunk");
		previewChunk = EditorGUILayout.Toggle (ChunkUtilities.previewChunk);
		ChunkUtilities.previewChunk = previewChunk;
		GUILayout.EndHorizontal ();

		GUILayout.EndVertical ();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button (Resources.Load("Refresh_Icon") as Texture, GUILayout.Width(19), GUILayout.Height(18))) {

			ChunkUtilities.chunkName = "";
			ChunkUtilities.useID = false;
			ChunkUtilities.chunkID = 0;
			ChunkUtilities.chunkMesh = null;
			ChunkUtilities.chunkColliderMesh = null;
			ChunkUtilities.chunkSizeMesh = null;
			ChunkUtilities.hasSpawnPoint = false;
			ChunkUtilities.hasTrigger = false;
			ChunkUtilities.enviroSpawnPoints = null;
			ChunkUtilities.previewChunk = false;
		}

		if (GUILayout.Button ("Create New Chunk")) {

			ChunkUtilities.create ();
		}
		GUILayout.EndHorizontal ();
	}

	public string chunkName;
	public bool useID;
	public int chunkID;

	public GameObject chunkMesh;
	public GameObject colliderMesh;
	public Mesh sizeMesh;

	public bool hasSpawnPoint;
	public bool hasTrigger;

	public Transform enviroSpawnPoints;

	public bool previewChunk;
}
