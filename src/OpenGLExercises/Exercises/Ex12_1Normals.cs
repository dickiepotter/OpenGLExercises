using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 12.1 — a lit diamond quad whose face normal points straight out
/// (<c>0, 0, 1</c>), so it lights up as it turns to face the light.
/// Port of <c>Ex12_1.java</c>.
/// </summary>
public sealed class Ex12_1Normals : ExerciseWindow
{
    public Ex12_1Normals() : base("Ex 12.1 - surface normals") { AngleStep = 5f; }

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

        // Original used a 5-element array; only the first four are read -> (1,0,0,0).
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
        GL.Normal3(0f, 0f, 1f);
        GL.Vertex3(0f, 0.25f, 0f);
        GL.Vertex3(0.25f, 0f, 0f);
        GL.Vertex3(0f, -0.25f, 0f);
        GL.Vertex3(-0.25f, 0f, 0f);
        GL.End();

        GL.PopMatrix();
        Rotate();
    }
}
