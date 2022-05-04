using System.Drawing;
using Main;
using static Logic.BasicImgOperations;
using static Logic.KvantDiskr;
using static Logic.Utils;

// Створюємо матрицю 5 x 5
byte[,] matrix = InputGetter.Matrix;

// Знаходимо максимальне значення в матриці. Для нашої задачі - це 25
byte maxValue = (from byte b in matrix select b).Max();

/*
 *  Створюємо палітру кольорів, указуючи максимальне значення,
 *  до якого треба масштабувати палітру
*/
byte[,,] map = GenerateColormap(scaleToMaxValue: maxValue);

// Застосовуємо палітру
byte[,,] matrixWithColorMap = ApplyColormap(matrix, map);

// Генеруємо зображення згідно з варіянтом 2
byte[,,] blackWhiteSquares = CreateBlackWhiteSquares();

// завантажуємо зображення з теки img
Bitmap skullBitmap = new(InputGetter.PathToSkull);

// перетворюємо його на тривимірну матрицю BGR
byte[,,] skullMatrix = ConvertImageToBgr(skullBitmap);

// Задаємо рівня квантування - "пороги" значень кольорів від 0 до 255
byte[] coloursThresholds = InputGetter.ColoursThreshold;

// Виконуємо квантування
byte[,,] skullMatrixQuantized = QuantizeImg(skullMatrix, coloursThresholds);

// Дискретизація
Size sampleBlockSize = InputGetter.SampleBlockSize;
byte[,,] skullMatrixSampled = SampleImg(skullMatrix, sampleBlockSize);

// Виведемо всі зображення fx і fy вказують, у скільки раз треба масштабувати зображення для кращої візуалізації
ShowImg("Палітра кольорів: ", map, fx: 30, fy: 20);
ShowImg("Матриця 5x5 (збільшена) з користувацькою палітрою", matrixWithColorMap, fx: 100, fy: 100);
ShowImg("Чорно-білі квадрати", blackWhiteSquares, fx: 5, fy: 5);

ShowImg("Череп (оригінал)", skullMatrix, fx: null, fy: null);

string coloursString = "{" + string.Join(",", coloursThresholds) + "}";
ShowImg($"Череп (після квантування). Рівні квантуванні - {coloursString}", skullMatrixQuantized, fx: null, fy: null);

string sampleBlockStr = $"{{{sampleBlockSize.Height},{sampleBlockSize.Width}}}";
ShowImg($"Череп (після дискретизації). Розмір блоку дискретизації - {sampleBlockSize}", skullMatrixSampled, null, null);

HideAll();
