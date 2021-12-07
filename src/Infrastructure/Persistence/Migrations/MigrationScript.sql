CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `AspNetUsers` (
    `Id` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `ProfileStatus` int NOT NULL,
    `FirstName` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `LastName` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Cui` char(13) CHARACTER SET utf8mb4 NULL,
    `ValidatedDpi` tinyint(1) NOT NULL,
    `Address` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ValidatedAddress` tinyint(1) NOT NULL,
    `Reputation` double NULL,
    `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Categories` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(512) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `LastModifiedBy` longtext CHARACTER SET utf8mb4 NULL,
    `LastModified` datetime(6) NULL,
    CONSTRAINT `PK_Categories` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `DeviceCodes` (
    `UserCode` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `DeviceCode` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `SubjectId` varchar(200) CHARACTER SET utf8mb4 NULL,
    `SessionId` varchar(100) CHARACTER SET utf8mb4 NULL,
    `ClientId` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(200) CHARACTER SET utf8mb4 NULL,
    `CreationTime` datetime(6) NOT NULL,
    `Expiration` datetime(6) NOT NULL,
    `Data` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_DeviceCodes` PRIMARY KEY (`UserCode`)
) CHARACTER SET utf8mb4;

CREATE TABLE `PersistedGrants` (
    `Key` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Type` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `SubjectId` varchar(200) CHARACTER SET utf8mb4 NULL,
    `SessionId` varchar(100) CHARACTER SET utf8mb4 NULL,
    `ClientId` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(200) CHARACTER SET utf8mb4 NULL,
    `CreationTime` datetime(6) NOT NULL,
    `Expiration` datetime(6) NULL,
    `ConsumedTime` datetime(6) NULL,
    `Data` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_PersistedGrants` PRIMARY KEY (`Key`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Pictures` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `StorageType` int NOT NULL,
    `PictureContent` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Pictures` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `TodoLists` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Colour` longtext CHARACTER SET utf8mb4 NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `LastModifiedBy` longtext CHARACTER SET utf8mb4 NULL,
    `LastModified` datetime(6) NULL,
    CONSTRAINT `PK_TodoLists` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `Products` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(512) CHARACTER SET utf8mb4 NOT NULL,
    `OtherNames` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
    `OwnerId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `Location_Latitude` double NULL,
    `Location_Longitude` double NULL,
    `Location_City` varchar(128) CHARACTER SET utf8mb4 NULL,
    `Location_State` varchar(128) CHARACTER SET utf8mb4 NULL,
    `Location_StaticMap` varchar(4096) CHARACTER SET utf8mb4 NULL,
    `CostPerDay` decimal(65,30) NOT NULL,
    `CostPerWeek` decimal(65,30) NULL,
    `CostPerMonth` decimal(65,30) NULL,
    `Rating` double NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `LastModifiedBy` longtext CHARACTER SET utf8mb4 NULL,
    `LastModified` datetime(6) NULL,
    CONSTRAINT `PK_Products` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Products_AspNetUsers_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `RatingToUsers` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `FromUserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `ToUserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `RatingValue` int NULL,
    `Comment` varchar(1024) CHARACTER SET utf8mb4 NULL,
    `RatingDate` datetime(6) NULL,
    CONSTRAINT `PK_RatingToUsers` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RatingToUsers_AspNetUsers_FromUserId` FOREIGN KEY (`FromUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RatingToUsers_AspNetUsers_ToUserId` FOREIGN KEY (`ToUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `UserProfileEvents` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `EventType` int NOT NULL,
    `EventDate` datetime(6) NOT NULL,
    `UserProfileId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `UserEventId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `Message` varchar(128) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_UserProfileEvents` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserProfileEvents_AspNetUsers_UserEventId` FOREIGN KEY (`UserEventId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UserProfileEvents_AspNetUsers_UserProfileId` FOREIGN KEY (`UserProfileId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `AddressPictures` (
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `PictureId` bigint NOT NULL,
    CONSTRAINT `PK_AddressPictures` PRIMARY KEY (`UserId`),
    CONSTRAINT `FK_AddressPictures_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AddressPictures_Pictures_PictureId` FOREIGN KEY (`PictureId`) REFERENCES `Pictures` (`Id`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `DpiPictures` (
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `PictureId` bigint NOT NULL,
    CONSTRAINT `PK_DpiPictures` PRIMARY KEY (`UserId`),
    CONSTRAINT `FK_DpiPictures_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_DpiPictures_Pictures_PictureId` FOREIGN KEY (`PictureId`) REFERENCES `Pictures` (`Id`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `ProfilePictures` (
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `PictureId` bigint NOT NULL,
    CONSTRAINT `PK_ProfilePictures` PRIMARY KEY (`UserId`),
    CONSTRAINT `FK_ProfilePictures_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ProfilePictures_Pictures_PictureId` FOREIGN KEY (`PictureId`) REFERENCES `Pictures` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `UserPictures` (
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `PictureId` bigint NOT NULL,
    CONSTRAINT `PK_UserPictures` PRIMARY KEY (`UserId`),
    CONSTRAINT `FK_UserPictures_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UserPictures_Pictures_PictureId` FOREIGN KEY (`PictureId`) REFERENCES `Pictures` (`Id`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `TodoItems` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ListId` int NOT NULL,
    `Title` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `Note` longtext CHARACTER SET utf8mb4 NULL,
    `Done` tinyint(1) NOT NULL,
    `Reminder` datetime(6) NULL,
    `Priority` int NOT NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `LastModifiedBy` longtext CHARACTER SET utf8mb4 NULL,
    `LastModified` datetime(6) NULL,
    CONSTRAINT `PK_TodoItems` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_TodoItems_TodoLists_ListId` FOREIGN KEY (`ListId`) REFERENCES `TodoLists` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `ProductCategories` (
    `CategoryId` bigint NOT NULL,
    `ProductId` bigint NOT NULL,
    CONSTRAINT `PK_ProductCategories` PRIMARY KEY (`CategoryId`, `ProductId`),
    CONSTRAINT `FK_ProductCategories_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ProductCategories_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `ProductPictures` (
    `ProductId` bigint NOT NULL,
    `PictureId` bigint NOT NULL,
    CONSTRAINT `PK_ProductPictures` PRIMARY KEY (`ProductId`, `PictureId`),
    CONSTRAINT `FK_ProductPictures_Pictures_PictureId` FOREIGN KEY (`PictureId`) REFERENCES `Pictures` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_ProductPictures_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `RatingToProducts` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `FromUserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `ProductId` bigint NOT NULL,
    `ProductRatingValue` int NULL,
    `OwnerRatingValue` int NULL,
    `Comment` varchar(1024) CHARACTER SET utf8mb4 NULL,
    `RatingDate` datetime(6) NULL,
    CONSTRAINT `PK_RatingToProducts` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RatingToProducts_AspNetUsers_FromUserId` FOREIGN KEY (`FromUserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_RatingToProducts_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `RentCosts` (
    `Duration` int NOT NULL,
    `ProductId` bigint NOT NULL,
    `Cost` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_RentCosts` PRIMARY KEY (`ProductId`, `Duration`),
    CONSTRAINT `FK_RentCosts_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `RentRequests` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `RequestDate` datetime(6) NOT NULL,
    `ProductId` bigint NOT NULL,
    `RequestorId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `StartDate` datetime(6) NOT NULL,
    `EndDate` datetime(6) NOT NULL,
    `Place` varchar(128) CHARACTER SET utf8mb4 NULL,
    `EstimatedCost` decimal(65,30) NOT NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `LastModifiedBy` longtext CHARACTER SET utf8mb4 NULL,
    `LastModified` datetime(6) NULL,
    CONSTRAINT `PK_RentRequests` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RentRequests_AspNetUsers_RequestorId` FOREIGN KEY (`RequestorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_RentRequests_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `Rents` (
    `RequestId` bigint NOT NULL,
    `Status` int NOT NULL,
    `StartDate` datetime(6) NULL,
    `EndDate` datetime(6) NULL,
    `TotalCost` decimal(65,30) NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `LastModifiedBy` longtext CHARACTER SET utf8mb4 NULL,
    `LastModified` datetime(6) NULL,
    CONSTRAINT `PK_Rents` PRIMARY KEY (`RequestId`),
    CONSTRAINT `FK_Rents_RentRequests_RequestId` FOREIGN KEY (`RequestId`) REFERENCES `RentRequests` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `RequestEvents` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `EventType` int NOT NULL,
    `RentRequestId` bigint NOT NULL,
    `EventDate` datetime(6) NOT NULL,
    `Message` varchar(128) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_RequestEvents` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RequestEvents_RentRequests_RentRequestId` FOREIGN KEY (`RentRequestId`) REFERENCES `RentRequests` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `Conflicts` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `RentId` bigint NOT NULL,
    `ModeratorId` varchar(95) CHARACTER SET utf8mb4 NULL,
    `Description` varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
    `ComplainantId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `ConflictDate` datetime(6) NOT NULL,
    `CreatedBy` longtext CHARACTER SET utf8mb4 NULL,
    `Created` datetime(6) NOT NULL,
    `LastModifiedBy` longtext CHARACTER SET utf8mb4 NULL,
    `LastModified` datetime(6) NULL,
    CONSTRAINT `PK_Conflicts` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Conflicts_AspNetUsers_ComplainantId` FOREIGN KEY (`ComplainantId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Conflicts_AspNetUsers_ModeratorId` FOREIGN KEY (`ModeratorId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Conflicts_Rents_RentId` FOREIGN KEY (`RentId`) REFERENCES `Rents` (`RequestId`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `RentEvents` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `EventType` int NOT NULL,
    `RentId` bigint NOT NULL,
    `EventDate` datetime(6) NOT NULL,
    `Message` varchar(128) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_RentEvents` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RentEvents_Rents_RentId` FOREIGN KEY (`RentId`) REFERENCES `Rents` (`RequestId`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `ConflictRecords` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ConflictId` bigint NOT NULL,
    `Description` varchar(1024) CHARACTER SET utf8mb4 NOT NULL,
    `RecordDate` datetime(6) NOT NULL,
    CONSTRAINT `PK_ConflictRecords` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ConflictRecords_Conflicts_ConflictId` FOREIGN KEY (`ConflictId`) REFERENCES `Conflicts` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `ChatMessages` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `RoomId` bigint NOT NULL,
    `SenderId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `MessageType` int NOT NULL,
    `SentDate` datetime(6) NULL,
    `Content` varchar(512) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ChatMessages` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ChatMessages_AspNetUsers_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `ChatRooms` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ProductId` bigint NOT NULL,
    `UserId` varchar(95) CHARACTER SET utf8mb4 NULL,
    `LastMessageId` bigint NULL,
    CONSTRAINT `PK_ChatRooms` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ChatRooms_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_ChatRooms_ChatMessages_LastMessageId` FOREIGN KEY (`LastMessageId`) REFERENCES `ChatMessages` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_ChatRooms_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `UserChatRooms` (
    `UserId` varchar(95) CHARACTER SET utf8mb4 NOT NULL,
    `RoomId` bigint NOT NULL,
    CONSTRAINT `PK_UserChatRooms` PRIMARY KEY (`UserId`, `RoomId`),
    CONSTRAINT `FK_UserChatRooms_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UserChatRooms_ChatRooms_RoomId` FOREIGN KEY (`RoomId`) REFERENCES `ChatRooms` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE INDEX `IX_AddressPictures_PictureId` ON `AddressPictures` (`PictureId`);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `IX_AspNetUsers_Cui` ON `AspNetUsers` (`Cui`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

CREATE UNIQUE INDEX `IX_Categories_Name` ON `Categories` (`Name`);

CREATE INDEX `IX_ChatMessages_RoomId` ON `ChatMessages` (`RoomId`);

CREATE INDEX `IX_ChatMessages_SenderId` ON `ChatMessages` (`SenderId`);

CREATE UNIQUE INDEX `IX_ChatRooms_LastMessageId` ON `ChatRooms` (`LastMessageId`);

CREATE UNIQUE INDEX `IX_ChatRooms_ProductId_UserId` ON `ChatRooms` (`ProductId`, `UserId`);

CREATE INDEX `IX_ChatRooms_UserId` ON `ChatRooms` (`UserId`);

CREATE INDEX `IX_ConflictRecords_ConflictId` ON `ConflictRecords` (`ConflictId`);

CREATE INDEX `IX_Conflicts_ComplainantId` ON `Conflicts` (`ComplainantId`);

CREATE INDEX `IX_Conflicts_ModeratorId` ON `Conflicts` (`ModeratorId`);

CREATE UNIQUE INDEX `IX_Conflicts_RentId` ON `Conflicts` (`RentId`);

CREATE UNIQUE INDEX `IX_DeviceCodes_DeviceCode` ON `DeviceCodes` (`DeviceCode`);

CREATE INDEX `IX_DeviceCodes_Expiration` ON `DeviceCodes` (`Expiration`);

CREATE INDEX `IX_DpiPictures_PictureId` ON `DpiPictures` (`PictureId`);

CREATE INDEX `IX_PersistedGrants_Expiration` ON `PersistedGrants` (`Expiration`);

CREATE INDEX `IX_PersistedGrants_SubjectId_ClientId_Type` ON `PersistedGrants` (`SubjectId`, `ClientId`, `Type`);

CREATE INDEX `IX_PersistedGrants_SubjectId_SessionId_Type` ON `PersistedGrants` (`SubjectId`, `SessionId`, `Type`);

CREATE INDEX `IX_ProductCategories_ProductId` ON `ProductCategories` (`ProductId`);

CREATE INDEX `IX_ProductPictures_PictureId` ON `ProductPictures` (`PictureId`);

CREATE INDEX `IX_Products_Name` ON `Products` (`Name`);

CREATE INDEX `IX_Products_OtherNames` ON `Products` (`OtherNames`);

CREATE INDEX `IX_Products_OwnerId` ON `Products` (`OwnerId`);

CREATE INDEX `IX_ProfilePictures_PictureId` ON `ProfilePictures` (`PictureId`);

CREATE INDEX `IX_RatingToProducts_FromUserId` ON `RatingToProducts` (`FromUserId`);

CREATE INDEX `IX_RatingToProducts_ProductId` ON `RatingToProducts` (`ProductId`);

CREATE INDEX `IX_RatingToUsers_FromUserId` ON `RatingToUsers` (`FromUserId`);

CREATE INDEX `IX_RatingToUsers_ToUserId` ON `RatingToUsers` (`ToUserId`);

CREATE INDEX `IX_RentEvents_RentId` ON `RentEvents` (`RentId`);

CREATE INDEX `IX_RentRequests_ProductId` ON `RentRequests` (`ProductId`);

CREATE INDEX `IX_RentRequests_RequestorId` ON `RentRequests` (`RequestorId`);

CREATE INDEX `IX_RequestEvents_RentRequestId` ON `RequestEvents` (`RentRequestId`);

CREATE INDEX `IX_TodoItems_ListId` ON `TodoItems` (`ListId`);

CREATE INDEX `IX_UserChatRooms_RoomId` ON `UserChatRooms` (`RoomId`);

CREATE INDEX `IX_UserPictures_PictureId` ON `UserPictures` (`PictureId`);

CREATE INDEX `IX_UserProfileEvents_UserEventId` ON `UserProfileEvents` (`UserEventId`);

CREATE INDEX `IX_UserProfileEvents_UserProfileId` ON `UserProfileEvents` (`UserProfileId`);

ALTER TABLE `ChatMessages` ADD CONSTRAINT `FK_ChatMessages_ChatRooms_RoomId` FOREIGN KEY (`RoomId`) REFERENCES `ChatRooms` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20211207023539_InitialCreate', '5.0.12');

COMMIT;

