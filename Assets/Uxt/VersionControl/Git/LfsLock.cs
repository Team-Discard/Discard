using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Uxt.VersionControl.Git
{
#if UNITY_EDITOR
    public static class LfsLock
    {
        public static async Task<List<string>> GetLockedFilesGuid()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\Program Files\\Git\\cmd\\git-lfs.exe",
                    CreateNoWindow = true,
                    WorkingDirectory = Application.dataPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    Arguments = "locks --json"
                };
                var git = Process.Start(processStartInfo);
                Debug.Assert(git != null);

                await Task.Run(() => { git.WaitForExit(); });

                var output = await git.StandardOutput.ReadToEndAsync();
                Debug.Log(output);

                return null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        [MenuItem("Git/Show Locked Files")]
        public static void ReadFiles()
        {
            GetLockedFilesGuid();
        }
    }
#endif
}