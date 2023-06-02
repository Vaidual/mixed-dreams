# MixedDreams
 ![GitHub last commit](https://img.shields.io/github/last-commit/Vaidual/mixed-dreams)
 
Back-end of POS like system with some storage additions.

## Features:
- Rest API.
- JWT token authorization.
- Onion, 4 layer architecture.
- Interaction with IoT emulator the broker.
- Interaction with cloud service.
- Fluent validation.
- UnitOfWork pattern.

## Installation

1. Clone git repo.
2. In appsetting.json set you database connection string, eg. `Data Source=your_connection_string;...`.
3. Update secret configuration. You can either add properties to main appsetting.json file or to local "secret" file (recommended).
Signature:
```json
{
  "Backblaze": {  
    "BucketId": "",  
    "WriteKey": {
      "KeyId": "",
      "keyName": "",
      "ApplicationKey": ""
    }
  },
  "Jwt": {
    "Audience": "",
    "Issuer": "",
    "SigningKey": ""
  },
  "HiveMQ": {
    "ClientId": "",
    "Server": "",
    "Username": "",
    "Password": ""
  }
}
```
Jwt config is required. You can skip other two but you need to comment this two lines in program.cs:
```csharp
 await app.Services.GetRequiredService<IStorageClient>().ConnectAsync();
 await app.Services.GetRequiredService<IDeviceService>().ConnectAsync();
 ```
Additional you will need to remove corresponding options registration from ServiceExtensions.cs in the infrastructure level with all dependencies of it:
```csharp
services.AddOptions<BackblazeOptions>()
    .Bind(config.GetSection(BackblazeOptions.Backblaze))
    .ValidateDataAnnotations()
    .ValidateOnStart();
services.AddOptions<HiveMQOptions>()
    .Bind(config.GetSection(HiveMQOptions.HiveMQ))
    .ValidateDataAnnotations()
    .ValidateOnStart();
```
4. Install for better experience [front-end](https://github.com/Vaidual/mixed-dreams-front).
