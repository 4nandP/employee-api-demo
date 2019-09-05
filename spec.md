A Get-Web-API targeting .net core that should return employee details with the Url details mentioned 
- Use the attached Json structure that has the response the API is expecting (see below) 
- The back-end can be an file system to retrieve the data.
- Good to have any IOC container for DI
- Micro-Service pattern to be followed (Ignore Authentication and Authorization)
- Unit-Test
- Good to have Swagger Enabled.
- Good to have logging enabled

Verb    | Url                             | Description
--      | --                              | --
**GET** | `api/v1/Employee/GetDetails/Id` | _Retrieves Employee Details_

> Note: Code to be uploaded to Candidate Git-Hub account and share the details


```json
{
   "QueryResponse":{
      "maxResults":"1",
      "startPosition":"1",
      "Employee":{
         "Organization":"false",
         "Title":"Mrs.",
         "GivenName":"Jane",
         "MiddleName":"Lane",
         "FamilyName":"Doe",
         "DisplayName":"Jane Lane Doe",
         "PrintOnCheckName":"Jane Lane Doe",
         "Active":"true",
         "PrimaryPhone":{
            "FreeFormNumber":"505.555.9999"
         },
         "PrimaryEmailAddr":{
            "Address":"user@example.com"
         },
         "EmployeeType":"Regular",
         "status":"Active",
         "Id":"Test123"         
      }
   }
}
```