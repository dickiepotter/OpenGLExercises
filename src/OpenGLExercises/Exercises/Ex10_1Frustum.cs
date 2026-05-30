using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 10.1 — the orbiting/stationary sphere pair seen through a perspective
/// projection set up directly with <c>glFrustum</c>. Port of <c>Ex10_1.java</c>.
/// </summary>
public sealed class Ex10_1Frustum : ExerciseWindow
{
    public Ex10_1Frustum() : base("Ex 10.1 - perspective via glFrustum") { AngleStep = 2.5f; }

    protected override void InitGL()
    {
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Frustum(-0.005, 0.005, -0.005, 0.005, 0.01, 5.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();

        PerspectiveSpheres.SetupLighting();
    }

    protected override void RenderScene() => PerspectiveSpheres.Draw(Angle, Rotate);
}
