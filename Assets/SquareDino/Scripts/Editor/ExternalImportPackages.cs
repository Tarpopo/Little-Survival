using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace SquareDino.Scripts
{
    public class ExternalImportPackages
    {
        private static Action<Boolean> DownloadsComplete;
        private delegate void HandlePackageComplete(Boolean error);
        
        public static void ImportPackage(string url)
        {
            var tempFile = Application.persistentDataPath + ".unitypackage"; //FileUtil.GetUniqueTempPathInProject();
            var dialogMessage = "Downloading and import " + Path.GetFileName(url);
            
            DownloadPackage(url, dialogMessage, tempFile, error =>
            {
                if (!error)
                {
                    AssetDatabase.ImportPackage(tempFile, true);
                    FileUtil.DeleteFileOrDirectory(tempFile);
                }
                else
                {
                    DownloadsComplete(true);
                }
            });
        }

        private static void DownloadPackage(string dependencyUrl, string dialogMessage, string destFile, HandlePackageComplete callback)
        {
            var lastProgress = 0.0f;
            var www = new WWW(dependencyUrl);
            
            EditorApplication.CallbackFunction checkDownload = null;
            checkDownload = () =>
            {
                if (!www.isDone)
                {
                    // Only update if this is the first update or we have made progress
                    if (lastProgress < 0.01f || (www.progress - lastProgress > 0.01f))
                    {
                        lastProgress = www.progress;
                        if (EditorUtility.DisplayCancelableProgressBar("Download package", dialogMessage, www.progress))
                        {
                            Debug.Log("Import cancelled by user.");
                            www.Dispose();
    
                            EditorUtility.ClearProgressBar();
                            EditorApplication.update -= checkDownload;
                            callback(true);
                        }
                    }
                }
                else
                {
                    EditorUtility.ClearProgressBar();
    
                    if (!String.IsNullOrEmpty(www.error))
                    {
                        EditorUtility.DisplayDialog("Download package",
                            "Unable to download " + dependencyUrl + ": " + System.Environment.NewLine + www.error,
                            "OK");
                        // Remove ourselves
                        EditorApplication.update -= checkDownload;
                        callback(true);
                    }
                    else
                    {
                        // Save package
                        File.WriteAllBytes(destFile, www.bytes);
                        
                        // Remove ourselves
                        EditorApplication.update -= checkDownload;
                        callback(false);
                    }
                }
            };
    
            // Check download each update cycle
            EditorApplication.update += checkDownload;
        }
    }   
}