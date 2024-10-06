# Asuro Coding Challenge

## Overview

AsuroCodingChallenge is a .NET-based web application designed to handle file uploads with user and customer management. It provides an API for uploading files, tracking their status, and managing user and customer information using an in-memory database.

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Testing](#testing)
- [Docker](#docker)

## Features

- File upload with status tracking
- User and customer management
- In-memory database for data storage
- Error handling and logging
- Swagger UI for API documentation

## Architecture

The project is structured into four main components:

1. **AsuroCodingChallenge.API**: The web API that handles incoming requests and manages file uploads.
2. **AsuroCodingChallenge.BusinessLogic**: The business logic layer that processes data and interacts with the data access layer.
3. **AsuroCodingChallenge.DataAccess**: The data access layer that communicates with the in-memory database.
4. **AsuroCodingChallenge.Tests**: Unit tests for the application.

## Technologies Used

- .NET 8.0
- ASP.NET Core
- Entity Framework Core
- In-memory database
- Swagger for API documentation
- Docker for containerization

## Getting Started

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/AsuroCodingChallenge.git
   cd AsuroCodingChallenge
2. Restore the dependencies:
   ```bash
    dotnet restore
3. Run the application:
   ```bash
   dotnet run --project AsuroCodingChallenge.API

### Docker
To build and run the application using Docker, follow these steps:

1. Ensure Docker is running on your machine.
2. Navigate to the root of the project directory.
3. Run the following command:
   ```bash
   docker-compose up --build
4. Access the application at http://localhost:5000/swagger.
