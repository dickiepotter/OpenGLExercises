using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 13 — texture mapping: the "monkey" texture wrapped over a lit, rotating 3D
/// box on a white background (the <c>PngTextureLoader</c> is replaced by
/// <see cref="Texture"/>).
/// </summary>
/// <remarks>
/// The original <c>Ex13.java</c> mapped the texture onto an irregular, half-built
/// fold of five quads (a front square plus mismatched bevel flaps) rather than a
/// clean shape. This port instead maps it onto a proper rectangular box — the
/// intent of a texture-mapping exercise — so the texture reads correctly on every
/// face as it spins. This is a deliberate departure from a literal port.
/// </remarks>
public sealed class Ex13Monkey : ExerciseWindow
{
    private const float H = 0.32f; // half-extent of the box (fits the [-0.5,0.5] ortho view when spun)

    private int _texture;

    public Ex13Monkey() : base("Ex 13 - texture mapping (monkey)") { AngleStep = 1.5f; }

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
        GL.Rotate(20f, 1f, 0f, 0f);   // fixed tilt so the top face is visible -> reads as 3D
        GL.Rotate(Angle, 0f, 1f, 0f); // animated spin

        GL.BindTexture(TextureTarget.Texture2D, _texture);
        GL.Enable(EnableCap.Texture2D);

        // Each face carries the full texture (0,0)->(1,1), wound counter-clockwise
        // when seen from outside, with an outward normal for the lighting.
        Face(0, 0, 1, (-H, -H, H), (H, -H, H), (H, H, H), (-H, H, H));   // front  (+Z)
        Face(0, 0, -1, (H, -H, -H), (-H, -H, -H), (-H, H, -H), (H, H, -H)); // back   (-Z)
        Face(1, 0, 0, (H, -H, H), (H, -H, -H), (H, H, -H), (H, H, H));   // right  (+X)
        Face(-1, 0, 0, (-H, -H, -H), (-H, -H, H), (-H, H, H), (-H, H, -H)); // left   (-X)
        Face(0, 1, 0, (-H, H, H), (H, H, H), (H, H, -H), (-H, H, -H));   // top    (+Y)
        Face(0, -1, 0, (-H, -H, -H), (H, -H, -H), (H, -H, H), (-H, -H, H)); // bottom (-Y)

        GL.Disable(EnableCap.Texture2D);
        GL.PopMatrix();
        Rotate();
    }

    private static void Face(float nx, float ny, float nz,
        (float x, float y, float z) a, (float x, float y, float z) b,
        (float x, float y, float z) c, (float x, float y, float z) d)
    {
        GL.Begin(PrimitiveType.Quads);
        GL.Normal3(nx, ny, nz);
        GL.TexCoord2(0f, 0f); GL.Vertex3(a.x, a.y, a.z);
        GL.TexCoord2(1f, 0f); GL.Vertex3(b.x, b.y, b.z);
        GL.TexCoord2(1f, 1f); GL.Vertex3(c.x, c.y, c.z);
        GL.TexCoord2(0f, 1f); GL.Vertex3(d.x, d.y, d.z);
        GL.End();
    }
}
