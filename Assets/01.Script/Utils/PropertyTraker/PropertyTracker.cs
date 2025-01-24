using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Hashira.Utils.PropertyTracker
{
    public static class PropertyTracker
    {
        public static void TrackChange<T>(string propertyName, T oldValue, T newValue, UnityEngine.Object context)
        {
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrames()?.FirstOrDefault(f =>
                f.GetFileLineNumber() != 0 &&
                !f.GetMethod().Name.Contains("TrackChange"));

            string fileName = frame?.GetFileName() ?? "Unknown";
            int lineNumber = frame?.GetFileLineNumber() ?? 0;

            fileName = Path.GetFileName(fileName);

            UnityEngine.Debug.Log($"[Property Changed] {context.GetType().Name}.{propertyName}\n" +
                                $"Old: {oldValue} -> New: {newValue}\n" +
                                $"at {fileName}:line {lineNumber}");
        }
    }
}
