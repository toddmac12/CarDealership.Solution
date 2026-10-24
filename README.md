# Car Dealership

ASP.NET Core MVC inventory app with a built-in SQLite database.

## Run

Install the .NET 9 SDK, then:

```powershell
git clone https://github.com/toddmac12/CarDealership.Solution.git
cd CarDealership.Solution
dotnet watch run --urls http://localhost:5001
```

Open http://localhost:5001. First launch creates `App_Data/dealership.db` and adds 12 sample vehicles. No database installation, account, credentials, or manual migration command is needed. Port 5001 avoids a BakeryTreat app already using port 5000.

Sample vehicles and prices are demonstration values. You can edit or delete them. Seed data runs once; restarting preserves edits and does not recreate deleted samples. New listings, edits, and deletions persist between runs.

## Features

- Add, view, edit, and confirm deletion of listings.
- Server-side validation for required model, whole-dollar USD price, and mileage; invalid submissions preserve form input and do not change inventory.
- Search by model, filter by maximum price/mileage, and sort by price, mileage, model, or newest listing.
- Anti-forgery protection on changes, proper missing-record/route responses, and accessible validation/status messages.

The app retains its original single-user inventory workflow: anyone who can reach it can manage listings. The default localhost address is intended for local use.

## Storage and backup

Stop the app and back up the `App_Data` folder. Keep it when updating the app. Database files are ignored by Git. Set `Database__FilePath` to choose another database file. The original in-memory version did not retain inventory when its process stopped.

## Validate

```powershell
dotnet test CarDealership.Tests/CarDealership.Tests.csproj --configuration Release
```

Tests exercise actual HTTP forms and the SQLite database, including seeding, restart persistence, validation, anti-forgery, missing routes, CRUD, search/filter/sort, and migration-model agreement.
