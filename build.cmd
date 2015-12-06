@echo off
cd %~dp0

setlocal EnableDelayedExpansion

where dnvm

if %ERRORLEVEL% neq 0 (
    @powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"
    set PATH=!PATH!;!USERPROFILE!\.dnx\bin
    set DNX_HOME=!USERPROFILE!\.dnx
    goto install
)

:install
call dnvm update-self
call dnvm install 1.0.0-rc1-update1
call dnvm use 1.0.0-rc1-update1

pushd Teloqui
call dnu restore
call dnu build
popd