using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 2.1 — a rotating GLUT wireframe teapot in an orthographic [0,1] view.
/// Port of the original <c>Ex2_1.java</c> (GL4Java).
/// </summary>
public sealed class Ex02_1WireTeapot : ExerciseWindow
{
    public Ex02_1WireTeapot() : base("Ex 2.1 - GLUT wire teapot") { }

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
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.Color3(1f, 1f, 1f);

        GL.PushMatrix();
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, 1.0f, 1.0f, 1.0f);
        GL.Scale(0.25f, 0.25f, 0.25f);
        Glut.WireTeapot(1.5);
        GL.PopMatrix();

        Rotate();
    }
}
