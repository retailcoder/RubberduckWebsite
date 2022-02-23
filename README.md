# Rubberduck Website  
This repository contains the source code for the [Rubberduck](https://github.com/rubberduck-vba/Rubberduck) project's website.

#### Build Status

- main:  
[![Build/Deploy Database (prod)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/main_rubberduckdb.yml/badge.svg)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/main_rubberduckdb.yml)  
[![Build/Deploy API (prod)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/main_rubberduckapi.yml/badge.svg)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/main_rubberduckapi.yml)  

- dev:  
[![Build Database (dev)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/msbuild_dev_windows.yml/badge.svg)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/msbuild_dev_windows.yml)  
[![windows dotnet build (dev)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/dev_dotnet_windows.yml/badge.svg)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/dev_dotnet_windows.yml)  
[![ubuntu dotnet build (dev)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/dev_dotnet_ubuntu.yml/badge.svg)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/dev_dotnet_ubuntu.yml)  

## Architecture

The solution components and their inter-dependencies can be depicted as follows:

![Diagram showing all solution projects and their inter-dependencies](https://user-images.githubusercontent.com/5751684/153332910-fabc0250-42ae-4754-a76d-cd47babcd3d4.png)

#### Rubberduck.Database

Data Tools project building the back-end SQL Server database.

#### Rubberduck.Model

A class library that defines a public model shared between projects.

#### RubberduckServices

A back-end library that references Rubberduck assemblies and exposes various services to leverage them. When [Rubberduck](https://github.com/rubberduck-vba/Rubberduck) builds a new release, a script should build the release tag in this repository, and update the .dll files under the `\Libs` folder.

#### Rubberduck.ContentServices

A back-end library that connects to the back-end database and defines the EF entity data model.
Also connects to the GitHub API to retrieve tags; downloads xmldoc assets and processes them into FeatureItem entities.

#### Rubberduck.API

Organizes the various back-end services into a REST API that abstracts them all behind public and admin/authenticated endpoints.

#### Rubberduck.Client

For the website, a service layer library that facilitates interacting with the REST API.
For the REST API, just another HTTP client.

#### Rubberduck.Website

The ASP.NET Core MVC website client only needs to know about *Rubberduck.Client* and *Rubberduck.Model*.

---

### Initial Setup

After forking the repository and cloning the solution, in order to run it locally you'll need to:

 - [Setup PAT](#SetupPAT)
 - [Setup OAuth](#SetupOauth)
 - [Setup rubberduckdb](#SetupDatabase)

Make sure **Rubberduck.API** and **Rubberduck.Website** are set as startup projects; launching a debugger session normally opens 2 browser tabs. Some exceptions may be thrown if the website starts before the API does, but the site should still be presenting a working home page.

#### <a id="SetupPAT">Setup PAT</a>

The API uses a Personal Access Token (PAT) to fetch tags and release metadata.

 - [Generate a PAT](https://github.com/settings/tokens/new) on GitHub
 - Store the PAT in a **rdapi_GITHUBAPIKEY** environment variable

This access token is making it simpler to eventually setup a headless execution that could be triggered by a Rubberduck build.

#### <a id="SetupOAuth">Setup OAuth</a>

Parts of the site front-end are inaccessible without an unauthenticated session. The backend does not store any user data; instead, authentication is delegated to GitHub through its OAuth2 API.

 - [Create a GitHub OAuth app](https://github.com/settings/applications/new) in your profile's _developer settings_:
   - The **Callback URL** should be https://localhost:44319/signin-github.
 - Save your OAuth app's **ClientId** and **ClientSecret** values in the `Rubberduck.Website` project's _user secrets_ using **ClientId** and **ClientSecret** for the keys:

    ```json
    {
      "ClientId": "0123456789abcdef0123",
      "ClientSecret": "0123456789abcdef0123456789abcdef01234567"
    }
    ```

   The secrets file is ignored by git and should never be checked in, but `Rubberduck.Website.csproj` will likely create a diff with a GUID representing the secrets file:

    ```xml
    <PropertyGroup>
      <TargetFramework>net5.0</TargetFramework>
      <UserSecretsId>f8edfe3d-b1ba-4067-bc48-3001fa9f1a4f</UserSecretsId>
    </PropertyGroup>
    ```

   Avoid checking in a diff on `Rubberduck.Website.csproj` that would only include a change in the `UserSecretsId` tag.

> **NOTE:** In order to access the protected content, your GitHub account must be a member of the [rubberduck-vba](https://github.com/rubberduck-vba) organization.

<small>But, it's your local database: feel free to just comment-out the `[Authorize]` attribute in the `AdminController` to easily skip authentication altogether.</small>


#### <a id="SetupDatabase">Setup rubberduckdb</a>

Right-click the `Rubberduck.Database` project and build it manually; it is excluded from the default `dotnet build` configuration because it is built with `MSBuild`.

Building the project generates a .dacpac that you can run by right-clicking the project again, and selecting _Publish..._

![Publish Database dialog](https://user-images.githubusercontent.com/5751684/154824151-ea29bd26-9dc8-4e23-a7dc-868341595409.png)

Publish to a local SQL Server instance to create the backend database; the connection string can go into `Rubberduck.API\appsettings.json` under a **RubberduckDb** key:

```json
  "ConnectionStrings": {
    "RubberduckDb": "Data Source=(localdb)\\ProjectsV13;Initial Catalog=rubberduckdb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False"
  },
```

As a convenience to other collaborators, changes to this configuration file should not be checked in unless other configuration keys are being added.

