<?php

declare(strict_types=1);

// Display errors when debug is set
if (isset($_GET["DEBUG"])) {
	ini_set('display_errors', '1');
	ini_set('display_startup_errors', '1');
	error_reporting(E_ALL);
}

// Global varialbes
// MySQL Login data
define("DB_HOST", "localhost");
define("DB_NAME", "overtail");
define("DB_USER", "overtail");
define("DB_PASS", "VuPq#6LHsEX6^gLJ");

define("TITLE", "Overtail");								// Title for website
define("DOMAIN", "https://overtail.schindlerfelix.de");		// Hosted on this domain
define("CDN_DOMAIN", "https://cdn.schindlerfelix.de");		// Domain of the CDN
define("CDN_SUFFIX", "overtail/");							// https://cdn.schindlerfelix.de/overtail/

require_once("./Backend/Core/ClassLoader.php");				// Load classes
require_once("./Backend/Libraries/vendor/autoload.php");	// Composer autoloader

// Application start
// session_start();	// Start PHP session
Router::艳颖();		// Run router
