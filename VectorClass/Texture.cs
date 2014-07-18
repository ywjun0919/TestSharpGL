using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TestSharpGL.VectorClass
{
    class Texture
    {
        Bitmap m_Bitmap = null;

        public Bitmap bitMap 
        {
            get { return m_Bitmap; }
            set { m_Bitmap = value; }
        }
        public Texture()
        {
        }

        /// <summary>
        /// 判断Texture是否可用
        /// </summary>
        /// <returns>返回值为true，表示纹理不可用，返回值为false，表示纹理可用</returns>
        public Boolean ISNULL()
        {
            Boolean res = false;
            if (null == m_Bitmap)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 使用图片作为纹理
        /// </summary>
        /// <param name="bitmap">图片对象</param>
        public Texture(Bitmap bitmap)
        {
            m_Bitmap = bitmap;
        }

        /// <summary>
        /// 根据纹理坐标，获取对应纹理坐标所在的像素值
        /// </summary>
        /// <param name="s">纹理的横坐标</param>
        /// <param name="t">纹理的纵坐标</param>
        /// <returns>获取对应纹理坐标所在的像素值</returns>
        public Color GetTextureColor(float s, float t)
        {
            return Common.GetTextureColor(this.m_Bitmap, s, t);
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 


    }
}
