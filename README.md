
---

```md
# ZBlogApp - ASPIRE-based Solution

## Overview
ZBlogApp is a modular web application built using **ASPIRE** and **.NET 9.0**. It consists of multiple services, including a Web API, Blazor Server, and various supporting libraries to provide a robust blogging platform. 

This project is designed to manage user authentication, article creation, and API-based content delivery efficiently.

## Technologies Used
- **.NET 9.0**
- **ASPIRE Framework**
- **Blazor Server**
- **Entity Framework Core**
- **C#**
- **SQLite** (or other DBMS)
- **REST API**

## Project Structure
```
## Project Structure

| Folder/File                 | Description |
|-----------------------------|-------------|
| `.github/`                  | GitHub workflows & CI/CD configs |
| `ZBlogA/`                   | Additional service or utility (if applicable) |
| `ZBlogApp.ApiService/`      | REST API service handling requests |
| `ZBlogApp.AppHost/`         | Application host for running all services |
| `ZBlogApp.BlazorServer/`    | Blazor Server project for UI & frontend logic |
| `ZBlogApp.Library/`         | Shared library with common models & utilities |
| `ZBlogApp.ServiceDefaults/` | Default service configurations |
| `ZBlogApp.Web/`             | Web frontend integration (if applicable) |
| `.gitignore`                | Git ignore rules |
| `convert_json_to_docx.py`   | Python script for JSON to DOCX conversion |
| `README.md`                 | Project documentation |
| `ZBlogApp.sln`              | Solution file for Visual Studio |

```

## Installation & Setup

### Prerequisites
- Install **.NET 9.0 SDK**: [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- Ensure **ASPIRE CLI** is installed:
  ```bash
  dotnet workload install aspire
  ```

### Running the Project
1. Clone the repository:
   ```bash
   git clone https://github.com/matt0917/ASPIRE_ZBlog.git
   cd ZBlogApp
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   cd ZBlogApp.AppHost
   dotnet watch
   ```

## API Endpoints
The `ZBlogApp.ApiService` exposes various RESTful API endpoints for managing articles, users, and authentication. 

Example:
- `GET /api/articles` → Fetch all articles
- `POST /api/articles` → Create a new article
- `GET /api/articles/{id}` → Get details of a specific article

## Authentication
- The Blazor Server project (`ZBlogApp.BlazorServer`) handles authentication and session management.
- Users can register, log in, and manage their blog posts.
- Authentication follows standard **JWT Token-based authentication** (if implemented).

## Database Setup
By default, the project uses **SqlServer** in a container within Docker. To configure it:
1. Update `appsettings.json` inside `ZBlogApp.BlazorServer` or `ZBlogApp.ApiService`:
   ```json
   {
     "ConnectionStrings": {
       "default sqlServer credential with you sqlserver base url and port number"
     }
   }
   ```

2. Apply migrations:
   ```bash
   dotnet ef migrations add InitialCreate --project ZBlogApp.BlazorServer
   dotnet ef database update --project ZBlogApp.BlazorServer
   ```

## Contributing
1. Fork the repository.
2. Create a new branch (`feature-branch`).
3. Commit changes and push.
4. Open a pull request.

## License
MIT License

Copyright (c) 2025 Joonseo Park

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


---
