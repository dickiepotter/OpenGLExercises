using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 6.2 — a solid lit sphere using the default material (no diffuse colour set,
/// so it renders in the default grey). Port of the original <c>Ex6_2.java</c>.
/// </summary>
/// <remarks>
/// The original also called <c>glMateriali(GL_FRONT, GL_SPECULAR, 25)</c>, which
/// is an invalid enum/value combination (specular is a 4-component colour, not a
/// scalar) and was a no-op in real GL; it is intentionally omitted here.
/// </remarks>
public sealed class Ex06_2SolidLitSphere : ExerciseWindow
{
    public Ex06_2SolidLitSphere() : base("Ex 6.2 - solid lit sphere (default material)") { AngleStep = 2.5f; }

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

        GL.Light(LightName.Light0, LightParameter.Diffuse, new[] { 1f, 1f, 1f, 1f });
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
        Glut.SolidSphere(0.5);
        GL.PopMatrix();
        Rotate();
    }
}
