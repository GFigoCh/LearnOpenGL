namespace LearnOpenGL;

public class Program
{
    private static void Main(string[] args)
    {
        using (var game = new Game(800, 600, "LearnOpenGL"))
        {
            game.Run();
        }
    }
}
