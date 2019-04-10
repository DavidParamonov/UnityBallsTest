using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using Core;
using System.Linq;
using Assets.Core.Model;

public class GameCore : MonoBehaviour {

    private List<MovingObject> movingObjects = new List<MovingObject>();
    private Camera mainCamera;
    private MovingObject currentObject = null;
    private int currentIndex = 0;
    private Vector3 cameraOffset;
    private GameObject ballPrefab;
    private Transform previosClicked = null;
    private float previosClickTime = 0;

    public void Start() {
        mainCamera = Camera.main;
        ballPrefab = (GameObject)Resources.Load("Ball");
        LoadPaths();
        if (movingObjects.Any())
            SwitchCameraToCurrentObject();
    }

    public MovingObjectDTO GetModel() {
        MovingObjectDTO ret = new MovingObjectDTO();
        if (currentObject != null) {
            ret.Speed = currentObject.GetSpeed();
            ret.Name = currentObject.GetObject().name;
            ret.MaxSpeed = Consts.MAX_SPEED;
        }
        return ret;
    }

    public void LateUpdate() {
        FindClickedObject();
        CalcCameraPosition();
        CalcCameraRotation();
    }

    public void SwitchToPreviosView() {
        if (currentIndex < movingObjects.Count - 1) {
            currentObject.StopMoving();
            currentIndex++;
            SwitchCameraToCurrentObject();
        }
    }

    public void SwitchToNextView() {
        if (currentIndex > 0) {
            currentObject.StopMoving();
            currentIndex--;
            SwitchCameraToCurrentObject();
        }
    }

    public void SetSpeed(float value) {
        if (currentObject != null)
            currentObject.SetSpeed(value * Consts.MAX_SPEED);
    }

    private void FindClickedObject() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                foreach (var movingObject in movingObjects) {
                    if (ObjectsAreSame(objectHit, movingObject)) {

                        Boolean isDoubleClick = ObjectsAreSame(previosClicked, movingObject) && (Time.time - previosClickTime) < Consts.DOUBLE_CLICK_INTERVAL;
                        previosClickTime = Time.time;
                        previosClicked = objectHit;
                        if (isDoubleClick) {
                            movingObject.ResetPathPostion();
                        } else {
                            if (movingObject.IsMoving())
                                movingObject.StopMoving();
                            else
                                movingObject.StartMoving();
                        }
                    }
                }
            }
        }
    }

    private bool ObjectsAreSame(Transform objectHit, MovingObject movingObject) {
        return objectHit != null && objectHit.name == movingObject.GetObject().name;
    }

    private void CalcCameraPosition() {
        if (currentObject != null) {
            var objectPosition = currentObject.GetObject().transform.position;
            mainCamera.transform.position = objectPosition + cameraOffset;
        }
    }

    private void CalcCameraRotation() {
        if (currentObject != null) {
            if (Input.GetMouseButton(1)) {
                var x = Input.GetAxis("Mouse X");
                var y = Input.GetAxis("Mouse Y");

                mainCamera.transform.LookAt(currentObject.GetObject().transform.position);
                mainCamera.transform.RotateAround(currentObject.GetObject().transform.position, Vector3.up, x * Consts.MOUSE_SENTESIVITY);
                mainCamera.transform.RotateAround(currentObject.GetObject().transform.position, Vector3.left, y * Consts.MOUSE_SENTESIVITY);
                cameraOffset = mainCamera.transform.position - currentObject.GetObject().transform.position;
            }
        }
    }

    private void SwitchCameraToCurrentObject() {
        currentObject = movingObjects[currentIndex];
        var pos = currentObject.GetObject().transform.position;

        mainCamera.transform.position = new Vector3(pos.x - Consts.CAMERA_DISTANCE_TO_OBJECT, pos.y, pos.z);
        mainCamera.transform.LookAt(currentObject.GetObject().transform);
        cameraOffset = mainCamera.transform.position - pos;
    }

    private void LoadPaths() {
        var textAssets = Resources.LoadAll<TextAsset>("PathData");
       // var files = Directory.EnumerateFiles(Path.Combine("Assets", "Resources", "PathData"), "*_path*.json");
        if (textAssets == null || !textAssets.Any())
            return;

        int i = 0;
        foreach (var textAsset in textAssets) {
            String json = textAsset.text;
            PathFileModel pathModel = JsonUtility.FromJson<PathFileModel>(json);

            var points = pathModel.ToArray();
            var path = new Core.ObjectPath(points.ToArray());
            var ball = Instantiate(ballPrefab);
            ball.name = String.Format("Ball {0}", i);

            var movingObject = gameObject.AddComponent<MovingObject>();
            movingObject.Init(ball, path);
            movingObjects.Add(movingObject);
            i++;
        }
    }
}