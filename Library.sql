-- phpMyAdmin SQL Dump
-- version 4.6.5.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:8889
-- Generation Time: Mar 02, 2018 at 01:39 AM
-- Server version: 5.6.35
-- PHP Version: 7.0.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `library`
--
CREATE DATABASE IF NOT EXISTS `library` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `library`;

-- --------------------------------------------------------

--
-- Table structure for table `active_checkouts`
--

CREATE TABLE `active_checkouts` (
  `id` int(11) NOT NULL,
  `checkout_id` int(11) NOT NULL,
  `due_date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `active_checkouts`
--

INSERT INTO `active_checkouts` (`id`, `checkout_id`, `due_date`) VALUES
(19, 57, '2018-03-15 16:36:13'),
(20, 58, '2018-03-15 16:36:13'),
(21, 59, '2018-03-15 16:36:14'),
(22, 60, '2018-03-15 16:36:14'),
(23, 61, '2018-03-15 16:36:15');

-- --------------------------------------------------------

--
-- Table structure for table `authors`
--

CREATE TABLE `authors` (
  `id` int(11) NOT NULL,
  `firstName` varchar(255) NOT NULL,
  `lastName` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `authors`
--

INSERT INTO `authors` (`id`, `firstName`, `lastName`) VALUES
(1, 'Kayla', 'O'),
(2, 'Bob', 'Ross'),
(3, 'Cam', 'Anderson'),
(4, 'Dwayne', 'Johnson'),
(5, 'J.K.', 'Rowling');

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE `books` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `books`
--

INSERT INTO `books` (`id`, `name`) VALUES
(8, 'Punching Cam'),
(10, 'Blocking Kayla\'s Punch'),
(11, 'Happy Trees'),
(13, 'How to be a Rock'),
(14, 'How To Fight'),
(18, 'A Really Great Book');

-- --------------------------------------------------------

--
-- Table structure for table `books_authors`
--

CREATE TABLE `books_authors` (
  `id` int(11) NOT NULL,
  `book_id` int(11) NOT NULL,
  `author_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `books_authors`
--

INSERT INTO `books_authors` (`id`, `book_id`, `author_id`) VALUES
(8, 8, 1),
(10, 10, 3),
(11, 11, 2),
(13, 13, 4),
(14, 14, 3),
(15, 14, 1),
(29, 18, 1),
(30, 18, 2),
(31, 18, 3),
(32, 18, 4);

-- --------------------------------------------------------

--
-- Table structure for table `checkouts`
--

CREATE TABLE `checkouts` (
  `id` int(11) NOT NULL,
  `copy_id` int(11) NOT NULL,
  `patron_id` int(11) NOT NULL,
  `checkout_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `checkouts`
--

INSERT INTO `checkouts` (`id`, `copy_id`, `patron_id`, `checkout_date`) VALUES
(41, 88, 7, '2018-03-01'),
(42, 88, 7, '2018-03-01'),
(44, 71, 7, '2018-03-01'),
(45, 88, 7, '2018-03-01'),
(47, 71, 7, '2018-03-01'),
(48, 59, 7, '2018-03-01'),
(49, 60, 7, '2018-03-01'),
(56, 31, 7, '2018-03-01'),
(57, 59, 6, '2018-03-01'),
(58, 60, 6, '2018-03-01'),
(59, 31, 6, '2018-03-01'),
(60, 32, 6, '2018-03-01'),
(61, 88, 6, '2018-03-01'),
(62, 71, 7, '2018-03-01'),
(63, 140, 7, '2018-03-01');

-- --------------------------------------------------------

--
-- Table structure for table `copies`
--

CREATE TABLE `copies` (
  `id` int(11) NOT NULL,
  `book_id` int(11) NOT NULL,
  `checked_out` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `copies`
--

INSERT INTO `copies` (`id`, `book_id`, `checked_out`) VALUES
(31, 10, 1),
(32, 10, 1),
(59, 11, 1),
(60, 11, 1),
(71, 14, 0),
(88, 18, 1),
(140, 14, 0);

-- --------------------------------------------------------

--
-- Table structure for table `patrons`
--

CREATE TABLE `patrons` (
  `id` int(11) NOT NULL,
  `firstName` varchar(255) NOT NULL,
  `lastName` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Dumping data for table `patrons`
--

INSERT INTO `patrons` (`id`, `firstName`, `lastName`) VALUES
(6, 'Bob', 'Ross'),
(7, 'Cameron', 'Anderson');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `active_checkouts`
--
ALTER TABLE `active_checkouts`
  ADD PRIMARY KEY (`id`),
  ADD KEY `active_checkouts_ibfk_1` (`checkout_id`);

--
-- Indexes for table `authors`
--
ALTER TABLE `authors`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `books_authors`
--
ALTER TABLE `books_authors`
  ADD PRIMARY KEY (`id`),
  ADD KEY `book_id` (`book_id`),
  ADD KEY `author_id` (`author_id`);

--
-- Indexes for table `checkouts`
--
ALTER TABLE `checkouts`
  ADD PRIMARY KEY (`id`),
  ADD KEY `checkouts_ibfk_1` (`copy_id`),
  ADD KEY `checkouts_ibfk_2` (`patron_id`);

--
-- Indexes for table `copies`
--
ALTER TABLE `copies`
  ADD PRIMARY KEY (`id`),
  ADD KEY `book_id` (`book_id`);

--
-- Indexes for table `patrons`
--
ALTER TABLE `patrons`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `active_checkouts`
--
ALTER TABLE `active_checkouts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;
--
-- AUTO_INCREMENT for table `authors`
--
ALTER TABLE `authors`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT for table `books`
--
ALTER TABLE `books`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;
--
-- AUTO_INCREMENT for table `books_authors`
--
ALTER TABLE `books_authors`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=39;
--
-- AUTO_INCREMENT for table `checkouts`
--
ALTER TABLE `checkouts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=64;
--
-- AUTO_INCREMENT for table `copies`
--
ALTER TABLE `copies`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=143;
--
-- AUTO_INCREMENT for table `patrons`
--
ALTER TABLE `patrons`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `active_checkouts`
--
ALTER TABLE `active_checkouts`
  ADD CONSTRAINT `active_checkouts_ibfk_1` FOREIGN KEY (`checkout_id`) REFERENCES `checkouts` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `books_authors`
--
ALTER TABLE `books_authors`
  ADD CONSTRAINT `books_authors_ibfk_1` FOREIGN KEY (`book_id`) REFERENCES `books` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `books_authors_ibfk_2` FOREIGN KEY (`author_id`) REFERENCES `authors` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `checkouts`
--
ALTER TABLE `checkouts`
  ADD CONSTRAINT `checkouts_ibfk_1` FOREIGN KEY (`copy_id`) REFERENCES `copies` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `checkouts_ibfk_2` FOREIGN KEY (`patron_id`) REFERENCES `patrons` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `copies`
--
ALTER TABLE `copies`
  ADD CONSTRAINT `copies_ibfk_1` FOREIGN KEY (`book_id`) REFERENCES `books` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
