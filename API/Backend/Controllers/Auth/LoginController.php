<?php

class LoginController extends Controller
{
	protected array $paths = ['/login'];
	protected array $methods = ['POST'];
	protected array $reqVar = ['email', 'password'];

	protected function execute(): void {
		$email = IO::POST('email');
		$q = new Query('SELECT `uuid`, `password` FROM `User` WHERE `email`=:email AND `activation` IS NULL;', [':email' => $email]);

		if (($user = $q->fetch()) !== null) {
			if (password_verify(IO::POST('password'), $user['password'])) {			// Check password
				if (password_needs_rehash($user['password'], PASSWORD_DEFAULT)) {	// Check if newer hashing algorithm is available, if so rehash and update password in `User` table
					$q = new Query('UPDATE `User` SET `password`=:pass WHERE `uuid`=:uuid;', [':pass' => password_hash(IO::POST('password'), PASSWORD_DEFAULT), ':uuid' => $user['uuid']]);
					if ($q->success())
						$user['password'] = (new Query('SELECT `password` FROM `User` WHERE `uuid`=:uuid', [':uuid' => $user['uuid']]))->fetch()['password'];
				}
				(new APIView(
					[
						'token' => Auth::generateToken($user['uuid'], $user['password'])
					]
				))->render();
				return;
			} else $error = 480;				// Wrong credentials
		} else {
			if (($user = (new Query('SELECT `uuid`, `password` FROM `User` WHERE `email`=:email AND `activation` IS NOT NULL;', [':email' => $email]))->fetch()) !== null) {
				if (password_verify(IO::POST('password'), $user['password']))
					$error = 481;				// E-Mail not confirmed
				else $error = 480;				// Wrong password
			} else $error = 482;				// User does not exists
		}
		(new ErrorView($error))->render();
	}
}
