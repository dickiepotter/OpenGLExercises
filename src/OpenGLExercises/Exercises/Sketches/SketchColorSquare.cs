using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises.Sketches;

/// <summary>
/// A filled square whose corner colours cycle continuously, one channel at a
/// time, giving a slowly shifting gradient. Port of the original
/// <c>Square 2 / Polygon.java</c>, including its exact colour-walk animation.
/// </summary>
public sealed class SketchColorSquare : ExerciseWindow
{
    private readonly float[,] _color = new float[4, 3];
    private int _a;
    private int _b;
    private bool _increase = true;

    public SketchColorSquare() : base("Colour-cycling square") { }

    protected override void InitGL()
    {
        GL.ClearColor(0f, 0f, 0f, 0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(0.0, 300.0, 0.0, 300.0, -1.0, 1.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
    }

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        Step();

        GL.Begin(PrimitiveType.Polygon);
        GL.Color3(_color[0, 0], _color[0, 1], _color[2, 0]);
        GL.Vertex2(255, 50);
        GL.Color3(_color[1, 0], _color[1, 1], _color[2, 1]);
        GL.Vertex2(255, 255);
        GL.Color3(_color[2, 0], _color[2, 1], _color[2, 2]);
        GL.Vertex2(50, 255);
        GL.Color3(_color[3, 0], _color[3, 1], _color[3, 2]);
        GL.Vertex2(50, 50);
        GL.End();
    }

    // Faithful reproduction of the original index-walking colour ramp.
    private void Step()
    {
        if (_increase)
        {
            _color[_a, _b] += 0.01f;
            if (_color[_a, _b] > 1.0f)
            {
                if (_b >= 2)
                {
                    if (_a >= 3) _increase = false;
                    else { _a++; _b = 0; }
                }
                else _b++;
            }
        }
        else
        {
            _color[_a, _b] -= 0.01f;
            if (_color[_a, _b] < 0.0f)
            {
                if (_b <= 0)
                {
                    if (_a <= 0) _increase = true;
                    else { _a--; _b = 0; }
                }
                else _b--;
            }
        }
    }
}
