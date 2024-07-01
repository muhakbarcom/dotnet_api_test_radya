# Online Bookstore Backend RESTful API

## Tech Stack

1. **PHP Framework:** .NET Core 8
2. **Unit Testing:** NUnit
3. **Documentation:** Swagger
4. **Logging UI:** Serilog

## Getting Started

1. Open Terminal/Bash.
2. Clone the repository:
   ```bash
   git clone https://github.com/muhakbarcom/dotnet_api_test_radya
   ```
3. Create the configuration environment (URL, Database, etc.) or copy from the example:
   ```bash
   cp .env.example .env
   ```
4. Run Migration, It will create a new database automaticly
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet watch run
   ```
6. Open the Swagger documentation at [http://localhost:5028/swagger/index.html](http://localhost:5028/swagger/index.html) (Default)

## Demo Accounts

- **Admin:**
  - UserName: `admin`
  - Password: `Password123!`
- **Customer:**
  - UserName: `cust1`
  - Password: `Password123!`
