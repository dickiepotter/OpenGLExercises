using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 4.2 — viewports that deliberately don't preserve aspect ratio: the third
/// cube is stretched across a full-width / half-height viewport, demonstrating
/// the distortion. Port of the original <c>Ex4_2.java</c>.
/// </summary>
public sealed class Ex04_2BrokenAspect : ExerciseWindow
{
    public Ex04_2BrokenAspect() : base("Ex 4.2 - broken aspect ratio") { }

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

        Cube(0, 0, w / 2, h / 2, 1f, 1f, 1f, 1f, 1f, 1f);
        Cube(w / 2, 0, w / 2, h / 2, 1f, 0f, 0f, -1f, 1f, 1f);
        Cube(0, h / 2, w, h / 2, 0f, 1f, 0f, -1f, -1f, 1f); // full width, half height -> stretched

        Rotate();
    }

    private void Cube(int vx, int vy, int vw, int vh,
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
