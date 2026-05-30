using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Exercises;

/// <summary>
/// Ex 2.2 — a rotating cube built by hand from coloured <c>GL_LINE_LOOP</c>
/// faces (no GLUT). Port of the original <c>Ex2_2.java</c>.
/// </summary>
public sealed class Ex02_2VectorCube : ExerciseWindow
{
    public Ex02_2VectorCube() : base("Ex 2.2 - hand-built vector cube") { }

    protected override void InitGL()
    {
        GL.ClearColor(0f, 0f, 0f, 0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
    }

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.Color3(1f, 1f, 1f);

        GL.PushMatrix();
        GL.Translate(0.5f, 0.5f, 0.0f);
        GL.Rotate(Angle, 1.0f, 1.0f, 1.0f);
        GL.Scale(0.25f, 0.25f, 0.25f);

        Face(1f, 0f, 0.8f, (1, 1, -1), (-1, 1, -1), (-1, 1, 1), (1, 1, 1));   // top
        Face(1f, 0f, 0.8f, (1, -1, 1), (-1, -1, 1), (-1, -1, -1), (1, -1, -1)); // bottom
        Face(1f, 0f, 0.8f, (1, 1, 1), (-1, 1, 1), (-1, -1, 1), (1, -1, 1));    // front
        Face(1f, 1f, 0f, (1, -1, -1), (-1, -1, -1), (-1, 1, -1), (1, 1, -1));  // back
        Face(0f, 0f, 1f, (-1, 1, 1), (-1, 1, -1), (-1, -1, -1), (-1, -1, 1));  // left
        Face(1f, 0.5f, 0.2f, (1, 1, -1), (1, 1, 1), (1, -1, 1), (1, -1, -1));  // right

        GL.PopMatrix();
        Rotate();
    }

    private static void Face(float r, float g, float b,
        (int x, int y, int z) a, (int x, int y, int z) c,
        (int x, int y, int z) d, (int x, int y, int z) e)
    {
        GL.Begin(PrimitiveType.LineLoop);
        GL.Color3(r, g, b);
        GL.Vertex3(a.x, a.y, a.z);
        GL.Vertex3(c.x, c.y, c.z);
        GL.Vertex3(d.x, d.y, d.z);
        GL.Vertex3(e.x, e.y, e.z);
        GL.End();
    }
}
