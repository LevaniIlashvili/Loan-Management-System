IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'lmsdb')
BEGIN
    CREATE DATABASE lmsdb;
END;