

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core {
    public class ObjectPath {

        private Vector3[] Points;
        int currentPosition;

        public ObjectPath(Vector3[] pathPoints) {
            currentPosition = 0;
            Points = pathPoints;
        }

        public void Reset() {
            currentPosition = 0;
        }

        public Vector3 First() {
            return Points[0];
        }

        public Vector3 Prev() {
            return Points[DecreasePosition()];
        }

        public Vector3 Next() {
            return Points[IncreasePosition()];
        }

        public Vector3 Current() {
            return Points[currentPosition];
        }

        public Vector3[] GetAll() {
            return Points;
        }

        private int DecreasePosition() {
            currentPosition = currentPosition > 0 ? currentPosition-- : 0;
            return currentPosition;
        }

        private int IncreasePosition() {
            if (currentPosition < Points.Length - 1)
                currentPosition++;
            return currentPosition;
        }

        public List<Vector3> GetPassed() {
            return Points.Take(currentPosition).ToList();
        }
    }
}
