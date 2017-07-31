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


-- Dumping database structure for dev_spartanclashbackend
DROP DATABASE IF EXISTS `dev_spartanclashbackend`;
CREATE DATABASE IF NOT EXISTS `dev_spartanclashbackend` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `dev_spartanclashbackend`;

-- Dumping structure for table dev_spartanclashbackend.t_h5matches
DROP TABLE IF EXISTS `t_h5matches`;
CREATE TABLE IF NOT EXISTS `t_h5matches` (
  `matchID` varchar(64) NOT NULL,
  `dateDetailsScan` datetime DEFAULT NULL,
  `datePlayersScan` datetime DEFAULT NULL,
  `dateResultsScan` datetime DEFAULT NULL,
  `dateCompaniesInvolvedUpdated` datetime DEFAULT NULL,
  `dateCustomTeamsUpdated` datetime DEFAULT NULL,
  PRIMARY KEY (`matchID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_h5matches_matchdetails
DROP TABLE IF EXISTS `t_h5matches_matchdetails`;
CREATE TABLE IF NOT EXISTS `t_h5matches_matchdetails` (
  `matchId` varchar(64) NOT NULL,
  `GameMode` int(32) DEFAULT NULL,
  `HopperId` varchar(64) DEFAULT NULL,
  `MapId` varchar(64) DEFAULT NULL,
  `MapVariant_ResourceType` int(32) DEFAULT NULL,
  `MapVariant_ResourceId` varchar(64) DEFAULT NULL,
  `MapVariant_OwnerType` int(32) DEFAULT NULL,
  `MapVariant_Owner` varchar(64) DEFAULT NULL,
  `GameBaseVariantID` varchar(64) DEFAULT NULL,
  `GameVariant_ResourceID` varchar(64) DEFAULT NULL,
  `GameVariant_ResourceType` int(11) DEFAULT NULL,
  `GameVariant_OwnerType` int(11) DEFAULT NULL,
  `GameVariant_Owner` varchar(64) DEFAULT NULL,
  `MatchCompleteDate` datetime DEFAULT NULL,
  `MatchDuration` varchar(64) DEFAULT NULL,
  `IsTeamGame` binary(50) DEFAULT NULL,
  `SeasonID` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`matchId`),
  CONSTRAINT `fk_matchID_h5matches_carnagereport` FOREIGN KEY (`matchId`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_h5matches_playersformatch
DROP TABLE IF EXISTS `t_h5matches_playersformatch`;
CREATE TABLE IF NOT EXISTS `t_h5matches_playersformatch` (
  `matchID` varchar(64) NOT NULL,
  `team1_Players` varchar(1024) DEFAULT NULL,
  `team2_Players` varchar(1024) DEFAULT NULL,
  PRIMARY KEY (`matchID`),
  CONSTRAINT `fk_matchID_h5matches_playersformatch` FOREIGN KEY (`matchID`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`team1_Players` is null or json_valid(`team1_Players`)),
  CONSTRAINT `CONSTRAINT_2` CHECK (`team2_Players` is null or json_valid(`team2_Players`))
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_h5matches_teamsinvolved_halowaypointcompanies
DROP TABLE IF EXISTS `t_h5matches_teamsinvolved_halowaypointcompanies`;
CREATE TABLE IF NOT EXISTS `t_h5matches_teamsinvolved_halowaypointcompanies` (
  `matchID` varchar(64) NOT NULL,
  `teamSource` varchar(128) DEFAULT 'Halo Waypoint',
  `team1_Primary` varchar(128) DEFAULT NULL,
  `team2_Primary` varchar(128) DEFAULT NULL,
  `team1_Secondary` varchar(128) DEFAULT NULL COMMENT 'Accomodates Warzone',
  `team2_Secondary` varchar(128) DEFAULT NULL COMMENT 'Accomodates Warzone',
  PRIMARY KEY (`matchID`),
  KEY `fk_teamSourceWaypoint` (`teamSource`),
  CONSTRAINT `fk_matchID_h5matches_teamsinvolved` FOREIGN KEY (`matchID`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_teamSourceWaypoint` FOREIGN KEY (`teamSource`) REFERENCES `t_teamsources` (`teamSource`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_h5matches_teamsinvolved_spartanclashfireteams
DROP TABLE IF EXISTS `t_h5matches_teamsinvolved_spartanclashfireteams`;
CREATE TABLE IF NOT EXISTS `t_h5matches_teamsinvolved_spartanclashfireteams` (
  `matchID` varchar(64) NOT NULL,
  `teamSource` varchar(128) DEFAULT 'Spartan Clash',
  `team1_Primary` varchar(128) DEFAULT NULL,
  `team2_Primary` varchar(128) DEFAULT NULL,
  `team1_Secondary` varchar(128) DEFAULT NULL COMMENT 'Accomodates Warzone',
  `team2_Secondary` varchar(128) DEFAULT NULL COMMENT 'Accomodates Warzone',
  PRIMARY KEY (`matchID`),
  KEY `fk_teamSourceWaypoint` (`teamSource`),
  CONSTRAINT `t_h5matches_teamsinvolved_spartanclashfireteams_ibfk_1` FOREIGN KEY (`matchID`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_h5matches_teamsinvolved_spartanclashfireteams_ibfk_2` FOREIGN KEY (`teamSource`) REFERENCES `t_teamsources` (`teamSource`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_players
DROP TABLE IF EXISTS `t_players`;
CREATE TABLE IF NOT EXISTS `t_players` (
  `gamertag` varchar(32) NOT NULL,
  `dateLastMatchScan` datetime DEFAULT NULL,
  `dateCompanyRosterUpdated` datetime DEFAULT NULL,
  `dateCustomTeamsUpdated` datetime DEFAULT NULL,
  `scanThresholdInDays` int(11) NOT NULL DEFAULT 7,
  PRIMARY KEY (`gamertag`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_players_to_h5matches
DROP TABLE IF EXISTS `t_players_to_h5matches`;
CREATE TABLE IF NOT EXISTS `t_players_to_h5matches` (
  `gamertag` varchar(32) NOT NULL,
  `matchID` varchar(64) NOT NULL,
  PRIMARY KEY (`gamertag`,`matchID`),
  KEY `gamertag` (`gamertag`),
  KEY `fk_matchID_players_to_matches` (`matchID`),
  CONSTRAINT `fk_gamertag_players_to_matches` FOREIGN KEY (`gamertag`) REFERENCES `t_players` (`gamertag`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_matchID_players_to_matches` FOREIGN KEY (`matchID`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_players_to_teams
DROP TABLE IF EXISTS `t_players_to_teams`;
CREATE TABLE IF NOT EXISTS `t_players_to_teams` (
  `gamertag` varchar(128) NOT NULL,
  `teamName` varchar(128) NOT NULL,
  `teamSource` varchar(128) NOT NULL,
  `lastUpdated` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`gamertag`,`teamName`,`teamSource`),
  UNIQUE KEY `gamertag_teamSource` (`gamertag`,`teamSource`),
  KEY `fk_teamSource_teamrosters` (`teamSource`),
  KEY `gamertag` (`gamertag`),
  KEY `fk_teamNameteamSource_playerstoteams` (`teamName`,`teamSource`),
  CONSTRAINT `fk_gamertag_playerstoteams` FOREIGN KEY (`gamertag`) REFERENCES `t_players` (`gamertag`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_teamNameteamSource_playerstoteams` FOREIGN KEY (`teamName`, `teamSource`) REFERENCES `t_teams` (`teamName`, `teamSource`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_teams
DROP TABLE IF EXISTS `t_teams`;
CREATE TABLE IF NOT EXISTS `t_teams` (
  `teamName` varchar(128) NOT NULL,
  `teamSource` varchar(128) NOT NULL,
  `lastUpdated` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `parentTeamName` varchar(128) DEFAULT NULL,
  `parentTeamSource` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`teamName`,`teamSource`),
  KEY `fk_teamsource_teams` (`teamSource`),
  KEY `teamName_teamSource` (`teamName`,`teamSource`),
  CONSTRAINT `fk_teamsource_teams` FOREIGN KEY (`teamSource`) REFERENCES `t_teamsources` (`teamSource`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table dev_spartanclashbackend.t_teamsources
DROP TABLE IF EXISTS `t_teamsources`;
CREATE TABLE IF NOT EXISTS `t_teamsources` (
  `teamSource` varchar(128) NOT NULL COMMENT 'Name of the source, like "Halo Waypoint."',
  `sourceURL` varchar(128) NOT NULL COMMENT 'Top level URL for the source, like "www.halowaypoint.com"',
  `teamCommonName` varchar(128) NOT NULL COMMENT 'Printable name for the team, like "Spartan Company"',
  `beganTrackingOnDate` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`teamSource`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;