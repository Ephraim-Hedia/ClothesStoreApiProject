# 🛍️ Store API

A complete **Clothing Store Backend API** built with **ASP.NET Core (.NET 8)** and **Entity Framework Core**.  
This API manages users, products, baskets, orders, and authentication using **JWT tokens**.

---

## 🚀 Technologies Used
- ASP.NET Core (.NET 8)
- Entity Framework Core
- Microsoft Identity
- JWT Authentication
- SQL Server
- Repository & Specification Pattern
- AutoMapper

---

## 🧩 Project Structure
Store.API/ → API Layer (Controllers, Startup)
Store.Services/ → Business Logic Layer (Services, DTOs, Handlers)
Store.Repositories/ → Data Access Layer (Generic Repository, Specifications)
Store.Data/ → Entities, DbContext, Configurations


---

## 🔐 Authentication & Authorization
- User registration and login with **ASP.NET Identity**
- JWT-based authentication
- Secured endpoints using `[Authorize]` attribute
- Extracts user information (email, ID) from the JWT claims

---

## 🛒 Main Features

### 🧍‍♂️ User & Authentication
- Register new users  
- Login with JWT token generation  
- Retrieve user profile using token claims  

### 📦 Basket Management
- Add products to basket (with color, size, and quantity)  
- Update or remove items  
- Automatically calculate total price  

### 📬 Address Management
- Add, update, delete, and retrieve user addresses  
- Each address is linked to the authenticated user (via JWT token)  

### 🧾 Orders
- Create orders directly from basket  
- Calculate subtotal and delivery price based on city  
- Track order status (Pending, Completed, Failed)  

### 🏷️ Products
- Retrieve product list with filters and sorting  
- Include product color, size, and price  
- Pagination supported through specifications  

---

## ⚙️ Configuration
Update your **appsettings.json** with the following:
```json
"ConnectionStrings": {
  "DefaultConnection": "Your SQL Server Connection String"
},
"Token": {
  "Key": "super_secret_key_12345",
  "Issuer": "https://localhost:5001",
  "ExpireMinutes": 60
}

## 🧪 How to Run

1. **Clone the repository**
   ```bash
   git clone https://github.com/Ephraim-Hedia/StoreAPI.git
2. **Navigate to the API project**
cd Store.API
3. **Apply database migrations**
dotnet ef database update
4. **Run the project**
dotnet run
5. **Open in browser or API testing tool**
https://localhost:5001/swagger

