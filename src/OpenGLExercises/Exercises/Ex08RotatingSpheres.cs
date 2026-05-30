using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 8 — a red sphere orbiting a stationary blue sphere, both with specular
/// highlights under a fixed light. Port of <c>Ex8.java</c>.
/// </summary>
/// <remarks>
/// The original's <c>glLighti(GL_LIGHT0, GL_SHININESS, 25)</c> is invalid
/// (shininess is a material property, not a light one) and was a no-op; omitted.
/// </remarks>
public sealed class Ex08RotatingSpheres : ExerciseWindow
{
    private static readonly float[] Red = { 1f, 0f, 0f, 1f };
    private static readonly float[] Blue = { 0f, 0f, 1f, 1f };
    private static readonly float[] White = { 1f, 1f, 1f, 1f };

    public Ex08RotatingSpheres() : base("Ex 8 - rotating spheres") { AngleStep = 2.5f; }

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

        GL.Light(LightName.Light0, LightParameter.Diffuse, White);
        GL.Light(LightName.Light0, LightParameter.Specular, White);
        GL.Light(LightName.Light0, LightParameter.Position, new[] { 0f, 1f, 1f, 0f });
        GL.Enable(EnableCap.Light0);
        GL.Enable(EnableCap.Lighting);
    }

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Orbiting red sphere.
        GL.PushMatrix();
        SetMaterial(Red);
        GL.Translate(0.5f, 0.5f, 0f);
        GL.Rotate(Angle, 0f, 1f, 0f);
        GL.Translate(0.5f, 0f, 0f);
        Glut.SolidSphere(0.15, 40, 40);
        GL.PopMatrix();

        // Stationary blue sphere.
        GL.PushMatrix();
        SetMaterial(Blue);
        GL.Translate(0.5f, 0.5f, 0.0f);
        Glut.SolidSphere(0.25, 40, 40);
        GL.PopMatrix();

        Rotate();
    }

    private static void SetMaterial(float[] diffuse)
    {
        GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, diffuse);
        GL.Material(MaterialFace.Front, MaterialParameter.Specular, White);
        GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 25f);
    }
}
