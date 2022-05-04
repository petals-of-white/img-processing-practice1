module Logic.Utils

open Emgu.CV
open Emgu.CV.Structure
open Emgu.CV.CvEnum
open System.Drawing

/// <summary>Масштабує матрицю</summary>
/// <param name="matrix">вхідна матриця</param>
/// <param name="fx">Значення масштабування за віссю абсцис</param>
/// <param name="fy">Значення масштабування за віссю ординат</param>
/// <returns>Масштабована матриця</returns>
let ScaleMatrix (matrix: byte [,,]) fx fy =
    let sourceMatrix = new Image<Bgr, byte>(matrix)

    let destMatrix = new Mat()

    CvInvoke.Resize(sourceMatrix, destMatrix, new Size(), fx, fy, Inter.Area)

    destMatrix.GetData().Clone() :?> byte [,,]


/// <summary>Конвертує зображення типу Bitmap в тривимірну матрицю BGR</summary>
/// <param name="bitmapImg">Ухідне зобраежння</param>
/// <returns>Матриця BGR</returns>
let ConvertImageToBgr (bitmapImg: Bitmap) =
    let img = bitmapImg.ToImage<Bgr, byte>()
    img.Data


/// <summary>Допоміжна функція для відображення на екран тривимірних масивів як зображень</summary>
/// <param name="title">Підпис зображення</param>
/// <param name="imgMatrixSource">Матриця зображення</param>
/// <param name="fx">Масштабування за віссю x</param>
/// <param name="fy">Масштабування за віссю y</param>
let ShowImg title (imgMatrixSource: byte [,,]) (fx: double option) (fy: double option) =
    let imgMatrix = imgMatrixSource.Clone() :?> byte [,,]

    // Перевірка на масштабування
    match (fx, fy) with
    | (Some x, Some y) when (x <> 0.0 && y <> 0.0) ->
        let img = new Image<Bgr, byte>(ScaleMatrix imgMatrix x y)
        CvInvoke.Imshow(title, img)
    | _ ->
        let img = new Image<Bgr, byte>(imgMatrix)
        CvInvoke.Imshow(title, img)

/// <summary>Приховує усі виведені зображення</summary>
let HideAll () =
    let key = CvInvoke.WaitKey()
    CvInvoke.DestroyAllWindows()
