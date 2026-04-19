# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Tech Stack

- **Frontend:** Angular 21.2.0 (TypeScript, strict mode)
- **Backend:** ASP.NET Core 10.0 (.NET 10)
- **Testing:** Vitest 4.0.8 + Jasmine (frontend), no backend tests yet
- **Code style:** Prettier (100-char width, single quotes), 2-space indent

## Commands

### Running the app

The preferred way is to open the solution in Visual Studio and run — it starts both the backend and the Angular dev server via SPA proxy.

Alternatively, run each separately:

```bash
# Backend (from ElShop.Server/)
dotnet run

# Frontend (from elshop.client/)
npm start
```

The backend serves HTTPS on port 7255, and the Angular dev server runs on port 52908 (with SSL using ASP.NET Core dev certs).

### Frontend commands (from `elshop.client/`)

```bash
npm start          # dev server with HTTPS (sets up certs via aspnetcore-https.js)
npm test           # run unit tests with Vitest
ng build           # production build
ng build --watch --configuration development  # watch mode
```

### Running a single test file

```bash
npx vitest run src/app/app.spec.ts
```

### Backend commands (from `ElShop.Server/`)

```bash
dotnet run         # run with https launch profile
dotnet build       # build only
```

## Ticket Workflow

For every ticket/task, follow this sequence without asking for confirmation at each step:

1. Create a branch: `git checkout -b <ticket-id>-short-description`
2. Implement the changes
3. Run tests — frontend: `npm test` (from `elshop.client/`); backend: `dotnet test` if test projects exist
4. If tests pass, commit, push the branch, and open a PR against `main`
5. If tests fail, fix them before pushing

Do all of this autonomously. No need to ask permission before creating the branch, pushing, or opening the PR.

## Architecture

### Frontend ↔ Backend communication

The Angular dev server proxies API requests to the backend. Configuration is in `elshop.client/src/proxy.conf.js` — it reads the backend port from `ASPNETCORE_HTTPS_PORT` (or `ASPNETCORE_URLS`) and routes matching paths to `https://localhost:7255`.

When adding a new API route, add the path pattern to `proxy.conf.js` so it's forwarded during development.

### Project wiring

`ElShop.Server.csproj` references the Angular project via `SpaProxyServerUrl` and `SpaProxyLaunchCommand`, so Visual Studio's F5 launch starts both processes. The `elshop.client.esproj` registers the frontend as a JavaScript SDK project in the solution.

### TypeScript strict mode

`tsconfig.json` enables full strict mode including `strictTemplates`, `noImplicitOverride`, and `noFallthroughCasesInSwitch`. Keep these satisfied — do not suppress with `// @ts-ignore` or cast to `any`.

### HTTPS / certificates

On first run, `aspnetcore-https.js` (called by `npm prestart`) exports ASP.NET Core dev certs to PEM files that Angular's dev server uses. If SSL errors appear, run `dotnet dev-certs https --trust` to regenerate.
