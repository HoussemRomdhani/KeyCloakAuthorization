## Keycloak Integration with ASP.NET Core

### 1 Run Keycloak with Docker compose

```bash
docker-compose -f key-cloak.yml up
 ```

### 2 Create a realm in Keycloak called "demo"
 Import realm, client, users and roles from  **realm-export.json** 

 ### 3 Prepare Postman collection 
 Import Postman collection from  **key-cloak.postman_collection.json** 

### 4 Run web api 

### 5 Call enpoints 
