using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 13 — texture mapping: a folded fan of quads carrying the "monkey" texture,
/// lit and rotating about Y on a white background. Port of <c>Ex13.java</c>
/// (whose <c>PngTextureLoader</c> is replaced by <see cref="Texture"/>).
/// </summary>
public sealed class Ex13Monkey : ExerciseWindow
{
    private int _texture;

    public Ex13Monkey() : base("Ex 13 - texture mapping (monkey)") { AngleStep = 2f; }

    protected override void InitGL()
    {
        _texture = Texture.LoadEmbedded("monkey1.png");

        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(-0.5, 0.5, -0.5, 0.5, -1.0, 1.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();

        GL.ClearColor(1f, 1f, 1f, 0f);
        GL.ClearDepth(1.0);
        GL.DepthFunc(DepthFunction.Less);
        GL.Enable(EnableCap.DepthTest);
        GL.ShadeModel(ShadingModel.Smooth);

        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new[] { 1f, 1f, 1f, 1f });
        GL.Light(LightName.Light0, LightParameter.Position, new[] { 0f, 1f, 1f, 0f });
        GL.Enable(EnableCap.Light0);
        GL.Enable(EnableCap.Lighting);
    }

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.PushMatrix();
        GL.Rotate(Angle, 0f, 1f, 0f);
        GL.BindTexture(TextureTarget.Texture2D, _texture);
        GL.Enable(EnableCap.Texture2D);

        GL.Begin(PrimitiveType.Quads);
        GL.Normal3(0f, 0f, 1f);
        Tex(0.25f, 0.25f); GL.Vertex3(-0.35f, -0.35f, 0.0f);
        Tex(0.25f, 0.75f); GL.Vertex3(-0.35f, 0.35f, 0.0f);
        Tex(0.75f, 0.75f); GL.Vertex3(0.35f, 0.35f, 0.0f);
        Tex(0.75f, 0.25f); GL.Vertex3(0.35f, -0.35f, 0.0f);
        GL.End();

        GL.Begin(PrimitiveType.Quads);
        GL.Normal3(0f, 0f, 1f);
        Tex(0.0f, 0.0f); GL.Vertex3(-0.5f, -0.5f, -0.35f);
        Tex(0.0f, 1f); GL.Vertex3(-0.5f, 0.5f, -0.35f);
        Tex(0.25f, 0.75f); GL.Vertex3(-0.35f, 0.35f, 0.0f);
        Tex(0.25f, 0.25f); GL.Vertex3(-0.35f, -0.35f, 0.0f);
        GL.End();

        GL.Begin(PrimitiveType.Quads);
        GL.Normal3(1f, 1f, 1f);
        Tex(0.75f, 0.25f); GL.Vertex3(0.35f, -0.35f, 0.0f);
        Tex(0.75f, 0.75f); GL.Vertex3(0.35f, 0.35f, 0.0f);
        Tex(1.0f, 1.0f); GL.Vertex3(0.35f, 0.35f, -0.35f);
        Tex(1.0f, 0.0f); GL.Vertex3(0.35f, -0.35f, -0.35f);
        GL.End();

        GL.Begin(PrimitiveType.Quads);
        GL.Normal3(1f, 1f, 1f);
        Tex(0.0f, 0.0f); GL.Vertex3(-0.35f, -0.35f, -0.35f);
        Tex(0.25f, 0.25f); GL.Vertex3(-0.35f, -0.35f, 0.0f);
        Tex(0.75f, 0.25f); GL.Vertex3(0.35f, -0.35f, 0.0f);
        Tex(1.0f, 0.0f); GL.Vertex3(0.35f, -0.35f, -0.35f);
        GL.End();

        GL.Begin(PrimitiveType.Quads);
        GL.Normal3(0f, 0f, 1f);
        Tex(0.0f, 0.0f); GL.Vertex3(-0.5f, -0.5f, -0.5f);
        Tex(1.0f, 0.0f); GL.Vertex3(-0.5f, 0.5f, -0.5f);
        Tex(0.25f, 0.75f); GL.Vertex3(-0.5f, 0.5f, 0.0f);
        Tex(0.25f, 0.25f); GL.Vertex3(-0.5f, -0.5f, 0.0f);
        GL.End();

        GL.Disable(EnableCap.Texture2D);
        GL.PopMatrix();
        Rotate();
    }

    private static void Tex(float s, float t) => GL.TexCoord2(s, t);
}
