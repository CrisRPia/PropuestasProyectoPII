namespace TrucoTesting;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

class Program
{
    static void Main(string[] args)
    {
        using Image<Argb32> canvas = new(1100, 1000);
        using Image card = Image.Load("./assets/CardBack.png");
        using Image sword = Image.Load("./assets/espada/12.png");

        var start = DateTime.Now;
        canvas.Mutate(
            (IImageProcessingContext x) =>
            {
                x.BackgroundColor(Color.FromRgb(22, 22, 22));

                // Opposing cards
                for (int i = 0; i < 2; i++)
                {
                    Console.WriteLine(i);
                    var temp = card.Clone(y => { });
                    temp.Mutate(
                        x =>
                        {
                            x.Resize((int)(card.Width * 0.9), (int)(card.Height * 0.9));
                            x.Rotate(5 * i);
                        }
                    );
                    Point p = new(card.Width / 3 * (i + 6), 10);
                    Console.WriteLine(p.X);
                    DrawImageExtensions.DrawImage(x, temp, p, 1);
                }

                // Own cards
                for (int i = 1; i >= -1; i--)
                {
                    Console.WriteLine(i);
                    var temp = sword.Clone(y => { });
                    temp.Mutate(
                        (x) =>
                        {
                            x.Rotate(5 * i);
                        }
                    );
                    Point p =
                        new(
                            i == -1
                                ? 0
                                : i == 0
                                    ? card.Width
                                    : (int)(card.Width * 1.9),
                            500
                        );
                    Console.WriteLine(p.X);
                    DrawImageExtensions.DrawImage(x, temp, p, 1);
                }

                // Blur opposing cards
                x.GaussianBlur(
                    5,
                    new Rectangle(new Point(0, 0), new Size(canvas.Width, canvas.Height / 2))
                );

                var lastPlayed = sword.Clone(y => { });

                lastPlayed.Mutate(x =>
                {
                    x.Resize((int)(card.Width * 0.66), (int)(card.Height * 0.66));
                });

                Point lp = new(200, 50);
                DrawImageExtensions.DrawImage(x, lastPlayed, lp, 1);
            }
        );

        Console.WriteLine($"Processing time: {DateTime.Now - start}");

        canvas.SaveAsPng("./card.png");
    }
}
