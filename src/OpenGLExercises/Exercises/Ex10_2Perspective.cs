using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 10.2 — the same scene as 10.1 but the projection is built with
/// <c>gluPerspective(50, 1, 0.01, 5)</c> instead of a raw frustum.
/// Port of <c>Ex10_2.java</c>.
/// </summary>
public sealed class Ex10_2Perspective : ExerciseWindow
{
    public Ex10_2Perspective() : base("Ex 10.2 - gluPerspective") { AngleStep = 2.5f; }

    protected override void InitGL()
    {
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        Glu.Perspective(50.0, 1.0, 0.01, 5.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();

        PerspectiveSpheres.SetupLighting();
    }

    protected override void RenderScene() => PerspectiveSpheres.Draw(Angle, Rotate);
}
