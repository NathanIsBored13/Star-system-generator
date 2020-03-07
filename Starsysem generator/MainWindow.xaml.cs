using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Starsysem_generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer passiveRotate;
        Timer MouseMovedCheck;
        AxisAngleRotation3D[] axisAngleRotation3Ds = new AxisAngleRotation3D[]
        {
            new AxisAngleRotation3D()
            {
                Axis = new Vector3D(-1, 0, 0)
            },
            new AxisAngleRotation3D()
            {
                Axis = new Vector3D(0, 0, -1)
            }
        };
        Point buffer;
        public MainWindow()
        {
            InitializeComponent();

            Canvas.MouseDown += Canvas_MouseDown;
            Canvas.MouseWheel += Canvas_MouseWheel;

            passiveRotate = new Timer(50);
            passiveRotate.Elapsed += PassiveRotate_Elapsed;

            MouseMovedCheck = new Timer(10);
            MouseMovedCheck.Elapsed += MouseMovedoveCheck_Elapsed;

            Transform3DGroup transform3DGroup = new Transform3DGroup();
            transform3DGroup.Children.Add(new RotateTransform3D()
            {
                Rotation = axisAngleRotation3Ds[0]
            });
            transform3DGroup.Children.Add(new RotateTransform3D()
            {
                Rotation = axisAngleRotation3Ds[1]
            });

            viewport.Camera = new PerspectiveCamera()
            {
                UpDirection = new Vector3D(0, 0, 1),
                LookDirection = new Vector3D(0, -20, -50),
                Position = new Point3D(0, 20, 50),
                FieldOfView = 100,
                Transform = transform3DGroup
            };

            Model3DGroup model3DGroup = new Model3DGroup();

            model3DGroup.Children.Add(new DirectionalLight()
            {
                Direction = new Vector3D(0, 0, -1),
                Color = Colors.White,
            });

            model3DGroup.Children.Add(new DirectionalLight()
            {
                Direction = new Vector3D(0, 0, 1),
                Color = Colors.White,
            });

            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                double r = 80 * random.NextDouble() + 20;
                double a = 2 * Math.PI * random.NextDouble() - Math.PI;
                Star star = new Star(new Point3D(r * Math.Sin(a), r * Math.Cos(a), 10 * random.NextDouble() - 5), 2 * random.NextDouble() - 1);
                model3DGroup.Children.Add(star.GetGeometry());
            }

            viewport.Children.Add(new ModelVisual3D()
            {
                Content = model3DGroup,
            });
            passiveRotate.Start();
        }

        private void PassiveRotate_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => axisAngleRotation3Ds[1].Angle += 0.05);
        }

        private void MouseMovedoveCheck_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    PerspectiveCamera perspectiveCamera = (PerspectiveCamera)viewport.Camera;
                    double r = Math.Sqrt(Math.Pow(perspectiveCamera.Position.X, 2) + Math.Pow(perspectiveCamera.Position.Y, 2) + Math.Pow(perspectiveCamera.Position.Z, 2));
                    axisAngleRotation3Ds[1].Angle += Mouse.GetPosition(viewport).X - buffer.X;
                    axisAngleRotation3Ds[0].Angle -= Mouse.GetPosition(viewport).Y - buffer.Y;
                    buffer = Mouse.GetPosition(viewport);
                }
                else
                {
                    MouseMovedCheck.Stop();
                    passiveRotate.Start();
                }
            });
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                passiveRotate.Stop();
                buffer = e.GetPosition(viewport);
                MouseMovedCheck.Start();
            });
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                PerspectiveCamera perspectiveCamera = (PerspectiveCamera)viewport.Camera;
                double r = Math.Sqrt(Math.Pow(perspectiveCamera.Position.X, 2) + Math.Pow(perspectiveCamera.Position.Y, 2) + Math.Pow(perspectiveCamera.Position.Z, 2));
                double ratio = r / (r + e.Delta / 100);
                ((PerspectiveCamera)viewport.Camera).Position = new Point3D(ratio * perspectiveCamera.Position.X, ratio * perspectiveCamera.Position.Y, ratio * perspectiveCamera.Position.Z);
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => passiveRotate.Stop());
        }
    }
}
