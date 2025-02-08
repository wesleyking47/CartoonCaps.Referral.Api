# CartoonCaps.Referral.Api

## Description

Carton Caps is an app that empowers consumers to raise money for the schools they care about, while buying the everyday products they love. This repository contains the API for a new referral feature that allows Carton Caps app users to refer their friends to install the Carton Caps app using shareable deferred deep links. These links enable a customized onboarding experience for referred users after they install the app.

## Installation Instructions

### Prerequisites

- Install Docker. You can download it from the [official Docker website](https://www.docker.com/get-started).
- Install the .NET EF tool globally:
    ```shell
    dotnet tool install --global dotnet-ef
    ```

### Steps

1. Clone the repository:
    ```shell
    git clone https://github.com/wesleyking47/CartoonCaps.Referral.Api.git
    cd CartoonCaps.Referral.Api
    ```

2. Build and run the Docker containers:
    ```shell
    docker compose up --build
    ```

3. While the Docker container is running, apply the database migrations:
    ```shell
    dotnet ef database update --project ./CartoonCaps.Referral.Infrastructure --connection "Host=localhost;Database=Referrals;Username=postgres;Password=Password1!"
    ```

4. To view the API specification, navigate to:
    ```
    http://localhost:8080/scalar
    ```

## Usage Instructions

### Endpoints

#### Create a new referral

**Request:**
```http
POST /api/v1/Referrals
Content-Type: application/json

{
    "refereeId": 12345,
    "referralCode": "ABCDE"
}
```

**Response:**
- **201 Created:** Referral created successfully.
- **400 Bad Request:** Invalid input data.
- **Default:** General error response.

#### Get referrals for a user

**Request:**
```http
GET /api/v1/Referrals/{userId}
```

**Response:**
- **200 OK:**
    ```json
    {
        "referralRecords": [
            {
                "refereeName": "John Doe",
                "referralStatus": "pending"
            }
        ]
    }
    ```
- **404 Not Found:** User not found.
- **Default:** General error response.

#### Update a referral

**Request:**
```http
PUT /api/v1/Referrals
Content-Type: application/json

{
    "refereeId": 12345,
    "status": "accepted"
}
```

**Response:**
- **204 No Content:** Referral updated successfully.
- **400 Bad Request:** Invalid input data.
- **404 Not Found:** Referral not found.
- **Default:** General error response.

#### Delete a referral

**Request:**
```http
DELETE /api/v1/Referrals
Content-Type: application/json

{
    "refereeId": 12345
}
```

**Response:**
- **200 OK:** Referral deleted successfully.
- **400 Bad Request:** Invalid input data.
- **404 Not Found:** Referral not found.
- **Default:** General error response.

## Technologies Used

- .NET 9
- Docker

## Testing

To run the tests for the project, use the .NET CLI:

```shell
dotnet test
```