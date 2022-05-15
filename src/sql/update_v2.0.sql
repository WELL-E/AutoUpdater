-- --------------------------------------------------------
-- 主机:                           124.71.144.66
-- 服务器版本:                        5.6.50-log - Source distribution
-- 服务器操作系统:                      Linux
-- HeidiSQL 版本:                  11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- 导出  表 juster_java.updateversioninfo 结构
CREATE TABLE IF NOT EXISTS `updateversioninfo` (
  `MD5` varchar(64) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'update packet md5' COMMENT 'MD5码',
  `PubTime` int(11) NOT NULL DEFAULT '0' COMMENT '版本发布时间',
  `Name` varchar(64) COLLATE utf8mb4_unicode_ci DEFAULT 'version name' COMMENT '版本名称',
  `Url` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'update url' COMMENT '更新包文件服务器地址',
  `Version` varchar(20) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'last version number' COMMENT '版本号',
  `ClientType` int(11) NOT NULL DEFAULT '1' COMMENT '客户端类型',
  `AppSecretKey` varchar(64) COLLATE utf8mb4_unicode_ci NOT NULL COMMENT 'app密钥',
  PRIMARY KEY (`MD5`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 数据导出被取消选择。

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
