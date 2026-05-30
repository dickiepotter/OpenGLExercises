using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises.Sketches;

// The original Cardioid / Sine / Petal / Polar sketches were small GL4Java
// programs that plotted a parametric curve as a GL_LINE_STRIP. Those four .java
// files are corrupted to binary on disk, so the classes below are faithful
// reconstructions from the curves' standard equations, following the same
// "ortho view + line strip + faint axes" style as the surviving sketches.

/// <summary>Cardioid: r = a(1 - cos θ).</summary>
public sealed class SketchCardioid : ExerciseWindow
{
    public SketchCardioid() : base("Cardioid  r = a(1 - cos theta)") { }

    protected override void InitGL() => Curve2D.Ortho(-2.5, 2.5, -2.5, 2.5);

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        Curve2D.Axes(2.5);
        GL.Color3(1f, 0.3f, 0.5f);
        GL.LineWidth(2);
        GL.Begin(PrimitiveType.LineLoop);
        for (double t = 0; t <= 2 * Math.PI; t += Math.PI / 360)
        {
            double r = 1.0 * (1 - Math.Cos(t));
            GL.Vertex2(r * Math.Cos(t), r * Math.Sin(t));
        }
        GL.End();
    }
}

/// <summary>Sine wave: y = sin(x).</summary>
public sealed class SketchSine : ExerciseWindow
{
    public SketchSine() : base("Sine  y = sin(x)") { }

    protected override void InitGL() => Curve2D.Ortho(-7, 7, -2, 2);

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        Curve2D.Axes(7);
        GL.Color3(0.4f, 1f, 0.6f);
        GL.LineWidth(2);
        GL.Begin(PrimitiveType.LineStrip);
        for (double x = -2 * Math.PI; x <= 2 * Math.PI; x += Math.PI / 360)
            GL.Vertex2(x, Math.Sin(x));
        GL.End();
    }
}

/// <summary>Rose / petal curve: r = cos(kθ) (k = 4 gives eight petals).</summary>
public sealed class SketchPetal : ExerciseWindow
{
    public SketchPetal() : base("Petal  r = cos(4 theta)") { }

    protected override void InitGL() => Curve2D.Ortho(-1.4, 1.4, -1.4, 1.4);

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        Curve2D.Axes(1.4);
        GL.LineWidth(2);
        GL.Begin(PrimitiveType.LineStrip);
        for (double t = 0; t <= 2 * Math.PI; t += Math.PI / 720)
        {
            double r = Math.Cos(4 * t);
            // colour the petals by angle for a bit of life
            GL.Color3((float)Math.Abs(Math.Cos(t)), 0.4f, (float)Math.Abs(Math.Sin(t)));
            GL.Vertex2(r * Math.Cos(t), r * Math.Sin(t));
        }
        GL.End();
    }
}

/// <summary>Limaçon (general polar curve): r = b + a·cos θ, with an inner loop.</summary>
public sealed class SketchPolar : ExerciseWindow
{
    public SketchPolar() : base("Polar limacon  r = 0.5 + cos theta") { }

    protected override void InitGL() => Curve2D.Ortho(-2, 2, -2, 2);

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        Curve2D.Axes(2);
        GL.Color3(0.4f, 0.7f, 1f);
        GL.LineWidth(2);
        GL.Begin(PrimitiveType.LineLoop);
        for (double t = 0; t <= 2 * Math.PI; t += Math.PI / 360)
        {
            double r = 0.5 + Math.Cos(t);
            GL.Vertex2(r * Math.Cos(t), r * Math.Sin(t));
        }
        GL.End();
    }
}

/// <summary>Shared 2D helpers for the curve sketches.</summary>
internal static class Curve2D
{
    public static void Ortho(double l, double r, double b, double t)
    {
        GL.ClearColor(0f, 0f, 0f, 0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(l, r, b, t, -1.0, 1.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
    }

    public static void Axes(double extent)
    {
        GL.LineWidth(1);
        GL.Begin(PrimitiveType.Lines);
        GL.Color3(0.4f, 0.4f, 0.4f);
        GL.Vertex2(-extent, 0); GL.Vertex2(extent, 0);
        GL.Vertex2(0, -extent); GL.Vertex2(0, extent);
        GL.End();
    }
}
