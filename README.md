# 🏠 HomeHunt

> A real estate web application for searching, browsing and managing properties for sale and for rent.

![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core_MVC-8.0-blue)
![EF Core](https://img.shields.io/badge/EF_Core-8.0-green)

---

## 📋 Table of Contents

- [About the Project](#-about-the-project)
- [Technologies Used](#-technologies-used)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [Features](#-features)
- [Usage](#-usage)
- [Database Setup](#-database-setup)
- [Configuration](#-configuration)
- [Contributing](#-contributing)
- [License](#-license)
- [Contact](#-contact)

---

## 📖 About the Project

HomeHunt helps visitors find a home and lets staff keep the listings up to date. Visitors can search properties by city, type, price, bedrooms and area, open a property to see its details, and contact the agent who lists it. Properties, locations and agents can all be added, edited and deleted through the site.

The project is built with ASP.NET Core MVC and Entity Framework Core. It is split into layers: controllers talk to services through contracts, and the services are the only layer that touches the database.

---

## 🧰 Technologies Used

| Technology                 | Version | Purpose                                    |
|----------------------------|---------|--------------------------------------------|
| .NET / C#                  | 8.0     | Platform and language                      |
| ASP.NET Core MVC           | 8.0     | Web framework                              |
| Entity Framework Core      | 8.0     | ORM / database access (Code First)         |
| SQL Server                 | -       | Database                                   |
| ASP.NET Core Identity      | 8.0     | Account pages (set up, not enforced yet)   |
| Razor Views                | -       | Server-side HTML rendering                 |
| Bootstrap                  | 5.1     | Styling and responsive layout              |
| jQuery + jQuery Validation | -       | Client-side form validation                |

---

## ✅ Prerequisites

Install these before you start:

| Tool | Why you need it | Download |
|------|-----------------|----------|
| .NET SDK 8.0 or newer | Build and run the app | [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download) |
| SQL Server (Express, Developer, or LocalDB) | The database | [microsoft.com/sql-server](https://www.microsoft.com/en-us/sql-server) |
| Git | Clone the repository | [git-scm.com](https://git-scm.com/) |
| Visual Studio 2022 *or* VS Code | Editor (optional if you use the command line) | [visualstudio.microsoft.com](https://visualstudio.microsoft.com/) |

**Check that everything is installed.** Open a terminal and run:

```bash
dotnet --version
git --version
```

You should see a version number for each (the .NET version should start with `8` or higher).

---

## 🚀 Getting Started

Pick **one** of the two ways below. Each step says what you should see, so you know it worked before moving on.

### Option A: Command line

**Step 1. Get the code**

```bash
git clone https://github.com/rifatcholakov/HomeHunt.git
cd HomeHunt
```

✔ You should now be in a folder that contains `HomeHunt.sln`.

**Step 2. Install the EF Core tool** (once per computer)

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

✔ Check it with `dotnet ef --version`. If the tool is already installed, you can skip this step.

**Step 3. Check the connection string**

No environment variables or credentials are required. The connection uses Windows authentication, so you only need to make sure the server name matches your SQL Server.

Open `HomeHunt.Web/appsettings.json` and look at `DefaultConnection`. The default points to a local SQL Server:

```
Server=.;Database=HomeHunt;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;
```

- Using the normal local SQL Server or SQL Express? Leave it as it is.
- Using LocalDB (comes with Visual Studio)? Change `Server=.` to `Server=(localdb)\MSSQLLocalDB`.

**Step 4. Create the database and the sample data**

```bash
dotnet ef database update --project HomeHunt.Data --startup-project HomeHunt.Web
```

✔ The last line says `Done.` A database named **HomeHunt** now exists, with 9 locations, 7 agents and 12 properties.

**Step 5. Run the application**

```bash
dotnet run --project HomeHunt.Web --launch-profile https
```

✔ The terminal prints `Now listening on: https://localhost:7231`. Open that address in your browser. You should see the HomeHunt home page with the properties.

To stop the app, press `Ctrl + C` in the terminal.

### Option B: Visual Studio

1. Open `HomeHunt.sln`.
2. In **Solution Explorer**, right-click `HomeHunt.Web` and choose **Set as Startup Project**.
3. Open **Tools → NuGet Package Manager → Package Manager Console**. Set **Default project** (the dropdown at the top of the console) to `HomeHunt.Data`.
4. Run:

   ```
   Update-Database
   ```

   ✔ The console ends without errors and the `HomeHunt` database is created.
5. Press **F5** (or the green **https** button). The browser opens the home page.


---

## 📁 Project Structure

The solution is split into five projects:

```
HomeHunt.sln
│
├── HomeHunt.Common/          # Shared code with no dependencies
│   ├── Enums/                #   PropertyType, ListingType, PropertyStatus
│   ├── Helpers.cs            #   Text normalization helpers
│   └── ValidationConstants.cs
│
├── HomeHunt.Data/            # Data access
│   ├── Models/               #   Entities: Property, Location, Agent
│   ├── Migrations/           #   EF Core migrations
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs           #   Sample data, added by a migration
│
├── HomeHunt.Service.Data/    # Contracts between the web app and the services
│   ├── Properties/           #   IPropertyService + Request/ and Response/ models
│   ├── Locations/            #   ILocationService + Request/ and Response/ models
│   └── Agents/               #   IAgentService + Request/ and Response/ models
│
├── HomeHunt.Services/        # Business logic (implements the contracts)
│
└── HomeHunt.Web/             # ASP.NET Core MVC application
    ├── Controllers/          #   MVC controllers
    ├── ViewModels/           #   Models used by the views and forms
    ├── Views/                #   Razor views (.cshtml)
    ├── wwwroot/              #   Static files (CSS, JS, images)
    ├── appsettings.json      #   App configuration
    └── Program.cs            #   App entry point and service registration
```

**How the projects depend on each other**

```
Web  ->  Services  ->  Data  ->  Common
 |          |
 +--------> Service.Data  ->  Common
```

Controllers send **request** models to a service and get **response** models back, then map them to view models. The service interfaces and these models live in `HomeHunt.Service.Data`.

---

## ✨ Features

- [x] Browse all properties as cards, newest first
- [x] Search and filter by city, property type, listing type (sale or rent), price range, bedrooms and area
- [x] Sort by newest, price (low to high or high to low) and largest area
- [x] Property details with price per square meter, location and agent contact links
- [x] Create, edit and delete properties
- [x] Create, edit and delete locations and agents (deleting one also deletes its properties)
- [x] Home page with statistics, browse by city and by type, and recently added properties
- [x] Validation on the server and in the browser, including unique agent email and phone and unique locations
- [x] Sample data: 9 locations, 7 agents and 12 properties
- [x] Responsive layout with Bootstrap
- [ ] Require login for creating, editing and deleting (Identity is configured but not enforced yet)
- [ ] Image upload (images are added as links)

---

## 💻 Usage

```
1. Open the Home page to see the statistics and the newest properties.
2. Go to "All Properties" to search, filter and sort the listings.
3. Open a property to see its details, its location and its agent.
4. Use "Add Property" to create a listing, then Edit or Delete it from its page or its card.
5. Use "Locations" and "Agents" to manage the places and the agents that properties belong to.
```

> 💡 **Tip:** The filters are part of the URL, so a search such as `/Properties?City=Sofia&ListingType=Rent` can be bookmarked and shared.

> ⚠️ **Deleting a location or an agent also deletes every property that belongs to it.** The confirmation page shows how many properties will be removed.

---

## 💾 Database Setup

The project uses **Entity Framework Core** with a Code First approach. The `DbContext` and the migrations are in `HomeHunt.Data`, so every EF command needs both projects:

- `--project HomeHunt.Data` is where the migrations live.
- `--startup-project HomeHunt.Web` is where the connection string is read from.

Create the database and the sample data:

```bash
dotnet ef database update --project HomeHunt.Data --startup-project HomeHunt.Web
```

The sample data is inserted by a migration, once, when the database is created.

After you change the entities, add a new migration and apply it:

```bash
dotnet ef migrations add MigrationName --project HomeHunt.Data --startup-project HomeHunt.Web
dotnet ef database update --project HomeHunt.Data --startup-project HomeHunt.Web
```

In Visual Studio's Package Manager Console, set the default project to `HomeHunt.Data`, then use `Add-Migration MigrationName` and `Update-Database`.

---

## 🔧 Configuration

> ✅ **No environment variables or credentials are required; authentication uses Windows (`Trusted_Connection`).**
> The app signs in to SQL Server with your Windows account, so there is no username, password or secret to set up.

| What | Needed? |
|------|---------|
| Environment variables | None |
| Usernames, passwords or API keys | None |
| User secrets | None |
| Connection string | Already set in `appsettings.json`. Change it only if your SQL Server has a different name. |

Key settings in `HomeHunt.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=HomeHunt;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

> ⚠️ **Never commit sensitive data** (passwords, API keys) to source control. If your connection string contains credentials, store it with [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) or the `ConnectionStrings__DefaultConnection` environment variable.

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a new branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add some feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request

---

## 📄 License

No license has been added to this project yet. Until one is added, all rights are reserved by the author.

---

## 📬 Contact

**Rifat Cholakov** – [@rifatcholakov](https://github.com/rifatcholakov)

Project Link: [https://github.com/rifatcholakov/HomeHunt](https://github.com/rifatcholakov/HomeHunt)
