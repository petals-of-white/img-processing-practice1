using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace OpenCVTest
{
    internal static class MatBasedCV
    {
        public static void MatrixTask()
        {
            // windows
            var originalWindow = "Original";
            var transformedWindow = "Transformed";

            // inputs
            int width = 5, height = 5;
            Size initialSize = new Size(width, height);

            // scaled size
            int xScale = 50, yScale = 50;
            Size scaledSize = initialSize with
            {
                Height = initialSize.Height * yScale,
                Width = initialSize.Width * xScale
            };

            // data matrix
            byte [,] data =
            {
                {1,     2,  3,  4,  5},
                {6,     7,  8,  9,  10},
                {11,    12, 13, 14, 15},
                {16,    17, 18, 19, 20},
                {21,    22, 23, 24, 25}
            };

            // work matrices
            Matrix<byte> startMatrix = new Matrix<byte>(data);
            

            Mat colormappedMatrix = new();
            Mat scaledMatrix = new();
            
            // colormap
            IInputArray colorMapDefault = GenerateColorMap();
            IInputArray colorMapScaled = GenerateColorMap(25);

            CvInvoke.ApplyColorMap(startMatrix, colormappedMatrix, colorMapScaled);

            CvInvoke.Resize(colormappedMatrix, scaledMatrix, scaledSize, interpolation: Inter.Area);

            ShowColormap(colorMapDefault, "Default Colormap");
            ShowColormap(colorMapScaled, "Scaled ColorMap");
            //CvInvoke.Imshow("Default Color Map", colorMapDefault);
            //CvInvoke.Imshow("Scaled Color Map", colorMapScaled);

            CvInvoke.Imshow(originalWindow, startMatrix);

            CvInvoke.Imshow(transformedWindow, scaledMatrix);

            CvInvoke.WaitKey();
            CvInvoke.DestroyAllWindows();
        }

        public static IInputArray GenerateColorMap(byte? scaleToMaxValue = null)
        {
            const int numberOfColours = 15;

            Matrix<byte> colorMapOutput = new(256, 1, 3);

            var range = 255 / numberOfColours;

            var colors = new Dictionary<int, Bgr>(numberOfColours)
            {
                {0, new Bgr(Color.Blue)},
                {1, new Bgr(Color.Purple)},
                {2, new Bgr(Color.Green)},
                {3, new Bgr(Color.Crimson)},
                {4, new Bgr(Color.LimeGreen)},
                {5, new Bgr(Color.DimGray)},
                {6, new Bgr(Color.Black)},
                {7, new Bgr(Color.BurlyWood)},
                {8, new Bgr(Color.Thistle)},
                {9, new Bgr(Color.RebeccaPurple)},
                {10, new Bgr(Color.IndianRed)},
                {11, new Bgr(Color.DarkGreen)},
                {12, new Bgr(Color.AliceBlue)},
                {13, new Bgr(Color.DarkMagenta)},
                {14, new Bgr(Color.Silver)},

            };

            int colorGroup;

            for (var c = 0; c < colorMapOutput.Rows; c++)
            {
                colorGroup = c / range;

                var colorForGroup = colors [colorGroup % numberOfColours].MCvScalar;

                colorMapOutput.GetRow(c).SetValue(colorForGroup);
            }

            if (scaleToMaxValue is not null)
            {
                Matrix<byte> scaledColormap = new(256, 1, 3);

                for (var c = 0; c <= scaleToMaxValue; c++)
                {
                    int pixelIndex = 255 / (scaleToMaxValue.Value + 1) * (c+1);

                    colorMapOutput.GetRow(pixelIndex).CopyTo(scaledColormap.GetRow(c));

                    //scaledColormap.GetRow(c).Bytes = colorMapOutput.GetRow(c).Bytes;
                }
                colorMapOutput = scaledColormap;
            }

            return colorMapOutput;
        }


        static void ShowColormap(IInputArray colormap, string windowName)
        {
            Mat resizedColormap = new();

            CvInvoke.Resize(colormap, resizedColormap, new Size { }, 50, 3, Inter.Area);

            CvInvoke.Imshow(windowName, resizedColormap);
        }

        public static void ColorMapNew()
        {
            var window1 = "mat1";
            var window2 = "mat2";
            var mat1 = new Mat(256, 1, DepthType.Cv8U, 3);

            var matrix = new Matrix<byte>(500, 400, 3);
            matrix.Mat.SetTo(new MCvScalar(255, 0, 0));
            //var kekos

            //var mat2= new Mat();
            CvInvoke.Randu(mat1, new MCvScalar(0, 0, 0), new MCvScalar(255, 255, 255));

            CvInvoke.Imshow(window1, matrix);
            //CvInvoke.Imshow(window2, mat2);
            CvInvoke.WaitKey();

            CvInvoke.DestroyAllWindows();
        }

        public static void ImgCreation()
        {
            var window = "snake";

            Mat snake = new(fileName: "img/snake.png", loadType: ImreadModes.Color);

            Mat snakeColorMapped = new();

            var map = new Mat(256, 1, DepthType.Cv8U, channels: 3);

            byte [,,] data = new byte [2, 3, 4]
            {
                {
                    { 1, 2, 3, 4 }, { 4, 3, 2, 1 }, { 1, 2, 3, 4 }
                },
                {
                    { 4, 3, 2, 1 }, { 1, 1, 1, 1 }, { 2, 2, 2, 2 }
                }
            };

            var matMap = new Mat(256, 1, type: DepthType.Cv8U, channels: 3);

            var row10 = matMap.Row(10);
            row10.SetTo(new ScalarArray(1));
            Console.WriteLine($"Mat is {matMap}");
            //var imgFromMat = matMap.ToImage<Bgr, Byte>();
            //for (int i = 0; i < imgFromMat.Rows; i++)
            //{
            //    for (int j = 0; j < imgFromMat.Cols; j++)
            //    {
            //        for (int channel = 0; channel < imgFromMat.NumberOfChannels; channel++)
            //        {
            //            imgFromMat.Data [i, j, channel] = (byte)i;
            //        }
            //    }
            //}

            //for (int i = 0; i < matMap; i++)
            var matrixMap = new Matrix<byte>(256, 1, 3);

            for (int i = 0; i < matrixMap.Rows; i++)
            {
                for (int j = 0; j < matrixMap.Cols; j++)
                {
                    //for (int channel = 0; channel < matrixMap.NumberOfChannels; channel++)
                    //{
                    matrixMap.Data [i, j] = 2;
                    //}
                }
            }

            var imageMap = new Image<Gray, byte>(new Size(1, 256));

            CvInvoke.ApplyColorMap(snake, snakeColorMapped, matMap);

            CvInvoke.Imshow(window, snakeColorMapped);
            CvInvoke.WaitKey();

            CvInvoke.DestroyWindow(window);
        }
    }
}
