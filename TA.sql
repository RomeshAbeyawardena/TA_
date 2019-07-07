IF OBJECT_ID('[dbo].[TokenPermission]') IS NOT NULL
DROP TABLE [dbo].[TokenPermission]

IF OBJECT_ID('[dbo].[Permission]') IS NOT NULL
DROP TABLE [dbo].[Permission]

IF OBJECT_ID('[dbo].[Token]') IS NOT NULL
DROP TABLE [dbo].[Token]

IF OBJECT_ID('[dbo].[User]') IS NOT NULL
DROP TABLE [dbo].[User]

IF OBJECT_ID('[dbo].[SiteAccess]') IS NOT NULL
DROP TABLE [dbo].[SiteAccess]

IF OBJECT_ID('[dbo].[AssetAccess]') IS NOT NULL
DROP TABLE [dbo].[AssetAccess]

IF OBJECT_ID('[dbo].[Asset]') IS NOT NULL
DROP TABLE [dbo].[Asset]

IF OBJECT_ID('[dbo].[Site]') IS NOT NULL
DROP TABLE [dbo].[Site]

CREATE TABLE [dbo].[User](
 [Id] INT NOT NULL IDENTITY(1,1)
,[UserToken] UNIQUEIDENTIFIER NOT NULL
,[EmailAddress] VARBINARY(MAX) NOT NULL
,[Password] VARBINARY(MAX) NOT NULL
,[Created] DATETIMEOFFSET NOT NULL
,[Modified] DATETIMEOFFSET NOT NULL
)

CREATE TABLE [dbo].[Token](
	 [Id] INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_Token PRIMARY KEY
	,[Key] VARCHAR(200) NOT NULL
		CONSTRAINT IQ_Token UNIQUE
	,[IsActive] BIT NOT NULL
	,[Created] DATETIMEOFFSET NOT NULL
	,[Expires] DATETIMEOFFSET NOT NULL
)

DECLARE @dateNow DATETIMEOFFSET = GETDATE()
DECLARE @defaultExpiry DATETIMEOFFSET = DATEADD(YEAR, 4, @dateNow)
INSERT INTO [dbo].[Token] ([Key],[IsActive],[Created],[Expires])
	VALUES ('60807141CC6E54CCCC8A72BEA111A95391C86DC5283643FFD4ABA75373987538685F2E4D007007BF915B4524B1F2EC56BA57DF56F3B1C65B971F902A7116A831',1, @dateNow, @defaultExpiry)

CREATE TABLE [dbo].[Permission](
	[Id] INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_Permission PRIMARY KEY
	,[Name] VARCHAR(200) NOT NULL
	,[Description] VARCHAR(2000) NOT NULL
	,[IsActive] BIT NOT NULL
	,[Created] DATETIMEOFFSET
	,[Modified] DATETIMEOFFSET
)

SET IDENTITY_INSERT  [dbo].[Permission] ON

INSERT INTO [dbo].[Permission]([Id],[Name],[Description],[IsActive],[Created],[Modified])
	VALUES  (1, 'Create', 'Ability to create', 1, @dateNow, @dateNow),
			(2, 'Read', 'Ability to read', 1, @dateNow, @dateNow),
			(3, 'Update', 'Ability to update', 1, @dateNow, @dateNow),
			(4, 'SoftDelete', 'Ability to soft delete', 1, @dateNow, @dateNow),
			(5, 'Delete', 'Ability to delete', 1, @dateNow, @dateNow),
			(6, 'TokenManager', 'Ability to manage API tokens', 1, @dateNow, @dateNow),
			(7, 'ApiAccess', 'Ability to make API calls', 1, @dateNow, @dateNow)

SET IDENTITY_INSERT  [dbo].[Permission] OFF

CREATE TABLE [dbo].[TokenPermission](
	[Id] INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_TokenPermission PRIMARY KEY
	,[PermissionId] INT NOT NULL
	,[TokenId] INT NOT NULL
		CONSTRAINT FK_TokenPermission_Token
			REFERENCES [dbo].[Token]([Id])
	,[Created] DATETIMEOFFSET NOT NULL
	,[Expires] DATETIMEOFFSET NOT NULL
	,CONSTRAINT IQ_TokenPermission UNIQUE([PermissionId], [TokenId])
)

INSERT INTO [dbo].[TokenPermission] ([PermissionId], [TokenId],[Created],[Expires])
	VALUES (1, 1, @dateNow, @defaultExpiry),
		(2, 1, @dateNow, @defaultExpiry),
		(3, 1, @dateNow, @defaultExpiry),
		(4, 1, @dateNow, @defaultExpiry),
		(5, 1, @dateNow, @defaultExpiry),
		(6, 1, @dateNow, @defaultExpiry),
		(7, 1, @dateNow, @defaultExpiry)

CREATE TABLE [dbo].[Site](
[Id] INT NOT NULL IDENTITY(1,1)
 CONSTRAINT PK_Site PRIMARY KEY
,[Name] VARCHAR(200) NOT NULL
,[Url] VARCHAR(2048) NOT NULL
,[JsonAttributes] VARCHAR(2048) NOT NULL
 CONSTRAINT CHK_Site_JsonAttributes
 CHECK (ISJSON([JsonAttributes]) IS NOT NULL)
,[IsActive] BIT NOT NULL
,[Created] DATETIMEOFFSET NOT NULL
,[Modified] DATETIMEOFFSET NOT NULL
)

CREATE TABLE [dbo].[Asset](
[Id] INT NOT NULL IDENTITY(1,1)
 CONSTRAINT PK_Asset PRIMARY KEY
,[SiteId] INT NOT NULL
 CONSTRAINT FK_Asset_Site
 REFERENCES [dbo].[Site]([Id])
,[Key] VARCHAR(200) NOT NULL
 CONSTRAINT IQ_Asset UNIQUE
,[RelativeUrl] VARCHAR(2048) NOT NULL
,[JsonAttributes] VARCHAR(2048) NOT NULL
 CONSTRAINT CHK_Asset_JsonAttributes
 CHECK (ISJSON([JsonAttributes]) IS NOT NULL)
,[IsActive] BIT NOT NULL
,[Created] DATETIMEOFFSET NOT NULL
,[Modified] DATETIMEOFFSET NOT NULL
)

CREATE TABLE [dbo].[AssetAccess](
[Id] INT NOT NULL IDENTITY(1,1)
 CONSTRAINT PK_AssetAccess PRIMARY KEY
,[AssetId] INT NOT NULL
 CONSTRAINT FK_AssetAccess_Asset
 REFERENCES [dbo].[Asset]([Id])
,[Reference] UNIQUEIDENTIFIER NOT NULL
 CONSTRAINT IQ_SiteAccess UNIQUE
,[JsonAttributes] VARCHAR(2048) NOT NULL
 CONSTRAINT CHK_AssetAccess_JsonAttributes
 CHECK (ISJSON([JsonAttributes]) IS NOT NULL)
,[Created] DATETIMEOFFSET NOT NULL
)

CREATE TABLE [dbo].[SiteAccess](
[Id] INT IDENTITY(1,1)
 CONSTRAINT PK_SiteAccess PRIMARY KEY
,[SiteId] INT NOT NULL
 CONSTRAINT FK_SiteAccess
 REFERENCES [dbo].[Site]([Id])
,[Reference] UNIQUEIDENTIFIER NOT NULL
 CONSTRAINT FK_SiteAccess_AssetAccess_Reference
 REFERENCES [dbo].[AssetAccess]([Reference])
,[IsVerifiedAccess] BIT NOT NULL
,[JsonAttributes] VARCHAR(2048) NOT NULL
 CONSTRAINT CHK_SiteAccess_JsonAttributes
 CHECK (ISJSON([JsonAttributes]) IS NOT NULL)
,[Created] DATETIMEOFFSET NOT NULL
)
GO

IF OBJECT_ID('[dbo].[GetSiteName]') IS NOT NULL
DROP FUNCTION [dbo].[GetSiteName]

GO

CREATE FUNCTION [dbo].[GetSiteName](@siteId INT)
RETURNS VARCHAR(200)
AS BEGIN
DECLARE @siteName VARCHAR(200)
SELECT @siteName = [Name] FROM [dbo].[Site]
WHERE [Id] = @siteId

RETURN @siteName
END

GO

IF OBJECT_ID('[dbo].[GetAssetKey]') IS NOT NULL
DROP FUNCTION [dbo].[GetAssetKey]
GO

CREATE FUNCTION [dbo].[GetAssetKey](@assetId INT)
RETURNS VARCHAR(200)
AS BEGIN
DECLARE @assetKey VARCHAR(200)
SELECT @assetKey = [Key] FROM [dbo].[Asset]
WHERE [Id] = @assetId

RETURN @assetKey
END
GO

IF OBJECT_ID('[dbo].[SiteAssetAccessView]') IS NOT NULL
DROP VIEW [dbo].[SiteAssetAccessView]

GO

CREATE VIEW [dbo].[SiteAssetAccessView]
AS SELECT [Site].[Id] [SiteId], [Asset].[Id] [AssetId], [dbo].GetSiteName([Site].Id) [SiteName], 
[dbo].[GetAssetKey]([Asset].[Id]) [AssetKey], 
COUNT([AssetAccess].[Id]) [Views]
FROM [dbo].[AssetAccess] [AssetAccess]
INNER JOIN [dbo].[Asset] [Asset]
ON Asset.Id = AssetAccess.AssetId
INNER JOIN [dbo].[Site] [Site]
ON Site.Id = Asset.SiteId
GROUP BY [Site].[Id], [Asset].[Id]
GO

IF OBJECT_ID('[dbo].[AddUpdateSiteAccessTrigger]') IS NOT NULL
DROP TRIGGER [dbo].[AddUpdateSiteAccessTrigger]
GO

CREATE TRIGGER [dbo].[AddUpdateSiteAccessTrigger]
ON [dbo].[AssetAccess]
AFTER INSERT
AS BEGIN
DECLARE @assetId INT
DECLARE @siteId INT

SELECT @assetId = [Inserted].[AssetId] FROM [Inserted]

SELECT @siteId = [SiteId] FROM [dbo].[Asset]
WHERE [Id] = @assetId

INSERT INTO [dbo].[SiteAccess]
(
    [SiteId],
    [Reference],
    [JsonAttributes],
    [Created]
)
SELECT 
   @siteId,                  -- SiteId - int
    [Reference],               -- Reference - uniqueidentifier
    [JsonAttributes],                 -- JsonAttributes - varchar(2048)
    SYSDATETIMEOFFSET() -- Created - datetimeoffset
 FROM [Inserted]
END