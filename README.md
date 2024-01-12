# Poznaj AI

Poznaj AI is a .NET 7 (C#) REST API, with JWT authorization.

## Table of Contents

- [Installation](#installation)
  - [System Requirements](#system-requirements)
  - [Installation Steps](#installation-steps)
  - [Database Configuration](#database-configuration)
  - [Running the Project](#running-the-project)
- [Configuration](#configuration)
- [API Documentation](#api-documentation)
- [Testing](#testing)
- [Project Status](#project-status)


## Installation

### System Requirements

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- SQL Server (or any compatible database)

### Installation Steps

1. **Clone the repository:**

    ```bash
    git clone https://github.com/kamil-kornek96/PoznajAIAPI
    cd PoznajAIAPI
    ```

2. **Install dependencies:**

    ```bash
    dotnet restore
    ```

3. **Run migrations to create the database:**

    ```bash
    dotnet ef database update
    ```

### Database Configuration

Configure the `app.Settings.json` file for database settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ExploreAI;Trusted_Connection=True;TrustServerCertificate=True;",
    "HangfireConnection": "Server=localhost;Database=HangfireDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
### Running the Project
Run the application:

```bash
dotnet build
dotnet run
```
### Configuration

Before running the application, modify the app.Settings.json file to set the following configurations:

JWT Settings:

```
"JwtSettings": {
  "Issuer": "https://localhost:7297/",
  "Audicence": "https://localhost:7297/",
  "Key": "your-secret-key"
}
```
Video Conversion Settings:

```
"VideoConversion": {
  "VideoBitrate": 1000,
  "AudioBitrate": 128,
  "Resolution": "640x480",
  "OutputDirectory": "Uploads/Videos"
}
```
Hangfire Settings:

```
"Hangfire": {
  "AccessId": "your-access-id"
}
```
Logging Settings:

```
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```
## API Documentation

Full API documentation is available here.

## Testing

Run unit tests:

```bash
dotnet test
```


## Project Status

This project is currently in development.

## Author

Kamil Kornek






















