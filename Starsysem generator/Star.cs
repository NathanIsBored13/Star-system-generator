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
using MS.Internal.Media3D;
using System.Windows.Media.Composition;

namespace Starsysem_generator
{
    class Star
    {
        private GeometryModel3D geometryModel3D;
        const int RESOLUTION = 10;
        public Star(Point3D center, double radius)
        {
            geometryModel3D = new GeometryModel3D();

            MeshGeometry3D meshGeometry = new MeshGeometry3D();
            double phi0, theta0;
            double dphi = Math.PI / RESOLUTION;
            double dtheta = 2 * Math.PI / (RESOLUTION * 2);

            phi0 = 0;
            double y0 = radius * Math.Cos(phi0);
            double r0 = radius * Math.Sin(phi0);
            for (int i = 0; i < RESOLUTION; i++)
            {
                double phi1 = phi0 + dphi;
                double y1 = radius * Math.Cos(phi1);
                double r1 = radius * Math.Sin(phi1);

                theta0 = 0;
                Point3D pt00 = new Point3D(center.X + r0 * Math.Cos(theta0), center.Y + y0, center.Z + r0 * Math.Sin(theta0));
                Point3D pt10 = new Point3D(center.X + r1 * Math.Cos(theta0), center.Y + y1, center.Z + r1 * Math.Sin(theta0));
                for (int j = 0; j < RESOLUTION * 2; j++)
                {
                    double theta1 = theta0 + dtheta;
                    Point3D pt01 = new Point3D(center.X + r0 * Math.Cos(theta1), center.Y + y0, center.Z + r0 * Math.Sin(theta1));
                    Point3D pt11 = new Point3D(center.X + r1 * Math.Cos(theta1), center.Y + y1, center.Z + r1 * Math.Sin(theta1));

                    AddTriangle(meshGeometry, pt00, pt11, pt10);
                    AddTriangle(meshGeometry, pt00, pt01, pt11);

                    theta0 = theta1;
                    pt00 = pt01;
                    pt10 = pt11;
                }
                phi0 = phi1;
                y0 = y1;
                r0 = r1;
            }

            meshGeometry.TextureCoordinates = new PointCollection
            {
                new Point(0, 1)


            };

            geometryModel3D.Geometry = meshGeometry;

            geometryModel3D.Material = new DiffuseMaterial()
            {
                Brush = Brushes.White
            };
            
            /*geometryModel3D.Material = new DiffuseMaterial()
            {
                Brush = new RadialGradientBrush
                {
                    Center = meshGeometry.TextureCoordinates[0],
                    GradientStops = new GradientStopCollection()
                    {
                        new GradientStop(Colors.Blue, 0),
                        new GradientStop(Colors.Red, 1)
                    },
                    ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation
                }
            };*/
        }

        private void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            // Create the points.
            int index1 = mesh.Positions.Count;
            mesh.Positions.Add(point1);
            mesh.Positions.Add(point2);
            mesh.Positions.Add(point3);

            // Create the triangle.
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1);
        }

        public GeometryModel3D GetGeometry() => geometryModel3D;
    }
}
