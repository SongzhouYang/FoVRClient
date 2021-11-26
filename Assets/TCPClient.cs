using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class TCPClient : MonoBehaviour {
	internal Boolean socketReady = false;
	TcpClient mySocket;
	NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	String Host = "localhost";
	Int32 Port = 8888;

	//StreamWriter writer = new StreamWriter("data.txt", true);
	int timer = 0;

	void Start () {
		// setupSocket ();
	}

	void Update () {
		try {
			Vector3 ray_origin = FoveInterface.GetGazeConvergence ().ray.origin;
			Vector3 ray_direction = FoveInterface.GetGazeConvergence ().ray.direction;
			Vector3 position = FoveInterface.GetHMDPosition ();
			Quaternion rotation = FoveInterface.GetHMDRotation ();
			Vector3 resultV = rotation * (ray_origin + ray_direction + position);
			string result = ray_origin.ToString() + ray_direction.ToString() + position.ToString() + rotation.ToString() + '\n';
			// Debug.Log (result.ToString());
			timer += 1;
			if (timer == 12) {
				timer = 0;
				//Debug.Log (result);
				//writer.WriteLine(result);
				//writer.Close();
				File.AppendAllText("data.txt", result);
			}
			// readSocket ();
		} catch { }
	}
	// **********************************************
	public void setupSocket () {
		try {
			mySocket = new TcpClient (Host, Port);
			theStream = mySocket.GetStream ();
			theWriter = new StreamWriter (theStream);
			theReader = new StreamReader (theStream);
			socketReady = true;
		} catch (Exception e) {
			Debug.Log ("Socket error: " + e);
		}
	}

	public void writeSocket (string theLine) {
		if (!socketReady)
			return;
		String foo = theLine + "\r\n";
		theWriter.Write (foo);
		theWriter.Flush ();
	}

	public String readSocket () {
		if (!socketReady)
			return "";
		try {
			return theReader.ReadLine ();
		} catch (Exception e) {
			return "";
		}
	}

	public void closeSocket () {
		if (!socketReady)
			return;
		theWriter.Close ();
		theReader.Close ();
		mySocket.Close ();
		socketReady = false;
	}
}
// end class TCPClient