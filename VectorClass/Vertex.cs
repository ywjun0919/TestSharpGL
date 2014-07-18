using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TestSharpGL.VectorClass
{
    /************************************************************************/
    /* 顶点类                                                               */
    /************************************************************************/
    class Vertex
    {
        //设置属性默认值
        Color v_Color = Color.Black;
        public Color V_Color
        {
            get { return v_Color; }
            set { v_Color = value; }
        }
        Vector3D v_Position = new Vector3D(Vector3D.ZERO);

        public Vector3D V_Position 
        {
            get { return v_Position; }
            set { v_Position = value; }
        }
        public Vertex()
        {

        }

        public Vertex(Vertex vertex)
        {
            v_Position = vertex.v_Position;
            v_Color = vertex.V_Color;
            m_S = vertex.m_S;
            m_T = vertex.m_T;
        }

        public Vertex(Vector3D vec) 
        {
            v_Position = vec;
        }

        public Vertex(float x,float y ,float z)
        {
            v_Position.X = x;
            v_Position.Y = y;
            v_Position.Z = z;
        }

        public Vertex(float x, float y, float z,Color color)
        {
            v_Position.X = x;
            v_Position.Y = y;
            v_Position.Z = z;
            v_Color = color;
        }

        public Vertex(float x, float y, float z, Color color,float s,float t)
        {
            v_Position.X = x;
            v_Position.Y = y;
            v_Position.Z = z;
            v_Color = color;
            m_S = s;
            m_T = t;
        }

        public Vertex(Vector3D vec, Color color) 
        {
            v_Color = color;
            v_Position = vec;
        }
        public Vertex(Vector3D vec, Color color,float s,float t)
        {
            v_Color = color;
            v_Position = vec;
            m_S = s;
            m_T = t;
        }
        public object Clone()
        {
            Vertex v = new Vertex();
            v.v_Color = v_Color;
            v.v_Position = v_Position;
            v.m_S = m_S;
            v.m_T = m_T;

            return v;
        }
        
        //纹理属性(s与t一般在[0,1])
        float m_S=0.0f;
        float m_T=0.0f;
        public float S 
        {
            get { return m_S; }
            set { m_S = value; }
        }

        public float T
        {
            get { return m_T; }
            set { m_T = value; }
        }
    }

}
