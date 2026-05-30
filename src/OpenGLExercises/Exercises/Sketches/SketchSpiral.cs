using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLExercises.Exercises.Sketches;

/// <summary>
/// Archimedean spirals (r grows linearly with angle) drawn as line strips and
/// continuously rotated, over a faint set of axes. Combines the original
/// <c>Spiril</c> / <c>Spiril animated</c> / <c>directional spiral</c> sketches:
/// Left/Right arrows reverse the spin direction (as in "directional spiral").
/// </summary>
public sealed class SketchSpiral : ExerciseWindow
{
    private double _deg;
    private bool _directionLeft;

    public SketchSpiral() : base("Spiral - Left/Right arrows reverse spin") { }

    protected override void InitGL()
    {
        GL.ClearColor(0f, 0f, 0f, 0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(-2, 2, -2, 2, -1, 1);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
    }

    protected override void RenderScene()
    {
        if (KeyboardState.IsKeyDown(Keys.Left)) _directionLeft = true;
        if (KeyboardState.IsKeyDown(Keys.Right)) _directionLeft = false;

        GL.Clear(ClearBufferMask.ColorBufferBit);

        DrawAxes();

        GL.Color3(1f, 1f, 0f); GL.LineWidth(5); MiniSpiral(1, 0);
        GL.Color3(1f, 0f, 1f); GL.LineWidth(5); MiniSpiral(0.5, 1);
        GL.Color3(0f, 1f, 0f); GL.LineWidth(5); MiniSpiral(0.5, -1);
        GL.Color3(0.2f, 1f, 0.8f); GL.LineWidth(5); MovingSpiral(0.5, 2);

        _deg += _directionLeft ? -1 : 1;
        if (_deg > 360 || _deg < -360) _deg = 0;
    }

    private void MiniSpiral(double scale, int translate)
    {
        GL.PushMatrix();
        GL.Translate(translate, translate, 0);
        GL.Rotate(_deg, 0, 0, 1);
        GL.Scale(scale, scale, scale);
        SpinAndDraw();
        GL.PopMatrix();
    }

    private void MovingSpiral(double scale, int translate)
    {
        GL.PushMatrix();
        GL.Scale(scale, scale, scale);
        GL.Rotate(_deg, 0, 0, 1);
        GL.Translate(translate, translate, 0);
        SpinAndDraw();
        GL.PopMatrix();
    }

    private void SpinAndDraw()
    {
        GL.PushMatrix();
        GL.Rotate(_deg, 0, 0, 1);
        DrawSpiral();
        GL.PopMatrix();
    }

    private static void DrawSpiral()
    {
        GL.Begin(PrimitiveType.LineStrip);
        double radius = 0;
        for (double theta = 0; theta <= 6 * Math.PI; theta += Math.PI / 180)
        {
            GL.Vertex2(Math.Cos(theta) * radius, Math.Sin(theta) * radius);
            radius += 0.001;
        }
        GL.End();
    }

    private static void DrawAxes()
    {
        GL.LineWidth(1);
        GL.Begin(PrimitiveType.Lines);
        GL.Color3(0.5f, 0.5f, 0.5f);
        GL.Vertex2(-10 * Math.PI, 0);
        GL.Vertex2(10 * Math.PI, 0);
        GL.Vertex2(0, -10);
        GL.Vertex2(0, 10);
        GL.End();
    }
}
