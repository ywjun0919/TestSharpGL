using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TestSharpGL.VectorClass
{
    /************************************************************************/
    /* 该类用于绘制图形                                                     */
    /************************************************************************/
    class Draw
    {
        #region 属性
        //视窗的矩形
        Bitmap _bmp;
        public  Shape m_shape = new Shape();
        private Graphics _graphics;
        private Color _color = Color.Black;
        private Coordinate coordinate;

        public Bitmap Bmp
        {
            get { return _bmp; }
            set { _bmp = value; }
        }
        public Graphics graphics
        {
            get { return _graphics; }
            set { _graphics = value; }
        }
        public Color color 
        {
            get { return _color; }
            set { _color = value; }
        }
        public void SetColor(int r, int g, int b)
        {
            color = Color.FromArgb(r, g, b);
        }
        
        #endregion

        #region 构造函数


        public Draw()
        {
        }

        public Draw(Bitmap bmp)
        {
            Bmp = bmp;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 开始绘制
        /// </summary>
        /// <param name="g"></param>
        public void Begin(Graphics g) 
        {
            graphics = g;
            DrawCoordinates2D();
        }
        public void End() 
        {
            graphics.DrawImage(Bmp, new Rectangle(100, 0, 500, 600));
            //m_shape = null;//暂时这样
        }
        public void DrawPixel(Vector2D point)
        {
            Vector3D newVector = new Vector3D(point.X, point.Y, 1) * Common.TransformToNewCoordinate(coordinate);
            Bmp.SetPixel((int)newVector.X, (int)newVector.Y, color);
        }

        public void DrawPixel(Vector2D point,Color c) 
        {
            color = c;
            Vector3D newVector = new Vector3D(point.X, point.Y, 1) * Matrix3D.GetAxisXSymmetry() * Matrix3D.GetTrans(coordinate.Center.X, coordinate.Center.Y);
            Bmp.SetPixel((int)newVector.X, (int)newVector.Y, color);
        }

        public void DrawLineByDDA(Vector2D point1,Vector2D point2)
        {
            float x;
            float dy, dx, y, m;

            Vector2D new_Point1 = new Vector2D(point1);
            Vector2D new_Point2 = new Vector2D(point2);
            if (new_Point1.X > new_Point2.X)
            {
                new_Point1.SwapVector(new_Point2);
            }

            dx = new_Point2.X - new_Point1.X;
            dy = new_Point2.Y - new_Point1.Y;

            if (Math.Abs(dx - 0.0) < 0.000001)
            {
                float min = new_Point1.Y, max = new_Point2.Y;
                if (new_Point1.Y>new_Point2.Y)
                {
                    min = new_Point2.Y;
                    max = new_Point1.Y;
                }
                for (y = min; y <= max; ++y)
                {
                    DrawPixel(new Vector2D(new_Point1.X, y), color);
                }
            }
            else 
            {
                m = dy / dx;
                
                y = new_Point1.Y;
                float min = new_Point1.Y, max = new_Point2.Y;
                
                if (min>max)
                {
                    Common.swap(ref min, ref max);
                }

                for (x = new_Point1.X; x <= new_Point2.X; ++x)
                {
                    if (0 == m)
                    {
                        DrawPixel(new Vector2D(x, new_Point1.Y), color);
                    }
                    else
                    {
                        for (float i = 0; i < Math.Abs(m); ++i)
                        {
                            DrawPixel(new Vector2D(x, 0.5f + y + i), color);
                            if (0.5f + y + i < min || 0.5f + y + i > max)
                            {
                                break;
                            }
                        }
                        y += m;
                    } 
                }
            }
        }

        public void DrawSharp(Shape new_Shape)
        {
            int size = new_Shape.m_node.Vertexs.Count;

            for (int index = 0; index < size; ++index)
            {
                DrawLineByDDA(new_Shape.m_node.Vertexs[index], new_Shape.m_node.Vertexs[(index+1)%size]);
            }
        }
        public void DrawSharp()
        {
            int size = m_shape.m_node.GetVertexNum();

            if (size <= 0 )
                return;
            Vector3D prp = sceneManager.scene.GetCurCamera().PRP;//暂时这样
            List<Vector2D> vector_Buffer = new List<Vector2D>();
            for (int index = 0; index < size;++index )
            {
                Vertex Vertex = m_shape.m_node.GetVertex(index);

                Vector2D proVec = Common.ProjectTransform(prp, Vertex.V_Position);
                vector_Buffer.Add(proVec);
            }

            if (m_shape.m_mode == ShapeMode.Triangle)
            {
                for (int index = 0; index < vector_Buffer.Count;index +=3 )
                {
                    DrawLineByDDA(vector_Buffer[index], vector_Buffer[(index + 1) % vector_Buffer.Count]);
                    DrawLineByDDA(vector_Buffer[(index + 1) % vector_Buffer.Count], vector_Buffer[(index + 2) % vector_Buffer.Count]);
                    DrawLineByDDA(vector_Buffer[(index + 2) % vector_Buffer.Count], vector_Buffer[index]);
                }
            }
        }

        public void DrawCoordinates2D()
        {
            coordinate = new Coordinate(500, 500, 400, 400, 300, 300);
            color = Color.Yellow;
            DrawLineByDDA(new Vector2D( - coordinate.Left,0), new Vector2D( coordinate.Right, 0));
            DrawLineByDDA(new Vector2D(0,  - coordinate.Top), new Vector2D(0,coordinate.Bottom));
            color = Color.Blue;
            //color = Color.Blue;

            //Vector3D newVector = new Vector3D(10, 10, 1) * Matrix3D.GetAxisXSymmetry() * Matrix3D.GetTrans(coordinate.Center.X, coordinate.Center.Y);
            //int x = (int)newVector.X;
            //int y = (int)newVector.Y;

            //DrawPixel(new Vector2D(x, y));
            //DrawPixel(new Vector2D(x - 1, y));
            //DrawPixel(new Vector2D(x, y - 1));
            //DrawPixel(new Vector2D(x + 1, y));
            //DrawPixel(new Vector2D(x, 1));
        }
        
        //具有环境光的线条画线方法（需要与画线方法合并）
        SceneManager sceneManager =SceneManager.CreateSceneManager();

        public void DrawLineByDDA(Vertex point1, Vertex point2)
        {
            float x;
            float dy, dx, y, m;

            if (point1 == null || point2 == null)
                return;
            Vector2D new_Point1 = new Vector2D(point1.V_Position);
            Vector2D new_Point2 = new Vector2D(point2.V_Position);
            if (new_Point1.X > new_Point2.X)
            {
                new_Point1.SwapVector(new_Point2);
            }

            dx = new_Point2.X - new_Point1.X;
            dy = new_Point2.Y - new_Point1.Y;

            if (Math.Abs(dx - 0.0) < 0.000001)
            {
                float min = new_Point1.Y, max = new_Point2.Y;
                if (new_Point1.Y > new_Point2.Y)
                {
                    min = new_Point2.Y;
                    max = new_Point1.Y;
                }
                for (y = min; y <= max; ++y)
                {
                    Vector3D color1 = new Vector3D(point1.V_Color.R, point1.V_Color.G, point1.V_Color.B);
                    Vector3D color2 = new Vector3D(point2.V_Color.R, point2.V_Color.G, point2.V_Color.B);
                    if (Math.Abs(new_Point2.X - new_Point1.X) < 0.01)
                    {
                        color = point1.V_Color;
                    }
                    else
                    {
                        Vector3D color3 = (y - new_Point2.Y) / (new_Point1.Y - new_Point2.Y) * color1 + (new_Point1.Y - y) / (new_Point1.Y - new_Point2.Y) * color2;

                        color = Color.FromArgb((int)color3.X, (int)color3.Y, (int)color3.Z);
                    }
                   
                    DrawPixel(new Vector2D(new_Point1.X, y), color);
                }
            }
            else
            {
                m = dy / dx;

                y = new_Point1.Y;
                float min = new_Point1.Y, max = new_Point2.Y;

                if (min > max)
                {
                    Common.swap(ref min, ref max);
                }

                for (x = new_Point1.X; x <= new_Point2.X; ++x)
                {
                    if (0 == m)
                    {
                        Vector3D color1 = new Vector3D(point1.V_Color.R, point1.V_Color.G, point1.V_Color.B);
                        Vector3D color2 = new Vector3D(point2.V_Color.R, point2.V_Color.G, point2.V_Color.B);

                        if (Math.Abs(new_Point2.X - new_Point1.X) < 0.01)
                        {
                            color = point1.V_Color;
                        }
                        else
                        {
                            Vector3D color3 = (new_Point2.X - x) / (new_Point2.X - new_Point1.X) * color1 + (x - new_Point1.X) / (new_Point2.X - new_Point1.X) * color2;

                            color = Color.FromArgb((int)color3.X, (int)color3.Y, (int)color3.Z);
                        }
                        
                        DrawPixel(new Vector2D(x, new_Point1.Y), color);
                    }
                    else
                    {
                        for (float i = 0; i < Math.Abs(m); ++i)
                        {
                            Vector3D color1 = new Vector3D(point1.V_Color.R, point1.V_Color.G, point1.V_Color.B);
                            Vector3D color2 = new Vector3D(point2.V_Color.R, point2.V_Color.G, point2.V_Color.B);

                            Vector3D color3 = (y - new_Point2.Y) / (new_Point1.Y - new_Point2.Y) * color1 + (new_Point1.Y - y) / (new_Point1.Y - new_Point2.Y) * color2;

                            color = Color.FromArgb((int)color3.X, (int)color3.Y, (int)color3.Z);
                            DrawPixel(new Vector2D(x, 0.5f + y + i),color);
                            if (0.5f + y + i < min || 0.5f + y + i > max)
                            {
                                break;
                            }
                        }
                        y += m;
                    }
                }
            }
        }

        //画三角形
        public void DrawTriangle()
        {
            int size = m_shape.m_node.GetVertexNum();
            Vector3D prp = sceneManager.scene.GetCurCamera().PRP;//暂时这样
            List<Vertex> vertex_Buffer = new List<Vertex>();
            for (int index = 0; index < size; index+= 3)
            {
                if (index % 3 != 3)
                {
                    Triangle triangle = new Triangle(m_shape.m_node.GetVertex(index), m_shape.m_node.GetVertex(index + 1), m_shape.m_node.GetVertex(index + 2));

                    for (int i =0;i<3;++i)
                    {
                        Vertex v = triangle.m_node.GetVertex(i);
                        sceneManager.ScenceRender(ref v, triangle.GetN());
                        Vector2D proVec = Common.ProjectTransform(prp, v.V_Position);
                        vertex_Buffer.Add(new Vertex(proVec.X,proVec.Y,1,v.V_Color));
                    }
                }
            }

            if (m_shape.m_mode == ShapeMode.Triangle)
            {
                for (int index = 0; index < vertex_Buffer.Count; index += 3)
                {
                    if (index % 3 != 3)
                    {
                        Triangle triangle = new Triangle(vertex_Buffer[index], vertex_Buffer[index + 1], vertex_Buffer[index + 2]);
                        Shape shape = triangle.Cut(-20, 50,-50, 20);
                        //Shape shape = triangle;
                        DrawSharp(shape);
                    }
                }
            }
        }

        public void DrawTriangle1()
        {
            int size = m_shape.m_node.GetVertexNum();
            Vector3D prp = sceneManager.scene.GetCurCamera().PRP;//暂时这样
            List<Vertex> vertex_Buffer = new List<Vertex>();
            for (int index = 0; index < size; index += 3)
            {
                if (index % 3 != 3)
                {
                    Triangle triangle = new Triangle(m_shape.m_node.GetVertex(index), m_shape.m_node.GetVertex(index + 1), m_shape.m_node.GetVertex(index + 2));

                    for (int i = 0; i < 3; ++i)
                    {
                        Vertex v = triangle.m_node.GetVertex(i);
                        sceneManager.ScenceRender(ref v, triangle.GetN());
                        Vector2D proVec = Common.ProjectTransform(prp, v.V_Position);
                        vertex_Buffer.Add(new Vertex(proVec.X, proVec.Y, 1, v.V_Color));
                    }

                }
            }

            if (m_shape.m_mode == ShapeMode.Triangle)
            {
                for (int index = 0; index < vertex_Buffer.Count; index += 3)
                {
                    if (index % 3 != 3)
                    {
                        Triangle triangle = new Triangle(vertex_Buffer[index], vertex_Buffer[index + 1], vertex_Buffer[index + 2]);
                        Shape shape = triangle.Cut(-50, 50, -50, 50);
                        //Shape shape = triangle;
                        PolygonFill(shape);
                    }
                    System.Console.WriteLine("Count:" + vertex_Buffer.Count);
                    System.Console.WriteLine("..."+index);
                }
            }
        }

        class Edge
        {
           public int yMax;
           public float x, deltax;
           public Color color;
           public Vector3D delataColor;
        };

        //多边形填充，好像没有颜色呢
        public void PolygonFill(Shape shape)
        {
            int ymin = 0;
            int ymax = 0;
            GetPolygonMinMax(shape, ref ymin, ref ymax);

           List<Edge>[] ET = InitScanLineETTable(shape, ymin, ymax);

            List<Edge> AEL = new List<Edge>();
            int y = ymin;
            for (int i = 0; i < ymax - ymin; ++i)
            {
                int et_Count = ET[i].Count;
                if ( et_Count> 0)
                {
                    for (int j = 0; j < et_Count; ++j)
                    {
                        AEL.Add(ET[i][j]);
                    }

                   // 
                    while (AEL.Count >= 2)
                    {
                        for (int j = 0; j < AEL.Count; j += 2)
                        {
                            Vertex v1 = new Vertex(AEL[j].x, y, 0, AEL[j].color);
                            Vertex v2 = new Vertex(AEL[(j + 1) % AEL.Count].x, y, 0, AEL[(j + 1) % AEL.Count].color);
                           // /if (v1.V_Position.X != v2.V_Position.X|| v1.V_Position.Y!= v2.V_Position.Y)
                            {
                                DrawLineByDDA(v1, v2);
                            }
                            y++;
                            AEL[j].x += AEL[j].deltax;
                            AEL[(j + 1) % AEL.Count].x += AEL[(j + 1) % AEL.Count].deltax;
                            AEL[j].color = Color.FromArgb(
                                Math.Abs(AEL[j].color.R + (int)AEL[j].delataColor.X)%255, 
                                Math.Abs(AEL[j].color.R + (int)AEL[j].delataColor.Y)%255,
                                Math.Abs(AEL[j].color.R + (int)AEL[j].delataColor.Z) % 255);


                            if (AEL[(j + 1) % AEL.Count].yMax < y)
                            {
                                AEL.RemoveAt((j + 1) % AEL.Count);
                               // j--;
                            }
                            if (AEL[j].yMax < y)
                            {
                                AEL.RemoveAt(j);
                                //j--;
                            } 
                        }
                    }
                }
            }
        }

        public void GetPolygonMinMax(Shape shape, ref int min, ref int max)
        {
            List<Vertex> vertexs = shape.m_node.Vertexs;
            int size = vertexs.Count;
            if(size<=0)
            {
                return;
            }
            min = (int)vertexs[0].V_Position.Y;
            max = (int)vertexs[0].V_Position.Y;

            for (int index = 1; index < size; ++index)
            {
                min = min < vertexs[index].V_Position.Y ?(int) min : (int)vertexs[index].V_Position.Y;
                max = max > vertexs[index].V_Position.Y ? (int)max :(int) vertexs[index].V_Position.Y;
            }
        }

        //计算分类表（ET）
        private List<Edge>[] InitScanLineETTable(Shape shape, int ymin, int ymax) 
        {
            
            List<Vertex> vertexs = shape.m_node.Vertexs;
            int size = vertexs.Count;
            List<Edge>[] ET = new List<Edge>[ymax - ymin];

            for (int i = 0; i < ymax - ymin; ++i)
            {
                ET[i] = new List<Edge>();
            }   

            for (int index = 0; index < size; ++index)
            {
                Vertex vertex1 = vertexs[index];
                Vertex vertex2 = vertexs[(index + 1) % size];

                Vertex vertex11 = vertexs[(index - 1 + size) % size];
                Vertex vertex22 = vertexs[(index + 2 + size) % size];

                //水平线不做处理
                if (vertex1.V_Position.Y != vertex2.V_Position.Y)
                {
                    Edge edge = new Edge();
                    edge.deltax = (float)(vertex1.V_Position.X - vertex2.V_Position.X) / (float)(vertex1.V_Position.Y - vertex2.V_Position.Y);
                    edge.delataColor = 1.0f / (float)(vertex1.V_Position.Y - vertex2.V_Position.Y) * 
                        (new Vector3D(vertex1.V_Color.R, vertex1.V_Color.G, vertex1.V_Color.B) 
                        - new Vector3D(vertex2.V_Color.R, vertex2.V_Color.G, vertex2.V_Color.B));
                    edge.color = vertex1.V_Color;

                    if (vertex1.V_Position.Y > vertex2.V_Position.Y)
                    {
                        edge.x = vertex2.V_Position.X;

                        //edge.yMax = (int)vertex1.V_Position.Y;
                        edge.yMax = vertex11.V_Position.Y >= vertex1.V_Position.Y ? (int)(vertex1.V_Position.Y - 1) : (int)vertex1.V_Position.Y;



                        ET[(int)(vertex2.V_Position.Y - ymin) % (ymax - ymin)].Add(edge);
                    }
                    else
                    {
                        edge.x = vertex1.V_Position.X;

                       // edge.yMax = (int)vertex2.V_Position.Y;
                        edge.yMax = vertex22.V_Position.Y >= vertex2.V_Position.Y ? (int)(vertex2.V_Position.Y - 1) : (int)vertex2.V_Position.Y;

                        ET[(int)(vertex1.V_Position.Y - ymin)%(ymax-ymin)].Add(edge);
                    }
                }
            }

            return ET;
        }

        //（该方法仅限于使用矩形对多边形进行裁剪）
        /// <summary>
        /// shape1 被裁剪的多边形，shape2裁剪的多边形(换做了int xmin,int ymin,int xmax,int max)
        /// 裁剪的时候还需要计算出交点的颜色值吧。。。
        /// </summary>
        /// <param name="shape1"></param>
        /// <param name="shape2"></param>
        //public void CutShape(Shape shape1,int xmin,int ymin,int xmax,int ymax)
        //{
        //    int size1 = shape1.m_node.Vertexs.Count;

        //    Shape shape = new Shape();

        //    for (int i = 0; i < size1; ++i)
        //    {
        //        bool condition1 = ISInRectangle(shape1.m_node.Vertexs[i], xmin, ymin, xmax, ymax) ;
        //        bool condition2 = ISInRectangle(shape1.m_node.Vertexs[(i + 1) % size1], xmin, ymin, xmax, ymax);
        //        if (condition1 && condition2)//两点都在矩形区域内
        //        {
        //            shape.m_node.Vertexs.Add(new Vertex(shape1.m_node.Vertexs[i]));
        //            shape.m_node.Vertexs.Add(new Vertex(shape1.m_node.Vertexs[(i + 1) % size1]));
        //        }
        //        else if(condition1 || condition2)//第一个点在矩形区域内，第二个点在矩形区域外
        //        {
        //            int deltax = Convert.ToInt32(shape1.m_node.Vertexs[i].V_Position.X - shape1.m_node.Vertexs[(i + 1) % size1].V_Position.X);
        //            int deltay = Convert.ToInt32(shape1.m_node.Vertexs[i].V_Position.Y - shape1.m_node.Vertexs[(i + 1) % size1].V_Position.Y);
        //            shape.m_node.Vertexs.Add(new Vertex(shape1.m_node.Vertexs[i]));
        //            if (deltax == 0)
        //            {
        //                //若于Y轴垂直，则只需求与y=ymin和y=ymax的交点
        //                //color需要再求
        //                Vertex v1 = new Vertex(shape1.m_node.Vertexs[i].V_Position.X,ymin,0);
        //                Vertex v2 = new Vertex(shape1.m_node.Vertexs[i].V_Position.X, ymax, 0);

        //                if (ISPositive(v1, shape1.m_node.Vertexs[i], shape1.m_node.Vertexs[(i + 1) % size1]))
        //                {
        //                    shape.m_node.Vertexs.Add(v1);
        //                }
        //                if (ISPositive(v2, shape1.m_node.Vertexs[i], shape1.m_node.Vertexs[(i + 1) % size1]))
        //                {
        //                    shape.m_node.Vertexs.Add(v2);
        //                }
        //            }
        //            else if (deltay == 0)
        //            {
        //                Vertex v1 = new Vertex(xmin, shape1.m_node.Vertexs[i].V_Position.Y, 0);
        //                Vertex v2 = new Vertex(xmax, shape1.m_node.Vertexs[i].V_Position.Y, 0);

        //                if (ISPositive(v1, shape1.m_node.Vertexs[i], shape1.m_node.Vertexs[(i + 1) % size1]))
        //                {
        //                    shape.m_node.Vertexs.Add(v1);
        //                }
        //                if (ISPositive(v2, shape1.m_node.Vertexs[i], shape1.m_node.Vertexs[(i + 1) % size1]))
        //                {
        //                    shape.m_node.Vertexs.Add(v2);
        //                }
        //            }
        //            else 
        //            {

        //            }
        //        }

        //    }
        //}

        //public bool ISInRectangle(Vertex vertex, int xmin, int ymin, int xmax, int ymax)
        //{
        //    return (vertex.V_Position.X <= xmax && vertex.V_Position.X >= xmin)
        //                       && (vertex.V_Position.Y <= ymax && vertex.V_Position.Y >= ymin);
        //}

        ///// <summary>
        ///// 判断交点是否在直线内，还是在直线的延长线上
        ///// </summary>
        ///// <param name="vertex">待判断的点</param>
        ///// <param name="vertex1">直线上的一个端点</param>
        ///// <param name="vertex2">直线上的另一个端点</param>
        ///// <returns>若待判断的点在两个端点内则返回true，否则返回false</returns>
        //public bool ISInLine(Vertex vertex, Vertex vertex1, Vertex vertex2)
        //{
        //    int xmin = 0 , xmax = 0;
        //    int ymin = 0,  ymax = 0;

        //    xmin = vertex1.V_Position.X > vertex2.V_Position.X ? (int)vertex2.V_Position.X : (int)vertex1.V_Position.X;
        //    xmax = vertex1.V_Position.X > vertex2.V_Position.X ? (int)vertex1.V_Position.X : (int)vertex2.V_Position.X;

        //    ymin = vertex1.V_Position.Y > vertex2.V_Position.Y ? (int)vertex2.V_Position.Y : (int)vertex1.V_Position.Y;
        //    ymax = vertex1.V_Position.Y > vertex2.V_Position.Y ? (int)vertex1.V_Position.Y : (int)vertex2.V_Position.Y;

        //    if ((vertex.V_Position.X >= xmin && vertex.V_Position.X <= xmax) && (vertex.V_Position.Y >= ymin && vertex.V_Position.Y <= ymax))
        //        return true;
        //    return false;
        //}

        ///// <summary>
        ///// 判断交点是否与线段同向
        ///// </summary>
        ///// <param name="vertex">待判断的交点</param>
        ///// <param name="vertex1">端点1</param>
        ///// <param name="vertex2">端点2</param>
        ///// <returns></returns>
        //public bool ISPositive(Vertex vertex, Vertex vertex1, Vertex vertex2)
        //{
        //    float res = (vertex.V_Position - vertex1.V_Position).DotMultiply(vertex2.V_Position - vertex1.V_Position);

        //    return res/Math.Abs(res)>0?true:false;
        //}
        #endregion
        
    }
}
