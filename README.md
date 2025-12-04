# <div align="center"> Ambev Developer Evaluation Project </div>

<div style="display: inline_block" align="center">
  <img src="https://img.shields.io/github/last-commit/ving-developer/Ambev.DeveloperEvaluation.WebApi?style=flat&logo=github"/>
  <a href="https://www.linkedin.com/in/henrique-barros-7b1812209/">
    <img src="https://img.shields.io/badge/Linkedin-Henrique%20Barros-blue?style=flat&logo=linkedin"/>
  </a>
</div>

<div style="display: inline_block" align="center">
<img alt="Dotnet 8" src="https://img.shields.io/badge/.NET-8-8A2BE2">
<br>
<br>
<br>
<br>
</div>

> This is a technical test project for Ambev. The project implements an API for managing sales, including creating, updating, and cancelling sales, as well as handling sale items. 
>
> For full specifications and detailed requirements, see the [Project Specifications](./.doc/project-description.md) document.

## Tech Stack
* .NET 8
* MediatR for CQRS & domain events
* Entity Framework Core for data access
* AutoMapper for DTO mapping
* FluentValidation for request validation
* NSubstitute & xUnit for unit testing

## API Endpoints

| Method | Endpoint                                 | Description                                           | Request Body                         | Response | Auth Required |
|--------|-----------------------------------------|-------------------------------------------------------|--------------------------------------|----------|---------------|
| POST   | /api/auth                                | Authenticate a user                                   | `AuthenticateUserRequest`            | `AuthenticateUserResponse` | No |
| POST   | /api/users                               | Create a new user (initial user allowed without auth)| `CreateUserRequest`                  | `UserResponse` | No |
| GET    | /api/users/{id}                          | Retrieve a user by ID                                 | -                                    | `UserResponse` | Yes |
| GET    | /api/users                               | List all users (paginated)                            | `ListUsersRequest`                   | `PaginatedResponse<UserResponse>` | Yes |
| PUT    | /api/users/{id}                          | Update an existing user                               | `UpdateUserRequest`                  | `UserResponse` | Yes |
| DELETE | /api/users/{id}                          | Delete a user by ID                                   | -                                    | Success message | Yes |
| POST   | /api/branches                            | Create a new branch                                   | `CreateBranchRequest`                | `BranchResponse` | Yes |
| GET    | /api/branches/{id}                       | Retrieve a branch by ID                                | -                                    | `BranchResponse` | Yes |
| GET    | /api/branches                            | List all branches (paginated)                         | `ListBranchesRequest`                | `PaginatedResponse<BranchResponse>` | Yes |
| DELETE | /api/branches/{id}                       | Delete a branch by ID                                  | -                                    | Success message | Yes |
| POST   | /api/products                            | Create a new product                                   | `CreateProductRequest`               | `ProductResponse` | Yes |
| GET    | /api/products/{id}                       | Retrieve a product by ID                                | -                                    | `ProductResponse` | Yes |
| GET    | /api/products                            | List all products (paginated)                          | `ListProductsRequest`                | `PaginatedResponse<ProductResponse>` | Yes |
| DELETE | /api/products/{id}                       | Delete a product by ID                                  | -                                    | Success message | Yes |
| GET    | /api/carts                               | List all carts (paginated, each cart represents a sale)| `ListCartsRequest`                  | `PaginatedResponse<CartResponse>` | Yes |
| POST   | /api/carts                               | Create a new cart (start a sale)                       | `CreateCartRequest`                  | `CartResponse` | Yes |
| GET    | /api/carts/{id}                          | Retrieve a cart by ID                                   | -                                    | `CartResponse` | Yes |
| PATCH  | /api/carts/{id}/cancel                    | Cancel a cart (sale)                                   | `CancelCartRequest`                  | Success message | Yes |
| POST   | /api/carts/{id}/items                     | Add an item to a cart                                   | `AddItemToCartRequest`               | `CartResponse` | Yes |
| DELETE | /api/carts/{cartId}/items/{itemId}       | Remove an item from a cart                               | -                                    | `CartResponse` | Yes |
| POST   | /api/carts/{id}/complete                  | Complete a cart (finalize sale)                         | -                                    | `CartResponse` | Yes |
| PUT    | /api/carts/{cartId}/items/{itemId}/quantity | Update item quantity in a cart                           | `UpdateItemQuantityRequest`          | `CartResponse` | Yes |

## How to Run the Project

Follow the steps below to set up and run the project locally:

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or another compatible database
- [Git](https://git-scm.com/)
- [Docker (required for running integration tests)](https://www.docker.com/)  
  ⚠️The integration tests rely on Docker containers (runs PostgreSQL) and will not run correctly without Docker installed and running.

### Steps to Run

1. **Clone the repository**
```bash
git clone https://github.com/ving-developer/Ambev.DeveloperEvaluation.WebApi.git
```

2. **Start application**
```bash
cd Ambev.DeveloperEvaluation.WebApi
cd backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet run
```
3. **Access the API**
The application will be available at: http://localhost:5119/swagger/index.html