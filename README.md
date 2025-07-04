# Hacker News API

This is a .NET 8 Web API backend for viewing the latest stories from Hacker News.

## Features

*   **Fetches Data**: Retrieves the newest stories from the official Hacker News API.
*   **Caching**: Implements in-memory caching to minimize redundant calls to the Hacker News API and improve performance.
*   **Pagination**: Supports paginating through the list of stories.
*   **Search**: Allows searching for stories based on their titles.
*   **Testing**: Includes a suite of unit and integration tests to ensure reliability.

## Prerequisites

Before you begin, ensure you have the following installed:

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup and Installation

1.  **Clone the repository:**
    ```sh
    git clone <repository-url>
    cd HackerNews
    ```

## How to Run the Application

1.  Open a new terminal.
2.  Navigate to the backend project directory:
    ```sh
    cd HackerNewsApi
    ```
3.  Run the application:
    ```sh
    dotnet run
    ```
4.  The API will be running at `http://localhost:5109`. You can view the Swagger UI for API documentation at `http://localhost:5109/swagger`.

## Running Tests

1.  Navigate to the backend project directory:
    ```sh
    cd HackerNewsApi
    ```
2.  Run the tests:
    ```sh
    dotnet test
    ```

## Deployment to Azure

This section provides instructions for deploying the backend API to Azure App Service on the free tier.

### Backend (.NET API) Deployment to Azure App Service

1.  **Install the Azure App Service Extension for VS Code.**
2.  **Open the `HackerNewsApi` folder in VS Code.**
3.  **Sign in to your Azure account** from the Azure extension.
4.  **Right-click on the `HackerNewsApi` project folder and select "Deploy to Web App...".**
5.  Follow the prompts:
    *   Select **"Create new Web App..."**.
    *   Enter a globally unique name for your app (e.g., `my-hacker-news-api-123`). This will become part of your URL.
    *   Select **.NET 8** as the runtime stack.
    *   Select **Free (F1)** as the pricing plan.
    *   When prompted to enable Application Insights, select **Skip for now**.
6.  The extension will package and deploy your API. Once complete, the API will be available at `https://<your-app-name>.azurewebsites.net`.
