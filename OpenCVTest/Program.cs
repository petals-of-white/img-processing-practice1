using System.Drawing;
using static Logic.BasicImgOperations;
using static Logic.KvantDiskr;
using static Logic.Utils;


// Створюємо матрицю 5 x 5
byte [,] matrix = Default5x5Matrix;

// Створюємо палітру кольорів
byte [,,] map = GenerateColormap(25);

// Застосовуємо палітру
byte [,,] matrixWithColorMap = ApplyColormap(matrix, map);

// Генеруємо зображення згідно з варіянтом 2
byte [,,] blackWhiteSquares = CreateBlackWhiteSquares();

// завантажуємо зображення з теки img
var skullBitmap = new Bitmap("img/skull.jpg");

// перетворюємо його на тривимірну матрицю BGR
byte[,,] skullMatrix = ConvertImageToBgr(skullBitmap);

// Виконуємо квантування
byte [] coloursThresholds = {10, 50, 100, 200, 230 };
byte[,,] skullMatrixQuantized = QuantizeImg(skullMatrix, coloursThresholds);

// Дискретизація
Size sampleBlockSize = new(16, 16);
byte [,,] skullMatrixSampled = SampleImg(skullMatrix, sampleBlockSize);


ShowImg("Матриця 5x5 (збільшена) з користувацькою палітрою", matrixWithColorMap, 100, 100);
ShowImg("Чорно-білі квадрати", blackWhiteSquares, 5, 5);

ShowImg("Череп (до квантування)", skullMatrix, null, null);
ShowImg($"Череп (після квантування). Рівні квантуванні - {{{string.Join(",", coloursThresholds)}}}", skullMatrixQuantized, null, null);

ShowImg($"Череп (після дискретизації). Розмір блоку дискретизації - {{{sampleBlockSize.Height},{sampleBlockSize.Width}}}", skullMatrixSampled, null, null);

HideAll();

