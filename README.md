# 🛒 Ecommerce Platform

[![Build Status](https://img.shields.io/github/actions/workflow/status/Abdullah-Builds/Ecommerce-/dotnet.yml?branch=main)](https://github.com/Abdullah-Builds/Ecommerce-/actions)  
[![License](https://img.shields.io/badge/license-Unlicensed-lightgrey.svg)](LICENSE)  
![Last Commit](https://img.shields.io/github/last-commit/Abdullah-Builds/Ecommerce-)  
![Contributors](https://img.shields.io/github/contributors/Abdullah-Builds/Ecommerce-)  

---

## 📌 Overview

**Ecommerce Platform** is a modern, scalable, and secure web application built with **ASP.NET Core**.  
It provides all the essential features for running an online shop — including authentication, product management, order processing, and email-based confirmations — making it an excellent starting point for e-commerce projects.  

---

## ✨ Features

✅ **User & Admin Management**  
- User registration with **email confirmation**  
- Secure login/logout system  
- Role-based authorization (Admin, Customer)  

✅ **Product Management**  
- Add, edit, and delete products  
- Manage stock and product images  
- Product details page with description & pricing  

✅ **Shopping Cart & Checkout**  
- Add/remove/update products in cart  
- Persistent cart across sessions  
- Order checkout workflow  

✅ **Order Management**  
- Place orders and track status  
- Admin can view and manage all orders  
- Customers receive **email confirmation** after successful order  

✅ **Email Notifications**  
- **Account verification** via email confirmation link  
- **Order confirmation** emails sent to customers  

✅ **UI & Experience**  
- Responsive design (desktop, tablet, mobile)  
- Clean product catalog and cart UI  

✅ **Security**  
- CSRF protection  
- Password hashing & secure authentication  
- Input validation  

✅ **Performance**  
- Efficient Entity Framework Core queries  
- Caching strategies for faster product loading  

---

## 🛠 Tech Stack

- **Backend**: ASP.NET Core (C#)  
- **Frontend**: HTML, CSS, JavaScript  
- **Database**: SQL Server / Entity Framework Core  
- **Email Service**: SMTP (configurable for Gmail, Outlook, etc.)  
- **Tools**: Node.js, npm (for frontend dependencies)  

---

## 🚀 Getting Started

### ✅ Prerequisites

- [.NET SDK (6.0 or higher)](https://dotnet.microsoft.com/)  
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)  
- [Node.js & npm](https://nodejs.org/)  
- SQL Server (local or cloud instance)  

### 📥 Installation

```bash
# Clone the repository
git clone https://github.com/Abdullah-Builds/Ecommerce-.git
cd Ecommerce-

# Navigate to the main project folder
cd MyApp

# Restore .NET dependencies
dotnet restore

# (Optional) Install frontend dependencies
npm install
