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
        private readonly GeometryModel3D geometryModel3D;
        private const int RESOLUTION = 10;

        private readonly double radius;
        private readonly Point3D center;
        private readonly String name;

        public struct StarInfo
        {
            public double radius;
            public Point3D center;
            public string name;
        }

        public Star(Point3D center, double radius, string name)
        {
            this.radius = radius;
            this.center = center;
            this.name = name;

            MeshGeometry3D meshGeometry = new MeshGeometry3D();
            double phi0, theta0;
            double dphi = Math.PI / RESOLUTION;
            double dtheta = 2 * Math.PI / (RESOLUTION * 2);

            phi0 = 0;
            for (int i = 0; i < RESOLUTION * 2; i++)
            {
                double phi1 = phi0 + dphi;

                theta0 = 0;
                Point3D pt00 = new Point3D(Math.Sin(phi0) * Math.Cos(theta0), Math.Cos(phi0), Math.Sin(phi0) * Math.Sin(theta0));
                Point3D pt10 = new Point3D(Math.Sin(phi1) * Math.Cos(theta0), Math.Cos(phi1), Math.Sin(phi1) * Math.Sin(theta0));
                for (int j = 0; j < RESOLUTION * 2; j++)
                {
                    double theta1 = theta0 + dtheta;
                    Point3D pt01 = new Point3D(Math.Sin(phi0) * Math.Cos(theta1), Math.Cos(phi0), Math.Sin(phi0) * Math.Sin(theta1));
                    Point3D pt11 = new Point3D(Math.Sin(phi1) * Math.Cos(theta1), Math.Cos(phi1), Math.Sin(phi1) * Math.Sin(theta1));

                    AddTriangle(meshGeometry, pt00, pt11, pt10);
                    AddTriangle(meshGeometry, pt00, pt01, pt11);

                    theta0 = theta1;
                    pt00 = pt01;
                    pt10 = pt11;
                }
                phi0 = phi1;
            }

            geometryModel3D = new GeometryModel3D()
            {
                Geometry = meshGeometry,
                Material = new MaterialGroup()
                {
                    Children = new MaterialCollection()
                    {
                        new DiffuseMaterial()
                        {
                            Brush = new LinearGradientBrush()
                            {
                                StartPoint = new Point(0, 0.5),
                                EndPoint = new Point(1, 0.5),
                                GradientStops = new GradientStopCollection()
                                {
                                    new GradientStop(Color.FromArgb(255, 255, 0, 0), 0),
                                    new GradientStop(Color.FromArgb(255, 255, 165, 0), 0.2),
                                    new GradientStop(Color.FromArgb(255, 255, 255, 0), 0.4),
                                    new GradientStop(Color.FromArgb(255, 0, 128, 0), 0.6),
                                    new GradientStop(Color.FromArgb(255, 0, 0, 255), 0.8),
                                    new GradientStop(Color.FromArgb(255, 128, 0, 128), 1)
                                }
                            }
                        }
                    }
                }
            };
        }


        private void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            int index1 = mesh.Positions.Count;
            mesh.Positions.Add(Transform_Point(point1));
            mesh.Positions.Add(Transform_Point(point2));
            mesh.Positions.Add(Transform_Point(point3));

            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1);

            mesh.TextureCoordinates.Add(new Point(Math.Acos(point1.Z) / Math.PI, 0.5));
            mesh.TextureCoordinates.Add(new Point(Math.Acos(point2.Z) / Math.PI, 0.5));
            mesh.TextureCoordinates.Add(new Point(Math.Acos(point3.Z) / Math.PI, 0.5));
        }

        private Point3D Transform_Point(Point3D point) => new Point3D()
        {
            X = point.X * radius + center.X,
            Y = point.Y * radius + center.Y,
            Z = point.Z * radius + center.Z
        };

        public GeometryModel3D GetGeometry() => geometryModel3D;

        public StarInfo GetInfo() => new StarInfo()
        {
            radius = radius,
            center = center,
            name = name
        };
    }
}
