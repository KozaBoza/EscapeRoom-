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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pokoje`
--

LOCK TABLES `pokoje` WRITE;
/*!40000 ALTER TABLE `pokoje` DISABLE KEYS */;
set autocommit=0;
INSERT INTO `pokoje` VALUES
(1,'2 krasnale i 2 mutanty','Odkryj jaka tajemnice skrywaja krasnale przed mutatantami. Czy dawane sekrety wyjda na jaw? Co skrywa mroczna jaskinia tych istot',3,200.00,4,60,'wolny'),
(2,'Straszny szpital','Pudzisz w pouszczonym szpitalu. NIe pamietasz niczego. Nie ma masz przy sobie nic, nawet butów. Czy dolasz okdryc co sie stalo?',4,450.00,6,90,'wolny'),
(3,'Tajemniczy pokoj','Standardowy pokoj testowy.',2,110.00,4,60,'wolny'),
(4,'Zaginiony Skarb','Przygoda w stylu Indiany Jonesa z zagadkami i pulapkami.',3,120.00,5,60,'wolny'),
(5,'Laboratorium Szalonego Naukowca','Eksperymenty, wybuchy i niebezpieczne substancje.',4,140.00,4,75,'wolny'),
(6,'Tajemnice Wiktorianskiego Dworu','Mroczna historia rodzinna pelna sekretow.',2,100.00,6,60,'wolny'),
(7,'Ucieczka z Wiezienia','Realistyczny pokoj z celami, kratami i straznikami.',5,150.00,4,90,'wolny'),
(8,'Kosmiczna Misja','Pokoj w stylu sci-fi z efektami swietlnymi i dzwiekowymi.',3,130.00,5,70,'wolny');
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
) ENGINE=InnoDB AUTO_INCREMENT=91 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recenzje`
--

LOCK TABLES `recenzje` WRITE;
/*!40000 ALTER TABLE `recenzje` DISABLE KEYS */;
set autocommit=0;
INSERT INTO `recenzje` VALUES
(75,1,1,'Ciekawy pokoj, dobra zabawa.','2025-06-25 21:24:21'),
(76,2,1,'Swietne zagadki i klimat.','2025-06-25 21:24:21'),
(77,3,2,'Pokoj wciagajacy, polecam.','2025-06-25 21:24:21'),
(78,4,2,'Niebanalne zadania, dobra organizacja.','2025-06-25 21:24:21'),
(79,5,3,'Latwy, ale przyjemny pokoj.','2025-06-25 21:24:21'),
(80,6,3,'Fajna atmosfera i pomyslowe rozwiazania.','2025-06-25 21:24:21'),
(81,7,4,'Troche trudny, ale satysfakcjonujacy.','2025-06-25 21:24:21'),
(82,8,4,'Bardzo dobra obsluga.','2025-06-25 21:24:21'),
(83,9,5,'Zaskakujace elementy, polecam.','2025-06-25 21:24:21'),
(84,1,5,'Dobrze przemyslany pokoj.','2025-06-25 21:24:21'),
(85,2,6,'Klimatyczny pokoj z fajna historia.','2025-06-25 21:24:21'),
(86,3,6,'Super wrazenia, zagadki na poziomie.','2025-06-25 21:24:21'),
(87,4,7,'Pokoj wymagajacy myslenia.','2025-06-25 21:24:21'),
(88,5,7,'Dobrze zbalansowany poziom trudnosci.','2025-06-25 21:24:21'),
(89,6,8,'Efekty swietne, swietna zabawa.','2025-06-25 21:24:21'),
(90,7,8,'Kosmiczny klimat zaskoczyl pozytywnie.','2025-06-25 21:24:21');
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
  `status` enum('zarezerwowana','odwolana','zrealizowana','oplacona') NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `uzytkownicy`
--

LOCK TABLES `uzytkownicy` WRITE;
/*!40000 ALTER TABLE `uzytkownicy` DISABLE KEYS */;
set autocommit=0;
INSERT INTO `uzytkownicy` VALUES
(1,'admin@er.pl','*4ACFE3202A5FF5CF467898FC58AAB1D615029441RG2ByvLxSgzE8jpxGvUwDA==:plRI5cXHchswtso04i5CBJmRTbRn4yJcv+5ODCYkLxo=','Tomasz','Witoldzin','420692137','2025-06-07 16:28:50',1),
(2,'user@hmail.pl','xrNGm4Kmi1wdBTLAThR4zQ==:5/d3G5WKWhAnlAaBKox0JKV2lz6+CBTvF98KJ/5EiTw=','Wodzislaw','Adamczyk','213742069','2025-06-07 16:30:47',0),
(3,'g.zr@kaermorhen.com','EhOWANrUG6l4vzreeM481Q==:6Y1dTC/RO9f+UVgw79taW7S5S+cBOIJRfzpybgyTJX8=','Geralt','z Rivii','123456789','2025-06-25 20:05:35',0),
(4,'yen.vengerberg@aretuza.mag','NNdqxUZ94ohDo7J0wAiRAQ==:zlSacQ7GbIqs5wextYLneCCoZQMTmOY1U+1vNwkovQo=','Yennefer','z Vengerbergu','234567890','2025-06-25 20:13:20',0),
(5,'jaskier.bard@oxenfurt.edu','/iNMFlG+Yd1cl64om8Dz7g==:d+quy5LzsV4sZwqGH9w5skYzV2Ej79sygNCmbE2e6TU=','Jaskier','Pankratz','456789012','2025-06-25 20:13:52',0),
(6,'ciri.jaskolka@zireael.pl','+oLsqvZ1NNWEU3vRN/z1DA==:TqFSzzg8QIIDBEbqHQTgfmiiIAncsyOBTlQvg3e+3rE=','Cirilla','z Cintry','345678901','2025-06-25 20:14:21',0),
(7,'triss.czarodziejka@temeria.gov','/wB/d4o+3iaL9wkUVevJ4g==:5NFozj4I/LlaINoiYWlsHUo2I+YdOOfwDtt6V/YJUF4=','Triss','Merigold','567890123','2025-06-25 20:14:59',0),
(8,'zoltan.krasnolud@mahakam.pl','7gMZzFGUmjHyn3J4hRc3YQ==:L/eXIoqjlQuv8UfV8l6sNv61iIfhvPwshY/OzfZ/1BU=','Zoltan','Chivay','901234567','2025-06-25 20:16:40',0),
(9,'emiel.regis@tesham-mutna.vamp','lSrpncWftK60vpgnkzFUPw==:uhe4nago0bW+LJNZlm/aEUpT02WTLcmwhhbUtyVAjt4=','Emiel','Regis','112233445','2025-06-25 20:17:09',0);
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `wiadomosci`
--

LOCK TABLES `wiadomosci` WRITE;
/*!40000 ALTER TABLE `wiadomosci` DISABLE KEYS */;
set autocommit=0;
INSERT INTO `wiadomosci` VALUES
(1,'Mam pytanie dotyczace rezerwacji pokoju numer 2.',1),
(2,'Czy moge zmienic termin mojej rezerwacji?',2),
(3,'Nie otrzymalem potwierdzenia rezerwacji na e-mail.',3),
(4,'Pokoj byl swietny, dziekuje za mila obsluge!',4),
(5,'Chcialbym otrzymac fakture za moja wizyte.',5),
(6,'Czy macie wolne terminy na nastepny weekend?',6),
(7,'Czy moge przyjsc z dzieckiem?',7),
(8,'Jakie sa godziny otwarcia w tygodniu?',8),
(9,'Czy jest mozliwosc prywatnej sesji?',9),
(10,'Czy moge zmienic liczbe graczy w mojej rezerwacji?',1);
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

-- Dump completed on 2025-06-25 21:26:44
