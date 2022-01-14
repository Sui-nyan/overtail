<?php

/**
 * Global utility functions
 */
class Utils
{
	/**
	 * Hashes the given data string with a chosen hash algorythm (Standard: SHA256)
	 *
	 * @param string $data The data to be hashed
	 * @param string $algo Hashing algorithmn - Standard: SHA256
	 * @return string Hashed data
	 */
	public static function encrypt(string $data, string $algo = "sha256"): string {
		return hash($algo, $data);
	}

	/**
	 * Takes a path, looks if it is a url, if not -> makes a CDN URL out of it
	 *
	 * @param string $path Filename or URL
	 * @return string Valid url
	 */
	public static function getCdnUrl(string $path): string {
		if (filter_var($path, FILTER_VALIDATE_URL) === false)
			return DOMAIN . "/" . CDN_SUFFIX . $path;
		else
			return $path;
	}

	/**
	 * Generates a random uuid (Version 4)
	 *
	 * @param string|null $data Random bytes
	 * @return string A random uuid
	 */
	public static function uuid(?string $data = null): string {
		// Generate 16 bytes (128 bits) of random data or use the data passed into the function.
		$data ??= random_bytes(16);
		assert(strlen($data) == 16);

		// Set version to 0100
		$data[6] = chr(ord($data[6]) & 0x0f | 0x40);
		// Set bits 6-7 to 10
		$data[8] = chr(ord($data[8]) & 0x3f | 0x80);

		// Output the 36 character UUID.
		return vsprintf('%s%s-%s-%s-%s-%s%s%s', str_split(bin2hex($data), 4));
	}

	/**
	 * Check if a string has the correct format to be a uuid (v4)
	 *
	 * @param string $uuid String to be checked
	 * @return boolean Whether it's the correct format or not
	 */
	public static function isUUID(string $uuid) {
		return preg_match("/^(?:[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}|00000000-0000-0000-0000-000000000000)$/i", $uuid) === 1;
	}
}
