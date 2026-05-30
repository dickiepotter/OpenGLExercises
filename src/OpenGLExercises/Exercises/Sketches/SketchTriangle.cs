using OpenGLExercises.Core;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLExercises.Exercises.Sketches;

/// <summary>
/// An RGB Gouraud-shaded triangle that spins about its centroid, with the
/// centroid marked. Port of the original interactive "Animated triangle"
/// (<c>Triangle.java</c>):
///   Space   toggle rotation
///   R/G/B   arm a vertex, then left-click to move it
/// The original also drew on-screen labels via GLUT bitmap fonts; OpenTK has no
/// bitmap-font helper, so the hints live in the window title instead.
/// </summary>
public sealed class SketchTriangle : ExerciseWindow
{
    private const float WLeft = -2f, WRight = 2f, WBottom = -2f, WTop = 2f;

    private float _redX = 0f, _redY = 0.5f;
    private float _greenX = -0.5f, _greenY = -0.5f;
    private float _blueX = 0.5f, _blueY = -0.5f;
    private float _centreX, _centreY;
    private float _degRot;
    private bool _rotate;
    private int _armedVertex; // 0 none, 1 red, 2 green, 3 blue

    public SketchTriangle() : base("Triangle - Space: spin, R/G/B then click: move vertex") { }

    protected override void InitGL()
    {
        GL.ClearColor(0f, 0f, 0f, 0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();
        GL.Ortho(WLeft, WRight, WBottom, WTop, -1.0, 1.0);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);
        switch (e.Key)
        {
            case Keys.R: _armedVertex = 1; _degRot = 0; break;
            case Keys.G: _armedVertex = 2; _degRot = 0; break;
            case Keys.B: _armedVertex = 3; _degRot = 0; break;
            case Keys.Space: _rotate = !_rotate; break;
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButton.Left || _armedVertex == 0) return;

        // Map window pixel -> world coordinates (y is flipped: window origin is top-left).
        float wx = WLeft + (float)(MouseState.X / ClientSize.X) * (WRight - WLeft);
        float wy = WBottom + (float)((ClientSize.Y - MouseState.Y) / ClientSize.Y) * (WTop - WBottom);

        switch (_armedVertex)
        {
            case 1: _redX = wx; _redY = wy; break;
            case 2: _greenX = wx; _greenY = wy; break;
            case 3: _blueX = wx; _blueY = wy; break;
        }
        _armedVertex = 0;
    }

    protected override void RenderScene()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _centreX = (_redX + _greenX + _blueX) / 3f;
        _centreY = (_redY + _greenY + _blueY) / 3f;

        GL.PushMatrix();
        GL.Translate(_centreX, _centreY, 0);
        GL.Rotate(_degRot, 0, 0, 1);
        GL.Translate(-_centreX, -_centreY, 0);

        GL.Begin(PrimitiveType.Triangles);
        GL.Color3(1f, 0f, 0f); GL.Vertex2(_redX, _redY);
        GL.Color3(0f, 1f, 0f); GL.Vertex2(_blueX, _blueY);
        GL.Color3(0f, 0f, 1f); GL.Vertex2(_greenX, _greenY);
        GL.End();

        if (_rotate) _degRot++;
        if (_degRot > 360) _degRot = 0;
        GL.PopMatrix();

        // Centroid marker.
        GL.PointSize(5);
        GL.Begin(PrimitiveType.Points);
        GL.Color3(1f, 1f, 1f);
        GL.Vertex2(_centreX, _centreY);
        GL.End();
    }
}
