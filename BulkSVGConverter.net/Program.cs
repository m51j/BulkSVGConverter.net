using System;
using System.IO;

using ExCSS;

using SkiaSharp;

using Svg.Skia;

class Program
{
    static void Main()
    {
        string sourceFolderPath = "Your images path";
        ConvertSvgToJpgInFolder(sourceFolderPath);
        Console.ReadLine();
    }

    static void ConvertSvgToJpgInFolder(string folderPath)
    {
        string[] svgFiles = Directory.GetFiles(folderPath, "*.svg", SearchOption.AllDirectories);

        foreach (var svgFilePath in svgFiles)
        {
            string jpgFilePath = Path.ChangeExtension(svgFilePath, ".jpg");

            ConvertSvgToJpg(svgFilePath, jpgFilePath);

            Console.WriteLine($"تم تحويل {Path.GetFileName(svgFilePath)} إلى {Path.GetFileName(jpgFilePath)}");
        }
    }

    static void ConvertSvgToJpg(string svgFilePath, string jpgFilePath)
    {

        var svg = new SKSvg();
        using (var inputStream = File.OpenRead(svgFilePath))
        {
            svg.Load(inputStream);
            var imageInfo = new SKImageInfo((int)svg.Picture.CullRect.Width*2, (int)svg.Picture.CullRect.Height*2);
            using (var surface = SKSurface.Create(imageInfo))
            using (var canvas = surface.Canvas)
            {
            
                // calculate the scaling need to fit to screen
                var scaleX = imageInfo.Width / svg.Picture.CullRect.Width;
                var scaleY = imageInfo.Height / svg.Picture.CullRect.Height;
                var matrix = SKMatrix.CreateScale((float)scaleX, (float)scaleY);
                try
                {
                    File.Delete(jpgFilePath);
                }
                catch (Exception)
                {

                  
                }
                // draw the svg
                canvas.Clear(SKColors.Transparent);
                canvas.DrawPicture(svg.Picture, ref matrix);
                canvas.Flush();

                using (var data = surface.Snapshot())
                using (var pngImage = data.Encode(SKEncodedImageFormat.Jpeg, 100))
                using (var stream = File.OpenWrite(jpgFilePath))
                {
                    pngImage.SaveTo(stream);
                }
               
            }
        }

      
       
    }
}
