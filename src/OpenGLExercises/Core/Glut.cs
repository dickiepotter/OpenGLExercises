using OpenTK.Graphics.OpenGL;

namespace OpenGLExercises.Core;

/// <summary>
/// Re-implementations of the handful of GLUT geometry helpers the exercises
/// relied on. GL4Java's <c>GLUTFuncLightImpl</c> provided these; OpenTK ships no
/// GLUT, so the primitives are drawn here in immediate mode to match the look of
/// the originals (same winding, comparable tessellation, outward normals so the
/// fixed-function lighting in the lit exercises behaves the same way).
/// </summary>
public static class Glut
{
    // ---- Cube -----------------------------------------------------------

    private static readonly float[][] CubeNormals =
    {
        new[] { 1f, 0f, 0f }, new[] { -1f, 0f, 0f },
        new[] { 0f, 1f, 0f }, new[] { 0f, -1f, 0f },
        new[] { 0f, 0f, 1f }, new[] { 0f, 0f, -1f },
    };

    // Each face as four corners (unit cube, scaled at draw time).
    private static readonly int[][] CubeFaces =
    {
        new[] { 1, 5, 7, 3 }, // +X
        new[] { 0, 2, 6, 4 }, // -X
        new[] { 2, 3, 7, 6 }, // +Y
        new[] { 0, 4, 5, 1 }, // -Y
        new[] { 4, 6, 7, 5 }, // +Z
        new[] { 0, 1, 3, 2 }, // -Z
    };

    private static (float x, float y, float z) CubeCorner(int i, float h) =>
        ((i & 1) == 0 ? -h : h, (i & 2) == 0 ? -h : h, (i & 4) == 0 ? -h : h);

    public static void SolidCube(double size)
    {
        float h = (float)size / 2f;
        GL.Begin(PrimitiveType.Quads);
        for (int f = 0; f < 6; f++)
        {
            GL.Normal3(CubeNormals[f][0], CubeNormals[f][1], CubeNormals[f][2]);
            foreach (int c in CubeFaces[f])
            {
                var (x, y, z) = CubeCorner(c, h);
                GL.Vertex3(x, y, z);
            }
        }
        GL.End();
    }

    public static void WireCube(double size)
    {
        float h = (float)size / 2f;
        for (int f = 0; f < 6; f++)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Normal3(CubeNormals[f][0], CubeNormals[f][1], CubeNormals[f][2]);
            foreach (int c in CubeFaces[f])
            {
                var (x, y, z) = CubeCorner(c, h);
                GL.Vertex3(x, y, z);
            }
            GL.End();
        }
    }

    // ---- Sphere ---------------------------------------------------------

    // GL4Java's GLUTFunc exposed single-argument sphere helpers; GLUT proper
    // always takes slices/stacks, so default to a reasonably smooth tessellation.
    private const int DefaultSphereDetail = 24;

    public static void SolidSphere(double radius) => SolidSphere(radius, DefaultSphereDetail, DefaultSphereDetail);
    public static void WireSphere(double radius) => WireSphere(radius, DefaultSphereDetail, DefaultSphereDetail);

    public static void SolidSphere(double radius, int slices, int stacks)
    {
        for (int i = 0; i < stacks; i++)
        {
            double lat0 = Math.PI * (-0.5 + (double)i / stacks);
            double lat1 = Math.PI * (-0.5 + (double)(i + 1) / stacks);
            double z0 = Math.Sin(lat0), zr0 = Math.Cos(lat0);
            double z1 = Math.Sin(lat1), zr1 = Math.Cos(lat1);

            GL.Begin(PrimitiveType.QuadStrip);
            for (int j = 0; j <= slices; j++)
            {
                double lng = 2 * Math.PI * (double)j / slices;
                double x = Math.Cos(lng), y = Math.Sin(lng);

                GL.Normal3((float)(x * zr0), (float)(y * zr0), (float)z0);
                GL.Vertex3((float)(radius * x * zr0), (float)(radius * y * zr0), (float)(radius * z0));
                GL.Normal3((float)(x * zr1), (float)(y * zr1), (float)z1);
                GL.Vertex3((float)(radius * x * zr1), (float)(radius * y * zr1), (float)(radius * z1));
            }
            GL.End();
        }
    }

    public static void WireSphere(double radius, int slices, int stacks)
    {
        // Latitude rings.
        for (int i = 1; i < stacks; i++)
        {
            double lat = Math.PI * (-0.5 + (double)i / stacks);
            double z = Math.Sin(lat), zr = Math.Cos(lat);
            GL.Begin(PrimitiveType.LineLoop);
            for (int j = 0; j < slices; j++)
            {
                double lng = 2 * Math.PI * (double)j / slices;
                double x = Math.Cos(lng) * zr, y = Math.Sin(lng) * zr;
                GL.Normal3((float)x, (float)y, (float)z);
                GL.Vertex3((float)(radius * x), (float)(radius * y), (float)(radius * z));
            }
            GL.End();
        }
        // Longitude half-circles.
        for (int j = 0; j < slices; j++)
        {
            double lng = 2 * Math.PI * (double)j / slices;
            double cl = Math.Cos(lng), sl = Math.Sin(lng);
            GL.Begin(PrimitiveType.LineStrip);
            for (int i = 0; i <= stacks; i++)
            {
                double lat = Math.PI * (-0.5 + (double)i / stacks);
                double z = Math.Sin(lat), zr = Math.Cos(lat);
                double x = cl * zr, y = sl * zr;
                GL.Normal3((float)x, (float)y, (float)z);
                GL.Vertex3((float)(radius * x), (float)(radius * y), (float)(radius * z));
            }
            GL.End();
        }
    }

    // ---- Icosahedron ----------------------------------------------------

    private const float T = 0.850650808352039932f;
    private const float S = 0.525731112119133606f;

    private static readonly float[][] IcoVerts =
    {
        new[] { -S, 0f, T }, new[] { S, 0f, T }, new[] { -S, 0f, -T }, new[] { S, 0f, -T },
        new[] { 0f, T, S }, new[] { 0f, T, -S }, new[] { 0f, -T, S }, new[] { 0f, -T, -S },
        new[] { T, S, 0f }, new[] { -T, S, 0f }, new[] { T, -S, 0f }, new[] { -T, -S, 0f },
    };

    private static readonly int[][] IcoFaces =
    {
        new[] { 0, 4, 1 }, new[] { 0, 9, 4 }, new[] { 9, 5, 4 }, new[] { 4, 5, 8 }, new[] { 4, 8, 1 },
        new[] { 8, 10, 1 }, new[] { 8, 3, 10 }, new[] { 5, 3, 8 }, new[] { 5, 2, 3 }, new[] { 2, 7, 3 },
        new[] { 7, 10, 3 }, new[] { 7, 6, 10 }, new[] { 7, 11, 6 }, new[] { 11, 0, 6 }, new[] { 0, 1, 6 },
        new[] { 6, 1, 10 }, new[] { 9, 0, 11 }, new[] { 9, 11, 2 }, new[] { 9, 2, 5 }, new[] { 7, 2, 11 },
    };

    public static void SolidIcosahedron()
    {
        GL.Begin(PrimitiveType.Triangles);
        foreach (var face in IcoFaces)
        {
            var n = FaceNormal(IcoVerts[face[0]], IcoVerts[face[1]], IcoVerts[face[2]]);
            GL.Normal3(n[0], n[1], n[2]);
            foreach (int v in face)
                GL.Vertex3(IcoVerts[v][0], IcoVerts[v][1], IcoVerts[v][2]);
        }
        GL.End();
    }

    public static void WireIcosahedron()
    {
        foreach (var face in IcoFaces)
        {
            GL.Begin(PrimitiveType.LineLoop);
            var n = FaceNormal(IcoVerts[face[0]], IcoVerts[face[1]], IcoVerts[face[2]]);
            GL.Normal3(n[0], n[1], n[2]);
            foreach (int v in face)
                GL.Vertex3(IcoVerts[v][0], IcoVerts[v][1], IcoVerts[v][2]);
            GL.End();
        }
    }

    private static float[] FaceNormal(float[] a, float[] b, float[] c)
    {
        float ux = b[0] - a[0], uy = b[1] - a[1], uz = b[2] - a[2];
        float vx = c[0] - a[0], vy = c[1] - a[1], vz = c[2] - a[2];
        float nx = uy * vz - uz * vy, ny = uz * vx - ux * vz, nz = ux * vy - uy * vx;
        float len = MathF.Sqrt(nx * nx + ny * ny + nz * nz);
        if (len == 0) return new[] { 0f, 0f, 1f };
        return new[] { nx / len, ny / len, nz / len };
    }

    // ---- Teapot (delegated to the Bezier-patch implementation) ----------

    public static void SolidTeapot(double size) => Teapot.Render(size, solid: true);
    public static void WireTeapot(double size) => Teapot.Render(size, solid: false);
}
