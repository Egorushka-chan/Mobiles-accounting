CREATE DATABASE  IF NOT EXISTS `mobiles accounting` /*!40100 DEFAULT CHARACTER SET utf8 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `mobiles accounting`;
-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: mobiles accounting
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `check`
--

DROP TABLE IF EXISTS `check`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `check` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(45) NOT NULL,
  `Date` datetime NOT NULL,
  `Program ID` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_checks_program1_idx` (`Program ID`),
  CONSTRAINT `fk_checks_program1` FOREIGN KEY (`Program ID`) REFERENCES `program` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `check`
--

LOCK TABLES `check` WRITE;
/*!40000 ALTER TABLE `check` DISABLE KEYS */;
INSERT INTO `check` VALUES (1,'VasyaUrka','2020-11-15 18:34:22',8);
/*!40000 ALTER TABLE `check` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `developer`
--

DROP TABLE IF EXISTS `developer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `developer` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `developer`
--

LOCK TABLES `developer` WRITE;
/*!40000 ALTER TABLE `developer` DISABLE KEYS */;
INSERT INTO `developer` VALUES (1,'ИП Розин'),(2,'Google'),(3,'Microsoft'),(4,'Valve');
/*!40000 ALTER TABLE `developer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `history`
--

DROP TABLE IF EXISTS `history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `history` (
  `Entrance_num` int NOT NULL AUTO_INCREMENT,
  `Action ID` int NOT NULL,
  `Date` datetime DEFAULT NULL,
  `Action` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Entrance_num`,`Action ID`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `history`
--

LOCK TABLES `history` WRITE;
/*!40000 ALTER TABLE `history` DISABLE KEYS */;
INSERT INTO `history` VALUES (1,1,'2020-11-15 18:34:22','Приехал к вам'),(2,1,'2021-01-17 21:33:46','Вход'),(2,2,'2021-01-17 21:34:25','Добавлен новый разработчик - 4. Valve'),(2,3,'2021-01-17 21:34:40','Добавлена новая программа - 9. CS:GO'),(3,1,'2021-01-17 21:40:23','Вход'),(4,1,'2021-01-17 21:41:39','Вход'),(5,1,'2021-01-17 21:41:44','Вход'),(6,1,'2021-01-17 21:41:46','Вход'),(7,1,'2021-01-17 21:46:05','Вход'),(8,1,'2021-01-17 21:46:05','Вход'),(9,1,'2021-01-17 21:46:17','Вход'),(10,1,'2021-01-17 21:46:33','Вход'),(11,1,'2021-01-17 21:46:33','Вход'),(11,2,'2021-01-17 21:47:35','Добавлен новый чек - 2. 9'),(12,1,'2021-01-17 21:48:11','Вход'),(13,1,'2021-01-17 21:51:31','Вход'),(14,1,'2021-01-17 21:51:31','Вход'),(14,2,'2021-01-17 21:51:59','Добавлен новый чек - 2. 9'),(15,1,'2021-01-17 21:52:20','Вход'),(16,1,'2021-01-17 21:53:31','Вход'),(17,1,'2021-01-17 21:54:12','Вход'),(18,1,'2021-01-17 21:54:15','Вход'),(19,1,'2021-01-17 21:54:40','Вход'),(20,1,'2021-01-17 21:54:41','Вход'),(21,1,'2021-01-17 21:55:50','Вход'),(22,1,'2021-01-17 21:56:51','Вход'),(23,1,'2021-01-17 21:56:51','Вход'),(24,1,'2021-01-17 21:57:03','Вход'),(25,1,'2021-01-17 21:57:03','Вход'),(26,1,'2021-01-17 21:57:35','Вход'),(27,1,'2021-01-17 21:57:45','Вход'),(28,1,'2021-01-18 12:25:44','Вход'),(29,1,'2021-01-18 12:26:34','Вход'),(30,1,'2021-01-18 12:28:37','Вход'),(31,1,'2021-01-18 20:15:59','Вход'),(32,1,'2021-01-18 20:17:37','Вход'),(33,1,'2021-01-18 20:18:49','Вход'),(34,1,'2021-01-18 20:19:38','Вход'),(35,1,'2021-01-18 20:20:49','Вход'),(36,1,'2021-01-18 20:21:17','Вход'),(37,1,'2021-01-18 20:26:28','Вход'),(38,1,'2021-01-18 20:27:17','Вход'),(39,1,'2021-01-18 20:28:59','Вход'),(40,1,'2021-01-18 20:30:07','Вход'),(41,1,'2021-01-18 20:39:01','Вход');
/*!40000 ALTER TABLE `history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `program`
--

DROP TABLE IF EXISTS `program`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `program` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Developer ID` int NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_program_developers_idx` (`Developer ID`),
  CONSTRAINT `fk_program_developers` FOREIGN KEY (`Developer ID`) REFERENCES `developer` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `program`
--

LOCK TABLES `program` WRITE;
/*!40000 ALTER TABLE `program` DISABLE KEYS */;
INSERT INTO `program` VALUES (1,'Word',3),(2,'PowerPoint',3),(3,'Excel',3),(4,'OneDrive',3),(5,'Youtube',2),(6,'Gmail',2),(7,'Переводчик',2),(8,'EatEatEat',1),(9,'CS:GO',4);
/*!40000 ALTER TABLE `program` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-01-18 20:42:18
