using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MainWindow()
        {
            InitializeComponent();

            viewport.Camera = new PerspectiveCamera()
            {
                UpDirection = new Vector3D(0, 0, 1),
                LookDirection = new Vector3D(1, -1, -1),
                Position = new Point3D(-4, 4, 4)
            };

            Model3DGroup model3DGroup = new Model3DGroup();
            model3DGroup.Children.Add(new DirectionalLight()
            {
                Direction = new Vector3D(1, -1, -1),
                Color = Colors.White,
            });

            model3DGroup.Children.Add(new GeometryModel3D()
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
                    },

                    Normals = new Vector3DCollection()
                    {
                        new Vector3D()
                    },
                },

                Material = new DiffuseMaterial()
                {
                    Brush = Brushes.Red
                },

                Transform = new RotateTransform3D()
                {
                    Rotation = new AxisAngleRotation3D()
                    {
                        Axis = new Vector3D(0, 0, 1),
                    },
                }
            });

            viewport.Children.Add(new ModelVisual3D()
            {
                Content = model3DGroup,
            });
        }
    }
}
