using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace LearnOpenGL;

public class Program
{
    private static void Main(string[] args)
    {
        var gws = GameWindowSettings.Default;
        gws.UpdateFrequency = 60.0;

        var nws = NativeWindowSettings.Default;
        nws.ClientSize = new Vector2i(800, 600);
        nws.Title = "Learning OpenGL";

        using (var game = new Game(gws, nws))
        {
            game.Run();
        }
    }
}
