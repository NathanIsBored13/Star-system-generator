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
            
            Canvas.Camera = new PerspectiveCamera()
            {
                Position = new Point3D(0, 0, 2),
                LookDirection = new Vector3D(0, 0, -1),
                FieldOfView = 60
            };

            ModelVisual3D modelVisual3D = new ModelVisual3D();
            Model3DGroup model3DGroup = new Model3DGroup();
            GeometryModel3D geometry3D = new GeometryModel3D();
            MeshGeometry3D meshGeometry3D = new MeshGeometry3D();

            model3DGroup.Children.Add(new PointLight()
            {
                Position = new Point3D(0, 0, 0),
                Color = Colors.White,
            });

            meshGeometry3D.Normals = new Vector3DCollection()
            {
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1)
            };

            meshGeometry3D.Positions = new Point3DCollection()
            {
                new Point3D(-1, -1, 0),
                new Point3D(-1, 1, 0),
                new Point3D(1, -1, 0),
                new Point3D(1, 1, 0)
            };

            geometry3D.Geometry = meshGeometry3D;
            model3DGroup.Children.Add(geometry3D);
            modelVisual3D.Content = model3DGroup;
            Canvas.Children.Add(modelVisual3D);
        }
    }
}
