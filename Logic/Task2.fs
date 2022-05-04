module Logic.KvantDiskr

open Emgu.CV
open Emgu.Util
open Emgu.CV.Structure
open System.Drawing

/// <summary>Функція квантування зображення</summary>
/// <param name="image">Тривимірна матриця зображення, BGR</param>
/// <param name="coloursThreshholds">Рівні квантування</param>
/// <returns>Матриця після квантування, BGR</returns>
let QuantizeImg (image: byte [,,]) coloursThreshholds =
    let sourceImg = new Image<Bgr, byte>(image.Clone() :?> byte [,,])

    /// Основна функція, що визначає, як само треба змінити колір при квантуванні.
    let reduceColor (color: byte) =

        let makeUniqueAndSort = set >> Seq.sort

        let uniqueSortedThreshholds = makeUniqueAndSort coloursThreshholds

        let reducedColour =
            let potentialColour =
                uniqueSortedThreshholds
                |> Seq.tryFind (fun c -> (c > color))

            match potentialColour with
            | Some (c) -> c
            | None -> 255uy

        reducedColour

    // "Понижаємо" колір для кожного пікселя
    for r = 0 to sourceImg.Rows - 1 do
        for c = 0 to sourceImg.Cols - 1 do
            for channel = 0 to sourceImg.NumberOfChannels - 1 do
                sourceImg.Data[ r, c, channel ] <- reduceColor sourceImg.Data[r, c, channel]

    sourceImg.Data


/// <summary>Функція дискретизації зображення</summary>
/// <param name="image">Тривимірна матриця зображення, BGR</param>
/// <param name="sampleBlockSize">Розмір блоку дискретизації</param>
/// <returns>Матриця після дискретизації, BGR</returns>
let SampleImg (image: byte [,,]) (sampleBlockSize: Size) =
    let blockHeight, blockWidth = sampleBlockSize.Height, sampleBlockSize.Width

    /// Визначимо, яка насправді потрібна ширина/висота матриці з урахуванням розміру блоку дискретизації
    let rows, cols =
        let adjustDimLength dimLen sampleSize =
            let requiredLength =
                match dimLen % sampleSize with
                | 0 -> dimLen
                | _ -> dimLen / sampleSize * sampleSize + sampleSize

            requiredLength

        // випралені висота і ширина
        let r = adjustDimLength (image.GetLength 0) sampleBlockSize.Height
        let c = adjustDimLength (image.GetLength 1) sampleBlockSize.Width
        r, c

    let img = new Image<Bgr, byte>(image.Clone() :?> byte [,,])

    // Виділяємо кожний прямокутник регіон зображення
    for r = 0 to rows / sampleBlockSize.Height - 1 do
        for c = 0 to cols / sampleBlockSize.Width - 1 do

            // Визначимо прямокутний регійон
            let region =
                new Rectangle(new Point(blockWidth * c, blockHeight * r), sampleBlockSize)

            // Установимо Region of Interest для зображення
            img.ROI <- region

            // Середнє значення кольору для виділеного регійону
            let mean = CvInvoke.Mean(img)

            // Перетворимо на масив byte
            let meanArray =
                CvInvoke.Mean(img).ToArray()[..2]
                |> Array.collect (fun v -> [| (byte v) |])

            // Установлюємо значення кожного піксель виділеного регійону середнім значенням mean
            for blockRow = 0 to img.Mat.Rows - 1 do
                for blockCol = 0 to img.Mat.Cols - 1 do
                    img
                        .Mat
                        .Row(blockRow)
                        .Col(blockCol)
                        .SetTo(meanArray)

    CvInvoke.cvResetImageROI (img.Ptr)

    let outputArray = img.Data.Clone() :?> byte [,,]

    outputArray
