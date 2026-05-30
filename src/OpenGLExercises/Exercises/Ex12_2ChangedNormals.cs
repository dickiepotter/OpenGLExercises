using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 12.2 — identical to 12.1 except the face normal is changed to
/// <c>(0, 0, -0.5)</c> (pointing away/back and non-unit), so the diamond stays
/// dark: it shows how a wrong normal breaks the lighting. Port of <c>Ex12_2.java</c>.
/// </summary>
public sealed class Ex12_2ChangedNormals : ExerciseWindow
{
    public Ex12_2ChangedNormals() : base("Ex 12.2 - changed normals") { AngleStep = 5f; }

    protected override void InitGL()
    {
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
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

        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new[] { 1f, 0f, 0f, 0f });
        GL.Light(LightName.Light0, LightParameter.Position, new[] { 0f, 1f, 1f, 0f });
        GL.Enable(EnableCap.Light0);
        GL.Enable(EnableCap.Lighting);
    }

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.PushMatrix();
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, 0f, 1f, 0f);

        GL.Begin(PrimitiveType.Quads);
        GL.Normal3(0f, 0f, -0.5f);
        GL.Vertex3(0f, 0.25f, 0f);
        GL.Vertex3(0.25f, 0f, 0f);
        GL.Vertex3(0f, -0.25f, 0f);
        GL.Vertex3(-0.25f, 0f, 0f);
        GL.End();

        GL.PopMatrix();
        Rotate();
    }
}
