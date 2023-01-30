using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public int maxPlayers;

	private const string typeName = "20141216_CGL_CP_WT2014_Polyguys_v01";			// unique(!) game description
	private const string gameName = "Test Server #";									// room name

	//private HostData[] hostList;													// list of open game servers

	public GameObject playerPrefab;													// the player object
	public Transform spawnPointP1;													// the spawning points
	public Transform spawnPointP2;
	private Transform[] spawnPoints;

	// Use this for initialization
	void Start () {
		// create the server on the client machine (localhost)
		//MasterServer.ipAddress = "127.0.0.1";

		spawnPoints = new Transform[] {spawnPointP1, spawnPointP2};
		//RefreshHostList();
	}
	
	// Update is called once per frame
	void Update () {
		//PrintNames();
	}

	/*void PrintNames(ArrayList viewIDs) {
		foreach (NetworkViewID ID in viewIDs) {
			Debug.Log("Finding " + ID);
			NetworkView view = NetworkView.Find(ID);
			Debug.Log(view.observed.name);
		}
	}*/
	
	/*
	private void StartServer(int maxPlayers){
		// initialize a game server on port 25000 with a maximum of 2 players and register it with the unity master server
		Network.InitializeServer(maxPlayers, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName + hostList.Length);
	}*/

		/*
		void OnServerInitialized(){
			Debug.Log("Server initialized!");
			SpawnPlayer(1);
		}*/

	/*
	private void RefreshHostList(){
		MasterServer.RequestHostList(typeName);
	}

		void OnMasterServerEvent(MasterServerEvent msEvent){
			if (msEvent == MasterServerEvent.HostListReceived){
				hostList = MasterServer.PollHostList();
			}
		}

	private void JoinServer(HostData hostData){
		Network.Connect(hostData);
	}

		void OnConnectedToServer(){
			Debug.Log("Server joined!");
			SpawnPlayer(2);
		}

	private void SpawnPlayer(int player){
		Network.Instantiate(playerPrefab, spawnPoints[player-1].position, Quaternion.identity, 0);
	}

		void OnPlayerDisconnected(NetworkPlayer player){
			Debug.Log("Cleaning up after the player. That dirty little shit!");
			Network.RemoveRPCs(player);
			Network.DestroyPlayerObjects(player);
		}*/


	/*
	// TODO: implement NEW UI!
	void OnGUI(){
		if (!Network.isClient && !Network.isServer){
			if (GUI.Button(new Rect(100,100,250,100), "Start Server")){
				StartServer (maxPlayers);
			}

			if (GUI.Button(new Rect(100,250,250,100), "Refresh Hostlist")){
				RefreshHostList ();
			}

			if (hostList != null){
				for (int i = 0; i < hostList.Length; i++){
					GUILayout.BeginHorizontal();    
					string name = hostList[i].gameName + " " + hostList[i].connectedPlayers + " / " + hostList[i].playerLimit;
					GUILayout.Label(name);
					GUILayout.Space (5);

					string hostInfo = "[";
					for (int h = 0; h < hostList[i].ip.Length; h++){
						hostInfo = hostInfo + h + ":" + hostList[i].port + " ";
					}
					hostInfo = hostInfo + "]";
					GUILayout.Label(hostInfo);
					GUILayout.Space (5);
					GUILayout.Label(hostList[i].comment);
					GUILayout.Space (5);
					GUILayout.FlexibleSpace();

					if (GUILayout.Button("Connect")){
						JoinServer(hostList[i]);
					}
				}

			}

		} else {
			GUILayout.BeginHorizontal();
			if (Network.isServer) GUILayout.Label("You are the server.");
			else if (Network.isClient) GUILayout.Label("You are the client.");
		}
	}*/
}
