# Rubberduck Website  
This repository contains the source code for the [Rubberduck](https://github.com/rubberduck-vba/Rubberduck) project's website.

#### Build Status

- main:  
[![dotnet build](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/dotnet.yml/badge.svg)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/dotnet.yml)
[![Build RDDB (prod)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/msbuild.yml/badge.svg)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/msbuild.yml)
- dev:  
[![dotnet build](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/dotnet.yml/badge.svg?branch=dev)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/dotnet.yml)
[![Build RDDB (dev)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/msbuild_dev.yml/badge.svg)](https://github.com/retailcoder/RubberduckWebsite/actions/workflows/msbuild_dev.yml)

## Projects

### RubberduckWebsite  
The website front-end, published to https://rubberduckvba.com.

### Rubberduck.API  
A .NET Web API that serves everything the website needs, published to https://api.rubberduckvba.com.

### Rubberduck.Model  
A library that defines the data model entities, DTOs, ViewModels, etc. shared between projects.

### Rubberduck.ContentServices  
Contains the logic for parsing xmldocs into website content, and for accessing the backend database.

### RubberduckServices  
A library that references Rubberduck libraries and exposes indenter and syntax highlighter services.

### Rubberduck.Database  
A SQL Server database project that defines the schema for the backing database.
