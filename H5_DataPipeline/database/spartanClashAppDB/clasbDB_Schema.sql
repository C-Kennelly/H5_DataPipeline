-- --------------------------------------------------------
-- Host:                         testspartanclash.cbr1lqfvizgf.us-west-2.rds.amazonaws.com
-- Server version:               10.0.24-MariaDB - MariaDB Server
-- Server OS:                    Linux
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for clashdb
DROP DATABASE IF EXISTS `clashdb`;
CREATE DATABASE IF NOT EXISTS `clashdb` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `clashdb`;

-- Dumping structure for table clashdb.t_clashdevset
DROP TABLE IF EXISTS `t_clashdevset`;
CREATE TABLE IF NOT EXISTS `t_clashdevset` (
  `MatchId` varchar(64) NOT NULL,
  `GameMode` int(32) DEFAULT NULL,
  `HopperId` text,
  `MapId` text,
  `MapVariant_ResourceType` int(32) DEFAULT NULL,
  `MapVariant_ResourceId` text,
  `MapVariant_OwnerType` int(32) DEFAULT NULL,
  `MapVariant_Owner` text,
  `GameBaseVariantID` text,
  `GameVariant_ResourceID` text,
  `GameVariant_ResourceType` int(11) DEFAULT NULL,
  `GameVariant_OwnerType` int(11) DEFAULT NULL,
  `GameVariant_Owner` text,
  `MatchCompleteDate` datetime DEFAULT NULL,
  `MatchDuration` text,
  `IsTeamGame` binary(50) DEFAULT NULL,
  `SeasonID` text,
  `Team1_Company1` text,
  `Team1_Company2` text,
  `Team2_Company1` text,
  `Team2_Company2` text,
  `Team1_Rank` int(11) NOT NULL DEFAULT '-1',
  `Team2_Rank` int(11) NOT NULL DEFAULT '-1',
  `Team1_Score` int(11) unsigned DEFAULT NULL,
  `Team2_Score` int(11) unsigned DEFAULT NULL,
  `Status` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`MatchId`),
  UNIQUE KEY `MatchId` (`MatchId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table clashdb.t_clashmetadata
DROP TABLE IF EXISTS `t_clashmetadata`;
CREATE TABLE IF NOT EXISTS `t_clashmetadata` (
  `id` varchar(16) NOT NULL DEFAULT 'active',
  `dataRefreshDate` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table clashdb.t_companies
DROP TABLE IF EXISTS `t_companies`;
CREATE TABLE IF NOT EXISTS `t_companies` (
  `company` varchar(32) NOT NULL,
  `rank` int(64) NOT NULL DEFAULT '-1',
  `wins` int(64) DEFAULT NULL,
  `losses` int(64) DEFAULT NULL,
  `total_matches` int(128) DEFAULT NULL,
  `win_percent` double DEFAULT NULL,
  `times_searched` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`company`),
  UNIQUE KEY `company` (`company`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table clashdb.t_company2matches
DROP TABLE IF EXISTS `t_company2matches`;
CREATE TABLE IF NOT EXISTS `t_company2matches` (
  `MatchId` varchar(64) NOT NULL,
  `company` varchar(32) NOT NULL,
  PRIMARY KEY (`MatchId`,`company`),
  KEY `fk_company` (`company`),
  CONSTRAINT `fk_MatchId` FOREIGN KEY (`MatchId`) REFERENCES `t_clashdevset` (`MatchId`),
  CONSTRAINT `fk_company` FOREIGN KEY (`company`) REFERENCES `t_companies` (`company`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table clashdb.t_mapmetadata
DROP TABLE IF EXISTS `t_mapmetadata`;
CREATE TABLE IF NOT EXISTS `t_mapmetadata` (
  `mapId` varchar(64) NOT NULL,
  `printableName` varchar(64) DEFAULT NULL,
  `imageURL` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`mapId`),
  UNIQUE KEY `mapName` (`mapId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
