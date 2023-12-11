# Diff
Simple diffing API project.

There are two http endpoints (<host>/v1/diff/<ID>/left and <host>/v1/diff/<ID>/right) that accept JSON containing base64 encoded binary data on both endpoints.

The provided data is diff-ed and the results are available on a third endpoint.

When you run REST API the description is available in swagger UI (default port is 5084). You can run it from VS as http(s) application.

# Config

```DBConnectionString``` should point to sql server with database DiffDB. The system administrator (SA) account provided in json example should be 
replaced with a user, that has the following permissions: SELECT, INSERT, UPDATE. 

For running the functional tests separate database should be established with a user that can also truncate tables.

If you want to run application without peristent storage then set DBConnectionString to null or empty string (functional tests can run with DB or with cache).

## Set up sql server in docker

See instructions here: https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash

Connect to SQL Server (https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash#connect-to-sql-server) and then 
create a new database.

## Prepare database

Initialise database:

```
CREATE DATABASE DiffDB;
GO
```

Create tables with:

```
USE DiffDB;
CREATE TABLE LeftDiff (id INT PRIMARY KEY, data VARCHAR(max) NOT NULL);
CREATE TABLE RightDiff (id INT PRIMARY KEY, data VARCHAR(max) NOT NULL);
GO
```

You should set up separate database for tests.


