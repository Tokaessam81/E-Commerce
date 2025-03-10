🛍️ E-Commerce API

🔗 API Documentation: https://ecommerceapitest.runasp.net/swagger/index.html

📌 Overview

E-Commerce API is a fully functional online store backend that supports
user authentication,
product management, 
orders, shopping cart,
and discount coupons. 

🚀 Features

✔ Authentication & Authorization (JWT & Google Auth).
✔ Email Confirmation (Verify user email upon registration).
✔ Product Management (CRUD operations).
✔ Shopping Cart & Orders .
✔ Coupons & Discounts (Apply discount codes).
✔ Rate Limiting (Protect API from abuse).
✔ Caching (Improve performance using Redis).
✔ Error Handling & Validation (Consistent API responses).
✔ Cloudinary Image Storage (Users upload images to Cloudinary, API stores the URL).
✔ Advanced Architecture (Follows Onion Architecture for better maintainability).
✔ Efficient Data Management (Uses Unit of Work & Generic Repository patterns).

🛠️ Tech Stack

Backend: .NET 8 Web API, Entity Framework Core

Architecture: Onion Architecture

Database: SQL Server

Caching: Redis

Authentication: JWT, Google Auth, Email Confirmation

Storage: Cloudinary (for image storage)

Deployment: Hosted on MonsterASP.NET & Redis Cloud

🔧 Installation & Setup

1️ Clone the repository:

git clone https://github.com/Tokaessam81/E-Commerce.git
cd Ecommerce-API

2️ Restore dependencies:

dotnet restore

3️ Apply database migrations:

dotnet ef database update

4️ Run the application:

dotnet run

5️ Open Swagger UI:

🔗 https://ecommerceapitest.runasp.net/swagger/index.html

