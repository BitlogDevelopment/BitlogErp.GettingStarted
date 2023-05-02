# Bitlog ERP API getting started project
This is a sample project to get you started with integration to Bitlog ERP.

Swagger: https://erpapi-development.bitlogwms.com/swagger/index.html

## Usage

The project is built as a CLI where you'd use commands to communicate with Bitlog ERP via a REST API. Use the `m` command to see the list of sub-menues.

There are some endpoints and data contracts created for you but some of them are not complete.

You may uncomment `HttpLogHanlder` dependency injection and change the logging level in the `Program.cs` to see communication details.

## Authentication

You are going to need the following credentials to communicate with Bitlog ERP:

- *Token Host*

URL to obtain an access token. Use the following URL:

https://loginservice-development.bitlogwms.com

- *Client ID* / *Client Secret*

You unique security pair to obtain an access token (provided by Bitlog)

- *Resett key*

A key for resetting data (provided by Bitlog)

- *API Host*

URL to Bitlog ERP API. Use the following URL:

https://erpapi-development.bitlogwms.com

## Dependencies

- **Refit.HttpClientFactory**

REST API library

- **Newtonsoft.Json**

JSON serialization library

- **Serilog**

Logging framework
