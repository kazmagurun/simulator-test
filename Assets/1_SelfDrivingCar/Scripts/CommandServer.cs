using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SocketIO;
using UnityStandardAssets.Vehicles.Car;
using System;
using System.Security.AccessControl;

public class CommandServer : MonoBehaviour
{
	public CarRemoteControl CarRemoteControl;
	public Camera FrontFacingCamera;
	private SocketIOComponent _socket;
	private CarController _carController;
	private bool _isConnected = false;

	// Use this for initialization
	void Start()
	{
		Debug.Log("Starting");
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
		_socket.On("open", OnOpen);
		_socket.On("steer", OnSteer);
		_socket.On("manual", onManual);
		_socket.On("telemetry", OnTelemetry);
		_socket.On("close", OnClose);
		_carController = CarRemoteControl.GetComponent<CarController>();
	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log("Update " + Time.deltaTime);
		if (_isConnected) EmitTelemetry(obj);
	}

	void OnOpen(SocketIOEvent obj)
	{
		// filter out spurious open events, which seem to happen a lot due to reconnect bugs.
		if (_isConnected) return;
  	_isConnected = true;
		Debug.Log("Connection Open");
	}

	void OnClose(SocketIOEvent obj) {
		_isConnected = false;
		Debug.Log("Connection Closed");
	}

	//
	void onManual(SocketIOEvent obj)
	{
		Debug.Log("onManual");
	}

	void OnSteer(SocketIOEvent obj)
	{
		Debug.Log("OnSteer");
		JSONObject jsonObject = obj.data;
		//    print(float.Parse(jsonObject.GetField("steering_angle").str));
		CarRemoteControl.SteeringAngle = float.Parse(jsonObject.GetField("steering_angle").str);
		CarRemoteControl.Acceleration = float.Parse(jsonObject.GetField("throttle").str);
	}

	void OnTelemetry(SocketIOEvent obj) {
		Debug.Log("OnTelemetry");
		EmitTelemetry(obj);
	}

	void EmitTelemetry(SocketIOEvent obj)
	{
		UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
			print("Attempting to Send...");
			//		// send only if it's not being manually driven
  		//		if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S))) {
	  	//			_socket.Emit("telemetry", new JSONObject());
		  //		}
  		// Collect Data from the Car
	  	Dictionary<string, string> data = new Dictionary<string, string>();
		  data["steering_angle"] = _carController.CurrentSteerAngle.ToString("N4");
  		data["throttle"] = _carController.AccelInput.ToString("N4");
 		  data["speed"] = _carController.CurrentSpeed.ToString("N4");
// 		  data["image"] = Convert.ToBase64String(CameraHelper.CaptureFrame(FrontFacingCamera));
		_ socket.Emit("telemetry", new JSONObject(data));
		});
	}
}
