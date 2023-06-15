


// 讀取 PNG 檔
using ImageMagick;

using (var image = new MagickImage(@"C:\Users\g9964\Desktop\1.png"))
{
    // 存成 JPG 檔
    image.Write(@"C:\Users\g9964\Desktop\1.avif");
}