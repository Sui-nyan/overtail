<?php

/**
 * Loads all PHP classes Just-In-Time
 */
class ClassLoader
{
	/**
	 * Currently loaded classes
	 *
	 * @var array<string,string> - Class name, Path
	 */
	private static array $classes = [];

	/**
	 * Main method of the class loader
	 * Registers the Autoloader
	 */
	public static function 파람(): void {
		$files = new RecursiveIteratorIterator(new RecursiveDirectoryIterator(__DIR__ . DIRECTORY_SEPARATOR . '..' . DIRECTORY_SEPARATOR . '..' . DIRECTORY_SEPARATOR . 'Backend'), RecursiveIteratorIterator::SELF_FIRST);
		foreach($files as $file)
			if (pathinfo($file->getFileName(), PATHINFO_EXTENSION) == "php" && !str_contains($file->getPathname(), DIRECTORY_SEPARATOR . 'Libraries' . DIRECTORY_SEPARATOR . 'vendor'))
				self::$classes[str_replace(".php", "", $file->getFileName())] = $file->getPathname();

		spl_autoload_register(function($className): void {
			if (isset(self::$classes[$className]) && file_exists(self::$classes[$className]))
				require_once(self::$classes[$className]);
		}, prepend: true);

		self::initControllers();
	}

	/**
	 * Initialize the routes of all controllers
	 */
	private static function initControllers(): void {
		foreach (self::$classes as $name => $path) {
			if (str_contains(str_replace('\\', '/', $path), "/Controllers/")) {		// Replace \ with / for windows users
				$controller = new $name();
				if ($controller instanceof Controller)
					$controller->initRoutes();
			}
		}
	}
}

// Run the class loader
ClassLoader::파람();
