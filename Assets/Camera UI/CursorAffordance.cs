﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

    // TODO solve fight between serialize and const
    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    CameraRaycaster cameraRaycaster;


	// Use this for initialization
	void Start () {
		cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged;  // registering this method to the group of methods

    }
	

	void OnLayerChanged (int newLayer) {
		// Debug.Log(cameraRaycaster.layerHit);
        switch (newLayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                return;
        }
	}

}