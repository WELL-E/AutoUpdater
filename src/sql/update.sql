CREATE TABLE `updateversioninfo` (
  `MD5` varchar(32) NOT NULL DEFAULT 'update packet md5',
  `PubTime` int DEFAULT '0',
  `Name` varchar(64) NOT NULL DEFAULT 'version name',
  `Url` varchar(255) NOT NULL DEFAULT 'update url',
  `Version` varchar(20) NOT NULL DEFAULT 'last version number',
  `ClientType` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`MD5`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ciupdateversioninfo;