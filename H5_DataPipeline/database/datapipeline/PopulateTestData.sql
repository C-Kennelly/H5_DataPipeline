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

-- Dumping data for table spartanclash_datapipeline.t_configoptions: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_configoptions` DISABLE KEYS */;
INSERT INTO `t_configoptions` (`configName`, `siteLaunchDate`, `companyClanBattleThreshold`, `matchHistoryReQueryDays`, `DNFNeededForLoss`) VALUES
	('active', '2018-01-01', 0.75, 0, 1);
/*!40000 ALTER TABLE `t_configoptions` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_h5matches: ~2,053 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_matchdetails: ~2,247 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_matchdetails` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_matchdetails` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_playersformatch: ~1,099 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_playersformatch` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_playersformatch` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_ranksandscores: ~1,983 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_ranksandscores` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_ranksandscores` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_teamsinvolved_halowaypointcompanies: ~1,179 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_teamsinvolved_halowaypointcompanies` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_teamsinvolved_halowaypointcompanies` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_teamsinvolved_spartanclashfireteams: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_teamsinvolved_spartanclashfireteams` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_teamsinvolved_spartanclashfireteams` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_players: ~10,252 rows (approximately)
/*!40000 ALTER TABLE `t_players` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_players` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_players_to_h5matches: ~2,575 rows (approximately)
/*!40000 ALTER TABLE `t_players_to_h5matches` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_players_to_h5matches` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_players_to_teams: ~81 rows (approximately)
/*!40000 ALTER TABLE `t_players_to_teams` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_players_to_teams` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_teams: ~6,160 rows (approximately)
/*!40000 ALTER TABLE `t_teams` DISABLE KEYS */;
INSERT INTO `t_teams` (`teamId`, `teamName`, `teamSource`, `beganTrackingDate`, `trackingIndex`, `lastUpdated`, `rosterLastUdated`, `parentTeamId`, `parentTeamName`, `parentTeamSource`) VALUES
	('0', '[NOCOMPANYFOUND]', 'Halo Waypoint', '2018-01-19 08:33:58', 1, '2018-01-23 09:39:12', NULL, NULL, NULL, NULL),
	('23eccace-86b9-4a15-ab42-077bd21903f0', 'ExO Delta Gaming', 'Halo Waypoint', '2018-01-19 08:33:58', 1, '2018-01-23 09:39:12', NULL, NULL, NULL, NULL);
/*!40000 ALTER TABLE `t_teams` ENABLE KEYS */;

-- Dumping data for table spartanclash_datapipeline.t_teamsources: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_teamsources` DISABLE KEYS */;
INSERT INTO `t_teamsources` (`teamSource`, `sourceURL`, `teamCommonName`, `beganTrackingOnDate`) VALUES
	('Halo Waypoint', 'www.halowaypoint.com', 'Spartan Company', '2018-01-23 03:32:00');
/*!40000 ALTER TABLE `t_teamsources` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
