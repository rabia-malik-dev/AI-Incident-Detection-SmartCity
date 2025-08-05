# MyApp - User Management System

This is a solo project that implements a basic User Management System using ASP.NET, SQL Server, and stored procedures.

---

## 🧾 Features

- User registration
- User login/authentication
- CRUD operations for user data
- Secure password handling (hashing)
- Stored procedures for all DB operations

---

## 🛠️ Tech Stack

- ASP.NET (Web Forms / MVC / .NET Framework)
- SQL Server
- ADO.NET
- Stored Procedures

---

## 🗃️ Project Structure


---

## 🧑‍💻 Local Setup Guide

Follow the steps below to run the project on your local machine:

### 1. Clone the Repository

```bash
git clone https://github.com/rabia-malik-dev/AI-Incident-Detection-SmartCity.git
cd AI-Incident-Detection-SmartCity

2. Set Up the Database
Open SQL Server Management Studio (SSMS).

Open and execute the SQL script from:

pgsql
Copy
Edit
MyApp/database/MyAppDB_Initial.sql
This will:

Create the database MyAppDB

Create all necessary tables (e.g., Users)

Insert sample data (if present)

3. Configure the Connection String
Open Web.config in the root directory.

Find the <connectionStrings> section.

Update the connection string to match your SQL Server instance:
<connectionStrings>
  <add name="MyDB"
       connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=MyAppDB;Integrated Security=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
📝 Replace YOUR_SERVER_NAME with your actual SQL Server name.

4. Run the Application
Open the solution in Visual Studio.

Press F5 or click Start Debugging to run the application.

The default browser will open to the homepage (e.g., Login page).
