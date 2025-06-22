/*M!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19-11.8.2-MariaDB, for Linux (x86_64)
--
-- Host: localhost    Database: escaperoom
-- ------------------------------------------------------
-- Server version	11.8.2-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*M!100616 SET @OLD_NOTE_VERBOSITY=@@NOTE_VERBOSITY, NOTE_VERBOSITY=0 */;

--
-- Table structure for table `platnosci`
--

DROP TABLE IF EXISTS `platnosci`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `platnosci` (
  `platnosc_id` int(11) NOT NULL AUTO_INCREMENT,
  `rezerwacja_id` int(11) NOT NULL,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `metoda_platnosci` enum('karta','gotowka') NOT NULL,
  `numer_transakcji` int(11) NOT NULL,
  PRIMARY KEY (`platnosc_id`),
  KEY `rezerwacja_id` (`rezerwacja_id`),
  KEY `uzytkownik_id` (`uzytkownik_id`),
  KEY `pokoj_id` (`pokoj_id`),
  CONSTRAINT `platnosci_ibfk_1` FOREIGN KEY (`rezerwacja_id`) REFERENCES `rezerwacje` (`rezerwacja_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `platnosci_ibfk_2` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `platnosci_ibfk_3` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `platnosci`
--

LOCK TABLES `platnosci` WRITE;
/*!40000 ALTER TABLE `platnosci` DISABLE KEYS */;
set autocommit=0;
/*!40000 ALTER TABLE `platnosci` ENABLE KEYS */;
UNLOCK TABLES;
commit;

--
-- Table structure for table `pokoje`
--

DROP TABLE IF EXISTS `pokoje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `pokoje` (
  `pokoj_id` int(11) NOT NULL AUTO_INCREMENT,
  `nazwa` varchar(150) NOT NULL,
  `opis` text DEFAULT NULL,
  `trudnosc` tinyint(4) NOT NULL,
  `cena` decimal(6,2) NOT NULL,
  `max_graczy` tinyint(4) NOT NULL,
  `czas_minut` int(11) NOT NULL,
  `status_pokoj` enum('wolny','zarezerwowany') DEFAULT NULL,
  PRIMARY KEY (`pokoj_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pokoje`
--

LOCK TABLES `pokoje` WRITE;
/*!40000 ALTER TABLE `pokoje` DISABLE KEYS */;
set autocommit=0;
INSERT INTO `pokoje` VALUES
(1,'2 krasnale i 2 mutanty','Odkryj jaka tajemnice skrywaja krasnale przed mutatantami. Czy dawane sekrety wyjda na jaw? Co skrywa mroczna jaskinia tych istot',3,200.00,4,60,'wolny'),
(2,'Straszny szpital','Pudzisz w pouszczonym szpitalu. NIe pamietasz niczego. Nie ma masz przy sobie nic, nawet butów. Czy dolasz okdryc co sie stalo?',4,450.00,6,90,'wolny');
/*!40000 ALTER TABLE `pokoje` ENABLE KEYS */;
UNLOCK TABLES;
commit;

--
-- Table structure for table `recenzje`
--

DROP TABLE IF EXISTS `recenzje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `recenzje` (
  `recenzja_id` int(11) NOT NULL AUTO_INCREMENT,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `opinia` text NOT NULL,
  `data_dodania` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`recenzja_id`),
  KEY `uzytkownik_id` (`uzytkownik_id`),
  KEY `pokoj_id` (`pokoj_id`),
  CONSTRAINT `Recenzje_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`),
  CONSTRAINT `Recenzje_ibfk_2` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recenzje`
--

LOCK TABLES `recenzje` WRITE;
/*!40000 ALTER TABLE `recenzje` DISABLE KEYS */;
set autocommit=0;
/*!40000 ALTER TABLE `recenzje` ENABLE KEYS */;
UNLOCK TABLES;
commit;

--
-- Table structure for table `rezerwacje`
--

DROP TABLE IF EXISTS `rezerwacje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `rezerwacje` (
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
  CONSTRAINT `Rezerwacje_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`),
  CONSTRAINT `Rezerwacje_ibfk_2` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rezerwacje`
--

LOCK TABLES `rezerwacje` WRITE;
/*!40000 ALTER TABLE `rezerwacje` DISABLE KEYS */;
set autocommit=0;
INSERT INTO `rezerwacje` VALUES
(1,2,2,'2025-05-29 21:00:00',3,'zrealizowana','2025-05-29 18:00:00'),
(2,2,1,'2025-06-13 10:00:00',4,'zarezerwowana','2025-06-07 17:00:00');
/*!40000 ALTER TABLE `rezerwacje` ENABLE KEYS */;
UNLOCK TABLES;
commit;

--
-- Table structure for table `sesje`
--

DROP TABLE IF EXISTS `sesje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `sesje` (
  `sesja_id` int(11) NOT NULL AUTO_INCREMENT,
  `rezerwacja_id` int(11) NOT NULL,
  `data_start` datetime NOT NULL,
  `data_koniec` datetime DEFAULT NULL,
  `czas_zwiazania` int(11) DEFAULT NULL,
  `czy_ukonczone` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`sesja_id`),
  KEY `rezerwacja_id` (`rezerwacja_id`),
  CONSTRAINT `Sesje_ibfk_1` FOREIGN KEY (`rezerwacja_id`) REFERENCES `rezerwacje` (`rezerwacja_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sesje`
--

LOCK TABLES `sesje` WRITE;
/*!40000 ALTER TABLE `sesje` DISABLE KEYS */;
set autocommit=0;
INSERT INTO `sesje` VALUES
(1,1,'2025-05-29 21:00:00','2025-05-29 22:30:00',70,1);
/*!40000 ALTER TABLE `sesje` ENABLE KEYS */;
UNLOCK TABLES;
commit;

--
-- Table structure for table `uzytkownicy`
--

DROP TABLE IF EXISTS `uzytkownicy`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `uzytkownicy` (
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `uzytkownicy`
--

LOCK TABLES `uzytkownicy` WRITE;
/*!40000 ALTER TABLE `uzytkownicy` DISABLE KEYS */;
set autocommit=0;
INSERT INTO `uzytkownicy` VALUES
(1,'admin@er.pl','*4ACFE3202A5FF5CF467898FC58AAB1D615029441','Tomasz','Witoldzin','420692137','2025-06-07 16:28:50',1),
(2,'user@hmail.pl','*D5D9F81F5542DE067FFF5FF7A4CA4BDD322C578F','Wodzislaw','Adamczyk','213742069','2025-06-07 16:30:47',0);
/*!40000 ALTER TABLE `uzytkownicy` ENABLE KEYS */;
UNLOCK TABLES;
commit;

--
-- Table structure for table `wiadomosci`
--

DROP TABLE IF EXISTS `wiadomosci`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `wiadomosci` (
  `id_wiadomosci` int(11) NOT NULL AUTO_INCREMENT,
  `wiadomosc` varchar(500) NOT NULL,
  `uzytkownik_id` int(11) NOT NULL,
  PRIMARY KEY (`id_wiadomosci`),
  KEY `uzytkownik_id` (`uzytkownik_id`),
  CONSTRAINT `wiadomosci_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wiadomosci`
--

LOCK TABLES `wiadomosci` WRITE;
/*!40000 ALTER TABLE `wiadomosci` DISABLE KEYS */;
set autocommit=0;
/*!40000 ALTER TABLE `wiadomosci` ENABLE KEYS */;
UNLOCK TABLES;
commit;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*M!100616 SET NOTE_VERBOSITY=@OLD_NOTE_VERBOSITY */;

-- Dump completed on 2025-06-22 12:02:39
