using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Shared scene for Ex 10.1, 10.2 and 11 — an orbiting red sphere and a
/// stationary blue sphere positioned a unit down the -Z axis. Those three
/// exercises differ only in how the projection (and, for Ex 11, the camera) is
/// set up, so the lighting and geometry live here to avoid duplication.
/// </summary>
internal static class PerspectiveSpheres
{
    private static readonly float[] Red = { 1f, 0f, 0f, 1f };
    private static readonly float[] Blue = { 0f, 0f, 1f, 1f };
    private static readonly float[] White = { 1f, 1f, 1f, 1f };

    public static void SetupLighting()
    {
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

    public static void Draw(float angle, Action advance)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // Orbiting red sphere.
        GL.PushMatrix();
        SetMaterial(Red);
        GL.Translate(0f, 0f, -1.0f);
        GL.Rotate(angle, 0f, 1f, 0f);
        GL.Translate(0.5f, 0f, 0f);
        Glut.SolidSphere(0.15, 40, 40);
        GL.PopMatrix();

        // Stationary blue sphere.
        GL.PushMatrix();
        SetMaterial(Blue);
        GL.Translate(0f, 0f, -1.0f);
        Glut.SolidSphere(0.25, 40, 40);
        GL.PopMatrix();

        advance();
    }

    private static void SetMaterial(float[] diffuse)
    {
        GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, diffuse);
        GL.Material(MaterialFace.Front, MaterialParameter.Specular, White);
        GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 25f);
    }
}
