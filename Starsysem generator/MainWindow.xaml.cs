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
            Model3DGroup model3DGroup = new Model3DGroup();
            MeshGeometry3D meshGeometry3D = new MeshGeometry3D();
            meshGeometry3D.Positions = new Point3DCollection()
            {
                new Point3D(-1, -1, -1),
                new Point3D(1, 1, 1)
            };
        }
    }
}
