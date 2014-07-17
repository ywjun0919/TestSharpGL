using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TestSharpGL.VectorClass
{
    enum ShapeMode{
        Triangle ,
        Rectangle
    }
    /************************************************************************/
    /* 形状类                                                               */
    /************************************************************************/

    class Shape
    {
        public Node m_node = new Node();

        public ShapeMode m_mode = ShapeMode.Triangle;

        public Shape() { }

        public Shape(Node node)
        {
            m_node = node;
        }

        public Shape(Node node, ShapeMode mode)
        {
            m_node = node;
            m_mode = mode; 
        }

        public bool Assert()
        {
            if (m_mode == ShapeMode.Triangle)
            {
            }
            //判断是否能组成一个三角形
            return true;
        }

        public Vertex InterSect(Vertex v1, Vertex v2, Shape clipBoundary)
        {
            Vertex newVertex = new Vertex();

            if (clipBoundary.m_node.Vertexs[0].V_Position.Y == clipBoundary.m_node.Vertexs[1].V_Position.Y)
            {
                newVertex.V_Position.Y = clipBoundary.m_node.Vertexs[0].V_Position.Y;
                newVertex.V_Position.X = v1.V_Position.X + (clipBoundary.m_node.Vertexs[0].V_Position.Y - v1.V_Position.Y)
                    * (v2.V_Position.X - v1.V_Position.X) / (v2.V_Position.Y - v1.V_Position.Y);

                Vector3D color1 = new Vector3D(v1.V_Color.R,v1.V_Color.G,v1.V_Color.B);
                Vector3D color2 = new Vector3D(v2.V_Color.R, v2.V_Color.G, v2.V_Color.B);

                Vector3D color = ( newVertex.V_Position.Y -v1.V_Position.Y ) /( v1.V_Position.Y - v2.V_Position.Y) * color1
                    + (v1.V_Position.Y - newVertex.V_Position.Y) / 
                    (v1.V_Position.Y - v2.V_Position.Y) * color2;
                newVertex.V_Color = Color.FromArgb((int)Math.Abs(color.X % 255), (int)Math.Abs(color.Y % 255), (int)Math.Abs(color.Z % 255));
            }
            else
            {
                newVertex.V_Position.X = clipBoundary.m_node.Vertexs[0].V_Position.X;
                newVertex.V_Position.Y = v1.V_Position.Y + (clipBoundary.m_node.Vertexs[0].V_Position.X - v1.V_Position.X)
                    * (v2.V_Position.Y - v1.V_Position.Y) / (v2.V_Position.X - v1.V_Position.X);

                newVertex.V_Color = v1.V_Color;
            }
            return newVertex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="xmin"></param>
        /// <param name="ymin"></param>
        /// <param name="xmax"></param>
        /// <param name="ymax"></param>
        /// <returns></returns>
        Boolean Inside(Vertex vertex, Shape clipBoundary)
        {
           if (clipBoundary.m_node.Vertexs[1].V_Position.X > clipBoundary.m_node.Vertexs[0].V_Position.X)
           {
               if (vertex.V_Position.Y >= clipBoundary.m_node.Vertexs[0].V_Position.X)
                   return true;
           }
           else if (clipBoundary.m_node.Vertexs[1].V_Position.X < clipBoundary.m_node.Vertexs[0].V_Position.X)
           {
               if (vertex.V_Position.Y <= clipBoundary.m_node.Vertexs[0].V_Position.Y)
                   return true;
           }
           else if (clipBoundary.m_node.Vertexs[1].V_Position.Y > clipBoundary.m_node.Vertexs[0].V_Position.Y)
           {
               if (vertex.V_Position.X <= clipBoundary.m_node.Vertexs[0].V_Position.X)
                   return true;
           }
           else if (clipBoundary.m_node.Vertexs[1].V_Position.Y < clipBoundary.m_node.Vertexs[0].V_Position.Y)
           {
               if (vertex.V_Position.X >= clipBoundary.m_node.Vertexs[0].V_Position.X)
                   return true;
           }
           return false;
        }

        void Output(Vertex vertex, ref Shape newShape)
        {
            newShape.m_node.Vertexs.Add(vertex);
        }

        Shape SutherlandHodgmanPolygonClip(Shape shapeIn, Shape clipBoundary)
        {
            Vertex s = new Vertex();
            Vertex p = new Vertex();
            Vertex I = new Vertex();
            Shape shapeOut = new Shape();
            int size = shapeIn.m_node.GetVertexNum();

            //这个地方有问题，当向上移动的时候，会出现shapeIn 为空的情况，暂时不知道为什么
            if (size <= 1)
            {
                return null;
            }
            s = shapeIn.m_node.Vertexs[size - 1];

            for (int j = 0; j < size; ++j)
            {
                p = shapeIn.m_node.Vertexs[j];
                if (Inside(p, clipBoundary))
                {
                    if (Inside(s, clipBoundary))
                        Output(p, ref shapeOut);
                    else
                    {
                        I = InterSect(s,p,clipBoundary);
                        Output(I,ref shapeOut);
                        Output(p,ref shapeOut);
                    }
                }
                else if (Inside(s, clipBoundary))
                {
                    I = InterSect(s,p,clipBoundary);
                    Output(I, ref shapeOut);
                }
                s = p;
            }
            return shapeOut;
        }

        public Shape Cut(int xmin, int xmax, int ymin, int ymax)
        {
            Shape newShape = new Shape();
            Shape clipBoundary = new Shape();

            clipBoundary.m_node.Add(new Vertex(xmin, ymax, 0));
            clipBoundary.m_node.Add(new Vertex(xmin, ymin, 0));
            newShape = new Shape(SutherlandHodgmanPolygonClip(this, clipBoundary).m_node);

            clipBoundary.m_node.Vertexs.Clear();
            clipBoundary.m_node.Add(new Vertex(xmin, ymin, 0));
            clipBoundary.m_node.Add(new Vertex(xmax, ymin, 0));
            newShape = new Shape(SutherlandHodgmanPolygonClip(newShape, clipBoundary).m_node);

            clipBoundary.m_node.Vertexs.Clear();
            clipBoundary.m_node.Add(new Vertex(xmax, ymin, 0));
            clipBoundary.m_node.Add(new Vertex(xmax, ymax, 0));
            newShape = new Shape(SutherlandHodgmanPolygonClip(newShape, clipBoundary).m_node);

            clipBoundary.m_node.Vertexs.Clear();
            clipBoundary.m_node.Add(new Vertex(xmax, ymax, 0));
            clipBoundary.m_node.Add(new Vertex(xmin, ymax, 0));
            newShape = new Shape(SutherlandHodgmanPolygonClip(newShape, clipBoundary).m_node);

            return newShape;
        }
    }

    /************************************************************************/
    /* 三角形类                                                         */
    /************************************************************************/
    class Triangle:Shape
    {

        public Vertex V1
        {
            get { return m_node.GetVertex(0); }
        }
        public Vertex V2
        {
            get { return m_node.GetVertex(1); }
        }
        public Vertex V3
        {
            get { return m_node.GetVertex(2); }
        }

        public Triangle(Vertex v1,Vertex v2,Vertex v3) 
        {
            m_node.Add(v1);
            m_node.Add(v2);
            m_node.Add(v3);
        }
        Vector3D N = new Vector3D();

        public Vector3D GetN()
        {
            Vector3D v1 = new Vector3D( V2.V_Position - V1.V_Position);
            Vector3D v2 = new Vector3D( V3.V_Position - V1.V_Position);
            Vector3D v = v1.CrossMultiply(v2);

            return v/v.Module();
        }
    }
}
