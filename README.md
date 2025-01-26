# HumanResourceTask

## Overview
**HumanResourceTask** is a technical task designed to demonstrate proficiency in creating a complete application using modern development practices. The project includes a REST API, a responsive web UI, a structured database layer, and comprehensive unit testing.

The solution is structured into multiple projects, each handling specific aspects of the application. It is designed to run using Visual Studio 2022.

---

## Technologies Used
- **C#**
- **.NET 8/9**
- **REST API** (using [FastEndpoints](https://fast-endpoints.com/))
- **SQL LocalDB** (accessed via [Dapper](https://github.com/DapperLib/Dapper))
- **MudBlazor** (for the web UI)
- **xUnit** (for unit tests)
- **Microsoft Entra** for OAuth 2.0 authentication

---

## Project Structure
The solution is organized into the following projects:

### Core
- **HumanResourceTask**  
  Contains shared logic and utilities used across the application.
- **HumanResourceTask.Data**  
  Handles data access using Dapper.
- **HumanResourceTask.Database**  
  Manages database creation, schema definition, and seeding.

### API
- **HumanResourceTask.Api**  
  Implements the REST API using FastEndpoints.
- **HumanResourceTask.Api.Dto**  
  Defines data transfer objects (DTOs) used by the API.
- **HumanResourceTask.Api.Dto.Validation**  
  Implements validation logic for DTOs.

### Web
- **HumanResourceTask.Web**  
  Provides a responsive and modern UI built with MudBlazor.

### Tests
- **HumanResourceTask.Api.Test**  
  Contains unit tests for the API.
- **HumanResourceTask.Api.Dto.Validation.Test**  
  Tests the validation logic for DTOs.
- **HumanResourceTask.Data.Test**  
  Tests data access logic.
- **HumanResourceTask.Test**  
  Includes other miscellaneous tests.

---

## Prerequisites
Before running the project, ensure you have the following installed:
- **Visual Studio 2022** (or later)
- **.NET 8/9 SDK**
- **SQL Server LocalDB** (comes with Visual Studio)
- A modern web browser (for accessing the UI)

---

## Getting Started

### 1. Clone the Repository
Clone the repository to your local machine:

```bash
git clone <repository-url>
cd HumanResourceTask
```

### 2. Open in Visual Studio
Open the solution file `HumanResourceTask.sln` in **Visual Studio 2022**.

### 3. Build the Solution
Build the solution:
- Open Visual Studio and load the solution.
- Select **Build Solution** (`Ctrl+Shift+B`) to ensure all projects compile successfully.

### 4. Run the Database Setup
Initialize and seed the LocalDB database:
1. In Solution Explorer, right-click on **HumanResourceTask.Database** and select **Set as Startup Project**.
2. Run the project using `F5` or **Start** in Visual Studio.

This will create and populate the SQL LocalDB database with required data.

### 5. Start the API and Web Applications
Run the API and web applications simultaneously:
1. Configure multiple startup projects:
   - Right-click on the solution in Solution Explorer.
   - Select **Properties** > **Startup Project**.
   - Choose **Multiple startup projects**.
   - Set both `HumanResourceTask.Api` and `HumanResourceTask.Web` to **Start**.
2. Start the solution using `F5`.

### 6. Access the Application
Once the application starts:
- The **API** will run at `http://localhost:7204`.
- The **Web UI** will run at another `http://localhost:7099` URL. Open this in your browser to interact with the application.
