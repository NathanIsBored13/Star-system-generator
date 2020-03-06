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
        Model3DGroup model3DGroup;
        DependencyProperty dependencyProperty = DependencyProperty.Register("Angle", typeof(double), typeof(Model3DGroup));
        private double angle
        {
            get { return (double)GetValue(dependencyProperty); }
            set { SetValue(dependencyProperty, value); }
        }
        public MainWindow()
        {
            InitializeComponent();

            viewport.Camera = new PerspectiveCamera()
            {
                UpDirection = new Vector3D(0, 0, 1),
                LookDirection = new Vector3D(0, -5, -5),
                Position = new Point3D(0, 5, 5),
                FieldOfView = 60
            };

            model3DGroup = new Model3DGroup();
            model3DGroup.Children.Add(new DirectionalLight()
            {
                Direction = new Vector3D(1, -1, -1),
                Color = Colors.White,
            });

            //Star star = new Star(new Point3D(1, 0, 0), 0.5, 50, 50);
            //model3DGroup.Children.Add(star.GetGeometry());

            //star = new Star(new Point3D(-1, 0, 0), 0.5, 50, 50);
            //model3DGroup.Children.Add(star.GetGeometry());

            RotateTransform3D rotateTransform3D = new RotateTransform3D();

            GeometryModel3D geometryModel3D = new GeometryModel3D()
            {
                Geometry = new MeshGeometry3D()
                {
                    Positions = new Point3DCollection()
                    {
                        new Point3D(-1, -1, -1),
                        new Point3D(1, -1, -1),
                        new Point3D(1, 1, -1),
                        new Point3D(-1, 1, -1),
                        new Point3D(-1, -1, 1),
                        new Point3D(1, -1, 1),
                        new Point3D(1, 1, 1),
                        new Point3D(-1, 1, 1)
                    },

                    TriangleIndices = new Int32Collection()
                    {
                        0, 1, 3, 1, 2, 3,
                        0, 4, 3, 4, 7, 3,
                        4, 6, 7, 4, 5, 6,
                        0, 4, 1, 1, 4, 5,
                        1, 2, 6, 6, 5, 1,
                        2, 3, 7, 7, 6, 2
                    }
                },

                Material = new DiffuseMaterial()
                {
                    Brush = Brushes.Red
                },

                Transform = rotateTransform3D
            };

            Rotation3DAnimation rotation3DAnimation = new Rotation3DAnimation()
            {
                Duration = TimeSpan.FromSeconds(5),
                RepeatBehavior = RepeatBehavior.Forever,
                To = new AxisAngleRotation3D()
                {
                    Axis = new Vector3D(0, 0, 1),
                    Angle = 0
                },
                From = new AxisAngleRotation3D()
                {
                    Axis = new Vector3D(0, 0, 1),
                    Angle = 180
                },
                
            };

            Storyboard.SetTarget(rotation3DAnimation, geometryModel3D);
            Storyboard.SetTargetProperty(rotation3DAnimation, new PropertyPath(RotateTransform3D.RotationProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(rotation3DAnimation);

            storyboard.Begin();

            model3DGroup.Children.Add(geometryModel3D);

            viewport.Children.Add(new ModelVisual3D()
            {
                Content = model3DGroup,
            });

            model3DGroup.Children.Last().Transform = new RotateTransform3D()
            {
                Rotation = new AxisAngleRotation3D()
                {
                    Axis = new Vector3D(0, 0, 1),
                    Angle = 60
                },
            };
        }
    }
}
