# Running and Testing

The `Run` script is your primary tool for running the application and executing tests. It automatically handles the complex setup of starting dependent services (databases, API) required.

## CRITICAL INFORMATION

- **Always use `isBackground: false`** to wait synchronously**
- **Always monitor `Run` with `get_terminal_output`** repeatedly until "This terminal is now free to use"
- **Always wait for "This terminal is now free to use"** before executing ANY other commands in that terminal

- **Background Services**: The `Run` command manages background processes for you. You don't need to manually start the API before running E2E tests—the script handles it.
- **Terminal Reuse**: You can run `Run` commands repeatedly in the same terminal. It detects if servers are already running and only restarts them if binaries have changed.

## Basic Usage

```powershell
# 1. Build project
# 2. Run ALL tests (Unit, Integration, E2E)
# 3. Start application servers (and keep them running)
Run
```

## Advanced Usage

There are many parameters you can give to tweak the behavior. You can combine all of them.

| Command | Description |
|---------|-------------|
| `Run` | Builds the solution, runs **ALL** tests, and starts servers in the background. |
| `Run -Clean` | Cleans all build artifacts (`bin`/`obj`) before running the standard workflow. |
| `Run -TestProject Api -TestName Auth` | Run only "Auth" tests within the "Api" project (combines filters). |
| `Run -TestProject Web.Tests` | Run only the **Web** component tests. |
| `Run -TestProject Api` | Run tests with "Api" in the project name. |
| `Run -TestName Filter` | Run only individual test methods containing "Filter". |
| `Run -Docker` | Run E2E tests using the production Docker stack (PostgreSQL). |
| `Run -Rebuild` | Perform a clean rebuild only, then exit (no tests/run). |
| `Run -Environment Production` | Run in Production mode (tests 'dotnet publish' artifacts). |
| `Run -StopServers` | Stops the servers after tests ran. Can be useful in a CI/CD environment. |
| `Stop-Servers` (without Run in front!) | Stops the servers directly. |

## URLs and Access Points

- **Web UI**: <https://localhost:5003> (Accept the self-signed certificate warning)
- **API**: <https://localhost:5001> (Swagger UI: <https://localhost:5001/swagger>)
- **Aspire Dashboard**: <https://localhost:18888> (View logs, traces, and metrics)

> **Note**: You might see browser warnings about "Your connection is not private" because localhost uses a self-signed certificate. Click **Advanced** -> **Proceed to localhost (unsafe)** to continue.

## Development Environment

Tech Hub is designed to be developed in a **containerized environment**. This ensures all dependencies (databases, SDKs, tools) are pre-configured and consistent for every developer.

### Option 1: GitHub Codespaces (Browser)

1. Open the repository in GitHub.
2. Click **Code** -> **Codespaces** -> **Create codespace on main**.
3. Wait for the environment to build.
4. You are ready to go!

### Option 2: DevContainer (VS Code)

1. Install **Docker Desktop** and **VS Code**.
2. Clone the repository.
3. Open the folder in VS Code.
4. When prompted, click **"Reopen in Container"** (or press `F1` and search for **"Dev Containers: Reopen in Container"**).
5. Wait for the container to build and initialize.

#### Debugging in VS Code with F5

This is the easiest way to run and debug the application.

1. Open the **Run and Debug** view (`Ctrl+Shift+D`).
2. Select **"Tech Hub (API + Web)"** from the dropdown configuration list.
3. Press **F5** or click the play button.

## Troubleshooting

### Database Schema Changes

If you encounter database-related errors after pulling code changes (especially errors about missing or renamed columns), you may need to rebuild the databases from scratch.

**Symptoms**:

- Error: `column "column_name" of relation "table_name" does not exist`
- Database migration warnings or failures
- Content sync failures on startup

**Solution**:

Delete the local database directories to force a clean rebuild:

```powershell
# First, remove Docker volumes if you've been using -Docker flag
docker compose down -v

# Then, remove SQLite databases (used in Development mode)
Remove-Item -Recurse -Force .databases/
```

After removing the databases:

1. **SQLite (Development mode)**: Run `Run` to recreate the database with the latest schema
2. **PostgreSQL (Docker mode)**: Run `Run -Docker` to rebuild containers with a fresh database

The ContentSync process will automatically populate the new database with content from the `collections/` directory during startup.

> **Note**: Database schema changes are rare but do happen. Always check the migration scripts in `src/TechHub.Infrastructure/Data/Migrations/` to understand what changed.
