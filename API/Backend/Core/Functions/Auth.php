<?php

/**
 * User authentication
 */
class Auth
{
	/**
	 * Checks whether the token is valid or not
	 *
	 * @param string|null $token The auth (bearer) token
	 * @return boolean True if valid, otherwise false
	 */
	public static function validateToken(?string $token = null): bool {
		if ($token === null) {
			if (($token = IO::getBearerToken()) == null) {
				return false;
			}
		}
		if (($decoded = base64_decode($token)) !== false) {
			$decToken = explode(".", $decoded);
			if (count($decToken) === 3) {
				if (($uuid = base64_decode($decToken[0])) != false && ($passHash = base64_decode($decToken[1])) != false && ($validUntil = base64_decode($decToken[2])) != false) {
					$dayDiff = (new DateTime())->diff(new DateTime($validUntil))->format('%r%a');

					// Date is between 1 and 30 days in the future
					if ($dayDiff > 0 && $dayDiff <= 30) {
						$q = new Query("SELECT `password` FROM `User` WHERE `uuid`=:uuid;", [":uuid" => $uuid]);
						if ($q->count() === 1 && ($user = $q->fetch()) !== null) {
							return password_verify($user['password'], $passHash);
						}
					}
				}
			}
		}

		return false;
	}

	/**
	 * Get UUID from authorization token
	 *
	 * @return string|null
	 */
	public static function tokenUuid(): ?string {
		if (($token = IO::getBearerToken()) != null) {
			if (($decoded = base64_decode($token)) !== false) {
				$decToken = explode(".", $decoded);
				if (($uuid = base64_decode($decToken[0])) !== false)
					return $uuid;
			}
		}
		return null;
	}

	/**
	 * Generates a token with lifespan of 30 days
	 *
	 * @param string $uuid UUID of a user
	 * @param string $hash Password hash of a user
	 * @return string Valid token for the next 30 days
	 */
	public static function generateToken(string $uuid, string $hash): string {
		$validUntil = (new DateTime())->add(new DateInterval('P30D'))->format("Y-m-d");
		$hashHash = password_hash($hash, PASSWORD_DEFAULT);
		return base64_encode(base64_encode($uuid) . "." . base64_encode($hashHash) . "." . base64_encode($validUntil));
	}
}
