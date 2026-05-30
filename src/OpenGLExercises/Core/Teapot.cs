using System.Globalization;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenGLExercises.Core;

/// <summary>
/// The Utah teapot, evaluated from Newell's 32 bicubic Bézier patches
/// (<c>teapotrim.bpt</c>, embedded as a resource). GL4Java's GLUT provided
/// <c>glutWireTeapot</c>/<c>glutSolidTeapot</c>; OpenTK has no GLUT, so the patch
/// data is tessellated here. The raw data is Z-up and not centred, so — like
/// GLUT — we rotate it Y-up and centre it on the origin, then scale so that
/// <c>size</c> behaves like the radius argument the other primitives use.
/// </summary>
public static class Teapot
{
    private const int Segments = 10; // tessellation per patch edge

    private static Vector3[][,]? _patches; // [patch][4,4] control points
    private static Vector3 _center;
    private static float _normScale;

    private static void EnsureLoaded()
    {
        if (_patches != null) return;

        var asm = Assembly.GetExecutingAssembly();
        var resName = asm.GetManifestResourceNames()
            .First(n => n.EndsWith("teapotrim.bpt", StringComparison.OrdinalIgnoreCase));
        using var stream = asm.GetManifestResourceStream(resName)!;
        using var reader = new StreamReader(stream);

        string[] Tokens() => reader.ReadLine()!
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        int patchCount = int.Parse(Tokens()[0], CultureInfo.InvariantCulture);
        var patches = new Vector3[patchCount][,];

        var min = new Vector3(float.MaxValue);
        var max = new Vector3(float.MinValue);

        for (int p = 0; p < patchCount; p++)
        {
            var dims = Tokens(); // e.g. "3 3" -> bicubic (4x4 control points)
            int du = int.Parse(dims[0], CultureInfo.InvariantCulture) + 1;
            int dv = int.Parse(dims[1], CultureInfo.InvariantCulture) + 1;
            var cp = new Vector3[du, dv];
            for (int i = 0; i < du; i++)
                for (int j = 0; j < dv; j++)
                {
                    var t = Tokens();
                    var v = new Vector3(
                        float.Parse(t[0], CultureInfo.InvariantCulture),
                        float.Parse(t[1], CultureInfo.InvariantCulture),
                        float.Parse(t[2], CultureInfo.InvariantCulture));
                    cp[i, j] = v;
                    min = Vector3.ComponentMin(min, v);
                    max = Vector3.ComponentMax(max, v);
                }
            patches[p] = cp;
        }

        _patches = patches;
        _center = (min + max) * 0.5f;
        var halfExtent = (max - min) * 0.5f;
        _normScale = 1f / MathF.Max(halfExtent.X, MathF.Max(halfExtent.Y, halfExtent.Z));
    }

    public static void Render(double size, bool solid)
    {
        EnsureLoaded();

        GL.PushAttrib(AttribMask.EnableBit | AttribMask.PolygonBit);
        GL.Enable(EnableCap.Normalize); // we scale the model, so renormalise normals
        if (!solid) GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

        GL.PushMatrix();
        float s = (float)size * _normScale;
        GL.Rotate(-90f, 1f, 0f, 0f);             // raw data is Z-up; GLUT shows it Y-up
        GL.Scale(s, s, s);
        GL.Translate(-_center.X, -_center.Y, -_center.Z);

        foreach (var patch in _patches!)
            RenderPatch(patch);

        GL.PopMatrix();
        GL.PopAttrib();
    }

    private static void RenderPatch(Vector3[,] cp)
    {
        // Sample positions and normals on an (n+1) x (n+1) grid, then draw quad strips.
        int n = Segments;
        var pos = new Vector3[n + 1, n + 1];
        var nrm = new Vector3[n + 1, n + 1];
        for (int i = 0; i <= n; i++)
        {
            float u = (float)i / n;
            for (int j = 0; j <= n; j++)
            {
                float v = (float)j / n;
                pos[i, j] = SurfacePoint(cp, u, v);
                var du = SurfaceDerivU(cp, u, v);
                var dv = SurfaceDerivV(cp, u, v);
                var normal = Vector3.Cross(du, dv);
                nrm[i, j] = normal.LengthSquared > 1e-12f ? Vector3.Normalize(normal) : Vector3.UnitY;
            }
        }

        for (int i = 0; i < n; i++)
        {
            GL.Begin(PrimitiveType.QuadStrip);
            for (int j = 0; j <= n; j++)
            {
                GL.Normal3(nrm[i, j]);
                GL.Vertex3(pos[i, j]);
                GL.Normal3(nrm[i + 1, j]);
                GL.Vertex3(pos[i + 1, j]);
            }
            GL.End();
        }
    }

    // Bicubic Bézier basis and its derivative.
    private static void Basis(float t, out float b0, out float b1, out float b2, out float b3)
    {
        float it = 1 - t;
        b0 = it * it * it;
        b1 = 3 * it * it * t;
        b2 = 3 * it * t * t;
        b3 = t * t * t;
    }

    private static void DBasis(float t, out float b0, out float b1, out float b2, out float b3)
    {
        float it = 1 - t;
        b0 = -3 * it * it;
        b1 = 3 * it * it - 6 * it * t;
        b2 = 6 * it * t - 3 * t * t;
        b3 = 3 * t * t;
    }

    private static Vector3 Eval(Vector3[,] cp, float[] bu, float[] bv)
    {
        var r = Vector3.Zero;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                r += bu[i] * bv[j] * cp[i, j];
        return r;
    }

    private static Vector3 SurfacePoint(Vector3[,] cp, float u, float v)
    {
        Basis(u, out var u0, out var u1, out var u2, out var u3);
        Basis(v, out var v0, out var v1, out var v2, out var v3);
        return Eval(cp, new[] { u0, u1, u2, u3 }, new[] { v0, v1, v2, v3 });
    }

    private static Vector3 SurfaceDerivU(Vector3[,] cp, float u, float v)
    {
        DBasis(u, out var u0, out var u1, out var u2, out var u3);
        Basis(v, out var v0, out var v1, out var v2, out var v3);
        return Eval(cp, new[] { u0, u1, u2, u3 }, new[] { v0, v1, v2, v3 });
    }

    private static Vector3 SurfaceDerivV(Vector3[,] cp, float u, float v)
    {
        Basis(u, out var u0, out var u1, out var u2, out var u3);
        DBasis(v, out var v0, out var v1, out var v2, out var v3);
        return Eval(cp, new[] { u0, u1, u2, u3 }, new[] { v0, v1, v2, v3 });
    }
}
