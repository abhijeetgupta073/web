# Scaffold Management System

Scaffold Management System is an ASP.NET MVC application used to manage scaffold projects, inventory, site requests, inspections, registers, deliveries, reports, and user access.

## Technology Stack

- ASP.NET MVC 5
- .NET Framework 4.8
- Entity Framework 6 Database-First
- Microsoft SQL Server
- C#
- Razor Views
- JavaScript, jQuery, Bootstrap
- Azure Blob Storage integration
- Visual Studio

## Main Features

- User authentication and access control
- Client and site management
- Scaffold register management
- Inventory management
- Site inventory tracking
- Site requests and material requests
- Delivery and return inventory management
- Damaged inventory management
- Site inspections and checklists
- Scaffold material list management
- Man-hours tracking
- Dashboard and reporting
- Document and image upload
- Azure Blob Storage integration
- Offline inspection and register support

## Solution Structure


## Prerequisites

Before running the application, install:

- Visual Studio with ASP.NET and web development workload
- .NET Framework 4.8 Developer Pack
- SQL Server
- SQL Server Management Studio
- Git

## Configuration

The project uses configuration values for:

- SQL Server database connection
- Entity Framework connection
- Azure Blob Storage
- SendGrid email service
- Application tokens
- PayPal settings

Sensitive values must not be committed to source control.

Update the local `Scaffold/Web.config` file with your environment-specific values:
