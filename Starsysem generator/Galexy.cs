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
    class Galexy
    {
        readonly Dictionary<GeometryModel3D, Star> moddles = new Dictionary<GeometryModel3D, Star>();
        readonly Model3DGroup model3DGroup = new Model3DGroup();

        GeometryModel3D highlighted = null;

        public Galexy(int GalexySize)
        {
            Random random = new Random();
            for (int i = 0; i < GalexySize; i++)
            {
                double r = 80 * random.NextDouble() + 20;
                double a = 2 * Math.PI * random.NextDouble() - Math.PI;
                Star star = new Star(new Point3D(r * Math.Sin(a), r * Math.Cos(a), 10 * random.NextDouble() - 5), 0.5 * random.NextDouble() + 0.5, $"{i}");
                moddles.Add(star.GetGeometry(), star);
                model3DGroup.Children.Add(star.GetGeometry());
            }
        }

        public Star.StarInfo StarClicked(GeometryModel3D model)
        {
            (model.Material as MaterialGroup).Children.Add(new DiffuseMaterial()
            {
                Brush = new SolidColorBrush()
                {
                    Color = Colors.Red
                }
            });
            return moddles[model].GetInfo();
        }

        public void HighlightStar(GeometryModel3D model)
        {
            Console.WriteLine("1");
            if (highlighted == null)
            {
                highlighted = model;
                (model.Material as MaterialGroup).Children.Add(new DiffuseMaterial(Brushes.Red));
            }
        }

        public void ReleaceHighlight()
        {
            Console.WriteLine("2");
            if (highlighted != null)
            {
                (highlighted.Material as MaterialGroup).Children.RemoveAt((highlighted.Material as MaterialGroup).Children.Count() - 1);
                highlighted = null;
            }
        }

        public Model3DGroup GetModel() => model3DGroup;
    }
}
