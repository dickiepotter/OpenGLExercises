using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 7.3 — a solid lit teapot using the default material. Port of <c>Ex7_3.java</c>.
/// </summary>
/// <remarks>
/// As in 6.2 the original's <c>glMateriali(GL_FRONT, GL_SPECULAR, 25)</c> was an
/// invalid no-op and is omitted; with no diffuse material set the teapot renders
/// in the default grey.
/// </remarks>
public sealed class Ex07_3SolidTeapot : ExerciseWindow
{
    public Ex07_3SolidTeapot() : base("Ex 7.3 - solid lit teapot") { AngleStep = 2.5f; }

    protected override void InitGL()
    {
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();

        GL.ClearColor(0f, 0f, 0f, 0f);
        GL.ClearDepth(1.0);
        GL.DepthFunc(DepthFunction.Less);
        GL.Enable(EnableCap.DepthTest);
        GL.ShadeModel(ShadingModel.Smooth);

        GL.Light(LightName.Light0, LightParameter.Specular, new[] { 1f, 1f, 1f, 1f });
        GL.Light(LightName.Light0, LightParameter.Position, new[] { 0f, 1f, 1f, 0f });
        GL.Enable(EnableCap.Light0);
        GL.Enable(EnableCap.Lighting);
    }

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.PushMatrix();
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, 1f, 1f, 1f);
        Glut.SolidTeapot(0.25);
        GL.PopMatrix();
        Rotate();
    }
}
