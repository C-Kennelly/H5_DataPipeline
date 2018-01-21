-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.2.7-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for spartanclash_datapipeline
CREATE DATABASE IF NOT EXISTS `spartanclash_datapipeline` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `spartanclash_datapipeline`;

-- Dumping structure for table spartanclash_datapipeline.t_configoptions
CREATE TABLE IF NOT EXISTS `t_configoptions` (
  `configName` varchar(16) NOT NULL DEFAULT 'active',
  `siteLaunchDate` date NOT NULL DEFAULT '2018-01-01',
  `companyClanBattleThreshold` double NOT NULL DEFAULT 1,
  `matchHistoryReQueryDays` double NOT NULL DEFAULT 1,
  PRIMARY KEY (`configName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_configoptions: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_configoptions` DISABLE KEYS */;
INSERT INTO `t_configoptions` (`configName`, `siteLaunchDate`, `companyClanBattleThreshold`, `matchHistoryReQueryDays`) VALUES
	('active', '2018-01-01', 1, 0.75);
/*!40000 ALTER TABLE `t_configoptions` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
