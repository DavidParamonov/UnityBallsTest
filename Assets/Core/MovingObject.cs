using System;
using UnityEngine;

namespace Core {
    public class MovingObject : MonoBehaviour {

        private GameObject Object;
        private ObjectPath Path;
        private float Speed = Consts.DEFAULT_SPEED;
        private Boolean isMoving = false;
        private LineRenderer lineRender;
        public void Init(GameObject source, ObjectPath path) {
            Object = source;
            Path = path;
            Object.transform.position = Path.First();            
            lineRender = Object.AddComponent<LineRenderer>();
        }

        public GameObject GetObject() {
            return Object;
        }

        public void ResetPathPostion() {
            isMoving = false;
            lineRender.positionCount = 0;
            Object.transform.position = Path.First();
            Speed = Consts.DEFAULT_SPEED;
            Path.Reset();
        }

        public void SetSpeed(float speed) {
            Speed = speed;
        }

        public float GetSpeed() {
            return Speed;
        }

        public void StartMoving() {
            isMoving = true;
        }

        public void StopMoving() {
            isMoving = false;
        }

        public Boolean IsMoving() {
            return isMoving;
        }

        public void Update() { 
            if (isMoving)
                CalcPosition();
            DrawPath();
        }

        private void DrawPath() {
            var points = Path.GetPassed();
            points.Add(Object.transform.position);
            lineRender.positionCount = points.Count;
            lineRender.SetPositions(points.ToArray());
        }

        private void CalcPosition() {
            float step = Speed * Time.deltaTime; 
            Vector3 target = Path.Current();
            Object.transform.position = Vector3.MoveTowards(Object.transform.position, target, step);
            if (Vector3.Distance(Object.transform.position, target) < 0.001f) {               
              
                Path.Next();
            }
            
        }
    }
}
