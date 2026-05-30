using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Core;

/// <summary>
/// The two GLU helpers the exercises used. OpenTK 4 dropped the GLU wrapper that
/// GL4Java exposed as <c>glu.gluPerspective</c> / <c>glu.gluLookAt</c>, so they
/// are reproduced here in terms of core GL calls (exactly what the classic GLU
/// implementations did), operating on the current matrix.
/// </summary>
public static class Glu
{
    /// <summary>Equivalent of <c>gluPerspective</c>; multiplies the current matrix.</summary>
    public static void Perspective(double fovyDegrees, double aspect, double near, double far)
    {
        double top = near * Math.Tan(fovyDegrees * Math.PI / 360.0);
        double right = top * aspect;
        GL.Frustum(-right, right, -top, top, near, far);
    }

    /// <summary>Equivalent of <c>gluLookAt</c>; multiplies the current matrix.</summary>
    public static void LookAt(
        double eyeX, double eyeY, double eyeZ,
        double centerX, double centerY, double centerZ,
        double upX, double upY, double upZ)
    {
        // forward = normalize(center - eye)
        double fx = centerX - eyeX, fy = centerY - eyeY, fz = centerZ - eyeZ;
        Normalize(ref fx, ref fy, ref fz);

        // side = normalize(forward x up)
        double sx = fy * upZ - fz * upY;
        double sy = fz * upX - fx * upZ;
        double sz = fx * upY - fy * upX;
        Normalize(ref sx, ref sy, ref sz);

        // up' = side x forward
        double ux = sy * fz - sz * fy;
        double uy = sz * fx - sx * fz;
        double uz = sx * fy - sy * fx;

        // Column-major 4x4 (m[col*4 + row]) as GL expects.
        double[] m =
        {
            sx, ux, -fx, 0,
            sy, uy, -fy, 0,
            sz, uz, -fz, 0,
            0, 0, 0, 1,
        };
        GL.MultMatrix(m);
        GL.Translate(-eyeX, -eyeY, -eyeZ);
    }

    private static void Normalize(ref double x, ref double y, ref double z)
    {
        double len = Math.Sqrt(x * x + y * y + z * z);
        if (len == 0) return;
        x /= len; y /= len; z /= len;
    }
}
