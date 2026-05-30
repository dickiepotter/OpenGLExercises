# From legacy OpenGL bindings to OpenTK

These exercises were originally written around 2003–2004 against **GL4Java**
(`gl4java.awt.GLAnimCanvas` + a GLUT helper). The sibling projects
(`VRfs`, `Tank Attack`) used a different but contemporary wrapper, **CSOpenGL**
(`CSOpenGL.GL`, `CSOpenGL.GLU`, `CSOpenGL.GLSurface`). Both bindings are long
dead and will not load on a modern machine.

This repo re-targets the code onto **[OpenTK](https://opentk.net) 4.x** running on
**.NET 8**. The rendering itself is still *fixed-function, immediate-mode*
OpenGL — the point of the port is to keep the original teaching code intact while
swapping out only the dead plumbing. This document records exactly what changed
and, importantly, the **default-state differences** that bite when you move
between GL bindings.

## Binding / API name mapping

| Concept | GL4Java | CSOpenGL | OpenTK 4 |
| --- | --- | --- | --- |
| GL entry points | `gl.glBegin(...)` | `GL.glBegin(...)` | `GL.Begin(...)` |
| Constants | `GL_QUADS` (mixed in) | `GL.GL_QUADS` | `PrimitiveType.Quads` (typed enums) |
| GLU helpers | `glu.gluPerspective` | `GLU.gluPerspective` | _none_ — see `Core/Glu.cs` |
| GLUT shapes | `glut.glutSolidTeapot` | _n/a_ | _none_ — see `Core/Glut.cs` |
| Window / canvas | `GLAnimCanvas` + `AutoFrame` | `GLSurface` (WinForms control) | `GameWindow` (see `Core/ExerciseWindow.cs`) |
| Make-current / swap | `glj.gljMakeCurrent()` / `gljSwap()` | `surface.UpdateGL()` | handled by `GameWindow`; call `SwapBuffers()` |
| Animation loop | `GLAnimCanvas` auto-redraw thread | manual timer | `GameWindow` render loop |

Key structural change: OpenTK uses **strongly-typed enums** instead of the loose
`int` GL constants. `glBegin(GL_QUADS)` becomes `GL.Begin(PrimitiveType.Quads)`,
`glEnable(GL_LIGHTING)` becomes `GL.Enable(EnableCap.Lighting)`, and so on. This
catches a whole class of "wrong constant in the wrong call" bugs at compile time —
several of which actually exist in the originals (see below).

## The one setting you must not miss: the context profile

Immediate mode (`glBegin/glEnd`), the matrix stack (`glOrtho`, `glRotatef`),
fixed-function lighting (`glLight*`, `glMaterial*`) and `GL_QUADS` were all
**removed from core OpenGL 3.1+**. OpenTK defaults a `GameWindow` to a *Core*
profile, where every one of those calls fails.

The fix (in `Core/ExerciseWindow.cs`) is to request a **compatibility profile**:

```csharp
new NativeWindowSettings
{
    Profile = ContextProfile.Compatability,   // note OpenTK's spelling
    APIVersion = new Version(3, 3),
}
```

On the dev machine this yields a `4.6.0 Compatibility Profile Context`, under
which all the legacy code runs unchanged. This is the single most important
difference when moving old GL code to OpenTK.

## Default-state differences that change what you see

Moving bindings can silently change the picture even when every call you wrote is
identical, because the **default GL state** you started from differs. The ones
that mattered here:

- **Back buffer alpha.** The originals clear to `glClearColor(0,0,0,0)` — alpha 0.
  On screen this is invisible, but when we grab the framebuffer to a PNG
  (`--capture`) those pixels are fully transparent. The capture code therefore
  forces the saved alpha opaque. (A real gotcha if you ever blend against the
  clear colour.)
- **Lighting defaults are the same — rely on them deliberately.** OpenGL's
  documented defaults (`LIGHT0`/material) are identical across GL4Java, CSOpenGL
  and OpenTK: `GL_LIGHT0` diffuse/specular default to white, material diffuse to
  grey `(0.8,0.8,0.8)`, light-model ambient to `(0.2,0.2,0.2)`. Several exercises
  *depend* on these (e.g. Ex 6.2 / 7.3 never set a diffuse material and render in
  the default grey). They are preserved exactly, so the look is unchanged.
- **`glShadeModel` default is `GL_SMOOTH`.** The flat-shaded exercise (Ex 7.1)
  sets `GL_FLAT` explicitly; everything else leans on the smooth default.
- **Normalisation.** The hand-tessellated teapot is scaled at draw time, so its
  normals are re-normalised with `GL_NORMALIZE` to keep the lighting correct —
  GLUT used to hand you unit-length normals for free.

## What had to be re-implemented (GLUT / GLU had no OpenTK equivalent)

- **`Core/Glut.cs`** — `WireCube`/`SolidCube`, `WireSphere`/`SolidSphere`,
  `WireIcosahedron`/`SolidIcosahedron`. Drawn in immediate mode with outward
  normals so the lit exercises shade identically.
- **`Core/Teapot.cs`** — the Utah teapot, evaluated from Newell's 32 bicubic
  Bézier patches (`Core/Data/teapotrim.bpt`). GLUT's teapot was a black box; here
  the patch data is tessellated and the surface normals come from the analytic
  partial derivatives. As GLUT did, the raw Z-up data is rotated Y-up and centred.
- **`Core/Glu.cs`** — `Perspective` (via `glFrustum`) and `LookAt` (builds the
  view matrix and `glMultMatrix`es it), matching the classic GLU implementations.
- **`Core/Texture.cs`** — replaces GL4Java's `PngTextureLoader` (Ex 13) using
  `StbImageSharp` to decode the PNG before `glTexImage2D`.

## Bugs in the originals, preserved-as-behaviour

Porting to typed enums surfaced a few invalid calls in the original Java that
were silent no-ops in real GL. They are intentionally **not** reproduced (they
did nothing), but are flagged in code comments:

- `glMateriali(GL_FRONT, GL_SPECULAR, 25)` (Ex 6.2, 7.3) — specular is a
  4-component colour, not a scalar; invalid enum/value, no-op.
- `glLighti(GL_LIGHT0, GL_SHININESS, 25)` (Ex 8, 9, 10, 11) — shininess is a
  material property, not a light one; no-op.
- A 5-element material array in Ex 12.x — only the first four floats are read.

The visible result is identical to the originals; the difference is that these
mistakes are now obvious instead of hidden.
