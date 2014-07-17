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
    /* 形状类（是否新建一个实体类，该实体类由多个Shape组成）                */
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

        /// <summary>
        /// 计算<v1,v2>（有向边）与裁剪边的交点
        /// </summary>
        /// <param name="v1">有向边的始点</param>
        /// <param name="v2">有向边的终点</param>
        /// <param name="clipBoundary">裁剪边</param>
        /// <returns><v1,v2>与裁剪边的交点</returns>
        public Vertex InterSect(Vertex v1, Vertex v2, Shape clipBoundary)
        {
            Vertex newVertex = new Vertex();

            if (clipBoundary.m_node.Vertexs[0].V_Position.Y == clipBoundary.m_node.Vertexs[1].V_Position.Y)//水平裁剪边
            {
                //交点的坐标的Y值与端点的Y值相等
                newVertex.V_Position.Y = clipBoundary.m_node.Vertexs[0].V_Position.Y;
                //计算交点的X值得坐标
                newVertex.V_Position.X = v1.V_Position.X + (clipBoundary.m_node.Vertexs[0].V_Position.Y - v1.V_Position.Y)
                    * (v2.V_Position.X - v1.V_Position.X) / (v2.V_Position.Y - v1.V_Position.Y);

                //根据端点的颜色值，使用Grourand着色方法进行插值，获取新顶点的颜色值
                Vector3D color1 = new Vector3D(v1.V_Color.R,v1.V_Color.G,v1.V_Color.B);
                Vector3D color2 = new Vector3D(v2.V_Color.R, v2.V_Color.G, v2.V_Color.B);

                Vector3D color = ( newVertex.V_Position.Y -v1.V_Position.Y ) /( v1.V_Position.Y - v2.V_Position.Y) * color1
                    + (v1.V_Position.Y - newVertex.V_Position.Y) /(v1.V_Position.Y - v2.V_Position.Y) * color2;
                newVertex.V_Color = Color.FromArgb((int)Math.Abs(color.X % 255), (int)Math.Abs(color.Y % 255), (int)Math.Abs(color.Z % 255));
            }
            else//垂直裁剪边
            {
                newVertex.V_Position.X = clipBoundary.m_node.Vertexs[0].V_Position.X;
                newVertex.V_Position.Y = v1.V_Position.Y + (clipBoundary.m_node.Vertexs[0].V_Position.X - v1.V_Position.X)
                    * (v2.V_Position.Y - v1.V_Position.Y) / (v2.V_Position.X - v1.V_Position.X);

                if ((int)v1.V_Position.Y != (int)v2.V_Position.Y)
                {
                    Vector3D color1 = new Vector3D(v1.V_Color.R, v1.V_Color.G, v1.V_Color.B);
                    Vector3D color2 = new Vector3D(v2.V_Color.R, v2.V_Color.G, v2.V_Color.B);
                    Vector3D color = (newVertex.V_Position.Y - v1.V_Position.Y) / (v1.V_Position.Y - v2.V_Position.Y) * color1
                        + (v1.V_Position.Y - newVertex.V_Position.Y) / (v1.V_Position.Y - v2.V_Position.Y) * color2;
                    newVertex.V_Color = Color.FromArgb((int)Math.Abs(color.X % 255), (int)Math.Abs(color.Y % 255), (int)Math.Abs(color.Z % 255));
                }
                else
                {
                    newVertex.V_Color = v1.V_Color;
                }
            }
            return newVertex;
        }

        /// <summary>
        /// 测试顶点vertex与裁剪边的内外关系
        /// </summary>
        /// <param name="vertex">待测试顶点</param>
        /// <param name="clipBoundary">裁剪边</param>
        /// <returns>当vertex位于内侧时，返回true，否则返回false</returns>
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

        /// <summary>
        /// 将一个顶点加入到新的新的多边形节点中
        /// </summary>
        /// <param name="vertex">新加入的顶点</param>
        /// <param name="newShape">顶点存放的多边形</param>
        void Output(Vertex vertex, ref Shape newShape)
        {
            newShape.m_node.Vertexs.Add(vertex);
        }

        /// <summary>
        /// Sutherland-Hodgman裁剪算法实现
        /// </summary>
        /// <param name="shapeIn">输入的多边形的形状</param>
        /// <param name="clipBoundary">裁剪边</param>
        /// <returns>新的多边形</returns>
        Shape SutherlandHodgmanPolygonClip(Shape shapeIn, Shape clipBoundary)
        {
            //<s,p>表示有向边
            Vertex s = new Vertex();
            Vertex p = new Vertex();
            Vertex I = new Vertex();
            Shape shapeOut = new Shape();
            int size = shapeIn.m_node.GetVertexNum();

            //多边形以Vn,V0,V1,V2...Vn-1表示，对边VnV0进行处理

            if (size < 1)
                return null;
            s = shapeIn.m_node.Vertexs[size - 1];

            for (int j = 0; j < size; ++j)
            {
                p = shapeIn.m_node.Vertexs[j];
                if (Inside(p, clipBoundary))
                {

                    if (Inside(s, clipBoundary))//p点在裁剪边的内侧,s点也在裁剪边的内侧
                        Output(p, ref shapeOut);//（s、p完全落在裁剪窗口的内侧，将p输出到结果多边形中）
                    else//p点在裁剪边的内侧，s在裁剪边的外侧，交点I和p都是结果多边形的顶点，应该先输出I，再输出P
                    {
                        I = InterSect(s,p,clipBoundary);
                        Output(I,ref shapeOut);
                        Output(p,ref shapeOut);
                    }
                }
                else if (Inside(s, clipBoundary))//p点在裁剪边的外侧，s点在裁剪边的内侧
                {
                    I = InterSect(s,p,clipBoundary);//计算出sp月裁剪边的交点，并输出I
                    Output(I, ref shapeOut);
                }
                s = p;//s指向顶点p，p在下次循环指向下一个顶点
            }
            return shapeOut;
        }

        public Shape Cut(int xmin, int xmax, int ymin, int ymax)
        {
            Shape newShape = new Shape();
            Shape clipBoundary = new Shape();

            clipBoundary.m_node.Add(new Vertex(xmin, ymax, 0));
            clipBoundary.m_node.Add(new Vertex(xmin, ymin, 0));
            Node node = SutherlandHodgmanPolygonClip(this, clipBoundary).m_node;
            if (node != null)
                newShape = new Shape(node);

            clipBoundary.m_node.Vertexs.Clear();
            clipBoundary.m_node.Add(new Vertex(xmin, ymin, 0));
            clipBoundary.m_node.Add(new Vertex(xmax, ymin, 0));
            if (newShape.m_node.Vertexs.Count != 0)
                node = SutherlandHodgmanPolygonClip(newShape, clipBoundary).m_node;
            if (node != null)
                newShape = new Shape(node);

            clipBoundary.m_node.Vertexs.Clear();
            clipBoundary.m_node.Add(new Vertex(xmax, ymin, 0));
            clipBoundary.m_node.Add(new Vertex(xmax, ymax, 0));
            if (newShape.m_node.Vertexs.Count != 0)
                node = SutherlandHodgmanPolygonClip(newShape, clipBoundary).m_node;
            if (node != null)
                newShape = new Shape(node);

            clipBoundary.m_node.Vertexs.Clear();
            clipBoundary.m_node.Add(new Vertex(xmax, ymax, 0));
            clipBoundary.m_node.Add(new Vertex(xmin, ymax, 0));
            if (newShape.m_node.Vertexs.Count != 0)
                node = SutherlandHodgmanPolygonClip(newShape, clipBoundary).m_node;
            if (node != null)
                newShape = new Shape(node);

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

    /// <summary>
    /// 矩形类
    /// </summary>
    class RectangleShape : Shape
    {

        public RectangleShape(Vertex v1, Vertex v2,Vertex v3,Vertex v4)
        {
            m_node.Vertexs.Add(v1);
            m_node.Vertexs.Add(v2);
            m_node.Vertexs.Add(v3);
            m_node.Vertexs.Add(v4);
        }

        public Vertex V1
        {
            get { return m_node.Vertexs[0]; }
            set { m_node.Vertexs[0] = value; }
        }

        public Vertex V2
        {
            get { return m_node.Vertexs[1]; }
            set { m_node.Vertexs[1] = value; }
        }

        public Vertex V3
        {
            get { return m_node.Vertexs[2]; }
            set { m_node.Vertexs[2] = value; }
        }

        public Vertex V4
        {
            get { return m_node.Vertexs[3]; }
            set { m_node.Vertexs[3] = value; }
        }
    }
}
