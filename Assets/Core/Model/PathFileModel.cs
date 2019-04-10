using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core.Model {
    [Serializable]
    public class PathFileModel {
        public float[] x;
        public float[] y;
        public float[] z;

        public Vector3[] ToArray() {
            List<Vector3> ret = new List<Vector3>();
            for (int i=0;i<x.Length;i++) {
                ret.Add(new Vector3(x[i], y[i], z[i]));
            }

            return ret.ToArray();
        }
    }
}
