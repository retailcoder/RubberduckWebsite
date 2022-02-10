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

## Projects

![Diagram showing all solution projects and their inter-dependencies](https://user-images.githubusercontent.com/5751684/153332910-fabc0250-42ae-4754-a76d-cd47babcd3d4.png)

#### Rubberduck.Database

Data Tools project building the back-end SQL Server database.

#### Rubberduck.Model

A class library that defines a public model shared between projects.

#### RubberduckServices

A back-end library that references Rubberduck assemblies and exposes various services to leverage them.

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