// *********************************************************************************
// The MIT License (MIT)
// Copyright (c) 2020 SpiralBlack https://github.com/SpiralBlack15
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// *********************************************************************************

using System.IO;
using System.Linq;
using UnityEngine;

namespace Spiral.Core
{
    public static class TextureTools 
    {
        public static void KillTexture(this Texture texture, bool undo = false)
        {
            if (texture != null) texture.KillObject(undo);
        }

        public static Texture2D GetTexture2D(this RenderTexture renderTexture)
        {
            int width  = renderTexture.width;
            int height = renderTexture.height;
            Texture2D texture2D = new Texture2D(width, height);
            RenderTexture active = RenderTexture.active;
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = active;
            return texture2D;
        }

        public static void Rotate180(this Texture2D texture2D)
        {
            var pixels = texture2D.GetPixels32().Reverse();
            texture2D.SetPixels32(pixels.ToArray());
            texture2D.Apply();
        }

        public static void FlipXSave(this Texture2D texture2D)
        {
            int w = texture2D.width; int h = texture2D.height;
            var pixels = texture2D.GetPixels32();
            var pixels2 = new Color32[pixels.Length];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int posA = y * w + x;
                    int posB = y * w + (w - x);
                    pixels2[posA] = pixels[posB];
                }
            }
            texture2D.SetPixels32(pixels2);
            texture2D.Apply();
        }

        public static void FlipYSave(this Texture2D texture2D)
        {
            int w = texture2D.width; int h = texture2D.height;
            var pixels = texture2D.GetPixels32();
            var pixels2 = new Color32[pixels.Length];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int posA = y * w + x;
                    int posB = (h - y - 1) * w + x;
                    pixels2[posA] = pixels[posB];
                }
            }
            texture2D.SetPixels32(pixels2);
            texture2D.Apply();
        }


        public static void SaveTexture(this Texture2D texture, string path, string name)
        {
            byte[] bytes = texture.EncodeToPNG();
            string filepath = path + "/" + name + ".png";
            File.WriteAllBytes(filepath, bytes);
        }
    }
}
