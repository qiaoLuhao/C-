using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHKJ
{
    class PictureHelp
    {
        private float mul = 1.0f;
        private Bitmap m_bmp;
        private PointF ptCenter;
        private float origin_x;
        private float origin_y;
        private float drawWidth;
        private float drawHeight;
        private float tmp_origin_x;
        private float tmp_origin_y;
        private float sfScale = 1.0F;
        private Point ptMouseDown;
        private string imagePath = " ";
        private bool isEnabled = false;

        //右上角缩小图方法
        public void SmallPicWidth(Bitmap objPic, double intWidth)
        {
            Bitmap objNewPic;
            try
            {
                mul = (float)(intWidth / Convert.ToDouble(objPic.Width));

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
