module Logic.BasicImgOperations

open Emgu.CV
open Emgu.CV.Structure
open Emgu.CV.CvEnum
open System.Drawing
open System.Runtime.InteropServices


/// Матриця 5x5 за замовчуванням, якщо немає бажання створювати свою
let Default5x5Matrix =
    let matrix =
        array2D [ [| 1uy; 2uy; 3uy; 4uy; 5uy |]
                  [| 6uy; 7uy; 8uy; 9uy; 10uy |]
                  [| 11uy; 12uy; 13uy; 14uy; 15uy |]
                  [| 16uy; 17uy; 18uy; 19uy; 20uy |]
                  [| 21uy; 22uy; 23uy; 24uy; 25uy |] ]

    matrix

/// Словник-таблиця кольорів
let private colors =
    [ 0, Bgr(Color.Blue)
      1, Bgr(Color.Purple)
      2, Bgr(Color.Green)
      3, Bgr(Color.Crimson)
      4, Bgr(Color.LimeGreen)
      5, Bgr(Color.DimGray)
      6, Bgr(Color.Black)
      7, Bgr(Color.BurlyWood)
      8, Bgr(Color.Azure)
      9, Bgr(Color.RebeccaPurple)
      10, Bgr(Color.IndianRed)
      11, Bgr(Color.DarkCyan)
      12, Bgr(Color.AliceBlue)
      13, Bgr(Color.DarkMagenta)
      14, Bgr(Color.Silver) ]
    |> dict

let NumberOfColours = 15

/// <summary>Застосовує палітру на двовимірну матрицю</summary>
/// <param name="source">Вхідна матриця</param>
/// <param name="colorMap">Палітра</param>
/// <returns></returns>
let ApplyColormap (source: byte [,]) (colorMap: byte [,,]) =
    let sourceMatrix = new Matrix<byte>(source)
    let handle = GCHandle.Alloc(colorMap, GCHandleType.Pinned)

    try
        let pointer = handle.AddrOfPinnedObject()

        let colorMapMatrix =
            Mat(colorMap.GetLength(0), colorMap.GetLength(1), DepthType.Cv8U, 3, pointer, 3)

        // colorMapMatrix.GetData()

        let destMatrix = Mat()

        // Застосуємо палітру
        CvInvoke.ApplyColorMap(sourceMatrix, destMatrix, colorMapMatrix)

        destMatrix.GetData() :?> byte [,,]

    finally
        if handle.IsAllocated then handle.Free()


/// <summary>
/// Генерує палітру
/// (є можливість задати масштабування,
/// щось на зразок imagesc в MATLAB)
/// </summary>
/// <param name="scaleToMaxValue">
/// Необов'язково значення: указує певне максимальне значення в матриці,
/// до якої буде застосовано цю палітру
/// </param>
/// <returns>Палітра</returns>
let GenerateColormap scaleToMaxValue =
    let colorMapOutput = new Matrix<byte>(256, 1, 3)

    // Діяпазон "ділянки" під колір. Для 15 кольорів буде дорівнювати 17
    let colorRegion = 255 / NumberOfColours

    let setColor row =
        let pixelIndex = (row / colorRegion) % NumberOfColours

        colorMapOutput
            .GetRow(row)
            .SetValue colors[pixelIndex].MCvScalar

    // 0..255
    let colormapRows = seq { 0 .. colorMapOutput.Rows - 1 }

    // Задамо колір для кожного значення від 0 до 255
    Seq.iter setColor colormapRows

    match scaleToMaxValue with
    // Якщо вказано scaleToMaxValue
    | Some (x) ->

        // Змінимо палітру до відповідно до цього значення
        let scaledColormap = new Matrix<byte>(256, 1, 3)
        let possibleIndexes = seq { 0..x }

        let setColor row =
            let pixelIndex = 255 / (x + 1) * (row + 1)

            colorMapOutput
                .GetRow(pixelIndex)
                .CopyTo(scaledColormap.GetRow row)

        Seq.iter setColor possibleIndexes

        scaledColormap.Mat.GetData().Clone() :?> byte [,,]
    | None -> // Інакше повернути первинну палітру
        let smth = colorMapOutput.Mat.GetData().Clone() :?> byte [,,]
        smth

/// <summary>
/// Створює чорно-білі квадрати (відповідно до варіянту 2)
/// </summary>
/// </param>
/// <returns>Матрицю зображення з квадратами</returns>
let CreateBlackWhiteSquares () =
    let height, width = 100, 100
    let squaresImg = Mat.Zeros(height, width, DepthType.Cv8U, 3)

    // два прямокутники
    let rightTop = Rectangle(width / 2, 0, width / 2 - 1, height / 2)
    let leftBottom = Rectangle(0, height / 2, width / 2, height / 2 - 1)

    // білого кольору
    let color = Bgr(Color.White).MCvScalar

    // Намалюємо обидва
    CvInvoke.Rectangle(squaresImg, rightTop, color, thickness = -width / 2)
    CvInvoke.Rectangle(squaresImg, leftBottom, color, thickness = -width / 2)

    squaresImg.GetData().Clone() :?> byte [,,]
