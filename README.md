To help you build a professional README that showcases your learning journey in backend security, you can use the content below. I have structured it to highlight the specific architectural choices you've made in your Program.cs, such as the Cookie-JWT hybrid approach and the Clean Architecture patterns.

💰 Budget Wars API
A modern .NET backend designed to master the complexities of Identity Management, JWT Authentication, and Secure Authorization. This project serves as a deep dive into building production-ready security layers for a financial tracking application.

🎯 Learning Objectives & Purpose
This project was built with the primary goal of advancing backend engineering skills in:

Identity Systems: Implementing Microsoft.AspNetCore.Identity for robust user management.

Secure Token Handling: Using a hybrid approach where JWTs are issued as HttpOnly cookies to mitigate XSS (Cross-Site Scripting) risks.

Refresh Token Patterns: Implementing long-lived refresh tokens to maintain user sessions securely.

Architecture Patterns: Utilizing MediatR for decoupled request handling and Global Exception Handling for clean API responses.

🔐 Security Architecture
1. The Authentication Flow (Cookie-JWT Hybrid)
Unlike traditional APIs that store tokens in localStorage, this API uses a more secure cookie-based delivery system:

Access Token: The API extracts the JWT from a cookie named ACCESS_Token during the OnMessageReceived event.

Refresh Token: A dedicated REFRESH_Token cookie is used to silently renew access without requiring user re-login.

2. User Management & Identity
Identity Framework: Uses AddIdentity<ApplicationUser, IdentityRole> to manage users, roles, and password hashing.

Database: Integrated with MySQL using Entity Framework Core and DbContext pooling for high performance.

Password Policies: Configured with custom rules (e.g., minimum length of 6, unique emails required).

🛠️ Tech Stack
Framework: .NET 8/9

Database: MySQL

Authentication: JWT Bearer & ASP.NET Identity

API Documentation:  postman

Patterns: MediatR, Repository Pattern, Dependency Injection
