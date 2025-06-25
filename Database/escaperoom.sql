-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Cze 25, 2025 at 08:17 PM
-- Wersja serwera: 10.4.32-MariaDB
-- Wersja PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `escaperoom`
--

DROP DATABASE IF EXISTS escaperoom;
CREATE DATABASE escaperoom;
USE escaperoom;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `platnosci`
--

CREATE TABLE `platnosci` (
  `platnosc_id` int(11) NOT NULL,
  `rezerwacja_id` int(11) NOT NULL,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `metoda_platnosci` enum('karta','gotowka') NOT NULL,
  `numer_transakcji` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `pokoje`
--

CREATE TABLE `pokoje` (
  `pokoj_id` int(11) NOT NULL,
  `nazwa` varchar(150) NOT NULL,
  `opis` text DEFAULT NULL,
  `trudnosc` tinyint(4) NOT NULL,
  `cena` decimal(6,2) NOT NULL,
  `max_graczy` tinyint(4) NOT NULL,
  `czas_minut` int(11) NOT NULL,
  `status_pokoj` enum('wolny','zarezerwowany') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `pokoje`
--

INSERT INTO `pokoje` (`pokoj_id`, `nazwa`, `opis`, `trudnosc`, `cena`, `max_graczy`, `czas_minut`, `status_pokoj`) VALUES
(1, '2 krasnale i 2 mutanty', 'Odkryj jaka tajemnice skrywaja krasnale przed mutatantami. Czy dawane sekrety wyjda na jaw? Co skrywa mroczna jaskinia tych istot', 3, 200.00, 4, 60, 'wolny'),
(2, 'Straszny szpital', 'Pudzisz w pouszczonym szpitalu. NIe pamietasz niczego. Nie ma masz przy sobie nic, nawet butów. Czy dolasz okdryc co sie stalo?', 4, 450.00, 6, 90, 'wolny'),
(4, 'Zaginiony Skarb', 'Przygoda w stylu Indiany Jonesa z zagadkami i pulapkami.', 3, 120.00, 5, 60, 'wolny'),
(5, 'Laboratorium Szalonego Naukowca', 'Eksperymenty, wybuchy i niebezpieczne substancje.', 4, 140.00, 4, 75, 'wolny'),
(6, 'Tajemnice Wiktorianskiego Dworu', 'Mroczna historia rodzinna pelna sekretow.', 2, 100.00, 6, 60, 'wolny'),
(7, 'Ucieczka z Wiezienia', 'Realistyczny pokoj z celami, kratami i straznikami.', 5, 150.00, 4, 90, 'wolny'),
(8, 'Kosmiczna Misja', 'Pokoj w stylu sci-fi z efektami swietlnymi i dzwiekowymi.', 3, 130.00, 5, 70, 'wolny');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `recenzje`
--

CREATE TABLE `recenzje` (
  `recenzja_id` int(11) NOT NULL,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `opinia` text NOT NULL,
  `data_dodania` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `rezerwacje`
--

CREATE TABLE `rezerwacje` (
  `rezerwacja_id` int(11) NOT NULL,
  `uzytkownik_id` int(11) NOT NULL,
  `pokoj_id` int(11) NOT NULL,
  `data_rozpoczecia` datetime NOT NULL,
  `liczba_osob` tinyint(4) NOT NULL,
  `status` enum('zarezerwowana','odwolana','zrealizowana','oplacona') NOT NULL,
  `data_utworzenia` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `rezerwacje`
--

INSERT INTO `rezerwacje` (`rezerwacja_id`, `uzytkownik_id`, `pokoj_id`, `data_rozpoczecia`, `liczba_osob`, `status`, `data_utworzenia`) VALUES
(1, 2, 2, '2025-05-29 21:00:00', 3, 'zrealizowana', '2025-05-29 18:00:00'),
(2, 2, 1, '2025-06-13 10:00:00', 4, 'zarezerwowana', '2025-06-07 17:00:00');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `uzytkownicy`
--

CREATE TABLE `uzytkownicy` (
  `uzytkownik_id` int(11) NOT NULL,
  `email` varchar(255) NOT NULL,
  `haslo_hash` varchar(255) NOT NULL,
  `imie` varchar(100) NOT NULL,
  `nazwisko` varchar(100) NOT NULL,
  `telefon` varchar(20) DEFAULT NULL,
  `data_rejestracji` datetime NOT NULL DEFAULT current_timestamp(),
  `admin` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `uzytkownicy`
--

INSERT INTO `uzytkownicy` (`uzytkownik_id`, `email`, `haslo_hash`, `imie`, `nazwisko`, `telefon`, `data_rejestracji`, `admin`) VALUES
(1, 'admin@er.pl', '*4ACFE3202A5FF5CF467898FC58AAB1D615029441RG2ByvLxSgzE8jpxGvUwDA==:plRI5cXHchswtso04i5CBJmRTbRn4yJcv+5ODCYkLxo=', 'Tomasz', 'Witoldzin', '420692137', '2025-06-07 16:28:50', 1),
(2, 'user@hmail.pl', 'xrNGm4Kmi1wdBTLAThR4zQ==:5/d3G5WKWhAnlAaBKox0JKV2lz6+CBTvF98KJ/5EiTw=', 'Wodzislaw', 'Adamczyk', '213742069', '2025-06-07 16:30:47', 0),
(3, 'g.zr@kaermorhen.com', 'EhOWANrUG6l4vzreeM481Q==:6Y1dTC/RO9f+UVgw79taW7S5S+cBOIJRfzpybgyTJX8=', 'Geralt', 'z Rivii', '123456789', '2025-06-25 20:05:35', 0),
(4, 'yen.vengerberg@aretuza.mag', 'NNdqxUZ94ohDo7J0wAiRAQ==:zlSacQ7GbIqs5wextYLneCCoZQMTmOY1U+1vNwkovQo=', 'Yennefer', 'z Vengerbergu', '234567890', '2025-06-25 20:13:20', 0),
(5, 'jaskier.bard@oxenfurt.edu', '/iNMFlG+Yd1cl64om8Dz7g==:d+quy5LzsV4sZwqGH9w5skYzV2Ej79sygNCmbE2e6TU=', 'Jaskier', 'Pankratz', '456789012', '2025-06-25 20:13:52', 0),
(6, 'ciri.jaskolka@zireael.pl', '+oLsqvZ1NNWEU3vRN/z1DA==:TqFSzzg8QIIDBEbqHQTgfmiiIAncsyOBTlQvg3e+3rE=', 'Cirilla', 'z Cintry', '345678901', '2025-06-25 20:14:21', 0),
(7, 'triss.czarodziejka@temeria.gov', '/wB/d4o+3iaL9wkUVevJ4g==:5NFozj4I/LlaINoiYWlsHUo2I+YdOOfwDtt6V/YJUF4=', 'Triss', 'Merigold', '567890123', '2025-06-25 20:14:59', 0),
(8, 'zoltan.krasnolud@mahakam.pl', '7gMZzFGUmjHyn3J4hRc3YQ==:L/eXIoqjlQuv8UfV8l6sNv61iIfhvPwshY/OzfZ/1BU=', 'Zoltan', 'Chivay', '901234567', '2025-06-25 20:16:40', 0),
(9, 'emiel.regis@tesham-mutna.vamp', 'lSrpncWftK60vpgnkzFUPw==:uhe4nago0bW+LJNZlm/aEUpT02WTLcmwhhbUtyVAjt4=', 'Emiel', 'Regis', '112233445', '2025-06-25 20:17:09', 0);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `wiadomosci`
--

CREATE TABLE `wiadomosci` (
  `id_wiadomosci` int(11) NOT NULL,
  `wiadomosc` varchar(500) NOT NULL,
  `uzytkownik_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `platnosci`
--
ALTER TABLE `platnosci`
  ADD PRIMARY KEY (`platnosc_id`),
  ADD KEY `rezerwacja_id` (`rezerwacja_id`),
  ADD KEY `uzytkownik_id` (`uzytkownik_id`),
  ADD KEY `pokoj_id` (`pokoj_id`);

--
-- Indeksy dla tabeli `pokoje`
--
ALTER TABLE `pokoje`
  ADD PRIMARY KEY (`pokoj_id`);

--
-- Indeksy dla tabeli `recenzje`
--
ALTER TABLE `recenzje`
  ADD PRIMARY KEY (`recenzja_id`),
  ADD KEY `uzytkownik_id` (`uzytkownik_id`),
  ADD KEY `pokoj_id` (`pokoj_id`);

--
-- Indeksy dla tabeli `rezerwacje`
--
ALTER TABLE `rezerwacje`
  ADD PRIMARY KEY (`rezerwacja_id`),
  ADD KEY `uzytkownik_id` (`uzytkownik_id`),
  ADD KEY `pokoj_id` (`pokoj_id`);

--
-- Indeksy dla tabeli `uzytkownicy`
--
ALTER TABLE `uzytkownicy`
  ADD PRIMARY KEY (`uzytkownik_id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indeksy dla tabeli `wiadomosci`
--
ALTER TABLE `wiadomosci`
  ADD PRIMARY KEY (`id_wiadomosci`),
  ADD KEY `uzytkownik_id` (`uzytkownik_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `platnosci`
--
ALTER TABLE `platnosci`
  MODIFY `platnosc_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `pokoje`
--
ALTER TABLE `pokoje`
  MODIFY `pokoj_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `recenzje`
--
ALTER TABLE `recenzje`
  MODIFY `recenzja_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT for table `rezerwacje`
--
ALTER TABLE `rezerwacje`
  MODIFY `rezerwacja_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `uzytkownicy`
--
ALTER TABLE `uzytkownicy`
  MODIFY `uzytkownik_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `wiadomosci`
--
ALTER TABLE `wiadomosci`
  MODIFY `id_wiadomosci` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `platnosci`
--
ALTER TABLE `platnosci`
  ADD CONSTRAINT `platnosci_ibfk_1` FOREIGN KEY (`rezerwacja_id`) REFERENCES `rezerwacje` (`rezerwacja_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `platnosci_ibfk_2` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `platnosci_ibfk_3` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `recenzje`
--
ALTER TABLE `recenzje`
  ADD CONSTRAINT `Recenzje_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`),
  ADD CONSTRAINT `Recenzje_ibfk_2` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`);

--
-- Constraints for table `rezerwacje`
--
ALTER TABLE `rezerwacje`
  ADD CONSTRAINT `Rezerwacje_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`),
  ADD CONSTRAINT `Rezerwacje_ibfk_2` FOREIGN KEY (`pokoj_id`) REFERENCES `pokoje` (`pokoj_id`);

--
-- Constraints for table `wiadomosci`
--
ALTER TABLE `wiadomosci`
  ADD CONSTRAINT `wiadomosci_ibfk_1` FOREIGN KEY (`uzytkownik_id`) REFERENCES `uzytkownicy` (`uzytkownik_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
