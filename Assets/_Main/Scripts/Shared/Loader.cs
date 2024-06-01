using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class StreamingAssetHelper
{
    public static string GetPath(string fileName)
    {

        //Create an array of file paths from which to choose
        string folderPath = Application.streamingAssetsPath + "/";  //Get path of folder
        //string[] filePaths = Directory.GetFiles(folderPath, "*.png"); // Get all files of type .png in this folder
        string path = folderPath + fileName;

        return path;
    }

    public static Sprite GetSpriteFromStreamingAsset(string imgPath)
    {
        imgPath = GetPath(imgPath);
        return Loader.LoadSprite(imgPath);
    }
    public static Texture GetTextureFromStreamingAsset(string imgPath)
    {
        imgPath = GetPath(imgPath);
        return Loader.LoadTexture(imgPath);
    }
    public static string GetUniqueFilename(string fullPath)
    {
        if (!Path.IsPathRooted(fullPath))
            fullPath = Path.GetFullPath(fullPath);
        if (File.Exists(fullPath))
        {
            String filename = Path.GetFileName(fullPath);
            String path = fullPath.Substring(0, fullPath.Length - filename.Length);
            String filenameWOExt = Path.GetFileNameWithoutExtension(fullPath);
            String ext = Path.GetExtension(fullPath);
            int n = 1;
            do
            {
                fullPath = Path.Combine(path, String.Format("{0} ({1}){2}", filenameWOExt, (n++), ext));
            }
            while (File.Exists(fullPath));
        }
        return fullPath;
    }
    public static string RemoveSubString(string sourceString, string subString)
    {


        int index = sourceString.IndexOf(subString);
        string cleanPath = (index < 0)
            ? sourceString
            : sourceString.Remove(index, subString.Length);

        return cleanPath;
    }
    public static string ImportSymbolToStreamingAssets(string source, string destination)
    {
        if (source == string.Empty)
        {
            return string.Empty;
        }

        var returnPath = "Images/Symbols/" + destination;
        var path = GetPath(returnPath);
        if (source == path)
        {
            return RemoveSubString(source, (Application.streamingAssetsPath + "/"));
        }
        path = GetUniqueFilename(path);
        Loader.ExportFromDirToDir(source, path);
        return RemoveSubString(path, (Application.streamingAssetsPath + "/"));
    }
    public static string ImportImageToStreamingAssets(string source, string destination)
    {
        if (source == string.Empty || !File.Exists(source))
        {
            return string.Empty;
        }

        var returnPath = "Images/" + destination;
        var path = GetPath(returnPath);
        if (source == path)
        {
            return RemoveSubString(source, (Application.streamingAssetsPath + "/"));
        }
        path = GetUniqueFilename(path);
        Loader.ExportFromDirToDir(source, path);
        return RemoveSubString(path, (Application.streamingAssetsPath + "/"));
    }
    public static string ImportVideoToStreamingAssets(string source, string destination)
    {
        if (source == string.Empty || !File.Exists(source))
        {
            return string.Empty;
        }
        var returnPath = "Videos/" + destination;
        var path = GetPath(returnPath);
        Loader.ExportFromDirToDir(source, path);
        return returnPath;
    }
    public static void DeleteFile(string file)
    {
        Debug.LogError("Delete File: " + file);
        string tmp = file;
        file = GetPath(file);
        if (File.Exists(file))
        {
            File.Delete(file);
        }
        else
        {
            Loader.DeleteFile(tmp);
        }
    }
    public static void SaveJsonToFile(string json, string file)
    {
        file = GetPath(file);
        if (File.Exists(file))
        {
            File.WriteAllText(file, json);
        }
    }
    public static string ReadJsonFromFile(string file)
    {
        file = GetPath(file);
        if (File.Exists(file))
        {
            return File.ReadAllText(file);
        }
        return "";
    }
}
public static class Loader
{
    static readonly ExtensionFilter[] IMAGE_EXTENSIONS = new[] {
    new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
    };
    static readonly ExtensionFilter[] VIDEO_EXTENSIONS = new[] {
    new ExtensionFilter("Video Files", "mp4", "m4v", "mpeg", "webm" ),
    };
    public static bool OpenMediaFileFromDevice(bool isVideo, out string path)
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open file", "", isVideo ? VIDEO_EXTENSIONS : IMAGE_EXTENSIONS, false);
        if (paths.Length > 0)
        {
            path = paths[0];
            if(File.Exists(path))
            {
                return true;
            }
        }

        path = "";
        return false;
    }
    public static Sprite LoadSprite(string file)
    {
        if (!File.Exists(file))
        {
            return null;
        }
        //Converts desired path into byte array
        byte[] pngBytes = System.IO.File.ReadAllBytes(file);

        //Creates texture and loads byte array data to create image
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(pngBytes);

        //Creates a new Sprite based on the Texture2D
        Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        return fromTex;
    }
    public static Texture2D LoadTexture(string file)
    {
        if (!File.Exists(file))
        {
            return null;
        }
        //Converts desired path into byte array
        byte[] pngBytes = System.IO.File.ReadAllBytes(file);

        //Creates texture and loads byte array data to create image
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(pngBytes);

        return tex;
    }
    public static Texture LoadVideo(string file)
    {
        return null;
    }
    public static void DeleteFile(string file)
    {
        if (File.Exists(file))
        {
            File.Delete(file);
        }
    }

    public static void ExportFromDirToDir(string source, string destination)
    {
        if (!File.Exists(source))
        {
            Debug.LogError("Source not existed");
            return;
        }
        if (!File.Exists(destination))
            File.Copy(source, destination);
        else
        {
            Debug.LogWarning("File have already Existed!");
        }
    }
}
