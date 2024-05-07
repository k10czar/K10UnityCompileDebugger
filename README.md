# Unity Assemblies Debugger

Small utility to show the time it takes to compile and assembly reload.

## Usage

Just by installing this in your project it will load itself on every reload.

If you want to see the report, you can open the utility window at `K10 > Unity Compilation Debugger`.

### Logs

By default it will use the console to print the total reloading time.

You can see an example log here:

```
Compilation Report: 4.14s

	Compilation: 1.42s
	Assembly Reload: 2.72s
```

#### Disabling the logs

If you wish to disable the logs, you can set this in the checkbox in the utility window.

## Installation

### Add as submodule on your Unity project repository

``git submodule add https://github.com/k10czar/UnityCompileDebugger.git "Assets/Plugins/K10/UnityCompileDebugger"``

## How To remove the submodule

1.  Delete from the  _.gitmodules_  file:

	`[submodule "Assets/Plugins/UnityCompileDebugger"]`
	
	`path = Assets/Plugins/UnityCompileDebugger`
	
	`url = https://github.com/k10czar/UnityCompileDebugger.git`
	
2.  Delete from  _.git/config_:

	`[submodule "Assets/Plugins/UnityCompileDebugger"]`
	
	`url = https://github.com/k10czar/UnityCompileDebugger.git`
	
	`active = true`
	
3.  Run:

	`git rm --cached "Assets/Plugins/UnityCompileDebugger"`

4.  Commit the superproject.

5.  Delete the submodule folder _`Assets/Plugins/UnityCompileDebugger`_.
