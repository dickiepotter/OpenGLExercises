using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 5 — four viewports, each with a different GLUT wireframe primitive
/// (cube, sphere, icosahedron, teapot). Port of the original <c>Ex5.java</c>.
/// </summary>
public sealed class Ex05ViewportsObjects : ExerciseWindow
{
    public Ex05ViewportsObjects() : base("Ex 5 - viewports and objects") { }

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

        GL.Color3(1f, 1f, 1f);
        GL.PushMatrix();
        GL.Viewport(0, 0, w / 2, h / 2);
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, 1f, 1f, 1f);
        Glut.WireCube(0.5);
        GL.PopMatrix();

        GL.Color3(1f, 0f, 0f);
        GL.PushMatrix();
        GL.Viewport(w / 2, h / 2, w / 2, h / 2);
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, -1f, 1f, 1f);
        Glut.WireSphere(0.5, 50, 50);
        GL.PopMatrix();

        GL.Color3(0f, 1f, 0f);
        GL.PushMatrix();
        GL.Viewport(w / 2, 0, w / 2, h / 2);
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, -1f, -1f, 1f);
        GL.Scale(0.5f, 0.5f, 0.5f);
        Glut.WireIcosahedron();
        GL.PopMatrix();

        GL.Color3(0f, 0f, 1f);
        GL.PushMatrix();
        GL.Viewport(0, h / 2, w / 2, h / 2);
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, 1f, 1f, -1f);
        Glut.WireTeapot(0.3);
        GL.PopMatrix();

        Rotate();
    }
}
