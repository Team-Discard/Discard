#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;

namespace AssetPostProcessors
{
    public class CustomAssetProcessor : UnityEditor.AssetModificationProcessor
    {
        public static string[] OnWillSaveAssets(string[] paths)
        {
            var pathsToSave = new List<string>();

            for (int i = 0; i < paths.Length; ++i)
            {
                var info = new FileInfo(paths[i]);
                if (false)
                {
                    UnityEditor.EditorUtility.DisplayDialog("Can not save.",
                        "Sorry, but '" + paths[i] + "' is Read-Only! Please Save as another name.",
                        "Ok");
                }
                else
                {
                    pathsToSave.Add(paths[i]);
                }
            }

            return pathsToSave.ToArray();
        }
    }
}

#endif