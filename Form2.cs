using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TestSharpGL.VectorClass;

namespace TestSharpGL
{
    public partial class Form2 : Form
    {
        Bitmap bmp;
       
        uint state = 2;

        Node node = new Node();
        Camera camera;
        SceneManager sceneManager = SceneManager.CreateSceneManager();
        Shape shape = new Shape();

        Entity entity = new Entity();

        void StateChange(uint newState)
        {
            state = newState;
        }
        public Form2()
        {
            InitializeComponent();

             camera = new Camera();

            //设置照相机的参考点
            camera.PRP = new Vector3D(0, 0, -200);
           
            sceneManager.scene.CameraAdd(camera);

            bmp = new Bitmap(1000, 1000);

            //draw = new Draw(bmp);
            camera.VRP = new Vector3D(0,0, 0);
            camera.U = new Vector3D(1, 0, 0);
            camera.V = new Vector3D(0, 1, 0);
            camera.N = new Vector3D(0, 0, 1);
            
            #region 暂时注释
            //node.Add(new Vertex(0.0f, 50.0f, 0.0f,Color.Red));
            //node.Add(new Vertex(-50.0f, -50.0f, 50.0f, Color.Blue));
            //node.Add(new Vertex(50.0f, -50.0f, 50.0f, Color.Yellow));

            //node.Add(new Vertex(0.0f, 50.0f, 0.0f, Color.Red));
            //node.Add(new Vertex(50.0f, -50.0f, 50.0f, Color.Yellow));
            //node.Add(new Vertex(50.0f, -50.0f, -50.0f, Color.Yellow));

            //node.Add(new Vertex(0.0f, 50.0f, 0.0f, Color.Red));
            //node.Add(new Vertex(50.0f, -50.0f, -50.0f, Color.Yellow));
            //node.Add(new Vertex(-50.0f, -50.0f, -50.0f,Color.Green));

            //node.Add(new Vertex(0.0f, 50.0f, 0.0f, Color.Red));
            //node.Add(new Vertex(-50.0f, -50.0f, -50.0f, Color.Green));
            //node.Add(new Vertex(-50.0f, -50.0f, 50.0f, Color.Blue));
            #endregion

            node.Add(new Vertex(Common.WorldTransform( new Vector3D( 0.0f, 50.0f, 0.0f),camera), Color.Red,0,0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, 50.0f), camera), Color.Blue,1,0));
            node.Add(new Vertex(Common.WorldTransform( new Vector3D(50.0f, -50.0f, 50.0f),camera), Color.Yellow,0f,1f));

            node.Add(new Vertex(Common.WorldTransform(new Vector3D(0.0f, 50.0f, 0.0f), camera), Color.Red, 0, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, -50.0f, 50.0f), camera), Color.Yellow, 1, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, -50.0f, -50.0f), camera), Color.Yellow, 0f, 1f));

            node.Add(new Vertex(Common.WorldTransform(new Vector3D(0.0f, 50.0f, 0.0f), camera), Color.Red, 0, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, -50.0f, -50.0f), camera), Color.Yellow, 1, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, -50.0f), camera), Color.Green, 0f, 1f));

            node.Add(new Vertex(Common.WorldTransform(new Vector3D(0.0f, 50.0f, 0.0f), camera), Color.Red, 0, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, -50.0f), camera), Color.Green, 1, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, 50.0f), camera), Color.Blue, 0f, 1f));

            Bitmap image = new Bitmap(@"D:\1359913743_8809.jpg");
            Texture texture = new Texture(image);
            sceneManager.scene.TextureAdd(texture);
            shape = new Shape(node);
            Vector3D v1 = new Vector3D(-50.0f, -50.0f, 50.0f);
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            ////显示图片

            //float x = (this.ClientRectangle.Width - 100) / 2;
            //float y = (this.ClientRectangle.Height) / 2;

            //bmp.SetPixel((int)x, (int)y, Color.Blue);
            //bmp.SetPixel((int)x, (int)y + 1, Color.Blue);
            //bmp.SetPixel((int)x + 1, (int)y + 1, Color.Blue);
            //bmp.SetPixel((int)x + 1, (int)y, Color.Blue);
            //bmp.SetPixel((int)x - 1, (int)y - 1, Color.Blue);
            //bmp.SetPixel((int)x - 1, (int)y, Color.Blue);

            //graphics.DrawImage(bmp, new Rectangle(100, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            bmp = new Bitmap(1000, 1000);
            Draw draw = new Draw(bmp);
            draw.Begin(graphics);
            draw.DrawCoordinates2D();
            draw.End();
            if (0 == state) 
            {
                Line1(graphics);
            }
            else if (1 == state)
            {
                Line2(graphics);
            }
            else if (2 == state )
            {
                //shape = new Shape(node);
                DrawTriangle( graphics);
            }
            else if (3 == state)
            {

                this.SetStyle(ControlStyles.ResizeRedraw |
                 ControlStyles.OptimizedDoubleBuffer |
                  ControlStyles.AllPaintingInWmPaint, true);
                this.UpdateStyles();
                bmp = new Bitmap(1000, 1000);
                draw = new Draw(bmp);
                draw.Begin(graphics);
                draw.DrawCoordinates2D();
                draw.End();
                Rotation(graphics,5);

            }
            else if (4 == state)
            {
                draw.Begin(graphics);
                bmp = new Bitmap(1000, 1000);
                draw = new Draw(bmp);
                draw.Begin(graphics);
                draw.DrawEntity(entity);
                draw.End();
            }
        }

        private void LineTest_Click(object sender, EventArgs e)
        {
            StateChange(1);
            this.Refresh();
        }

        public void Line1(Graphics graphics) 
        {
            Draw draw = new Draw(bmp);
            draw = new Draw(bmp);
            draw.Begin(graphics);
            draw.color = Color.Red;
            Vector2D v2 = new Vector2D(10, 20);
            Vector2D v1 = new Vector2D(50, 80);
            draw.DrawLineByDDA(v1, v2);
            draw.End();
        }

        public void Line2(Graphics graphics)
        {
            Draw draw = new Draw(bmp);
            draw.Begin(graphics);
            draw.color = Color.Red;
            Shape shape = new Shape(node);
            draw.DrawSharp(shape);
           // Vector2D v2 = new Vector2D(100, 20);
          //  Vector2D v1 = new Vector2D(50, 80);
           // draw.DrawLineByDDA(v1, v2);
            draw.End();
        }

        public void Rotation(Graphics graphics, double theta)
        {
            
            bmp = new Bitmap(1000, 1000);
            Draw draw = new Draw(bmp);
            
            for (int i = 0; i < node.GetVertexNum();++i )
            {
                node.Vertexs[i].V_Position = node.Vertexs[i].V_Position * Matrix4D.GetYAxisRotation(theta);
            }

            draw.Begin(graphics);

            draw.m_shape.m_node = node;
            Entity entity = new Entity();
            int size = node.GetVertexNum();
            Bitmap image = new Bitmap(@"D:\1359913743_8809.jpg");
            Texture newTexture = new Texture(image);

            String[] imagePath = new String[4] { @"D:\1359913743_8809.jpg", @"D:\1.jpg", @"D:\2.jpg", @"D:\3.jpg" }; 

            for (int index = 0; index < size; index +=3)
            {
                Shape newShape = new Shape();
                newShape.m_node.Add(node.Vertexs[index]);
                newShape.m_node.Add(node.Vertexs[(index + 1) % size]);
                newShape.m_node.Add(node.Vertexs[(index + 2)%size]);

                if (index/3 < imagePath.Length)
                {
                    newShape.SetTexture(new Texture(new Bitmap(imagePath[0])));
                }
                entity.AddShape(newShape);
            }
            draw.DrawEntity(entity);
            draw.End();
        }

        private void button_Triangle_Click(object sender, EventArgs e)
        {
            node.Vertexs.Clear();
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(0.0f, 50.0f, 0.0f), camera), Color.Red));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, 50.0f), camera), Color.Blue));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, -50.0f, 50.0f), camera), Color.Black));
            //node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, 35.0f, -69.0f), camera), Color.Gray));
            StateChange(1);
            this.Refresh();
        }

        protected void DrawTriangle(Graphics graphics)
        {
            Draw draw = new Draw(bmp);
            draw = new Draw(bmp);
            draw.Begin(graphics);
         //   draw.color = Color.Red;

            Vector3D v1 = new Vector3D(1, 2, 3);
            Vector3D v2 = new Vector3D(3, 2, 3);
            List<Vector3D> list_vector = new List<Vector3D>();
            list_vector.Add(v1);
            list_vector.Add(v2);

            draw.m_shape = shape;

            draw.DrawTriangle();
            draw.End();
        }

        protected void DrawTriangleCut(Graphics graphics)
        {
            Draw draw = new Draw(bmp);
            draw = new Draw(bmp);
            draw.Begin(graphics);
            //   draw.color = Color.Red;

            Vector3D v1 = new Vector3D(1, 2, 3);
            Vector3D v2 = new Vector3D(3, 2, 3);
            List<Vector3D> list_vector = new List<Vector3D>();
            list_vector.Add(v1);
            list_vector.Add(v2);

            draw.m_shape = shape;

            draw.DrawTriangle();
            draw.End();
        }

        private void button_Light_Click(object sender, EventArgs e)
        {
            PointLight light = new PointLight(0, 300, -100, Color.Gray, 10);
            sceneManager.scene.LightAdd(light);
            shape = new Shape(node);
            StateChange(2);
            this.Refresh();
        }

        private void button_Rotation_Click(object sender, EventArgs e)
        {
            StateChange(3);
            this.Refresh();
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (state != 3)
                return;
            switch (e.KeyData)
            {
                case Keys.W: camera.VRP.Y -= 5; break;
                case Keys.S: camera.VRP.Y += 5; break;
                case Keys.A: camera.VRP.X += 5; break;
                case Keys.D: camera.VRP.X -= 5; break;
                case Keys.R: camera.VRP.Z += 5; break;
                case Keys.F: camera.VRP.Z -= 5; break;
            }

            node.Vertexs.Clear();
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(0.0f, 50.0f, 0.0f), camera), Color.Red, 0, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, 50.0f), camera), Color.Blue, 1, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, -50.0f, 50.0f), camera), Color.Yellow, 0f, 1f));

            node.Add(new Vertex(Common.WorldTransform(new Vector3D(0.0f, 50.0f, 0.0f), camera), Color.Red, 0, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, -50.0f, 50.0f), camera), Color.Yellow, 1, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, -50.0f, -50.0f), camera), Color.Yellow, 0f, 1f));

            node.Add(new Vertex(Common.WorldTransform(new Vector3D(0.0f, 50.0f, 0.0f), camera), Color.Red, 0, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(50.0f, -50.0f, -50.0f), camera), Color.Yellow, 1, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, -50.0f), camera), Color.Green, 0f, 1f));

            node.Add(new Vertex(Common.WorldTransform(new Vector3D(0.0f, 50.0f, 0.0f), camera), Color.Red, 0, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, -50.0f), camera), Color.Green, 1, 0));
            node.Add(new Vertex(Common.WorldTransform(new Vector3D(-50.0f, -50.0f, 50.0f), camera), Color.Blue, 0f, 1f));

            StateChange(3);
            this.Refresh();
        }

        private void button_AddTexture_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(@"D:\1359913743_88091.jpg");

            //shape2.m_node.Add();
            Shape shape1 = new Shape();
            Shape shape2 = new Shape();
            Shape shape3 = new Shape();
            Shape shape4 = new Shape();

            camera.PRP = new Vector3D(0, 0, -200);

            sceneManager.scene.CameraAdd(camera);

            bmp = new Bitmap(1000, 1000);

            //draw = new Draw(bmp);
            camera.VRP = new Vector3D(0, 0, 0);
            camera.U = new Vector3D(1, 0, 0);
            camera.V = new Vector3D(0, 1, 0);
            camera.N = new Vector3D(0, 0, 1);



            shape1.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(-200, 0, 0), camera), Color.Blue, 1.0f, 1.0f));
            shape1.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(0, 0, 0), camera), Color.Blue, 0.0f, 1.0f));
            shape1.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(0, 200, 0), camera), Color.Blue, 0.0f, 0.0f));
            shape1.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(-200, 200, 0), camera), Color.Blue, 1.0f, 0.0f));

            shape2.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(0, 0, 0), camera), Color.Blue, 1.0f, 1.0f));
            shape2.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(200, 0, 30), camera), Color.Blue, 0.0f, 1.0f));
            shape2.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(200, 200, 30), camera), Color.Blue, 0.0f, 0.0f));
            shape2.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(0, 200, 0), camera), Color.Blue, 1.0f, 0.0f));

            //shape1.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(-100, 0, 0), camera), Color.Blue, 0.0f, 0.0f));
            //shape1.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(100, 0, 0), camera), Color.Blue, 1.0f, 0.0f));
            //shape1.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(-100, 50, 100), camera), Color.Blue, 0.0f, 1.0f));

            //shape2.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(100, 0, 0), camera), Color.Blue, 0.0f, 0.0f));
            //shape2.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(100, 50, 100), camera), Color.Blue, 1.0f, 1.0f));
            //shape2.m_node.Add(new Vertex(Common.WorldTransform(new Vector3D(-100, 50, 100), camera), Color.Blue, 0.0f, 1.0f));

            Texture texture = new Texture(image);
            shape1.SetTexture(texture);
            shape2.SetTexture(texture);
            entity.AddShape(shape1);
            entity.AddShape(shape2);
            //entity.AddShape(shape3);
            //entity.AddShape(shape4);

           // Color color =image.GetPixel(1,1);

            //Texture texture = new Texture(image);
            //sceneManager.scene.TextureAdd(texture);
            StateChange(4);
            this.Refresh();
        }

    }

}
