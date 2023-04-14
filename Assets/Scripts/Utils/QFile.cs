using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Experimental.Rendering;

namespace Utils
{
    public class QFile
    {
        public static string textureRelativePath = "Assets/Texture/";
        public static string txtRelativePath = "Assets/Plugins/WalkerAliasRandomizer/";
        public static string ResourcePath = "Assets/Resources/";


        public static Texture2D ColorStream2Texture(int width, int height, Color[] colorStream)
        {
            if (width <= 0 || height <= 0)
                return null;

            Texture2D texture = new Texture2D(width, height);

            texture.SetPixels(colorStream);
            texture.Apply();

            return texture;
        }

        public static Texture2D FloatArray2Texture2D(int width, int height, float[] color)
        {
            Color[] colors = new Color[width * height];
            for (int i = 0; i < color.Length; i++)
                colors[i] = new Color(color[i], color[i], color[i], 1);

            Texture2D texture = ColorStream2Texture(width, height, colors);

            return texture;
        }

        public static Texture2D IntArray2Texture2D(int width, int height, int[] color)
        {
            Color[] colors = new Color[width * height];
            for (int i = 0; i < color.Length; i++)
                colors[i] = new Color(color[i], color[i], color[i], 1);

            Texture2D texture = ColorStream2Texture(width, height, colors);

            return texture;
        }

        public static Texture2D RtTexture2Texture2D(RenderTexture renderTexture)
        {
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();
            RenderTexture.active = null;
            return texture;
        }

        public static void SaveTexture2D(Texture2D texture, string filename)
        {
            byte[] bytes = texture.EncodeToPNG();
            UnityEngine.Object.Destroy(texture);
            System.IO.File.WriteAllBytes(textureRelativePath + filename, bytes);
            Debug.Log("write to File over");
            UnityEditor.AssetDatabase.Refresh();
        }

        public static void SaveRenderTexture(RenderTexture rtTexture, string filename)
        {
            Texture2D texture = RtTexture2Texture2D(rtTexture);
            SaveTexture2D(texture, filename);
        }

        public static void SaveListToTextFile(List<int> list, string txtFileName)
        {
            FileStream fs = new FileStream(txtRelativePath + txtFileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < list.Count; i++) sw.WriteLine(list[i]);
            //关闭此文件t 
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static void SaveListToTextFile(List<float> list, string txtFileName)
        {
            FileStream fs = new FileStream(txtRelativePath + txtFileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < list.Count; i++) sw.WriteLine(list[i]);
            //关闭此文件t 
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static void SaveListToTextFile(List<string> list, string txtFileName)
        {
            FileStream fs = new FileStream(txtRelativePath + txtFileName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < list.Count; i++) sw.WriteLine(list[i]);
            //关闭此文件t 
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static void SaveArrayToTextFile(string[] array, string txtFileName)
        {
            List<string> list = new();
            list.AddRange(array);

            SaveListToTextFile(list, txtFileName);
        }

        public static void SaveArrayToTextFile(float[] array, string txtFileName)
        {
            List<float> list = new();
            list.AddRange(array);

            SaveListToTextFile(list, txtFileName);
        }

        public static float[] ReadFloatBinaryFile(string FileName, int arraySize)
        {
            float[] floatArray = new float[arraySize];
            using (BinaryReader reader = new BinaryReader(File.Open(ResourcePath + FileName, FileMode.Open)))
            {
                int i = 0;
                int length = (int)reader.BaseStream.Length;
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                while (reader.BaseStream.Position < length)
                {
                    // 3.
                    // Read integer.
                    float v = reader.ReadSingle();
                    floatArray[i] = v;
                    i++;
                }
            }
            return floatArray;
        }

        public static int[] ReadIntBinaryFile(string FileName, int arraySize)
        {
            int[] intArray = new int[arraySize];
            using (BinaryReader reader = new BinaryReader(File.Open(ResourcePath + FileName, FileMode.Open)))
            {
                int i = 0;
                int length = (int)reader.BaseStream.Length;
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                while (reader.BaseStream.Position < length)
                {
                    // 3.
                    // Read integer.
                    int v = reader.ReadInt32();
                    intArray[i] = v;
                    i++;
                }
            }
            return intArray;
        }
    }
}