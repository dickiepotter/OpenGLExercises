using OpenTK.Graphics.OpenGL;
using System.Reflection;
using StbImageSharp;

namespace OpenGLExercises.Core;

/// <summary>
/// Loads an embedded PNG into a GL texture. Replaces GL4Java's
/// <c>PngTextureLoader</c> (used by Ex 13); OpenTK ships no image loader, so the
/// decode is done with StbImageSharp and uploaded via <c>glTexImage2D</c>.
/// </summary>
public static class Texture
{
    /// <summary>Decode an embedded resource and return a bound 2D texture id.</summary>
    public static int LoadEmbedded(string resourceSuffix)
    {
        var asm = Assembly.GetExecutingAssembly();
        var name = asm.GetManifestResourceNames()
            .First(n => n.EndsWith(resourceSuffix, StringComparison.OrdinalIgnoreCase));
        using var stream = asm.GetManifestResourceStream(name)!;
        // PNG rows run top-to-bottom, but glTexImage2D treats the first row as the
        // bottom of the texture. Flip on load so the image is the right way up for
        // the exercise's texture coordinates (otherwise the monkey is upside down).
        StbImage.stbi_set_flip_vertically_on_load(1);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        int tex = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, tex);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        return tex;
    }
}
