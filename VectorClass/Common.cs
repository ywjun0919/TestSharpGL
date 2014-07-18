using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TestSharpGL.VectorClass
{
    class Common
    {
        enum ProjectMode{
            Perspective,
            Parallel
        };

        public static void swap(ref float a, ref float b)
        {
            a = a + b;
            b = a - b;
            a = a - b;
        }

        public static Vector3D WorldTransform(Vector3D v, Camera camera)
        {

            Matrix4D M_WC_VRC = new Matrix4D(
                camera.U.X,camera.U.Y,camera.U.Z,-(camera.U.X * camera.VRP.X + camera.U.Y * camera.VRP.Y + camera.U.Z * camera.VRP.Z),
                camera.V.X,camera.V.Y,camera.V.Z,-(camera.V.X * camera.VRP.X + camera.V.Y * camera.VRP.Y + camera.V.Z * camera.VRP.Z),
                camera.N.X,camera.N.Y,camera.N.Z,-(camera.N.X * camera.VRP.X + camera.N.Y * camera.VRP.Y + camera.N.Z * camera.VRP.Z),
                0,0,0,1
                );

            //Matrix4D inverse_M_WC_VRC = M_WC_VRC.inverse();

            return v * M_WC_VRC;
        }
        /************************************************************************/
        /* 该函数用于进行投影变换 ,prp 表示投影参考点,Q表示要投影的点，
         *默认为透视投影                                                        */
        /************************************************************************/
        public static Vector2D ProjectTransform(Vector3D prp,Vector3D Q)
        {
            Matrix4D projectMatrix = TransformMatrix.GetMPer(prp);

            Vector4D new_prp = new Vector4D(Q.X,Q.Y,Q.Z,1);
            Vector4D res =  new_prp * projectMatrix;
            return new Vector2D(res.X / res.W, res.Y / res.W);
        }

        public static Matrix3D TransformToNewCoordinate(Coordinate Coordinate)
        {
            return Matrix3D.GetAxisXSymmetry() * Matrix3D.GetTrans(Coordinate.Center.X, Coordinate.Center.Y);
        }

        /// <summary> 漫反射和环境光,获取色彩
        ///   漫反射和环境光求取亮度(暂时放于此处，且如何能产生色彩呢)  需要加入衰减函数(f(d))   
        ///   Ia 表示环境光的颜色（R,G,B）
        ///   Ip 表示光源的颜色，表示可以有多个光源
        ///   Ka 表示物体表面的环境光反射系数
        ///   Kd 表示物体表面的漫反射系数
        ///   Ks 表示物体表面的镜面反射系数
        ///   L为物体上的点到光源的向量，N为物体表面的法向量，V为物体上的点到视点的向量
        /// </summary>
        public static Vector3D GetBrightness(Vector3D Ia, List<Vector3D> Ip, Vector3D Ka, Vector3D Kd, Vector3D Ks, List<Vector3D> L, Vector3D N, Vector V)
        {
            if (Ip.Count != L.Count)
            {
                Console.WriteLine("参数错误！");
                return Vector3D.ZERO;
            }

            int size = Ip.Count;

            //I返回最后观察到的颜色值，ka * Ia 表示环境光
            Vector3D I = new Vector3D(Ka * Ia) ;

            //需要单独的参数传入，暂时不传入
            Vector3D CDecay = new Vector3D(1,0.01f,0.00001f);

            for (int i = 0; i < size; ++i )
            {
                Vector3D H = new Vector3D((L[i] + V) / 2.0f);

                float d = L[i].Module();
                float f = DecayFunction(d, CDecay.X, CDecay.Y, CDecay.Z);
                I = I + f * Ip[i] * (L[i]*N * Kd / L[i].Module() + H*N * Ks / H.Module());
            }
            return new Vector3D(I.X%255,I.Y%255,I.Z%255);
        }

        /************************************************************************/
        /* 衰减函数，其中C0,C1,C2的值用以控制f(d)变化的快慢，常数项C0防止f(d)变化过大*/
        /* f(d)的最大值为1，d表示物体距离光源的距离                               */
        /************************************************************************/
        public static float DecayFunction(float d,float C0,float C1, float C2)
        {
            float fd = 1.0f/(C0 + C1* d + C2 * d * d);
            return fd < 1 ? fd : 1;
        }

        public static Vertex GetTextureLocation(Vertex v, Vertex v1, Vertex v2, Vertex v3)
        {
            //Matrix4D matrix = new Matrix4D(
            //    v1.V_Position.X - v2.V_Position.X,v3.V_Position.X - v2.V_Position.X,0,0,
            //    v1.V_Position.Y - v2.V_Position.Y,v3.V_Position.Y - v2.V_Position.Y,0,0,
            //    v1.V_Position.Z - v2.V_Position.Z,v3.V_Position.Z - v2.V_Position.Z,1,1,
            //    0,0,1,1
            //    );
            //Matrix4D matrix1 = matrix.inverse();

            //float a = v1.V_Position.X - v2.V_Position.X;
            //float b = v3.V_Position.X - v2.V_Position.X;
            //float c = v1.V_Position.Y - v2.V_Position.Y;
            //float d = v3.V_Position.Y - v2.V_Position.Y;

            //float mode = a*d - c*b;

            //Matrix4D matrix = new Matrix4D(
            //    a,-b,0,0,
            //    -c,d,0,0,
            //    0,0,1,0,
            //    0,0,0,1
            //    );

            //Vector3D vector1 = new Vector3D(v.V_Position.X - v2.V_Position.X, v.V_Position.Y - v2.V_Position.Y, v.V_Position.Z - v2.V_Position.Z) ;
            //Vector3D coeff_Vector = new Vector3D(v.V_Position.X - v2.V_Position.X, v.V_Position.Y - v2.V_Position.Y, v.V_Position.Z - v2.V_Position.Z) * matrix / mode;

            //float s = coeff_Vector.X * (v1.S - v2.S) + coeff_Vector.X * (v3.S - v2.S) + v2.S;
            //float t = coeff_Vector.Y * (v1.T - v2.T) + coeff_Vector.Y * (v3.T - v2.T) + v2.T;

            //v.S = Math.Abs(s);
            //v.T = Math.Abs(t);
            //if (v.S > 1)
            //    v.S = 1;
            //if (v.T > 1)
            //    v.T = 1;

            Matrix3D matrix = new Matrix3D(
                v1.V_Position.X ,v2.V_Position.X,v3.V_Position.X,
                v1.V_Position.Y,v2.V_Position.Y,v3.V_Position.Y,
                1,1,1
                );
            Matrix3D inverse_Matrix = new Matrix3D();
            matrix.Inverse(ref inverse_Matrix);

            Vector3D coef =  new Vector3D(v.V_Position.X, v.V_Position.Y, 1) * inverse_Matrix;
            float s = coef.X * v1.S + coef.X * v2.S + coef.X *v3.S;
            float t = coef.Y * v1.T + coef.Y * v2.T + coef.Y * v3.T;

            v.S = Math.Abs(s);
            v.T = Math.Abs(t);
            return v;
        }

        public static Color GetTextureColor(Bitmap bitmap,float s,float t)
        {
            int i = (int)(bitmap.Width * s);
            int j = (int)(bitmap.Height * t);
            if (i < 0 || j < 0)
                return Color.Blue;
            if (i >= bitmap.Width || j >= bitmap.Height)
                return Color.Blue;
            return bitmap.GetPixel(i,j);
        }
    }
}
