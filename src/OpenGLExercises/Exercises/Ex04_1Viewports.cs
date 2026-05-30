using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 4.1 — the same wire cube drawn into four quadrant viewports, each spinning
/// about a different axis. Port of the original <c>Ex4_1.java</c>.
/// </summary>
public sealed class Ex04_1Viewports : ExerciseWindow
{
    public Ex04_1Viewports() : base("Ex 4.1 - multiple viewports") { }

    protected override void InitGL()
    {
        GL.ClearColor(0f, 0f, 0f, 0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
    }

    protected override void RenderScene()
    {
        int w = ClientSize.X, h = ClientSize.Y;
        GL.Clear(ClearBufferMask.ColorBufferBit);

        Quadrant(0, 0, w / 2, h / 2, 1f, 1f, 1f, 1f, 1f, 1f);
        Quadrant(w / 2, h / 2, w / 2, h / 2, 1f, 0f, 0f, -1f, 1f, 1f);
        Quadrant(w / 2, 0, w / 2, h / 2, 0f, 1f, 0f, -1f, -1f, 1f);
        Quadrant(0, h / 2, w / 2, h / 2, 0f, 0f, 1f, 1f, 1f, -1f);

        Rotate();
    }

    private void Quadrant(int vx, int vy, int vw, int vh,
        float r, float g, float b, float ax, float ay, float az)
    {
        GL.Color3(r, g, b);
        GL.PushMatrix();
        GL.Viewport(vx, vy, vw, vh);
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, ax, ay, az);
        Glut.WireCube(0.5);
        GL.PopMatrix();
    }
}
