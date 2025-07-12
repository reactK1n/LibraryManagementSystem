# 📚 Library Management System API

This is a simple **Library Management System API** built with **ASP.NET Core** and **Entity Framework Core**.

---

## ✨ Features

- **User Authentication & Authorization**
  - Register, Login

- **Book Management**
  - Create, Retrieve (by ID or filter), Update, Delete books
- **Global Authentication**
  - All book-related endpoints require the user to be logged in

---

## 🚀 Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- A SQL Server instance (e.g. LocalDB or full SQL Server)

---

## 🛠️ How to Run

### 1. Clone the Repository

```bash
git clone https://github.com//library-management-system.git
cd LibraryManagementSystem/LibraryManagementSystem.Api
```

### 2. Apply Migrations and Seed Data (if any)

If you're using **EF Core CLI**, make sure it's installed globally:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

### 3. Run the Application

```bash
dotnet run
```

The API will be hosted at:

```
http://localhost:5085
```

---

## 🔐 Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST   | `/api/Auth/register` | Register a new user |
| POST   | `/api/Auth/login` | Login and get JWT token |

---

## 📘 Book Endpoints (Protected)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/Books` | Get all books or search by title/author |
| GET    | `/api/Books/{id}` | Get a specific book |
| POST   | `/api/Books` | Create a book |
| PUT    | `/api/Books/{id}` | Update a book |
| DELETE | `/api/Books/{id}` | Delete a book |

You must be authenticated to access any book endpoint.

---

## 📦 Sample DTOs

### Register

```json
{
  "email": "user@example.com",
  "password": "Password123!",
  "firstname": "John",
  "lastname": "Doe"
}
```

### Book Create

```json
{
  "title": "Refactoring",
  "author": "Martin Fowler",
  "isbn": "9780201485677",
  "publishedDate": "1999-07-08T00:00:00"
}
```

---

## 💡 Notes

- You can run everything locally with just **Visual Studio** or **VS Code**.
- Docker was not required for this setup.
- For protected routes, include the JWT token in the `Authorization` header.

```http
Authorization: Bearer your_jwt_token_here
```

---

## 🧠 Contribution

You're welcome to fork and contribute!

---

## 📝 License

MIT