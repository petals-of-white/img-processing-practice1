using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace OpenCVTest
{

    internal static class ImageBasedCV
    {
        public static void HelloWorld()
        {
            var win1 = "Test Window"; //The name of the window

            CvInvoke.NamedWindow(win1); //Create the window using the specific name

            Mat img = new(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
            img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

            //Draw "Hello, world." on the image using the specific font
            CvInvoke.PutText(
               img,
               "Hello, world",
               new System.Drawing.Point(10, 80),
               FontFace.HersheyComplex,
               1.0,
               new Bgr(0, 255, 0).MCvScalar);

            CvInvoke.Imshow(win1, img); //Show the image
            CvInvoke.WaitKey(0);  //Wait for the key pressing event
            CvInvoke.DestroyWindow(win1); //Destroy the window if key is pressed

            ////Create a 3 channel image of 400x200
            //using (Mat img = new Mat(200, 400, DepthType.Cv8U, 3))
            //{
            //    img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

            // //Draw "Hello, world." on the image using the specific font CvInvoke.PutText( img,
            // "Hello, world", new System.Drawing.Point(10, 80), FontFace.HersheyComplex, 1.0, new
            // Bgr(0, 255, 0).MCvScalar);

            // //Show the image using ImageViewer from Emgu.CV.UI ImageViewer.Show(img, "Test Window");
        }

        // Image<Color, Depth> class is automatically garbage-collected!!
        public static void CreateImage()
        {
            // Default img 480 x 320
            Image<Gray, Byte> img1 = new(width: 480, height: 320);

            // Img with background
            Image<Bgr, Byte> imgWithBackground = new(width: 480, height: 320, new Bgr(255, 0, 0));

            // Reading From file (Snake mgs3)
            Image<Bgr, Byte> imgFromFile = new("img/snake.png");

            // From Bitmap
            //Bitmap bmp = new("img/snake.png");
            //Image<Bgr, Byte> imgFromBitmap = new(bmp);
        }

        public static void GettingSettingPixels()
        {
            Image<Bgr, Byte> img = new(width: 480, height: 320);

            // The safe (slow way)

            var y = 50;
            var x = 100;

            // Obtain a pixel
            Bgr color = img [y, x];

            // Set a pixel
            (y, x) = (x, y);
            img [y, x] = color;

            // The fast way!! Use data property
            var something = img.Data [1, 3, 4];
        }

        // Methods
        public static void Methods()
        {
            Image<Bgr, Byte> img = new(width: 320, height: 500);

            //reverted
            var revertedImg = img.Not();

            //Drawing...
            // .Draw()

            //conversion to gray single
            var graySingle = img.Convert<Gray, Single>();
        }
    }
}
