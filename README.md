# JsonTransformerApi
Consist Test Task - Service for transforming a flat list of people into a hierarchical structure

## How to Run
This API and MongoDB can be launched in containers using a **docker-compose** file. You can also run the API separately, for example, using Visual Studio, but you need to ensure that the database connection string in the **appsettings.json** file is correct.
## Endpoints
#### POST https://localhost:8001/api/Token
This endpoint is used to obtain a token for bearer authentication. The password parameter is passed in the message body and must be one of the allowed passwords (e.g. "123"). The list of allowed passwords is stored in the **appsettings.json** file.

> **Note:** Storing any secrets in clear text in code is not secure. For production code, use a secret store like HashiCorp Vault, AWS Secrets Manager, or Azure Key Vault.

**Example Request**

    curl -X 'POST' \
      'https://localhost:8001/api/Token' \
      -H 'accept: */*' \
      -H 'Content-Type: application/json' \
      -d '"123"'
    
#### POST https://localhost:8001/api/People/Transform

This endpoint transforms an array of flat persons into a hierarchical structure. The Authorization header with the token generated from the previous endpoint is required.

**Example Request**

    curl -X 'POST' \
      'https://localhost:8001/api/People/transform' \
      -H 'accept: */*' \
      -H 'Authorization: bearer generated_token' \
      -H 'Content-Type: application/json' \
      -d '[
      {
        "id": 1,
        "name": "david",
        "parent": null
      }
    ]'
The result is saved to the SimpleDB database in the People collection. The connection string and settings are stored in the **appsettings.json** file.

## SwaggerUI
The SwaggerUI is available for the service at **https://localhost:8001/swagger/index.html**. It is important to authenticate before calling the **api/People/Transform** endpoint using the lock icon button after obtaining the token.