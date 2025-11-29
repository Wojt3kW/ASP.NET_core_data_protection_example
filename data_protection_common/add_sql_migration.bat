@echo off
setlocal

REM Check if migration name was provided
if "%~1"=="" (
    echo Usage: add_sql_migration.bat ^<MigrationName^>
    echo Example: add_sql_migration.bat InitialCreate
    exit /b 1
)

set MIGRATION_NAME=%~1
set STARTUP_PROJECT=..\data_protection_with_EF
set OUTPUT_DIR=SqlMigrations

echo Adding migration: %MIGRATION_NAME%
echo Output directory: %OUTPUT_DIR%
echo.

dotnet ef migrations add %MIGRATION_NAME% --startup-project %STARTUP_PROJECT% --output-dir %OUTPUT_DIR%

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Migration '%MIGRATION_NAME%' created successfully in %OUTPUT_DIR% folder.
) else (
    echo.
    echo Failed to create migration.
)

endlocal