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
        readonly Galexy galexy = new Galexy(1000);

        Point mouseLast;
        bool drag;

        readonly Timer passiveRotate;

        readonly AxisAngleRotation3D[] axisAngleRotation3Ds = new AxisAngleRotation3D[]
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

        GeometryModel3D highlighted;

        public MainWindow()
        {
            InitializeComponent();

            Canvas.MouseDown += Canvas_MouseDown;
            Canvas.MouseMove += Canvas_MouseMove;
            Canvas.MouseUp += Canvas_MouseUp;
            Canvas.MouseWheel += Canvas_MouseWheel;

            passiveRotate = new Timer(50);
            passiveRotate.Elapsed += PassiveRotate_Elapsed;

            viewport.Camera = new PerspectiveCamera()
            {
                UpDirection = new Vector3D(0, 0, 1),
                LookDirection = new Vector3D(0, -20, -50),
                Position = new Point3D(0, 20, 50),
                FieldOfView = 100,
                Transform = new Transform3DGroup()
                {
                    Children = new Transform3DCollection()
                    {
                        new RotateTransform3D()
                        {
                            Rotation = axisAngleRotation3Ds[0]
                        },

                        new RotateTransform3D()
                        {
                            Rotation = axisAngleRotation3Ds[1]
                        }
                    }
                }
            };

            viewport.Children.Add(new ModelVisual3D()
            {
                Content = new Model3DGroup()
                {
                    Children = new Model3DCollection()
                    {            
                        new AmbientLight()
                        {
                            Color = Colors.White
                        },
                        new PointLight()
                        {
                            Position = new Point3D(0, 0, 0),
                            Color = Colors.Red,
                        }
                    }
                }
            });

            viewport.Children.Add(new ModelVisual3D()
            {
                Content = galexy.GetModel()
            });
            passiveRotate.Start();
        }

        private void PassiveRotate_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                axisAngleRotation3Ds[1].Angle += 0.05;
                if (VisualTreeHelper.HitTest(viewport, Mouse.GetPosition(viewport)) is RayMeshGeometry3DHitTestResult result)
                {
                    GeometryModel3D model = (GeometryModel3D)result.ModelHit;
                    if (highlighted != model)
                    {
                        if (highlighted != null)
                        {
                            (highlighted.Material as MaterialGroup).Children.RemoveAt((highlighted.Material as MaterialGroup).Children.Count() - 1);
                        }
                        (model.Material as MaterialGroup).Children.Add(new DiffuseMaterial(Brushes.Red));
                        highlighted = model;
                    }
                }
                else
                {
                    if (highlighted != null)
                    {
                        (highlighted.Material as MaterialGroup).Children.RemoveAt((highlighted.Material as MaterialGroup).Children.Count() - 1);
                        highlighted = null;
                    }
                }
            });
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            passiveRotate.Stop();
            mouseLast = e.GetPosition(viewport);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && (drag || Math.Pow(e.GetPosition(viewport).X - mouseLast.X, 2) + Math.Pow(e.GetPosition(viewport).Y - mouseLast.Y, 2) > 100))
            {
                if (!drag)
                {
                    drag = true;
                }
                else
                {
                    axisAngleRotation3Ds[1].Angle += e.GetPosition(viewport).X - mouseLast.X;
                    axisAngleRotation3Ds[0].Angle -= e.GetPosition(viewport).Y - mouseLast.Y;
                }
                mouseLast = e.GetPosition(viewport);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!drag && VisualTreeHelper.HitTest(viewport, e.GetPosition(viewport)) is RayMeshGeometry3DHitTestResult result)
            {
                Star.StarInfo starinfo = galexy.StarClicked(result.ModelHit as GeometryModel3D);
                starinfoLable.Content = $"ID: {starinfo.name}\nSize: {starinfo.radius}\nPosition: [{starinfo.center.X}, {starinfo.center.Y}]";
            }
            drag = false;
            passiveRotate.Start();
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            PerspectiveCamera perspectiveCamera = (PerspectiveCamera)viewport.Camera;
            double r = Math.Sqrt(Math.Pow(perspectiveCamera.Position.X, 2) + Math.Pow(perspectiveCamera.Position.Y, 2) + Math.Pow(perspectiveCamera.Position.Z, 2));
            double ratio = r / (r + e.Delta / 100);
            ((PerspectiveCamera)viewport.Camera).Position = new Point3D(ratio * perspectiveCamera.Position.X, ratio * perspectiveCamera.Position.Y, ratio * perspectiveCamera.Position.Z);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            passiveRotate.Stop();
        }
    }
}
