using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 11 — adds a camera: the perspective scene from 10.2 is viewed from above
/// and behind using <c>gluLookAt(0, 2, -3, 0, 0, -1, 0, 1, 0)</c>.
/// Port of <c>Ex11.java</c>.
/// </summary>
public sealed class Ex11Camera : ExerciseWindow
{
    public Ex11Camera() : base("Ex 11 - perspective with camera") { AngleStep = 2.5f; }

    protected override void InitGL()
    {
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        Glu.Perspective(50.0, 1.0, 0.01, 5.0);

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
        // Camera placed above/behind, looking at the back sphere. This stays on the
        // modelview stack for the whole run; each frame pushes/pops on top of it.
        Glu.LookAt(0, 2, -3, 0, 0, -1, 0, 1, 0);

        PerspectiveSpheres.SetupLighting();
    }

    protected override void RenderScene() => PerspectiveSpheres.Draw(Angle, Rotate);
}
