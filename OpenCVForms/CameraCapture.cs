using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace OpenCVForms
{
    public partial class CameraCapture : Form
    {
        private VideoCapture capture;
        private bool captureInProgress;

        private void ProcessFrame(object? sender, EventArgs arg)
        {
            //Image<Bgr, Byte> ImageFrame = capture.QueryFrame();
            var ImageFrame = capture.QueryFrame();

            CamImageBox.Image = ImageFrame;

        }
        public CameraCapture()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (capture is null)
            {
                try
                {
                    capture = new VideoCapture();
                }

                catch (NullReferenceException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                if (captureInProgress)
                {
                    // if camera is getting frames then stop the capture and
                    // set button Text "Start" for resuming capture
                    btnStart.Text = "Start!";
                    Application.Idle -= ProcessFrame;
                }
                else
                {
                    // if camera is not getting frames then start the capture and set button
                    // to "Stop"
                    btnStart.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }
                captureInProgress = !captureInProgress;
            }
        }

        private void ReleaseDate()
        {
            if (capture is not null)
            {
                capture.Dispose();
            }
        }
    }
}