using System.Drawing;
using Microsoft.Extensions.Configuration;

namespace Main;

/// <summary>
/// Клас для зчитування даних з .json файлу
/// </summary>
internal static class InputGetter
{
    private static readonly IConfigurationRoot _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("settings.json").Build();

    /// <summary>
    /// Матриця
    /// </summary>
    public static byte [,] Matrix
    {
        get
        {
            var jaggedMatrix = _config.GetSection("Matrix").Get<byte [] []>();

            var length = jaggedMatrix.Length;
            var matrix = new byte [length, length];

            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < length; j++)
                {
                    matrix [i, j] = jaggedMatrix [i] [j];
                }
            }
            return matrix;
        }
    }

    /// <summary>
    /// Шлях до файлу з черепом
    /// </summary>
    public static string PathToSkull { get => _config.GetValue<string>("PathToSkull"); }

    /// <summary>
    /// Розмір блоків дискретизації
    /// </summary>
    public static Size SampleBlockSize { get => _config.GetSection("SampleBlockSize").Get<Size>(); }

    /// <summary>
    /// Рівні квантування - пороги кольорів
    /// </summary>
    public static byte [] ColoursThreshold { get => _config.GetSection("ColoursThresholds").Get<byte []>(); }
}
