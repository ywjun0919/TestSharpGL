using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSharpGL.VectorClass
{
    class Entity
    {
        //由多个多边形组成一个实体对象
        List<Shape> m_Entity = new List<Shape>();

        public List<Shape> GetEntity()
        {
            return m_Entity;
        }

        public void AddShape(Shape shape)
        {
            m_Entity.Add(shape);
        }
    }
}
