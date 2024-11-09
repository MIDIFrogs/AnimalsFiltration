using System;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Avalonia.Microcharts
{
    public class MicrochartControl : Control
    {
        private readonly CustomDrawingOperation custom = new CustomDrawingOperation();

        private Chart chart;

        public Chart Chart
        {
            get => chart;
            set
            {
                bool flag = false;
                if (value != chart)
                {
                    flag = true;
                }
                SetAndRaise(ChartProperty, ref chart, value);
                if (flag) 
                    InvalidateVisual();
            }
        }

        public static readonly DirectProperty<MicrochartControl, Chart> ChartProperty =
            AvaloniaProperty.RegisterDirect<MicrochartControl, Chart>("Chart", c => c.Chart, (c, v) => c.Chart = v);

        protected override Size MeasureOverride(Size availableSize)
            => availableSize;

        public override void Render(DrawingContext context)
        {
            this.custom.Bounds = this.Bounds;
            this.custom.Chart = this.chart;

            context.Custom(custom);
        }

        private class CustomDrawingOperation : ICustomDrawOperation
        {
            public Chart Chart { get; set; }
            public void Dispose() { }
            public bool HitTest(Point p) => false;
            public bool Equals(ICustomDrawOperation other) => this.Equals(other);

            public Rect Bounds { get; set; }

            public void Render(ImmediateDrawingContext context)
            {
                var bitmap = new SKBitmap((int)Bounds.Width, (int)Bounds.Height, false);
                var canvas = new SKCanvas(bitmap);
                canvas.Save();

                Chart.Draw(canvas, (int)Bounds.Width, (int)Bounds.Height);
                var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
                if (leaseFeature != null)
                {
                    using var lease = leaseFeature.Lease();
                    lease.SkCanvas.DrawBitmap(bitmap, (int)Bounds.X, (int)Bounds.Y);
                }
            }
        }
    }
}