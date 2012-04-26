@echo off
cls
..\..\tools\nant-0.91\NAnt.exe -buildfile:BonaStoco.AP1.Web.build %* -t:net-4.0 -D:bonastoco-bin=..\..\bonastoco-bin -D:external-bin=..\..\external-lib -D:tools=..\..\tools -D:bonastoco.gui-bin=..\..\bonastoco.gui-bin -D:build.base=build