# Algorithm Test Cases

This repository houses a Blazor web application, which showcases implementations of artificial intelligence (AI) algorithms. This repository hosts the software's source code as well as the built, deployed web application, accomplished with GitHub Actions and GitHub pages.

## Prerequisite Software

- [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) (Required)
- [Docker](https://www.docker.com/get-started) (Required to run local build)

## Local Development

To run the Blazor development server, run

```bash
make run-local
```

To build locally, run

```bash
make build
```

and to run the build, run

```bash
make run-build
```

Alternatively, you can create a build and run it in one step by running

```bash
make build-and-run
```

Note that `make run-build` and `make build-and-run` require a Docker installation.

## Branching, Changes, and Deployment

This repository contains three main branches:

- `master` - Main version of the repository
- `development` - Working version of the repository
- `gh-pages` - Built and deployed web application

To make changes, create a feature branch off of `development` and make pull requests to `development`. When you are ready to make a release, make a pull request from `development` to `master`.

A push or merge to `master` will automatically trigger a GitHub Action, which creates a build of the application and deploys that build to the `gh-pages` branch. This branch is hosted with GitHub pages.
