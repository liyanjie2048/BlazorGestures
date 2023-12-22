setlocal enabledelayedexpansion

set /p key=

for /R "dist" %%i in (*.nupkg) do (
	echo %%i
	dotnet nuget push "%%i" --source liyanjie2048 --api-key %key%
)
