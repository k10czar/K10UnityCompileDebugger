using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityCompilationDebugger;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

// Based on https://gist.github.com/karljj1/9c6cce803096b5cd4511cf0819ff517b
[InitializeOnLoad]
public class UnityCompilationDebug
{
	const string SFXS_FOLDER = "SFXs/";
	const string compilationSuccessSfx = SFXS_FOLDER + "compileSuccess";
	const string compilationFailSfx = SFXS_FOLDER + "compileFail";

	internal const string PendingCompilationReportEditorPref = "PendingCompilationReportKey";
	internal const string CompilationReportEditorPref = "CompilationReportKey";
	internal const string LogEnabledPref = "AsmdefDebugLogKey";

	private static readonly CompilationReport CompilationReport = new CompilationReport();
	private static readonly Dictionary<object, DateTime> StartTimes = new Dictionary<object, DateTime>();

	private static double compilationTotalTime;

	private static string TOTAL_COLOR = "<color=#FFA500>";
	private static string COMPILATION_COLOR = "<color=#D0FF14>";
	private static string RELOAD_COLOR = "<color=#87CEFA>";
	private static string SIGN_COLOR = "<color=#F0E68C>";
	

	static UnityCompilationDebug()
	{
		CompilationPipeline.compilationStarted += CompilationPipelineOnCompilationStarted;
		CompilationPipeline.compilationFinished += CompilationPipelineOnCompilationFinished;
		AssemblyReloadEvents.beforeAssemblyReload += AssemblyReloadEventsOnBeforeAssemblyReload;
		AssemblyReloadEvents.afterAssemblyReload += AssemblyReloadEventsOnAfterAssemblyReload;
	}

	private static void CompilationPipelineOnCompilationStarted( object assembly )
	{
		StartTimes[assembly] = DateTime.UtcNow;
	}

	private static void CompilationPipelineOnCompilationFinished( object assembly )
	{
		var timeSpan = DateTime.UtcNow - StartTimes[assembly];
		compilationTotalTime += timeSpan.TotalMilliseconds;
	}

	private static void AssemblyReloadEventsOnBeforeAssemblyReload()
	{
		var totalCompilationTimeSeconds = compilationTotalTime / 1000f;
		CompilationReport.compilationTotalTime = totalCompilationTimeSeconds;
		CompilationReport.reloadEventTimes = DateTime.UtcNow.ToBinary();
		EditorPrefs.SetString( PendingCompilationReportEditorPref, JsonUtility.ToJson( CompilationReport ) );
	}

	private static void AssemblyReloadEventsOnAfterAssemblyReload()
	{
		var reportJson = EditorPrefs.GetString( PendingCompilationReportEditorPref );
		if( string.IsNullOrEmpty( reportJson ) ) return;
		EditorPrefs.DeleteKey( PendingCompilationReportEditorPref );

		var report = JsonUtility.FromJson<CompilationReport>( reportJson );

		var date = DateTime.FromBinary( report.reloadEventTimes );
		report.assemblyReloadTotalTime = ( DateTime.UtcNow - date ).TotalSeconds;

		EditorPrefs.SetString( CompilationReportEditorPref, reportJson );

		if( !EditorPrefs.GetBool( LogEnabledPref, true ) ) return;

		var totalTimeSeconds = report.compilationTotalTime + report.assemblyReloadTotalTime;
		Debug.Log( $"📝 Compilation Report: {TOTAL_COLOR}{totalTimeSeconds:F2}s</color> ⏳\n"+
				$"{SIGN_COLOR}(</color> Compilation🛠: {COMPILATION_COLOR}{report.compilationTotalTime:F2}s</color> {SIGN_COLOR}+</color> Assembly Reload🔄: {RELOAD_COLOR}{report.assemblyReloadTotalTime:F2}s</color> {SIGN_COLOR})</color>" );

		EditorAudioUtils.PlayClip( compilationSuccessSfx );
	}
}