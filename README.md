Agri_Connect is an agricultural platform built with ASP.NET Core. It allows users to register farmers, manage their products, and provides an interface to manage data efficiently. The application includes features such as user authentication, role-based access, and a responsive UI built with Bootstrap.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)

---

## Features

- **User Authentication**: Allows users log in, and manage their profiles.
- **Farmer Registration & Management**: Employees can register farmers, edit their profiles, and list them.
- **Product Management**: Farmers can add, edit, and view products they offer for sale.
- **Role-Based Access**: Admin roles (like "Employee") have the ability to manage farmers and products.
- **Responsive UI**: The UI is designed with responsiveness in mind, using Bootstrap for an optimal experience across devices.

---

## Technologies Used

- **ASP.NET Core**: Web application framework for building the backend.
- **Entity Framework Core**: ORM (Object-Relational Mapping) for interacting with the database.
- **Identity Framework**: For user authentication and role management.
- **Bootstrap**: For responsive front-end design.
- **SQL Server**: Database management system.
- **JavaScript**: For dynamic and interactive UI elements.
- **Placehold.co**: Used for placeholder images in case no image URL is provided by users.

---

## Installation

Follow these steps to get the project up and running locally:

### Prerequisites

Before installing the project, ensure you have the following installed:

- **.NET SDK** (version 6.0 or later) 
- **SQL Server** (LocalDB or a remote instance) - For database management.
- **Visual Studio** (or Visual Studio Code) - IDE for developing and running ASP.NET applications.

### Steps to Install

1. **Clone the Repository**:

   - Open a terminal or Git Bash and clone the repository to your local machine:
   git clone https://github.com/Mckyle-Singh/Agri-Connect.git

2. **Restore the Nuget Packages**:

   - Open the solution file (.sln) in Visual Studio or navigate to the project directory in your terminal, and run:
     dotnet restore

3. **Set Up the Database:**:
    - Open the project in Visual Studio
    - Update the connection string in appsettings.json to point to your local or remote SQL Server instance

4. **Apply Migrations**:
    - Run migration in package manager console: dotnet ef database update

5. **Seed Data:**:
    - The seeded data can be found in the Data folder
    - Once the project is run the data will be seeded with Employee login details 


### Login Details for Employee(Admin)

These are the login details for the Employe user once your project is up and running:
    
    - Username/Email : "admin@gmail.com"
    - Password:Test1234!


  