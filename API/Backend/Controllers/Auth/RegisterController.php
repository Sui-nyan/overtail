<?php

class RegisterController extends Controller
{
	protected array $paths = ['/register'];
	protected array $methods = ['POST'];
	protected array $reqVar = ['email', 'password'];

	protected function execute(): void {
		$email = IO::POST('email');

		do {
			$uuid = Utils::uuid();
		} while (intval((new Query('SELECT count(`uuid`) count FROM `User` WHERE `uuid`=:uuid;', [':uuid' => $uuid]))->fetch()['count']) > 0);

		$success = false;
		try {
			$q = new Query(									// Insert user into database
				'INSERT INTO `User` (`uuid`, `email`, `password`, `activation`) VALUES (:uuid, :email, :pass, 0);',
				[
					':uuid' => $uuid,
					':email' => $email,
					':pass' => password_hash(IO::POST('password'), PASSWORD_DEFAULT)
				]
			);

			$message = 'Welcome to Overtail, please <a href="'.DOMAIN.'/activate/'.urlencode($uuid).'">click here</a> activate your account.';
			$mail = new Mail($email, 'Account activation', $message);

			$success = $q->success() && $mail->send();		// Register is successful when 1. user inserted in database; 2. activation email has been send
		} catch (PDOException) {							// Insert query failed bc of e. g. unique constraints
			// Check if an account with the email exists already
			if ((new Query('SELECT `uuid` FROM `User` WHERE `email`=:email;', [':email' => $email]))->count() > 0) {
				(new ErrorView(486))->render();				// Custom error code (E-Mail already taken)
				return;
			}
		}
		(new APIView(['success' => $success]))->render();
	}
}
