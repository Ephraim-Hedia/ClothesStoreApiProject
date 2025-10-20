# 🛍️ StoreAPI

StoreAPI is a complete backend solution for an online clothing store built with **.NET 8**, following **Clean Architecture** principles.  
It supports authentication, user management, product management, shopping baskets, and order creation with JWT-based authorization.

---

## 🚀 Features

- 🧑‍💻 **Authentication & Authorization** — Secure login and registration using JWT.
- 🧺 **Basket Management** — Add, update, and delete basket items dynamically.
- 🏷️ **Product Management** — Manage products, categories, colors, and sizes.
- 🚚 **Order Processing** — Create and manage orders from basket data.
- 🏡 **Address Management** — Save and update user shipping addresses.
- 🛡️ **Clean Architecture** — With Entity Framework, Repository Pattern, and Unit of Work.
- 🧾 **Swagger UI** — Interactive API documentation for easy testing.

---

## 🧩 Technologies Used

- ASP.NET Core 8 (Web API)
- Entity Framework Core (Code-First)
- AutoMapper
- JWT Authentication
- SQL Server
- Swagger / OpenAPI
- Repository & Unit of Work Pattern

---

## 🧪 How to Run

### 1. Clone the repository
```bash
git clone https://github.com/Ephraim-Hedia/StoreAPI.git
```

### 2. Navigate to the API project
```bash
cd Store.API
```

### 3. Apply database migrations
```bash
dotnet ef database update
```

### 4. Run the project
```bash
dotnet run
```

### 5. Open Swagger UI
```
https://localhost:5001/swagger
```

---

## ⚙️ Environment Variables

You must configure the following settings in your `appsettings.json` or environment variables:

```json
"Token": {
  "Key": "your_secret_jwt_key_here",
  "Issuer": "your_app_name",
  "ExpireMinutes": "60"
},
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=StoreClothesDbContext;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

---

## 🧑‍💻 Developer

**Guirguis Hedia**  
- 💼 Embedded Software & .NET Developer  
- 📧 Email: guirguishedia@gmail.com  
- 🔗 [LinkedIn](https://www.linkedin.com/in/guirguis-hedia-501446207/)  
- 💻 [GitHub](https://github.com/Ephraim-Hedia)

---

## 🏁 License

This project is open-source and available under the **MIT License**.
