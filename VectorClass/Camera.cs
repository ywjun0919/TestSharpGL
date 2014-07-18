using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSharpGL.VectorClass
{
    class Camera
    {
        //观察坐标系原点
        Vector3D vrp = new Vector3D();
        //投影参考点
        Vector3D prp = new Vector3D();
        Vector3D n = new Vector3D();
        Vector3D u = new Vector3D();
        Vector3D v = new Vector3D();

        public Vector3D VRP
        {
            get { return vrp; }
            set { vrp = value; }
        }

        public Vector3D PRP
        {
            get { return prp; }
            set { prp = value; }
        }

        public Vector3D N 
        {
            get { return n; }
            set { n = value; }
        }

        public Vector3D U
        {
            get { return u; }
            set { u = value; }
        }

        public Vector3D V
        {
            get { return v; }
            set { v = value; }
        }

        //前裁剪面F

        //后裁剪面B
        
        //umin,umax,vmin,vmax
    }
}
