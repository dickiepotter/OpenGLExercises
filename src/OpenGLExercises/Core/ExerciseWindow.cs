using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenGLExercises.Core;

/// <summary>
/// Base class for every ported exercise. This is the OpenTK equivalent of the
/// original GL4Java <c>GLAnimCanvas</c> + <c>AutoFrame</c> pairing: it owns the
/// window and a continuously-redrawn animation loop, and exposes two override
/// points that map directly onto the originals:
///
///   GL4Java <c>init()</c>     -> <see cref="InitGL"/>
///   GL4Java <c>display()</c>  -> <see cref="RenderScene"/>
///
/// Buffer-swap (<c>glj.gljSwap()</c>) and context-current handling
/// (<c>glj.gljMakeCurrent()</c>) are done by the loop, so a port only contains
/// the actual GL drawing — exactly the body the originals wrapped.
/// </summary>
public abstract class ExerciseWindow : GameWindow
{
    /// <summary>Rotation accumulator shared by the spinning exercises.</summary>
    protected float Angle;

    /// <summary>Degrees added to <see cref="Angle"/> per frame by <see cref="Rotate"/>.</summary>
    protected float AngleStep = 1.0f;

    private readonly string? _capturePath;
    private readonly int _captureAfterFrames;
    private int _frame;

    protected ExerciseWindow(string title, int width = 300, int height = 300)
        : base(GameWindowSettings.Default, MakeSettings(title, width, height))
    {
        _capturePath = CaptureRequest.Path;
        _captureAfterFrames = CaptureRequest.AfterFrames;
        if (_capturePath != null)
            UpdateFrequency = 0; // run as fast as possible when capturing headlessly
    }

    private static NativeWindowSettings MakeSettings(string title, int width, int height) => new()
    {
        ClientSize = new Vector2i(width, height),
        Title = title,
        // Fixed-function GL (glBegin/glEnd, glLight, glMaterial, glOrtho) needs a
        // compatibility profile; the OpenTK default is Core, which rejects them.
        Profile = ContextProfile.Compatability,
        APIVersion = new Version(3, 3),
    };

    /// <summary>One-time GL state setup. Mirrors the original <c>init()</c>.</summary>
    protected abstract void InitGL();

    /// <summary>Per-frame drawing. Mirrors the original <c>display()</c> body.</summary>
    protected abstract void RenderScene();

    /// <summary>Advance the shared rotation, wrapping at 360 like the originals.</summary>
    protected void Rotate()
    {
        Angle += AngleStep;
        if (Angle > 360.0f) Angle -= 360.0f;
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        InitGL();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        RenderScene();
        SwapBuffers();

        if (_capturePath != null && ++_frame >= _captureAfterFrames)
        {
            CaptureToPng(_capturePath);
            Close();
        }
    }

    /// <summary>Read the back/front framebuffer and save it (used by --capture).</summary>
    private void CaptureToPng(string path)
    {
        int w = ClientSize.X, h = ClientSize.Y;
        var pixels = new byte[w * h * 4];
        GL.ReadBuffer(ReadBufferMode.Front);
        GL.ReadPixels(0, 0, w, h, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

        // GL origin is bottom-left; PNG is top-left, so flip vertically. Also force
        // the alpha channel opaque: the exercises clear to an alpha of 0 (faithful to
        // the originals), which would otherwise make the saved image fully transparent.
        var flipped = new byte[pixels.Length];
        int stride = w * 4;
        for (int y = 0; y < h; y++)
            Array.Copy(pixels, y * stride, flipped, (h - 1 - y) * stride, stride);
        for (int i = 3; i < flipped.Length; i += 4)
            flipped[i] = 255;

        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        Png.Save(path, w, h, flipped);
        Console.WriteLine($"Captured {w}x{h} -> {path}");
    }
}

/// <summary>
/// Set by the launcher when invoked with <c>--capture</c>; consumed by every
/// <see cref="ExerciseWindow"/> at construction so exercises need no capture code.
/// </summary>
public static class CaptureRequest
{
    public static string? Path { get; set; }
    public static int AfterFrames { get; set; } = 60;
}
