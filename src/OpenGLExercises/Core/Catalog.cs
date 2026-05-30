namespace OpenGLExercises.Core;

/// <summary>One entry in the exercise menu.</summary>
/// <param name="Id">Short slug used on the command line (e.g. <c>ex1</c>, <c>spiral</c>).</param>
/// <param name="Title">Human-readable description.</param>
/// <param name="Category">Grouping shown in the menu.</param>
/// <param name="Factory">Creates the window for this exercise.</param>
public record ExerciseInfo(string Id, string Title, string Category, Func<ExerciseWindow> Factory);

/// <summary>
/// Central registry of every ported exercise. The launcher reads this to build
/// the menu and to resolve a command-line id. New ports are added in one place.
/// </summary>
public static class Catalog
{
    public static readonly IReadOnlyList<ExerciseInfo> All = new List<ExerciseInfo>
    {
        new("ex1", "GLUT wire cube (ortho)", "OpenGL course", () => new Exercises.Ex01WireCube()),
        new("ex2_1", "GLUT wire teapot (ortho)", "OpenGL course", () => new Exercises.Ex02_1WireTeapot()),
        new("ex2_2", "Hand-built vector cube", "OpenGL course", () => new Exercises.Ex02_2VectorCube()),
        new("ex4_1", "Multiple viewports", "OpenGL course", () => new Exercises.Ex04_1Viewports()),
        new("ex4_2", "Broken aspect ratio", "OpenGL course", () => new Exercises.Ex04_2BrokenAspect()),
        new("ex5", "Viewports and objects", "OpenGL course", () => new Exercises.Ex05ViewportsObjects()),
        new("ex6_1", "Solid lit cube", "OpenGL course", () => new Exercises.Ex06_1SolidLitCube()),
        new("ex6_2", "Solid lit sphere", "OpenGL course", () => new Exercises.Ex06_2SolidLitSphere()),
        new("ex7_1", "Flat-shaded sphere", "OpenGL course", () => new Exercises.Ex07_1FlatSphere()),
        new("ex7_2", "Smooth-shaded sphere", "OpenGL course", () => new Exercises.Ex07_2SmoothSphere()),
        new("ex7_3", "Solid lit teapot", "OpenGL course", () => new Exercises.Ex07_3SolidTeapot()),
        new("ex8", "Rotating spheres", "OpenGL course", () => new Exercises.Ex08RotatingSpheres()),
        new("ex9", "Rotating spheres and light", "OpenGL course", () => new Exercises.Ex09RotatingSpheresLight()),
        new("ex10_1", "Perspective via glFrustum", "OpenGL course", () => new Exercises.Ex10_1Frustum()),
        new("ex10_2", "Perspective via gluPerspective", "OpenGL course", () => new Exercises.Ex10_2Perspective()),
        new("ex11", "Perspective with camera", "OpenGL course", () => new Exercises.Ex11Camera()),
        new("ex12_1", "Surface normals", "OpenGL course", () => new Exercises.Ex12_1Normals()),
        new("ex12_2", "Changed normals", "OpenGL course", () => new Exercises.Ex12_2ChangedNormals()),
        new("ex13", "Texture mapping (monkey)", "OpenGL course", () => new Exercises.Ex13Monkey()),

        new("spiral", "Rotating Archimedean spirals", "2D sketches", () => new Exercises.Sketches.SketchSpiral()),
        new("square", "Colour-cycling square", "2D sketches", () => new Exercises.Sketches.SketchColorSquare()),
        new("triangle", "Interactive RGB triangle", "2D sketches", () => new Exercises.Sketches.SketchTriangle()),
        new("cardioid", "Cardioid curve", "2D sketches", () => new Exercises.Sketches.SketchCardioid()),
        new("sine", "Sine wave", "2D sketches", () => new Exercises.Sketches.SketchSine()),
        new("petal", "Rose / petal curve", "2D sketches", () => new Exercises.Sketches.SketchPetal()),
        new("polar", "Polar limaçon", "2D sketches", () => new Exercises.Sketches.SketchPolar()),
    };

    public static ExerciseInfo? Find(string id) =>
        All.FirstOrDefault(e => string.Equals(e.Id, id, StringComparison.OrdinalIgnoreCase));
}
