using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace UnityCompilationDebugger
{
	
    public class UnityCompilationDebugWindow : EditorWindow
    {
        private static string reportJson;
        private static CompilationReport report;
        private bool logEnabled;
        private bool sfxEnabled;
        private GUIStyle toogleStyle;

        [MenuItem("K10/Unity Compilation Debugger")]
        private static void Init()
        {
            var window = (UnityCompilationDebugWindow) GetWindow(typeof(UnityCompilationDebugWindow), false, "Unity Compilation Debugger");
            window.Show();
        }

        [DidReloadScripts]
        private static void OnReload()
        {
            reportJson = EditorPrefs.GetString(UnityCompilationDebug.CompilationReportEditorPref);
        }

        private static CompilationReport GenerateReport(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonUtility.FromJson<CompilationReport>(json);
        }

        private void OnEnable()
        {
            logEnabled = EditorPrefs.GetBool(UnityCompilationDebug.LogEnabledPref, true);
            sfxEnabled = EditorPrefs.GetBool(UnityCompilationDebug.SfxEnabledPref, false);
            toogleStyle = new GUIStyle( EditorStyles.textArea ) {

            };
        }

        private void OnGUI()
        {
            if (report == null)
            {
                report = GenerateReport(reportJson);
            }

            if (report == null)
            {
                EditorGUILayout.HelpBox(
                    "No compilation report found. Modify a script to trigger a recompilation", MessageType.Warning);
                return;
            }

            GUILayout.Label("Post Compilation Report", toogleStyle);

            var date = DateTime.FromBinary(report.reloadEventTimes);
            var totalTimeSeconds = report.compilationTotalTime + report.assemblyReloadTotalTime;

            EditorGUILayout.TextField("Compilation Report", $"{totalTimeSeconds:F2} seconds", EditorStyles.boldLabel);

			EditorGUILayout.FloatField( "Compilation Time", (float)report.compilationTotalTime, EditorStyles.boldLabel );
			EditorGUILayout.FloatField( "Assembly Reload Time", (float)report.assemblyReloadTotalTime, EditorStyles.boldLabel );

            EditorGUILayout.Space( 5 );

            GUILayout.Label( "Print compilation time after reload", toogleStyle );
            var enableLog = EditorGUILayout.ToggleLeft("Use Debug.Log", logEnabled);
            
            EditorGUILayout.Space( 5 );
            
            GUILayout.Label( "Play SFX after compilation over", toogleStyle );
            var enableSfx = EditorGUILayout.ToggleLeft("Play SFX", logEnabled);

            if (logEnabled != enableLog)
            {
                EditorPrefs.SetBool(UnityCompilationDebug.LogEnabledPref, enableLog);
                logEnabled = enableLog;
            }

            if (sfxEnabled != enableSfx)
            {
                EditorPrefs.SetBool(UnityCompilationDebug.SfxEnabledPref, enableSfx);
                sfxEnabled = enableSfx;
            }
        }
    }
}