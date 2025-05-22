/*M!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19  Distrib 10.11.11-MariaDB, for Linux (x86_64)
--
-- Host: localhost    Database: ESCAPEROOM
-- ------------------------------------------------------
-- Server version	10.11.11-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Pokoje`
--

DROP TABLE IF EXISTS `Pokoje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Pokoje` (
  `pokoj_id` int(11) NOT NULL AUTO_INCREMENT,
  `nazwa` varchar(150) NOT NULL,
  `opis` text DEFAULT NULL,
  `trudnosc` tinyint(4) NOT NULL,
  `cena_za_godzine` decimal(6,2) NOT NULL,
  `max_graczy` tinyint(4) NOT NULL,
  `czas_minut` int(11) NOT NULL,
  PRIMARY KEY (`pokoj_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Pokoje`
--

LOCK TABLES `Pokoje` WRITE;
/*!40000 ALTER TABLE `Pokoje` DISABLE KEYS */;
/*!40000 ALTER TABLE `Pokoje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Rezerwacje`
--

DROP TABLE IF EXISTS `Rezerwacje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Rezerwacje` (
  `rezerwacja_id` int(11) NOT NULL AUTO_INCREMENT,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `data_rozpoczecia` datetime NOT NULL,
  `liczba_osob` tinyint(4) NOT NULL,
  `status` enum('zarezerwowana','odwolana','zrealizowana') NOT NULL,
  `data_utworzenia` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`rezerwacja_id`),
  KEY `uzytkownik_id` (`uzytkownik_id`),
  KEY `pokoj_id` (`pokoj_id`),
  CONSTRAINT `Rezerwacje_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `Uzytkownicy` (`uzytkownik_id`),
  CONSTRAINT `Rezerwacje_ibfk_2` FOREIGN KEY (`pokoj_id`) REFERENCES `Pokoje` (`pokoj_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Rezerwacje`
--

LOCK TABLES `Rezerwacje` WRITE;
/*!40000 ALTER TABLE `Rezerwacje` DISABLE KEYS */;
/*!40000 ALTER TABLE `Rezerwacje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Sesje`
--

DROP TABLE IF EXISTS `Sesje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Sesje` (
  `sesja_id` int(11) NOT NULL AUTO_INCREMENT,
  `rezerwacja_id` int(11) NOT NULL,
  `data_start` datetime NOT NULL,
  `data_koniec` datetime DEFAULT NULL,
  `czas_zwiazania` int(11) DEFAULT NULL,
  `czy_ukonczone` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`sesja_id`),
  KEY `rezerwacja_id` (`rezerwacja_id`),
  CONSTRAINT `Sesje_ibfk_1` FOREIGN KEY (`rezerwacja_id`) REFERENCES `Rezerwacje` (`rezerwacja_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Sesje`
--

LOCK TABLES `Sesje` WRITE;
/*!40000 ALTER TABLE `Sesje` DISABLE KEYS */;
/*!40000 ALTER TABLE `Sesje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Uzytkownicy`
--

DROP TABLE IF EXISTS `Uzytkownicy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Uzytkownicy` (
  `uzytkownik_id` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(255) NOT NULL,
  `haslo_hash` varchar(255) NOT NULL,
  `imie` varchar(100) NOT NULL,
  `nazwisko` varchar(100) NOT NULL,
  `telefon` varchar(20) DEFAULT NULL,
  `data_rejestracji` datetime NOT NULL DEFAULT current_timestamp(),
  `admin` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`uzytkownik_id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Uzytkownicy`
--

LOCK TABLES `Uzytkownicy` WRITE;
/*!40000 ALTER TABLE `Uzytkownicy` DISABLE KEYS */;
/*!40000 ALTER TABLE `Uzytkownicy` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Zespoly`
--

DROP TABLE IF EXISTS `Zespoly`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Zespoly` (
  `zespol_id` int(11) NOT NULL AUTO_INCREMENT,
  `rezerwacja_id` int(11) NOT NULL,
  `nazwa_zespolu` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`zespol_id`),
  KEY `rezerwacja_id` (`rezerwacja_id`),
  CONSTRAINT `Zespoly_ibfk_1` FOREIGN KEY (`rezerwacja_id`) REFERENCES `Rezerwacje` (`rezerwacja_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Zespoly`
--

LOCK TABLES `Zespoly` WRITE;
/*!40000 ALTER TABLE `Zespoly` DISABLE KEYS */;
/*!40000 ALTER TABLE `Zespoly` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-22 15:26:05
