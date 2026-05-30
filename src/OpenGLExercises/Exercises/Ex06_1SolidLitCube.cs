using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 6.1 — a solid, smooth-shaded, lit cube (first use of lighting/materials).
/// Port of the original <c>Ex6_1.java</c>.
/// </summary>
public sealed class Ex06_1SolidLitCube : ExerciseWindow
{
    public Ex06_1SolidLitCube() : base("Ex 6.1 - solid lit cube") { AngleStep = 2.5f; }

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

        GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new[] { 1f, 0f, 0f, 1f });
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
        Glut.SolidCube(0.5);
        GL.PopMatrix();
        Rotate();
    }
}
