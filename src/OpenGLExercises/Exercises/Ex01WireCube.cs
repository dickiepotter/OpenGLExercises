using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 1 — a rotating GLUT wireframe cube in an orthographic [0,1] view.
/// Port of the original <c>Ex1.java</c> (GL4Java).
/// </summary>
public sealed class Ex01WireCube : ExerciseWindow
{
    public Ex01WireCube() : base("Ex 1 - GLUT wire cube") { }

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
        Glut.WireCube(2.5);
        GL.PopMatrix();

        Rotate();
    }
}
