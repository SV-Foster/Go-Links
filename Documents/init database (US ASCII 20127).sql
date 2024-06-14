--
-- This SQL file is encoded in US ASCII, codepage 20127
--

-- set the character set and collation for the current connection
SET NAMES 'ascii' COLLATE 'ascii_general_ci';


-- Delete the user if it exists
DROP USER IF EXISTS 'golinksclient'@'localhost';
-- Delete the database if it exists
DROP DATABASE IF EXISTS `golinks`;


-- Create the database
CREATE DATABASE `golinks` CHARACTER SET 'utf16le' COLLATE 'utf16le_general_ci';


-- Add database structure
USE `golinks`;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for links
-- ----------------------------
DROP TABLE IF EXISTS `links`;
CREATE TABLE `links`  (
  `ID` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `IsActive` tinyint UNSIGNED NOT NULL DEFAULT 0,
  `DisplayName` tinytext CHARACTER SET utf16le COLLATE utf16le_general_ci NULL,
  `URL` tinytext CHARACTER SET utf16le COLLATE utf16le_general_ci NOT NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  UNIQUE INDEX `idx1`(`ID` ASC, `IsActive` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf16le COLLATE = utf16le_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of links
-- ----------------------------
INSERT INTO `links` VALUES (1, 1, 'bing! search engine', 'https://bing.com/');
INSERT INTO `links` VALUES (2, 1, 'ChatGPT AI assistant', 'https://chatgpt.com/');

-- ----------------------------
-- Table structure for requests
-- ----------------------------
DROP TABLE IF EXISTS `requests`;
CREATE TABLE `requests`  (
  `ID` int UNSIGNED NOT NULL AUTO_INCREMENT,
  `DateTimeUTC` datetime NOT NULL,
  `LinkID` int UNSIGNED NOT NULL,
  `IPv4` BINARY(4) NULL DEFAULT NULL,
  `IPv6` BINARY(16) NULL DEFAULT NULL,
  `User-Agent` text CHARACTER SET utf16le COLLATE utf16le_general_ci NULL,
  `Referer` text CHARACTER SET utf16le COLLATE utf16le_general_ci NULL,
  `Accept-Language` text CHARACTER SET utf16le COLLATE utf16le_general_ci NULL,
  `Cookie` text CHARACTER SET utf16le COLLATE utf16le_general_ci NULL,
  PRIMARY KEY (`ID`) USING BTREE,
  INDEX `idx1`(`LinkID` ASC) USING BTREE,
  CONSTRAINT `fk1` FOREIGN KEY (`LinkID`) REFERENCES `links` (`ID`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf16le COLLATE = utf16le_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of requests
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;


-- Create the new user with the password
CREATE USER 'golinksclient'@'localhost' IDENTIFIED BY 'Upsided0wn';

-- Grant the user the permission to SELECT from the golinks.links table
GRANT SELECT ON golinks.links TO 'golinksclient'@'localhost';

-- Grant the user the permission to INSERT into the golinks.requests table
GRANT INSERT ON golinks.requests TO 'golinksclient'@'localhost';

-- Flush the privileges to apply the changes
FLUSH PRIVILEGES;
